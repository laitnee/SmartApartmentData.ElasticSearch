using SmartApartmentData.ElasticSearch.Application.Services;

namespace SmartApartmentData.ElasticSearch.Application;

using Microsoft.Extensions.DependencyInjection;

public static class BootstrapApplication
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddTransient<ISearchService, SearchService>();
    }
}