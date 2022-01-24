using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

public interface IESService
{
    Task SearchAsync();
    Task CreateIndexAsync<T>() where T : IIndexDefinition;

    Task AddDocumentAsync<TDefinition,TItem>(List<TItem> documents) where TDefinition : IIndexDefinition, new() where TItem : IndexItem;
}