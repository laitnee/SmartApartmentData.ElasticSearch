using Microsoft.Extensions.Logging;
using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.Application.Models.Requests;
using SmartApartmentData.ElasticSearch.Application.Models.Responses;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Models;

namespace SmartApartmentData.ElasticSearch.Application.Services;

public class SearchService : ISearchService
{
    private readonly ILogger<SearchService> logger;
    private readonly IESService esSvc;

    public SearchService(ILogger<SearchService> logger, IESService esSvc)
    {
        this.logger = logger;
        this.esSvc = esSvc;
    }
    public async Task<IResult<List<Dictionary<string,AutoCompleteResponse>>>> AutoCompleteSearch(AutoCompleteSearchRequest req)
    {
        var searchResponses = await esSvc.SearchAsync(new SMAutoCompleteSearchRequest(req.Phrase, req.Market, req.State, req.Limit));
        var response = searchResponses.Select( x =>
                                            new Dictionary<string, AutoCompleteResponse>(){
                                            {
                                                x.type, new AutoCompleteResponse(x.type, x.Id, x.Name, x.FormerName, x.Market, x.State )
                                            }}
                                        ).ToList();
        
        return new Result<List<Dictionary<string,AutoCompleteResponse>>>(AppStatusCodes.Success, "Autocomplete Success", response);
    }
}