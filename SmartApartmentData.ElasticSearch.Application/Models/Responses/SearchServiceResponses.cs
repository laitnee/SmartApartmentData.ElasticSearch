using SmartApartmentData.ElasticSearch.Domain.Entities;

namespace SmartApartmentData.ElasticSearch.Application.Models.Responses;

public record AutoCompleteResponse(string type, string Id, string? Name, string? FormerName, string? Market, string? State);