namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIPlanIndivItem")]
    public partial class HRKPIPlanIndivItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string KPIPlanCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string Measure { get; set; }

        [StringLength(20)]
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

    }
}
