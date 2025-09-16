namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisGenLeaveDFirstPay")]
    public partial class HisGenLeaveDFirstPay
    {
        [Key]
        public long TranNo { get; set; }

        public int PeriodID { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        [StringLength(100)]
        public string LeaveDesc { get; set; }

        [StringLength(100)]
        public string LeaveOthDesc { get; set; }

        public DateTime? LeaveDate { get; set; }

        [StringLength(15)]
        public string ForMular { get; set; }

        public decimal? BaseSalary { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? WorkHour { get; set; }

        [StringLength(1)]
        public string Measure { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Amount { get; set; }

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
