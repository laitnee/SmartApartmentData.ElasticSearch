namespace SmartApartmentData.ElasticSearch.Application.Models.Requests;

public record SearchRequest
{
    public string Phrase { get; init; }
    public string Market { get; init; }
    public int Limit { get; init; }
}
