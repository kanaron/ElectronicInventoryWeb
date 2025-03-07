using Domain.Dto;
using Domain.Mappers;
using MediatR;
using Persistence;

namespace Application.InventoryItems;

public class EditInventoryItem
{
    public class Command : IRequest
    {
        public UpdateInventoryItemDto ItemDto { get; set; }
        public Guid ItemId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext context)
        {
            _appDbContext = context;
        }


        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var itemToEdit = (await _appDbContext.InventoryItems.FindAsync([request.ItemId], cancellationToken: cancellationToken));

            if (itemToEdit == null)
            {
                return;
            }

            var (standardValue, standardUnit, rawValue) = InventoryMappers.NormalizeComponentValue(request.ItemDto.Value);

            itemToEdit.Type = request.ItemDto.Type;
            itemToEdit.Symbol = request.ItemDto.Symbol;
            itemToEdit.Category = request.ItemDto.Category;
            itemToEdit.Value = rawValue;
            itemToEdit.StandardValue = standardValue;
            itemToEdit.StandardUnit = standardUnit;
            itemToEdit.Package = request.ItemDto.Package;
            itemToEdit.Quantity = request.ItemDto.Quantity;
            itemToEdit.Location = request.ItemDto.Location;
            itemToEdit.DatasheetLink = request.ItemDto.DatasheetLink;
            itemToEdit.StoreLink = request.ItemDto.StoreLink;
            itemToEdit.PhotoUrl = request.ItemDto.PhotoUrl;
            itemToEdit.MinStockLevel = request.ItemDto.MinStockLevel;
            itemToEdit.Description = request.ItemDto.Description;
            itemToEdit.IsActive = request.ItemDto.IsActive;
            itemToEdit.Tags = request.ItemDto.Tags;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
