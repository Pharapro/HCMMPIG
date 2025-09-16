namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFEmpResign")]
    public partial class HREFEmpResign
    {
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string EmpCode { get; set; }

        public DateTime DocDate { get; set; }

        [Required]
        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(250)]
        public string ReasonCEO { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(100)]
        public string ChangedBy { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [StringLength(150)]
        public string Department { get; set; }

        [StringLength(150)]
        public string Position { get; set; }

        [StringLength(150)]
        public string Section { get; set; }

        [StringLength(30)]
        public string DocumentNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime ResigneDate { get; set; }

        [StringLength(150)]
        public string Branch { get; set; }

        [StringLength(100)]
        public string ResignType { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string AttachFile { get; set; }
    }
}
