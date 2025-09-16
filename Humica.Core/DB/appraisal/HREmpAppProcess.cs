namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpAppProcess")]
    public partial class HREmpAppProcess
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(250)]
        public string EmployeeName { get; set; }

        [StringLength(50)]
        public string AppraisalType { get; set; }

        [StringLength(250)]
        public string Department { get; set; }

        [StringLength(250)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }
        public int InYear { get; set; }

        [Column(TypeName = "date")]
        public DateTime PeriodFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime PeriodTo { get; set; }

        public decimal TotalScore { get; set; }

        public decimal Rate { get; set; }

        public string Attachment { get; set; }
        [StringLength(50)]
        public string DocumentRef { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(20)]
        public string Grade { get; set; }

        public string Comment { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
