namespace Domain.TmeJResults;

public class ApiResult<DataType>
{
    public string Status { get; set; }
    public DataType Data { get; set; }
}
