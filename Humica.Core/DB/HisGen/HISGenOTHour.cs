namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenOTHour")]
    public partial class HISGenOTHour
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        public long TranNo { get; set; }

        public long? INYear { get; set; }

        public int? INMonth { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

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

        public int PeriodID { get; set; }
    }
}
