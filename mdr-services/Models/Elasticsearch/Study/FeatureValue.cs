using Nest;

namespace mdr_services.Models.Elasticsearch.Study
{
    public class FeatureValue
    {
        [Number(Name = "id")]
        #nullable enable
        public int? Id { get; set; }
        
        [Text(Name = "name")]
        #nullable enable
        public string? Name { get; set; }
    }
}