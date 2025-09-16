namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApprSelfAssessment")]
    public partial class HRApprSelfAssessment
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PortalCode { get; set; }
        [StringLength(200)]
        public string PortalName { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string QuestionCode { get; set; }
        public string Description1 { get; set; }

        public bool IsQCM { get; set; }

        public string Description2 { get; set; }

        public int InOrder { get; set; }
        public bool? IsRequired { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ChangedOn { get; set; }
    }
}
