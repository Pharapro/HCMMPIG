namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFReqProbation")]
    public partial class HREFReqProbation
    {
        public int ID { get; set; }

        [StringLength(30)]
        public string DocumentNo { get; set; }

        [StringLength(10)]
        public string ProbationType { get; set; }

        [Required]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(150)]
        public string Department { get; set; }

        [StringLength(150)]
        public string Position { get; set; }

        [StringLength(150)]
        public string Section { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public decimal? Increase { get; set; }

        [Column(TypeName = "date")]
        public DateTime Probation { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        public decimal? OldSalary { get; set; }

        public decimal? NewSalary { get; set; }

        [StringLength(250)]
        public string AttachFile { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
