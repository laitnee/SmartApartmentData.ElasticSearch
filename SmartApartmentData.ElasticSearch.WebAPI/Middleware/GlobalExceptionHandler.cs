using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using SmartApartmentData.ElasticSearch.Application;
using SmartApartmentData.ElasticSearch.Application.Models;
using SmartApartmentData.ElasticSearch.Domain.Exceptions;

namespace SmartApartmentData.ElasticSearch.WebAPI.Middleware;

//extension method handles app scoped try catch
public static class GlobalExceptionHandlerMiddleware
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    
                context.Response.ContentType = "application/json";
                    
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error.GetBaseException();

                    logger.LogError($"Something went wrong: {contextFeature.Error}");

                    string responseMessage = string.Empty;
                        
                    switch (exception)
                    {
                        case InAppException:
                            responseMessage = contextFeature.Error.Message;
                            break;
                        default:
                            responseMessage = "Error Processing Request";
                            break;
                    }
                        
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new Result(responseMessage, AppStatusCodes.ServiceError)
                        ));
                }                                         
            });
        });
    }
}