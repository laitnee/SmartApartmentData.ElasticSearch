using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartApartmentData.ElasticSearch.Application;
using SmartApartmentData.ElasticSearch.Application.Models;

namespace SmartApartmentData.ElasticSearch.WebAPI.Attributes;

public class ValidateRequest : ActionFilterAttribute
{
        
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelState = context.ModelState;
            
        if (!modelState.IsValid)
        {
            var errors = modelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                
            var response = new Result<List<string>>("Request Validation Error", 
                AppStatusCodes.RequestValidationError)
            {
                Data = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}