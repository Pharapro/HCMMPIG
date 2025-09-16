namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIEvaluation")]
    public partial class HRKPIEvaluation
    {
        [Key]
        [StringLength(50)]
        public string KPIEvaCode { get; set; }

        [StringLength(50)]
        public string HandleCode { get; set; }

        [StringLength(150)]
        public string HandleName { get; set; }
        [StringLength(250)]
        public string Position { get; set; }

        [StringLength(50)]
        public string CriteriaType { get; set; }

        [StringLength(50)]
        public string KPIType { get; set; }

        [StringLength(250)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExpectedDate { get; set; }
        public DateTime? Deadline { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocumentDate { get; set; }

        [StringLength(50)]
        public string DirectedByCode { get; set; }

        [StringLength(250)]
        public string DirectedByName { get; set; }
        [StringLength(250)]
        public string DirectedPosition { get; set; }

        public decimal? TotalAchievement { get; set; }

        public decimal? KPIAverage { get; set; }

        public decimal? TotalScore { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public decimal? TotalWeight { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }
        public int Member { get; set; }
        [StringLength(50)]
        public string TeamName { get; set; }

        [StringLength(50)]
        public string AssignedBy { get; set; }
        public string DocRef { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
        [StringLength(10)]
        public string KPICategory { get; set; }
        public decimal? FinalResult { get; set; }
        public decimal? FinalResultPA { get; set; }
        public string Grade { get; set; }
        public decimal? TotalScoreEval { get; set; }
        public int? InYear { get; set; }
    }
}
