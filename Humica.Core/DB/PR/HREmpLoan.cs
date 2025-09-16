namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpLoan")]
    public partial class HREmpLoan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string LoanID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime LoanDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime PayMonth { get; set; }

        public decimal LoanAm { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public decimal BeginingBalance { get; set; }

        public decimal EndingBalance { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }
}
