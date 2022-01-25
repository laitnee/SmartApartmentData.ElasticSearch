using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Models;
using Xunit;

namespace SmartApartmentData.ElasticSearch.Test;

public class ESSearchServiceTest
{
    private readonly IElasticClient client ;
    public ESSearchServiceTest()
    {
        client = SetupElasticClient();
    }

    private ElasticClient SetupElasticClient()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
        settings.ThrowExceptions(alwaysThrow: true);
        settings.DisableDirectStreaming();
        settings.PrettyJson();

        return new ElasticClient(settings);
    }
    public void Dispose()
    {
    }
    
    
    [Fact]
    public async Task ESService_CreatesIndex_Successful()
    {
        #region setup
        
        IESService esService = new ESService(new Logger<ESService>(new LoggerFactory()), client);
        
        client.Indices.Delete(ElasticSearchConstants.ManagementIndexName);
        
        var exists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        if (exists.Exists) throw new Exception("Error setting up test");
        
        #endregion
        
        await esService.CreateIndexAsync<ManagementIndexDefinition>();
        
        var indexExists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        Assert.True(indexExists.Exists);
        
    }
    
    [Fact]
    public async Task ESService_DeleteIndex_Successful()
    {
        #region setup
        
        IESService esService = new ESService(new Logger<ESService>(new LoggerFactory()), client);

        client.Indices.Delete(ElasticSearchConstants.ManagementIndexName);
        
        var indexExists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        if (indexExists.Exists) throw new Exception("Error setting up test");

        client.Indices.Create(ElasticSearchConstants.ManagementIndexName);

        indexExists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        if (!indexExists.Exists) throw new Exception("Error setting up test");
        #endregion
        
        await esService.DeleteIndexAsync<ManagementIndexDefinition>();
        
        var exists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);

        Assert.False(exists.Exists);
    }
    
    [Fact]
    public async Task ESService_SearchDocument_Successful()
    {
        #region setup
        
        IESService esService = new ESService(new Logger<ESService>(new LoggerFactory()), client);

        client.Indices.Delete(ElasticSearchConstants.PropertyIndexName);

        client.Indices.Delete(ElasticSearchConstants.ManagementIndexName);
        
        var exists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);

        if (exists.Exists) throw new Exception("Error setting up test");   
        
        await esService.CreateIndexAsync<PropertyIndexDefinition>();

        await esService.CreateIndexAsync<ManagementIndexDefinition>();

        var indexExists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        if (!indexExists.Exists) throw new Exception("Error setting up test");
        #endregion


        await esService.AddDocumentAsync<ManagementIndexDefinition, ManagementES>(mangementESDocs);
        
        //sleep because indexing takes time to be ready for query 
        Thread.Sleep(1000);

        var docs = await esService.SearchAsync(new SMAutoCompleteSearchRequest("stone", limit:25));
        
        var docCount = docs.Count();

        var expectedCount = 1;
        Assert.Equal(expectedCount, docCount);
    }

    [Fact]
    public async Task ESService_AddDocument_Successful()
    {
        #region setup
        
        IESService esService = new ESService(new Logger<ESService>(new LoggerFactory()), client);
        
        client.Indices.Delete(ElasticSearchConstants.ManagementIndexName);
        
        var exists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);

        if (exists.Exists) throw new Exception("Error setting up test");   

        await esService.CreateIndexAsync<PropertyIndexDefinition>();

        await esService.CreateIndexAsync<ManagementIndexDefinition>();

        var indexExists = client.Indices.Exists(ElasticSearchConstants.ManagementIndexName);
        
        if (!indexExists.Exists) throw new Exception("Error setting up test");
        #endregion


       await esService.AddDocumentAsync<ManagementIndexDefinition, ManagementES>(mangementESDocs);
       
       //sleep because indexing takes time to be ready for query 
       Thread.Sleep(1000);
       
       var docs = client.Search<ManagementES>(s => s
           .Size(10)
           .Index( new string[] { ElasticSearchConstants.ManagementIndexName} )
           .Query( b => b
               .Match(f => f
                   .Query("stone")
                   .Field(f => f.Name)
           )));

       var docCount = docs.Hits.Select(hit => hit.Source).Count();

        var expectedCount = 1;
        Assert.Equal(expectedCount, docCount);
        
         docs = client.Search<ManagementES>(s => s
            .Size(10)
            .Index( new string[] { ElasticSearchConstants.ManagementIndexName} )
            .Query( b => b
                .Match(f => f
                    .Query("heaven")
                    .Field(f => f.Name)
                )));

         docCount = docs.Hits.Select(hit => hit.Source).Count();

         expectedCount = 1;
        Assert.Equal(expectedCount, docCount);
    }

    private List<ManagementES> mangementESDocs => new List<ManagementES>()
    {
        new ManagementES()
        {
            Name = "Bluestone Properties",
            ManagementID = 123,
            State = "AS",
            Market = "Austin"
        },
        new ManagementES()
        {
            Name = "Heaven Road",
            ManagementID = 2,
            State = "HS",
            Market = "Houston"
        }
    };
}