using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MdrService.Models.Lup
{
    [Table("study_types", Schema = "lup")]
    public class StudyType
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("list_order")]
        public int ListOrder { get; set; }
        
        [Column("source")]
        public string Source { get; set; }
        
        [Column("date_added", TypeName = "Date")]
        public DateTime DateAdded { get; set; }
    }
}