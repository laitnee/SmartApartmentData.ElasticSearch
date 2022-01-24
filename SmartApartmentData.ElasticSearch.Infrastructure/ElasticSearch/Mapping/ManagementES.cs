using Nest;

namespace SmartApartmentData.ElasticSearch.Infrastructure.ElasticSearch.Mapping;

[ElasticsearchType(IdProperty = nameof(ManagementES.ManagementID), Name = nameof(ManagementES))]
public class ManagementES
{
    public int ManagementID { get; set; }

    public string Name { get; set; }

    public string Market { get; set; }

    public string State { get; set; }
}