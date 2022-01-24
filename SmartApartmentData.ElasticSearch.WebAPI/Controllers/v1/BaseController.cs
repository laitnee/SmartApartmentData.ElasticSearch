using Microsoft.AspNetCore.Mvc;
using SmartApartmentData.ElasticSearch.Application;
using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.WebAPI.Attributes;

namespace SmartApartmentData.ElasticSearch.WebAPI.Controllers.v1;


[ApiController]
[Route("v1/[controller]")]
[ValidateRequest]
public class BaseController : ControllerBase
{
    protected ActionResult<T> HandleResponse<T>(T result) where T : Result
    {
        return result.StatusCode switch
        {
            AppStatusCodes.Success => Ok(result),
            AppStatusCodes.ServiceError => StatusCode(500, result),
            _ => BadRequest(result)
        };
    }
}