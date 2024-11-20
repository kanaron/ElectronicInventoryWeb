﻿using ElectronicInventoryWeb.Server.TmeModels;

namespace ElectronicInventoryWeb.Server.TmeJResults;

public class GetDescriptionJResult : ApiResult<GetDescriptionJResultData>
{
    public string Status { get; set; }
    public GetDescriptionJResultData Data { get; set; }
}

public class GetDescriptionJResultData
{
    public List<ProductWithDescription> ProductList { get; set; }
    public string Language { get; set; }
}
