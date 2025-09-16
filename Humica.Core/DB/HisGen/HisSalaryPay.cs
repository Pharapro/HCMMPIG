namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HisSalaryPay")]
    public partial class HisSalaryPay
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PeriodID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        public decimal? Salary { get; set; }

        public decimal? AllwTax { get; set; }

        public decimal? TaxableIncome { get; set; }

        public decimal? Tax { get; set; }

        public decimal? BenefitAfterTax { get; set; }

        public decimal? NetPay { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocumentDate { get; set; }
    }
}
