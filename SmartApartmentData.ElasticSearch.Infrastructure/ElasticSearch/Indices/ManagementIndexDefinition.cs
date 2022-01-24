using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public class ManagementIndexDefinition : IndexDefinition<ManagementES>, IIndexDefinition
{
    
    private readonly ILogger logger;
    public ManagementIndexDefinition(ILogger logger) : base(logger)
    {
        this.logger = logger;
    }

    public override string IndexName { get; set; } = ElasticSearchConstants.ManagementIndexName;
    
    public override async Task CreateIndexAsync(IElasticClient client)
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
                    ))
                
                .Analyzers( ca => ca
                    
                    .Custom( ElasticSearchConstants.CustomStandardAnalyzer, csa => csa 
                        .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                        .Tokenizer("standard"))
                    
                    .Custom( ElasticSearchConstants.AutoCompleteTokenizer, csa => csa 
                        .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                        .Tokenizer(ElasticSearchConstants.AutoCompleteTokenizer))
                    
                    .Custom( ElasticSearchConstants.KeywordAnalyzer, csa => csa 
                        .Filters("trim", "lowercase")
                        .Tokenizer("keyword"))
                ))
            )
            .Map<ManagementES>(m =>
                m.AutoMap<ManagementES>()));
        if (response.ServerError.Error != null) logger.LogInformation("Error occured while trying to index on servier");
    }

    public override Task DeleteIndexAsync(IElasticClient client)
    {
        throw new NotImplementedException();
    }
}