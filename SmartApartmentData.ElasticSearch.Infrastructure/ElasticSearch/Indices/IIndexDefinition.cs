using Nest;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public interface IIndexDefinition<T>
{
    string IndexName { get; set; }
    Task CreateIndexAsync(IElasticClient client);
    Task DeleteIndexAsync(IElasticClient client);
    Task AddDocumentToIndexAsync<T>(IElasticClient client, List<T> documents) where T : class;
}