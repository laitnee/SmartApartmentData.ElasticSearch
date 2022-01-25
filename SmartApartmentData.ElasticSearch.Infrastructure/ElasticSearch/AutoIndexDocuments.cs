using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Domain.Entities;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

public class AutoIndexDocuments : BackgroundService
{
    private readonly ILogger<AutoIndexDocuments> logger;
    private readonly IServiceProvider serviceProvider;

    public AutoIndexDocuments(ILogger<AutoIndexDocuments> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting IBackground Service");
        await IndexDcocument(stoppingToken);
    }

    private async Task IndexDcocument(CancellationToken stoppingToken)
    {
        logger.LogInformation("Indexing and Documentation Starting...");
        
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var esService =  scope.ServiceProvider.GetRequiredService<IESService>();

        await CreateIndexesAsync(esService);

        await AddPropertyDocuments(esService);

        await AddManagementDocuments(esService);
        
        logger.LogInformation("Indexing and Documentation Ending, Stopping background service...");
    }

    private async Task CreateIndexesAsync(IESService esService)
    {
        
        await esService.DeleteIndexAsync<PropertyIndexDefinition>();
        await esService.DeleteIndexAsync<ManagementIndexDefinition>();
        
        logger.LogInformation("Indexing Property...");
        await esService.CreateIndexAsync<PropertyIndexDefinition>();
        
        logger.LogInformation("Indexing Management...");
        await esService.CreateIndexAsync<ManagementIndexDefinition>();
    }

    private async Task AddPropertyDocuments(IESService esService)
    {
        logger.LogInformation("Documenting Properties...");
        
        using StreamReader reader = new StreamReader(ElasticSearchConstants.PropertiesFilePath);
        
        var propDictionary = await reader.ReadToEndAsync();
    
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var properties =  JsonSerializer.Deserialize<List<Dictionary<string, Property>>>(propDictionary, options)
            .SelectMany(x => x.Values);
        
        var propertiesES = properties.Select<Property,PropertyES>(x => PropertyES.Map(x));

        var chunkSize = 5000;
        var properiesEsChunck = propertiesES.Chunk(5000);

        foreach (var chunk in properiesEsChunck)
        {
            await esService.AddDocumentAsync<PropertyIndexDefinition,PropertyES>(chunk.ToList());
        }
    }
    private async Task AddManagementDocuments(IESService esService)
    {
        logger.LogInformation("Documenting Management...");
        
        using StreamReader reader = new StreamReader(ElasticSearchConstants.ManagementFilePath);
        
        var mgmtDictionary = await reader.ReadToEndAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var managements = JsonSerializer.Deserialize<List<Dictionary<string, Management>>>(mgmtDictionary, options)
            .SelectMany(x => x.Values);
        
        
        var managementES = managements.Select<Management,ManagementES>(x => ManagementES.Map(x));
        
        var chunkSize = 5000;
        var managementEsChunck = managementES.Chunk(5000);

        foreach (var chunk in managementEsChunck)
        {
            await esService.AddDocumentAsync<ManagementIndexDefinition,ManagementES>(chunk.ToList());
        }
    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}