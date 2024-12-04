namespace ElectronicInventoryWeb.Server.TmeModels;

public class ErrorResult
{
    public string Status { get; private set; }
    public string Error { get; private set; }

    public ErrorResult(string status, string errorMessage)
    {
        this.Status = status;
        this.Error = errorMessage;
    }
}
