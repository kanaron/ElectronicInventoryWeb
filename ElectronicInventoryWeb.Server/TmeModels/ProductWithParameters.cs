namespace ElectronicInventoryWeb.Server.TmeModels;

public class ProductWithParameters
{
    public string? Symbol { get; set; }
    public List<Parameters>? ParameterList { get; set; }
}
