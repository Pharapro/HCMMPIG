namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIMonthlyTracking")]
    public partial class HRKPIMonthlyTracking
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AssignCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Indicator { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ItemCode { get; set; }
        [Key]
        [Column(Order = 3)]
        public int InMonth { get; set; }

        public string KPI { get; set; }

        public decimal Weight { get; set; }

        public decimal? Target { get; set; }
        public string Method { get; set; }
        public int? InYear { get; set; }
        public decimal? D1 { get; set; }

        public decimal? D2 { get; set; }

        public decimal? D3 { get; set; }

        public decimal? D4 { get; set; }

        public decimal? D5 { get; set; }

        public decimal? D6 { get; set; }

        public decimal? D7 { get; set; }

        public decimal? D8 { get; set; }

        public decimal? D9 { get; set; }

        public decimal? D10 { get; set; }

        public decimal? D11 { get; set; }

        public decimal? D12 { get; set; }
        public decimal? D13 { get; set; }
        public decimal? D14 { get; set; }
        public decimal? D15 { get; set; }
        public decimal? D16 { get; set; }
        public decimal? D17 { get; set; }
        public decimal? D18 { get; set; }
        public decimal? D19 { get; set; }
        public decimal? D20 { get; set; }
        public decimal? D21 { get; set; }
        public decimal? D22 { get; set; }
        public decimal? D23 { get; set; }
        public decimal? D24 { get; set; }
        public decimal? D25 { get; set; }
        public decimal? D26 { get; set; }
        public decimal? D27 { get; set; }
        public decimal? D28 { get; set; }
        public decimal? D29 { get; set; }
        public decimal? D30 { get; set; }
        public decimal? D31 { get; set; }

        public decimal? Actual { get; set; }
    }
}
