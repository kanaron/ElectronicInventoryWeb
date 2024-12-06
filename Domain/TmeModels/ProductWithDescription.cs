namespace Domain.TmeModels;

public class ProductWithDescription
{
    public string? Symbol { get; set; }
    public string? CustomerSymbol { get; set; }
    public string? OriginalSymbol { get; set; }
    public string? EAN { get; set; }
    public string? Producer { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? Category { get; set; }
    public string? Photo { get; set; }
    public string? Thumbnail { get; set; }
    public double? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public int? SuppliedAmount { get; set; }
    public int? MinAmount { get; set; }
    public int? Multiples { get; set; }
    public List<Packing>? Packing { get; set; }
    public List<string>? ProductStatusList { get; set; }
    public string? Unit { get; set; }
    public string? ProductInformationPage { get; set; }
    public string? Guarantee { get; set; }
    public string? OfferId { get; set; }
    public List<string>? Certificates { get; set; }
}

public class Packing
{
    public string? Id { get; set; }
    public string? IdUnit { get; set; }
    public string? PackingTranslation { get; set; }
    public int? Amount { get; set; }
}
