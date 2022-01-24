using Microsoft.Extensions.Logging;
using Nest;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public abstract class IndexDefinition<T> : IIndexDefinition<T>
{
    private readonly ILogger logger;
    public IndexDefinition(ILogger logger)
    {
        this.logger = logger;
    }
    
    public abstract string IndexName { get; set; }

    public abstract Task CreateIndexAsync(IElasticClient client);

    public abstract Task DeleteIndexAsync(IElasticClient client);

    public virtual async Task AddDocumentToIndexAsync<T>(IElasticClient client, List<T> documents) where T: class
    {
        if (documents.Any())
        {
            var response = await client.IndexManyAsync<T>(documents);
            if (response.Errors) 
            {
                foreach (var itemWithError in response.ItemsWithErrors) 
                {
                    logger.LogInformation($"Failed to index document {itemWithError.Id}: {itemWithError.Error}");
                }
            }
        }
    }
}