#nullable enable
namespace mdr_services.Contracts.Responses.v1.ObjectListResponse
{
    public class ObjectRelationshipListResponse
    {
        public int? Id { get; set; }
        
        public string? RelationshipType { get; set; }
        
        public int? TargetObjectId { get; set; }
    }
}