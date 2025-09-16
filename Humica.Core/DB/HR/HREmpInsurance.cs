namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpInsurance")]
    public partial class HREmpInsurance
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string InsurType { get; set; }

        [StringLength(10)]
        public string InsurComp { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string PolicyNo { get; set; }

        [StringLength(250)]
        public string Dependent { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [StringLength(20)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
