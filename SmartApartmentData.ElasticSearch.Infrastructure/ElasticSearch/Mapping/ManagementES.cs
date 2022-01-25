using Nest;
using SmartApartmentData.ElasticSearch.Domain.Entities;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

[ElasticsearchType(IdProperty = nameof(ManagementES.ManagementID), RelationName = nameof(ManagementES))]
public class ManagementES : IndexItem
{
    public int ManagementID { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.AutoCompleteAnalyzer, Name = nameof(Name))]
    public string Name { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(Market))]
    public string Market { get; set; }

    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(State))]
    public string State { get; set; }

    internal static ManagementES Map(Management management)
    {
        return new ManagementES()
        {
            ManagementID = management.ManagementID,
            Name = management.Name,
            Market = management.Market,
            State = management.State
        };
    }
}