namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEduc")]
    public partial class HREmpEduc
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }
        public string Department { get; set; }
        [StringLength(10)]
        public string EduType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(200)]
        public string EdcCenter { get; set; }

        [StringLength(200)]
        public string Major { get; set; }

        [StringLength(20)]
        public string Result { get; set; }

        [StringLength(150)]
        public string Generation { get; set; }

        public decimal? Cost { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public string AttachFile { get; set; }
    }
}
