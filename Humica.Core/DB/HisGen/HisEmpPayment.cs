namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisEmpPayment")]
    public partial class HisEmpPayment
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string CompanyCode { get; set; }

        public int? PeriodID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(10)]
        public string IncomeType { get; set; }

        [StringLength(50)]
        public string PayType { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public string Description { get; set; }

        public decimal? Amount { get; set; }
        [StringLength(20)]
        public string TaxType { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }
        public bool? IsSalary { get; set; }
    }
}
