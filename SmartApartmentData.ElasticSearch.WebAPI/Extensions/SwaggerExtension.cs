using System.Reflection;
using Microsoft.OpenApi.Models;

namespace SmartApartmentData.ElasticSearch.WebAPI.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Apartment Data Elasticsearch", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        
        
    }
    
    public static void UseSwaggerDoc(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Apartment Data Elasticsearch");
            c.RoutePrefix = "swagger";
        });
    }
}