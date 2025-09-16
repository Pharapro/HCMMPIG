namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAPPIncSalary")]
    public partial class HRAPPIncSalary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(50)]
        public string Requestor { get; set; }

        [StringLength(250)]
        public string RequestorName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }

        public int TotalEmployee { get; set; }
        public decimal TotalIncrease { get; set; }
        [StringLength(100)]
        public string CareerType { get; set; }
        public string Remark { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
