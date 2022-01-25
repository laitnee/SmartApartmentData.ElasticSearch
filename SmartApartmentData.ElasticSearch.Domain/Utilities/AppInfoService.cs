namespace SmartApartmentData.ElasticSearch.Domain.Utilities;

public class AppInfoService : IAppInfoService
{
    public string GetEnvironment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
}