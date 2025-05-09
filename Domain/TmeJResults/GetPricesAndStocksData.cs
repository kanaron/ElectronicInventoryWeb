using Domain.TmeModels;

namespace Domain.TmeJResults;

public class GetPricesAndStocksJResult : ApiResult<GetPricesAndStocksData>
{
    public string Status { get; set; }
    public GetPricesAndStocksData Data { get; set; }
}

public class GetPricesAndStocksData
{
    public List<ProductWithPrices> ProductList { get; set; }
    public string Currency { get; set; }
}
