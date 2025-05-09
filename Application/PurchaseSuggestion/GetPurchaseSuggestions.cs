using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PurchaseSuggestion;

public class GetPurchaseSuggestions
{
    public class Query : IRequest<List<PurchaseSuggestionDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Query, List<PurchaseSuggestionDto>>
    {
        private readonly AppDbContext _context;
        private readonly TmeApiService _tme;

        public Handler(AppDbContext context, TmeApiService tme)
        {
            _context = context;
            _tme = tme;
        }

        public async Task<List<PurchaseSuggestionDto>> Handle(Query request, CancellationToken ct)
        {
            var userInventory = await _context.InventoryItems
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(ct);

            var bomItems = await _context.BomItems
                .Include(b => b.Project)
                .Where(b =>
                    b.Project.UserId == request.UserId &&
                    b.IsRelevant &&
                    !b.IsPlaced)
                .ToListAsync(ct);

            var result = new List<PurchaseSuggestionDto>();

            foreach (var bom in bomItems)
            {
                int available = bom.SelectedInventoryItemIds
                    .Select(id => userInventory.FirstOrDefault(i => i.Id == id))
                    .Where(i => i != null)
                    .Sum(i => i!.Quantity - i.ReservedForProjects);

                if (available >= bom.Quantity)
                    continue;

                string searchTerm = $"{bom.Category} {bom.Value} {bom.Package}";
                var products = await _tme.SearchProductsAsync(request.Token, searchTerm);

                var filtered = products
                    .Where(p => p.ProductStatusList == null || !p.ProductStatusList.Any(
                        status => status is "CANNOT_BE_ORDERED" or "INVALID" or "NOT_IN_OFFER"))
                    .Take(20)
                    .ToList();

                if (!filtered.Any()) continue;

                var symbols = filtered.Select(p => p.Symbol).Where(s => !string.IsNullOrWhiteSpace(s)).Take(50).ToList();
                var priceList = await _tme.GetPricesAndStocksAsync(request.Token, symbols);

                var suggestions = new List<(TmeSuggestionDto dto, decimal cost)>();

                foreach (var desc in filtered)
                {
                    var priceData = priceList.FirstOrDefault(p => p.Symbol == desc.Symbol);
                    if (priceData?.PriceList == null || priceData.PriceList.Count == 0)
                        continue;

                    var quantity = bom.Quantity;
                    var currentTier = priceData.PriceList
                        .Where(p => p.Amount <= quantity)
                        .OrderByDescending(p => p.Amount)
                        .FirstOrDefault();

                    var nextTier = priceData.PriceList
                        .Where(p => p.Amount > quantity)
                        .OrderBy(p => p.Amount)
                        .FirstOrDefault();

                    if (currentTier == null && nextTier == null)
                        continue;

                    decimal currentTotal = currentTier != null ? (decimal)(currentTier.PriceValue * quantity) : decimal.MaxValue;
                    decimal nextTotal = nextTier != null ? (decimal)(nextTier.PriceValue * nextTier.Amount) : decimal.MaxValue;

                    decimal finalTotal;
                    int qtyToOrder;

                    if (currentTotal <= nextTotal)
                    {
                        finalTotal = currentTotal;
                        qtyToOrder = quantity;
                    }
                    else
                    {
                        finalTotal = nextTotal;
                        qtyToOrder = nextTier.Amount;
                    }

                    suggestions.Add((
                        new TmeSuggestionDto
                        {
                            Symbol = desc.Symbol ?? "",
                            Description = desc.Description ?? "",
                            Url = desc.ProductInformationPage ?? "",
                            Currency = "PLN",
                            QuantityToOrder = qtyToOrder,
                            TotalCost = Math.Round(finalTotal, 2)
                        },
                        finalTotal
                    ));
                }

                var spaced = suggestions
                    .OrderBy(x => x.cost)
                    .Select(x => x.dto)
                    .ToList();

                var spacedCount = spaced.Count;
                var selected = spacedCount switch
                {
                    <= 3 => spaced,
                    4 => new List<TmeSuggestionDto> { spaced[0], spaced[2], spaced[3] },
                    _ => new List<TmeSuggestionDto>
                    {
                        spaced[0],
                        spaced[spacedCount / 3],
                        spaced[2 * spacedCount / 3],
                        spaced[^1]
                    }
                };

                result.Add(new PurchaseSuggestionDto
                {
                    BomValue = bom.Value,
                    Package = bom.Package,
                    QuantityNeeded = bom.Quantity,
                    Suggestions = selected
                });
            }

            return result;
        }
    }
}
