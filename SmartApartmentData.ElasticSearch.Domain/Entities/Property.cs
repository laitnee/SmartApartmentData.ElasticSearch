namespace SmartApartmentData.ElasticSearch.Domain.Entities;

public class Property
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
}