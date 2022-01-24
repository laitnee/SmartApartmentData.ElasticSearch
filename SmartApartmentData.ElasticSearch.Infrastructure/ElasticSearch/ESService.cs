using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

public class ESService : IESService
{
    private readonly IElasticClient client;
    private readonly ILogger<ESService> logger;

    public ESService(ILogger<ESService> logger, IElasticClient client)
    {
        this.client = client;
        this.logger = logger;
    }
    public Task SearchAsync()
    {
        throw new NotImplementedException();
    }

    public async Task CreateIndexAsync<T>() where T : IIndexDefinition
    {
        logger.LogInformation("Indexing Started");
        var indexDefinition = (T) Activator.CreateInstance(typeof(T), new Object[]{logger})!;
        await indexDefinition.CreateIndexAsync(client);
        logger.LogInformation("Indexing Successful");
    }
    

    public async Task AddDocumentAsync<TDefinition,TItem>(List<TItem> documents) where TDefinition : IIndexDefinition, new() where TItem : IndexItem
    {
        var indexDefinition = (TDefinition) Activator.CreateInstance(typeof(TDefinition))!;
        await indexDefinition.AddDocumentToIndexAsync<TItem>(client, documents);
    }
}