namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIEvaItem")]
    public partial class HRKPIEvaItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string KPIEvaCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(10)]
        public string Symbol { get; set; }
        [StringLength(20)]
        public string Indicator { get; set; }
        public string KPI { get; set; }
        public decimal? Target { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public decimal Weight { get; set; }

        public decimal? Actual { get; set; }

        public decimal? Score { get; set; }
        public decimal? ScoreEval { get; set; }
        public decimal? SubScore { get; set; }

        public decimal Variance { get; set; }
        public decimal TargetPer { get; set; }
        [StringLength(50)]
        public string Color { get; set; }
    }
}
