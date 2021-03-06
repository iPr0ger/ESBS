using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable
namespace MdmService.Models.Study
{
    [Table("study_features", Schema = "mdr")]
    public class StudyFeature
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("sd_sid")]
        public string? SdSid { get; set; }
        
        [Column("feature_type_id")]
        public int? FeatureTypeId { get; set; }

        [Column("feature_value_id")]
        public int? FeatureValueId { get; set; }
        
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
    }
}