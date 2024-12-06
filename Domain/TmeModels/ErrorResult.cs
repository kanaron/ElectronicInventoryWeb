namespace Domain.TmeModels;

public class ErrorResult
{
    public string Status { get; private set; }
    public string Error { get; private set; }

    public ErrorResult(string status, string errorMessage)
    {
        Status = status;
        Error = errorMessage;
    }
}
