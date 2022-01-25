using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Models;

public record SMAutoCompleteSearchRequest(string SearchPhrase, string Market = "", string State = "", int limit = 25);

public record IndexItemResponse(string type, string Id, string? Name, string? FormerName, string? Market, string? State);