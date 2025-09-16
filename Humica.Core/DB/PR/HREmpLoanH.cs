namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpLoanH")]
    public partial class HREmpLoanH
    {
        [Key]
        [StringLength(30)]
        public string LoanID { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public int? Period { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal Amount { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
