using Nest;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

[ElasticsearchType(IdProperty = nameof(ManagementES.ManagementID), Name = nameof(ManagementES))]
public class ManagementES : IndexItem
{
    public int ManagementID { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.AutoCompleteAnalyzer, Name = nameof(Name))]
    public string Name { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(Market))]
    public string Market { get; set; }

    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(State))]
    public string State { get; set; }
}