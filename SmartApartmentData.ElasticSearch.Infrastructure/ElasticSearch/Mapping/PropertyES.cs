using Nest;
using SmartApartmentData.ElasticSearch.Domain.Entities;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

[ElasticsearchType(IdProperty = nameof(PropertyES.PropertyID), Name = nameof(PropertyES))]
public class PropertyES : IndexItem
{
    public int PropertyID { get; set; }
    [Text(Analyzer = ElasticSearchConstants.AutoCompleteAnalyzer, Name = nameof(Name))]
    public string Name { get; set; }
    [Text(Analyzer = ElasticSearchConstants.AutoCompleteAnalyzer, Name = nameof(FormerName))]
    public string FormerName { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.AutoCompleteAnalyzer, Name = nameof(FormerName))]
    public string StreetAddress { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(City))]
    public string City { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(Market))]
    public string Market { get; set; }
    
    [Text(Analyzer = ElasticSearchConstants.KeywordAnalyzer, Name = nameof(State))]
    public string State { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }

    
    internal static PropertyES Map(Property property)
    {
        return new PropertyES()
        {
            PropertyID = property.PropertyID,
            Name = property.Name,
            FormerName = property.FormerName,
            StreetAddress = property.StreetAddress,
            City = property.City,
            Market = property.Market,
            State = property.State,
            Lat = property.Lat,
            Lng = property.Lng
        };
    }
}