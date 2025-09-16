namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpSelfEvaluation")]
    public partial class HREmpSelfEvaluation
    {
        [Key]
        [StringLength(30)]
        public string EvaluationCode { get; set; }

        [StringLength(30)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime EvaluationDate { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
