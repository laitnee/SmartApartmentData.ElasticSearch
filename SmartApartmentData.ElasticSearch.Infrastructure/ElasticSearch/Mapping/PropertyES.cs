using Nest;
using SmartApartmentData.ElasticSearch.Domain.Entities;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

[ElasticsearchType(IdProperty = nameof(PropertyES.PropertyID), Name = nameof(PropertyES))]
public class PropertyES
{
    public int PropertyID { get; set; }
    public string Name { get; set; }
    public string FormerName { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Market { get; set; }
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