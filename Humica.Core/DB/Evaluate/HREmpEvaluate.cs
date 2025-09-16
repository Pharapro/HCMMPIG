namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEvaluate")]
    public partial class HREmpEvaluate
    {
        [Key]
        [StringLength(15)]
        public string EvaluateID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(30)]
        public string EmpName { get; set; }

        [StringLength(10)]
        public string EvaluateType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EvaluateDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EvalFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EvalToDate { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        public DateTime? DateOfHire { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(1)]
        public string Result { get; set; }

        public int TotalScore { get; set; }

        [StringLength(15)]
        public string AssignedTo { get; set; }

        [StringLength(200)]
        public string AssignedPosition { get; set; }
        public string Strengths { get; set; }
        public string Weakness { get; set; }
        public string TrainingProgram { get; set; }
        public string Comments { get; set; }
        public string ActionPlan { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(30)]
        public string ChangedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ChengedOn { get; set; }
    }
}