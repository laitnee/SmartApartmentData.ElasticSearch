using System.ComponentModel.DataAnnotations;

namespace SmartApartmentData.ElasticSearch.Application.Models.Requests;

public record AutoCompleteSearchRequest
{
    [Required]
    public string Phrase { get; init; }
    
    public string? Market { get; init; }
    
    public string? State { get; init; }

    public int Limit { get; init; } = 25;
}
