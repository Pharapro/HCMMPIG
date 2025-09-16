namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIPlanIndividual")]
    public partial class HRKPIPlanIndividual
    {
        [Key]
        [StringLength(30)]
        public string Code { get; set; }

        [StringLength(30)]
        public string PlanRef { get; set; }
        [StringLength(50)]
        public string CriteriaType { get; set; }
        [StringLength(50)]
        public string KPIType { get; set; }

        [StringLength(150)]
        public string CriteriaName { get; set; }
        public string Description { get; set; }
        public DateTime DocumentDate { get; set; }
        [StringLength(20)]
        public string PlanerCode { get; set; }
        [StringLength(200)]
        public string PlanerName { get; set; }
        [StringLength(250)]
        public string PlanerPosition { get; set; }
        [StringLength(20)]
        public string PICCode { get; set; }
        [StringLength(200)]
        public string PICName { get; set; }
        [StringLength(250)]
        public string PICPosition { get; set; }
        public decimal? KPIAverage { get; set; }
        [StringLength(50)]
        public string DirectedByCode { get; set; }
        [StringLength(250)]
        public string DirectedByName { get; set; }
        public decimal? TotalWeight { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? Deadline { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
        [StringLength(20)]
        public string ReStatus { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
