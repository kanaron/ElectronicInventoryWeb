namespace Domain.TmeModels;

public class ProductWithPrices
{
    public string Symbol { get; set; }
    public string Unit { get; set; }
    public int VatRate { get; set; }
    public string VatType { get; set; }
    public Price[] PriceList { get; set; }
}
