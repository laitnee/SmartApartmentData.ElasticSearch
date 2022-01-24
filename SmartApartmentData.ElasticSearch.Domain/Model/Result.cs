namespace SmartApartmentData.ElasticSearch.Application.Models;


public record Result(string StatusCode, string Message = "") : IResult
{
    public string Message { get; set; } = Message;
    public string StatusCode { get; set; } = StatusCode;
}
public record Result<T>(string StatusCode, string Message = "", T Data = null) : IResult<T> where T : class, new()
{
    public T Data { get; set; } = Data;
    public string Message { get; set; } = Message;
    public string StatusCode { get; set; } = StatusCode;
}
