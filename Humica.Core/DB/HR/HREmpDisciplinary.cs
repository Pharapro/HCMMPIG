namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpDisciplinary")]
    public partial class HREmpDisciplinary
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string AllName { get; set; }

        [StringLength(30)]
        public string DiscType { get; set; }

        [StringLength(10)]
        public string DiscAction { get; set; }

        [Column(TypeName = "date")]
        public DateTime TranDate { get; set; }

        public string Remark { get; set; }

        public string Reference { get; set; }
        public string AttachPath { get; set; }

        [Column(TypeName = "image")]
        public byte[] AttachDoc { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public string Consequence { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(100)]
        public string DocumentNo { get; set; }
        [StringLength(20)]
        public string RequestBy { get; set; }
        public string DocType { get; set; }
    }
}
