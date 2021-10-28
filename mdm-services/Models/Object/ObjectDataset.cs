using System;
using LinqToDB.Mapping;

#nullable enable
namespace mdm_services.Models.Object
{
    [Table("object_datasets", Schema = "mdr")]
    public class ObjectDataset
    {
        [PrimaryKey, Identity]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("sd_oid")]
        public string? SdOid { get; set; }
        
        [Column("record_keys_type_id")]
        public int? RecordKeysTypeId { get; set; }
        
        [Column("record_keys_details")]
        public string? RecordKeysDetails { get; set; }
        
        [Column("deident_type_id")]
        public int? DeidentTypeId { get; set; }
        
        [Column("deident_direct")]
        public bool? DeidentDirect { get; set; }
        
        [Column("deident_hipaa")]
        public bool? DeidentHipaa { get; set; }
        
        [Column("deident_dates")]
        public bool? DeidentDates { get; set; }
        
        [Column("deident_noarr")]
        public bool? DeidentNonarr { get; set; }
        
        [Column("deident_kanon")]
        public bool? DeidentKanon { get; set; }
        
        [Column("deident_details")]
        public string? DeidentDetails { get; set; }
        
        [Column("consent_type_id")]
        public int? ConsentTypeId { get; set; }
        
        [Column("consent_noncommercial")]
        public bool? ConsentNoncommercial { get; set; }
        
        [Column("consent_geog_restrict")]
        public bool? ConsentGeogRestrict { get; set; }
        
        [Column("consent_research_type")]
        public bool? ConsentResearchType { get; set; }
        
        [Column("consent_genetic_only")]
        public bool? ConsentGeneticOnly { get; set; }
        
        [Column("consent_no_methods")]
        public bool? ConsentNoMethods { get; set; }
        
        [Column("consent_details")]
        public string? ConsentDetails { get; set; }
        
        [Column("created_od")]
        public DateTime? CreatedOn { get; set; }

    }
}