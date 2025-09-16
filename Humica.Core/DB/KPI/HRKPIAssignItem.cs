namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIAssignItem")]
    public partial class HRKPIAssignItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AssignCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string EmpCode { get; set; }
        [StringLength(20)]
        public string Indicator { get; set; }
        [StringLength(50)]
        public string Measure { get; set; }
        //[StringLength(250)]
        public string KPI { get; set; }

        public string ActionPlan { get; set; }
        public decimal? Target { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public decimal Weight { get; set; }

        public decimal? Actual { get; set; }

        public decimal? Score { get; set; }

        public string Remark { get; set; }

        public decimal? Acheivement { get; set; }

        [StringLength(20)]
        public string InputType { get; set; }
        [StringLength(10)]
        public string Symbol { get; set; }
        [StringLength(50)]
        public string Options { get; set; }
    }
}
