using Serilog;
using SmartApartmentData.ElasticSearch.Application;
using SmartApartmentData.ElasticSearch.Infrastructure;
using SmartApartmentData.ElasticSearch.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services

builder.Services.ConfigureLogs();
builder.Host.UseSerilog();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

#endregion


#region Configure Application 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDoc();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion