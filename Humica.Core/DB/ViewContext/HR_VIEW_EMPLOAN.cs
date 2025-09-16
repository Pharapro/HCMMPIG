namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_VIEW_EMPLOAN
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string LoanID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime ToDate { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal LoanAmount { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal Amount { get; set; }
    }
}
