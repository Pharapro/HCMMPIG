namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HREFBenefit")]
    public partial class HREFBenefit
    {
        [Key]
        [StringLength(30)]
        public string DocNo { get; set; }

        [Required]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [Required]
        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(10)]
        public string BenefitType { get; set; }

        public decimal OldAmount { get; set; }

        public decimal Increase { get; set; }

        public decimal NewAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        [StringLength(200)]
        public string AttachFile { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
