namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_VIEW_EmpEvaluationForm
    {
        [Key]
        [StringLength(15)]
        public string EvaluateID { get; set; }
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(60)]
        public string AllName { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Level { get; set; }

        [StringLength(10)]
        public string EvaluateType { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EvaluateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EvalFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EvalToDate { get; set; }

        public int TotalScore { get; set; }

        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(10)]
        public string AssignedTo { get; set; }
    }
}