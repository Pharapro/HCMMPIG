namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIActivitiesItem")]
    public partial class HRKPIActivitiesItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AVTCode { get; set; }

        [StringLength(50)]
        public string KPICode { get; set; }

        [StringLength(50)]
        public string KPIElement { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Matric { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        public decimal? Value { get; set; }

        [StringLength(50)]
        public string Actual { get; set; }

        public decimal? Difference { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        public decimal? Acheivement { get; set; }

        public decimal? AcheivementByEachKPI { get; set; }

        [StringLength(20)]
        public string InputType { get; set; }

        public decimal? TargetAmount { get; set; }

        public decimal? TargetPercentag { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TargentMonth { get; set; }

        public decimal? ActualAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ActualMonth { get; set; }
    }
}
