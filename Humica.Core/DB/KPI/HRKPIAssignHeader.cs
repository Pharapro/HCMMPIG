namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIAssignHeader")]
    public partial class HRKPIAssignHeader
    {
        [Key]
        [StringLength(50)]
        public string AssignCode { get; set; }

        [StringLength(50)]
        public string HandleCode { get; set; }

        [StringLength(150)]
        public string HandleName { get; set; }
        [StringLength(250)]
        public string Position { get; set; }

        [StringLength(50)]
        public string CriteriaType { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string KPIType { get; set; }

        [StringLength(250)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(50)]
        public string ReStatus { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExpectedDate { get; set; }
        public DateTime? Deadline { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CompletedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocumentDate { get; set; }

        [StringLength(50)]
        public string ReviewBy { get; set; }
        [StringLength(50)]
        public string DirectedByCode { get; set; }

        [StringLength(250)]
        public string DirectedByName { get; set; }
        public string PlanerCode { get; set; }
        [StringLength(200)]
        public string PlanerName { get; set; }
        [StringLength(250)]
        public string PlanerPosition { get; set; }

        [StringLength(50)]
        public string AcknowledgeBy { get; set; }

        [StringLength(50)]
        public string VerifyBy { get; set; }

        [StringLength(50)]
        public string ApprovedBy { get; set; }

        public decimal? TotalAchievement { get; set; }

        public decimal? KPIAverage { get; set; }

        public decimal? TotalScore { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public decimal? TotalWeight { get; set; }

        [StringLength(20)]
        public string KPICode { get; set; }

        public string PICPosition { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }
        public int Member { get; set; }
        [StringLength(50)]
        public string TeamName { get; set; }

        [StringLength(50)]
        public string ScreenID { get; set; }

        [StringLength(50)]
        public string AssignedBy { get; set; }
        public string DocRef { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        [StringLength(10)]
        public string KPICategory { get; set; }
        public decimal? FinalResult { get; set; }
        public string Grade { get; set; }
        public DateTime? AutoAccept { get; set; }
        public int InYear { get; set; } 
    }
}
