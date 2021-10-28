#nullable enable
namespace mdr_services.Contracts.Responses.v1.ObjectListResponse
{
    public class ObjectDescriptionListResponse
    {
        public int? Id { get; set; }
        
        public string? DescriptionType { get; set; }
        
        public string? DescriptionLabel { get; set; }
        
        public string? DescriptionText { get; set; }
        
        public string? LangCode { get; set; }
    }
}