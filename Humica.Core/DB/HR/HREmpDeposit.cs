namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpDeposit")]
    public partial class HREmpDeposit
    {
        [Key]
        [StringLength(30)]
        public string DepositNum { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmployeeName { get; set; }

        [StringLength(20)]
        public string DeductionType { get; set; }

        [StringLength(120)]
        public string Deduction { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        public int? Period { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Deposit { get; set; }

        public decimal? TotalDeposit { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PayBack { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }
    }
}
