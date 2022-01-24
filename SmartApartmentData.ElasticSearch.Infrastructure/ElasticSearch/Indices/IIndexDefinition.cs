using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public interface IIndexDefinition
{
    string IndexName { get; set; }
    Task CreateIndexAsync(IElasticClient client);
    Task DeleteIndexAsync(IElasticClient client);
    Task AddDocumentToIndexAsync<T>(IElasticClient client, List<T> documents) where T: IndexItem;
}