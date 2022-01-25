using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Models;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

public interface IESService
{
    Task CreateIndexAsync<T>() where T : IIndexDefinition;

    Task AddDocumentAsync<TDefinition, T>(List<T> documents) where TDefinition : IIndexDefinition where T : IndexItem;
    
    Task DeleteIndexAsync<T>() where T : IIndexDefinition;

    Task<List<IndexItemResponse>> SearchAsync(SMAutoCompleteSearchRequest req);


}