using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;

namespace SmartApartmentData.ElasticSearch.Infrastructure;

public static class BootStrapInfrastructure
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddElasticSearch(service, configuration);
        
        service.AddHostedService<AutoIndexDocuments>();

        service.AddScoped<IESService, ESService>();
        
    }
    private static void AddElasticSearch(IServiceCollection service, IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));

        var client = new ElasticClient(settings);

        service.AddSingleton<IElasticClient>(client);
    }
}