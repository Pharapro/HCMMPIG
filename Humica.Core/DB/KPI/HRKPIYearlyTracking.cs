namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIYearlyTracking")]
    public partial class HRKPIYearlyTracking
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

        public string KPI { get; set; }

        public decimal Weight { get; set; }

        public decimal? Target { get; set; }
        public string Method { get; set; }

        public decimal? JAN { get; set; }

        public decimal? FEB { get; set; }

        public decimal? MAR { get; set; }

        public decimal? APR { get; set; }

        public decimal? MAY { get; set; }

        public decimal? JUN { get; set; }

        public decimal? JUL { get; set; }

        public decimal? AUG { get; set; }

        public decimal? SEP { get; set; }

        public decimal? OCT { get; set; }

        public decimal? NOV { get; set; }

        public decimal? DECE { get; set; }

        public decimal? Actual { get; set; }
    }
}
