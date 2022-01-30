using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.Application.Models.Requests;
using SmartApartmentData.ElasticSearch.Application.Models.Responses;
using SmartApartmentData.ElasticSearch.Application.Services;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

namespace SmartApartmentData.ElasticSearch.WebAPI.Controllers.v1;

public class SearchController : BaseController
{
    private readonly ILogger<SearchController> logger;
    
    private readonly ISearchService searchSvc;

    public SearchController(ILogger<SearchController> logger, ISearchService searchSvc)
    {
        this.logger = logger;
        this.searchSvc = searchSvc;
    }
    
    /// <summary>
    ///     Returns key value pair of type and record that match search phrase
    /// </summary>
    /// <remarks>
    ///     This endpoint will return 25 records by default.
    /// </remarks>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("autocomplete")]
    [Produces("application/json")]
    public async Task<ActionResult> AutoComplete([FromQuery]AutoCompleteSearchRequest request)
    {
        var response = await searchSvc.AutoCompleteSearch(request);
        return ConvertToActionResult(response);
    }
}