namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIPlanItem")]
    public partial class HRKPIPlanItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string KPIPlanCode { get; set; }

        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string Measure { get; set; }

        [StringLength(20)]
        [Key]
        [Column(Order = 1)]
        public string Indicator { get; set; }

        [StringLength(250)]
        public string KPI { get; set; }

        public string ActionPlan { get; set; }

        public decimal? Target { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public string Remark { get; set; }
        public string Symbol { get; set; }
        public int? SubTotalEmp { get; set; }
        public decimal? Actual { get; set; }
        public decimal? Achievement { get; set; }
        public decimal? TargetValue { get; set; }
    }
}