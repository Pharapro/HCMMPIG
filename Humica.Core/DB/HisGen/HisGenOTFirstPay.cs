namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisGenOTFirstPay")]
    public partial class HisGenOTFirstPay
    {
        [Key]
        public long TranNo { get; set; }

        public int PeriodID { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string PayStatus { get; set; }

        public DateTime? OTDate { get; set; }

        public decimal? BaseSalary { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? WorkHour { get; set; }

        [StringLength(10)]
        public string OTCode { get; set; }

        public decimal? OTHour { get; set; }

        [StringLength(100)]
        public string OTDesc { get; set; }

        [StringLength(100)]
        public string OTHDesc { get; set; }

        public decimal? OTRate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(100)]
        public string OTFormula { get; set; }

        [StringLength(1)]
        public string Measure { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(15)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? InMonth { get; set; }

        public int? InYear { get; set; }
    }
}
