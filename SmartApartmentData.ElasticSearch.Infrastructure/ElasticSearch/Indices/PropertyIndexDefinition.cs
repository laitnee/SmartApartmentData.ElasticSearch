using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public class PropertyIndexDefinition : IndexDefinition<PropertyES>, IIndexDefinition
{
    private readonly ILogger logger;
    
    public PropertyIndexDefinition(ILogger logger) : base(logger)
    {
        this.logger = logger;
    }

    public override string IndexName { get; set; } = ElasticSearchConstants.PropertyIndexName;
    
    public override async Task CreateIndexAsync(IElasticClient client)
    {
        var existResponse = await client.Indices.ExistsAsync(IndexName);
        if (!existResponse.Exists)
        {
            var response = await client.Indices.CreateAsync(IndexName, c => c
                .Settings(s => s.Analysis( a => a.
                    TokenFilters( tf => tf.Stop(ElasticSearchConstants.EnglishStopTokenFilter, sf => sf
                        .StopWords("_English_")            
                    ))
                    .Tokenizers( tz => tz.
                        NGram(ElasticSearchConstants.AutoCompleteTokenizer, desc => desc
                            .MinGram(3)
                            .MaxGram(4)
                            .TokenChars(new [] {TokenChar.Letter, TokenChar.Digit})
                        )
                    )
                    .Analyzers( ca => ca
                        .Custom( ElasticSearchConstants.CustomStandardAnalyzer, csa => csa 
                            .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                            .Tokenizer("standard"))
                    
                        .Custom( ElasticSearchConstants.AutoCompleteAnalyzer, csa => csa 
                            .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                            .Tokenizer(ElasticSearchConstants.AutoCompleteTokenizer))
                    
                        .Custom( ElasticSearchConstants.KeywordAnalyzer, csa => csa 
                            .Filters("trim", "lowercase")
                            .Tokenizer("keyword"))
                
                    ))
                    
                )
                .Map<PropertyES>(m => m.AutoMap<PropertyES>()));
            
            if (!response.IsValid) throw new Exception("error creating index property");

        }
        
    }

    // private TypeMappingDescriptor<PropertyES> Mappings()
    // {
    //     return new TypeMappingDescriptor<PropertyES>()
    //         .AutoMap<PropertyES>()
    //         .Properties(p => p
    //             .Text(t => t
    //                 .Name(n => n.Name)
    //                 .Analyzer(ElasticSearchConstants.AutoCompleteAnalyzer)
    //             )
    //             .Text(t => t
    //                 .Name(n => n.FormerName)
    //                 .Analyzer(ElasticSearchConstants.AutoCompleteAnalyzer)
    //             )
    //             .Text(t => t
    //                 .Name(n => n.StreetAddress)
    //                 .Analyzer(ElasticSearchConstants.AutoCompleteAnalyzer))
    //             .Text(t => t
    //                 .Name(n => n.Market)
    //                 .Analyzer(ElasticSearchConstants.KeywordAnalyzer))
    //             .Text(t => t
    //                 .Name(n => n.State)
    //                 .Analyzer(ElasticSearchConstants.KeywordAnalyzer))
    //         );
    //
    //
    // }

    public override async Task DeleteIndexAsync(IElasticClient client)
    {
        var existResponse = await client.Indices.ExistsAsync(IndexName);
        if (existResponse.IsValid && existResponse.Exists)
        {
            var response = await client.Indices.DeleteAsync(this.IndexName);
            if (!response.IsValid) throw new Exception("Error Deleting Index");
        }
    }
}