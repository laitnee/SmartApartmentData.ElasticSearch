using System.Text.Json.Serialization;

namespace SmartApartmentData.ElasticSearch.Domain.Entities;

public class Management : BaseEntity
{
    [JsonPropertyName("mgmtID")]
    public int ManagementID { get; set; }

    public string Name { get; set; }

    public string Market { get; set; }

    public string State { get; set; }
}