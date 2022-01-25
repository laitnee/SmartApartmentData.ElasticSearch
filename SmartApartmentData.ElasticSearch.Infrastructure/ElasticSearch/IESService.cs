using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

public interface IESService
{
    Task SearchAsync();
    Task CreateIndexAsync<T>() where T : IIndexDefinition;

    Task AddDocumentAsync<TDefinition, T>(List<T> documents) where TDefinition : IIndexDefinition where T : IndexItem;
}