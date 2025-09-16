namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpAppraisal")]
    public partial class HREmpAppraisal
    {
        [Key]
        [StringLength(15)]
        public string ApprID { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string AppraisalType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AppraiserDate { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateJoin { get; set; }

        public int InYear { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PeriodFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PeriodTo { get; set; }

        [StringLength(50)]
        public string Result { get; set; }

        public decimal? TotalScore { get; set; }

        [StringLength(50)]
        public string AppraiserCode { get; set; }

        [StringLength(200)]
        public string AppraiserName { get; set; }

        [StringLength(200)]
        public string AppraiserPosition { get; set; }
        [StringLength(50)]
        public string AppraiserCode2 { get; set; }

        [StringLength(250)]
        public string AppraiserName2 { get; set; }
        [StringLength(50)]
        public string DirectedByCode { get; set; }

        [StringLength(200)]
        public string DirectedByName { get; set; }

        [StringLength(200)]
        public string AppraiserPosition2 { get; set; }

        [StringLength(200)]
        public string AppraiserComment { get; set; }

        [StringLength(200)]
        public string AppraiserComment2 { get; set; }

        public DateTime AppraiserStart { get; set; }
        public DateTime? AppraiserDeadline { get; set; }

        public DateTime? AppraiserDeadline2 { get; set; }

        [StringLength(50)]
        public string AppraiserNext { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(30)]
        public string ReStatus { get; set; }

        public decimal? Rating { get; set; }

        public int? ApprovedStep { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        [StringLength(50)]
        public string KPIType { get; set; }
        public DateTime? KPIExpectedDate { get; set; }
        public DateTime? KPIDeadline { get; set; }
        [StringLength(20)]
        public string KPIStatus { get; set; }
        [StringLength(250)]
        public string Branch { get; set; }
        public decimal? KPIScore { get; set; }

        public decimal? FinalScore { get; set; }
        [StringLength(10)]
        public string Grade { get; set; }
        [StringLength(20)]
        public string KPICategory { get; set; }
        [StringLength(20)]
        public string KPIReference { get; set; } 
    }
}
