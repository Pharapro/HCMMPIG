namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEvaluateProcess")]
    public partial class HREmpEvaluateProcess
    {
        [Key]
        [StringLength(15)]
        public string EvaluateID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(70)]
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

        [StringLength(1)]
        public string Result { get; set; }

        public int TotalScore { get; set; }
        public decimal Increase { get; set; }
        [StringLength(10)]
        public string AssignedTo { get; set; }
        [StringLength(200)]
        public string AssignedPosition { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(2000)]
        public string Comment { get; set; }
        [StringLength(250)]
        public string Attachment { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}