namespace Domain.Dto;

public class PurchaseSuggestionDto
{
    public string BomValue { get; set; }
    public string Package { get; set; }
    public int QuantityNeeded { get; set; }
    public List<TmeSuggestionDto> Suggestions { get; set; } = new();
}

public class TmeSuggestionDto
{
    public string Symbol { get; set; }
    public string Description { get; set; }
    public decimal TotalCost { get; set; }
    public int QuantityToOrder { get; set; }
    public string Currency { get; set; }
    public string Url { get; set; }
}

