#nullable enable
namespace mdr_services.Contracts.Responses.v1.StudyListResponse
{
    public class StudyFeatureListResponse
    {
        public int? Id { get; set; }
        
        public string? FeatureType { get; set; }
        
        public string? FeatureValue { get; set; }
    }
}