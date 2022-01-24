using Microsoft.Extensions.Logging;
using Nest;
using SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Indices;

public class ManagementIndex : IndexDefinition<ManagementES>, IIndexDefinition<ManagementES>
{
    private readonly ILogger<ManagementIndex> logger;
    public ManagementIndex(ILogger<ManagementIndex> logger) : base(logger)
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
                        .MaxGram(5)
                        .TokenChars(new [] {TokenChar.Letter, TokenChar.Digit})
                    ))
                
                .Analyzers( ca => ca
                    
                    .Custom( ElasticSearchConstants.CustomStandardAnalyzer, csa => csa 
                        .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                        .Tokenizer("Standard"))
                    
                    .Custom( ElasticSearchConstants.AutoCompleteTokenizer, csa => csa 
                        .Filters(ElasticSearchConstants.EnglishStopTokenFilter, "trim", "lowercase")
                        .Tokenizer(ElasticSearchConstants.AutoCompleteTokenizer))
                ))
            )
            .Map<ManagementES>(m =>
                m.AutoMap<ManagementES>()));    
    }

    public override Task DeleteIndexAsync(IElasticClient client)
    {
        throw new NotImplementedException();
    }
}