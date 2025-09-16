namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpPhischk")]
    public partial class HREmpPhischk
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime CHKDate { get; set; }

        [StringLength(10)]
        public string MedicalType { get; set; }

        [StringLength(10)]
        public string HospCHK { get; set; }

        [StringLength(10)]
        public string Result { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(15)]
        public string Createdby { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(15)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
