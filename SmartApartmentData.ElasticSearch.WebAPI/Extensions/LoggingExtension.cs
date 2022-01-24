using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace SmartApartmentData.ElasticSearch.WebAPI.Extensions;

public static class LoggingExtension
{
    public static void ConfigureLogs(this IServiceCollection service)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(ConfigureEls(configuration, env))
            .CreateLogger();
    }


    public static ElasticsearchSinkOptions ConfigureEls(IConfiguration configuration, string env)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration["ELK_local_Configuration:Uri"])){
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower()}-{env.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}