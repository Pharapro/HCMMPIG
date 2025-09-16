namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIAssignIndicator")]
    public partial class HRKPIAssignIndicator
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AssignCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Indicator { get; set; }
        [StringLength(250)]
        public string IndicatorType { get; set; }

        public decimal Weight { get; set; }

        public decimal? Acheivement { get; set; }
        public decimal? Score { get; set; }
    }
}
