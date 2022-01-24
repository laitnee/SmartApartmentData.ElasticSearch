using Microsoft.AspNetCore.Mvc;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

namespace SmartApartmentData.ElasticSearch.WebAPI.Controllers.v1;

public class SearchController : BaseController
{
    private readonly ILogger<SearchController> logger;
    private readonly IESService eSsvc;

    public SearchController(ILogger<SearchController> logger, IESService ESsvc)
    {
        this.logger = logger;
        eSsvc = ESsvc;
    }

    [HttpGet]
    public async Task<IActionResult> IndexMyGuy()
    {
        logger.LogInformation("Welcome to .net apps");
        await eSsvc.CreateIndexAsync<PropertyIndexDefinition>();
        return Ok();
    }
}