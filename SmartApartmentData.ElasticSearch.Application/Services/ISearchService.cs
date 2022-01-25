using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.Application.Models.Requests;
using SmartApartmentData.ElasticSearch.Application.Models.Responses;

namespace SmartApartmentData.ElasticSearch.Application.Services;

public interface ISearchService
{
    Task<IResult<List<Dictionary<string,AutoCompleteResponse>>>>  AutoCompleteSearch(AutoCompleteSearchRequest autoCompleteSearchRequest);
}