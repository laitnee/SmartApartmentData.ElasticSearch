using System.Text.Json;
using System.Xml;
using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Models;

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
    
    public async Task CreateIndexAsync<T>() where T : IIndexDefinition
    {
        logger.LogInformation("Indexing Started");
        var indexDefinition = (T) Activator.CreateInstance(typeof(T), new Object[]{logger})!;
        await indexDefinition.CreateIndexAsync(client);
        logger.LogInformation("Indexing Successful");
    }
    

    public async Task AddDocumentAsync<TDefinition, T>(List<T> documents) where TDefinition : IIndexDefinition where T : IndexItem
    {
        var indexDefinition = (TDefinition) Activator.CreateInstance(typeof(TDefinition), new Object[]{logger});
        await indexDefinition.AddDocumentToIndexAsync<T>(client, documents);
    }

    public async Task DeleteIndexAsync<T>() where T : IIndexDefinition
    {
        logger.LogInformation("Deleting Index Started");
        var indexDefinition = (T) Activator.CreateInstance(typeof(T), new Object[]{logger})!;
        await indexDefinition.DeleteIndexAsync(client);
        logger.LogInformation("Deleting Successful");
    }

    public async Task<List<IndexItemResponse>> SearchAsync(SMAutoCompleteSearchRequest req)
    {
        var query = BuildSearchQuery(req);
        
        var response = await client.SearchAsync<Object>(b => b
            .Size(req.limit)
            .Index( new string[] {  ElasticSearchConstants.PropertyIndexName, ElasticSearchConstants.ManagementIndexName} )
            .Query(q => query));
        
        if(response.IsValid) logger.LogInformation(JsonSerializer.Serialize(response.Hits));

        var result = response.Hits.Select(hits => ProcessSearchResult(hits)).ToList();

       return result;
    }

    private IndexItemResponse ProcessSearchResult(IHit<Object> documents)
    {

        if (documents.Source is not null)
        {
            var type = documents.Index;
            var id = documents.Id;
            
            var source = (Dictionary<string,object>) documents.Source;
            
            source.TryGetValue("Name", out object nameObj);
            source.TryGetValue("FormerName", out object formerNameObj);
            source.TryGetValue("Market", out object marketObj);
            source.TryGetValue("State", out object stateObj);

            var name = Convert.ToString(nameObj);
            var formerName = Convert.ToString(formerNameObj);
            var market = Convert.ToString(marketObj);
            var state = Convert.ToString(stateObj);
            
            return new IndexItemResponse(type, id, name, formerName, market, state);
        }

        return null;
    }
    
    private QueryContainer BuildSearchQuery(SMAutoCompleteSearchRequest req)
    {
        var query = new QueryContainerDescriptor<PropertyES>()
            .MultiMatch(mm => mm
                .Fields( f => f
                    .Field(f => f.Name)
                    .Field(f => f.FormerName)
                    .Field(f => f.StreetAddress)
                )
                .Query(req.SearchPhrase.ToLower()));

        if (!String.IsNullOrWhiteSpace(req.Market))
            query = query && new QueryContainerDescriptor<PropertyES>()
                .Term(t => t.Market, req.Market.ToLower());

        if (!String.IsNullOrWhiteSpace(req.State))
            query = query && new QueryContainerDescriptor<PropertyES>()
                .Term(t => t.State, req.State.ToLower());
        
        query = query || new QueryContainerDescriptor<ManagementES>()
            .MultiMatch(mm => mm
                .Fields( f => f
                    .Field(f => f.Name.ToLower())
                )
                .Query(req.SearchPhrase));

        if (!String.IsNullOrWhiteSpace(req.Market))
            query = query && new QueryContainerDescriptor<ManagementES>()
                .Term(t => t.Market, req.Market.ToLower());



        if (!String.IsNullOrWhiteSpace(req.State))
            query = query && new QueryContainerDescriptor<ManagementES>()
                .Term(t => t.State, req.State.ToLower());

        return query;
    }
}