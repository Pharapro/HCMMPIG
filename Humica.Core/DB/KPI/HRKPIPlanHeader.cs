namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIPlanHeader")]
    public partial class HRKPIPlanHeader
    {
        [Key]
        [StringLength(30)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Category { get; set; }
        [StringLength(30)]
        public string KPICategory { get; set; }
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
        public DateTime? ExpectedDate { get; set; }
        public DateTime? Dateline { get; set; }
        public decimal? KPIAverage { get; set; }

        public decimal? TotalWeight { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
        public int? TotalEmp { get; set; }
        public int? TotalByIndividual { get; set; }
        public int? TotalEmpTeam { get; set; }
        public int? InYear { get; set; }
    }
}
