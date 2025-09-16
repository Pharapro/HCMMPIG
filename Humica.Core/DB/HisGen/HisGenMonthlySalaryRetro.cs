namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisGenMonthlySalaryRetro")]
    public partial class HisGenMonthlySalaryRetro
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public int? InYear { get; set; }

        public int? InMonth { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? WorkedDay { get; set; }

        public decimal? ActualWorkedDay { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Rate { get; set; }

        public decimal? EarnedAmount { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}
