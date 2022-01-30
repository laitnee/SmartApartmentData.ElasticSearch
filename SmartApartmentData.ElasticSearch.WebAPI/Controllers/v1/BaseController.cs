using Microsoft.AspNetCore.Mvc;
using SmartApartmentData.ElasticSearch.Application;
using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.WebAPI.Attributes;
using IResult = SmartApartmentData.ElasticSearch.Application.Models.IResult;

namespace SmartApartmentData.ElasticSearch.WebAPI.Controllers.v1;


[ApiController]
[Route("api/v1/[controller]")]
[ValidateRequest]
public class BaseController : ControllerBase
{
    protected ActionResult ConvertToActionResult<T>(T result) where T : IResult
    {
        return result.StatusCode switch
        {
            AppStatusCodes.Success => Ok(result),
            AppStatusCodes.ServiceError => StatusCode(500, result),
            _ => BadRequest(result)
        };
    }
}