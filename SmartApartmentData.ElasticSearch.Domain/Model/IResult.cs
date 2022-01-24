namespace SmartApartmentData.ElasticSearch.Application.Models;

public interface IResult
{
    public string Message { get; set; }
    public string StatusCode { get; set; }
}
public interface IResult<T> : IResult where T : class
{
    public T Data { get; set; }
    public string Message { get; set; }
    public string StatusCode { get; set; }
}