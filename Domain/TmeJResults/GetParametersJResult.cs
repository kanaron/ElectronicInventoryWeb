using Domain.TmeModels;

namespace Domain.TmeJResults;

public class GetParametersJResult : ApiResult<GetParametersJResultData>
{
    public string Status { get; set; }
    public GetParametersJResultData Data { get; set; }
}

public class GetParametersJResultData
{
    public List<ProductWithParameters> ProductList { get; set; }
    public string Language { get; set; }
}
