namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpContract")]
    public partial class HREmpContract
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string ConType { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [Column(TypeName = "image")]
        public byte[] Contract { get; set; }

        public string ContractPath { get; set; }

        [StringLength(10)]
        public string ContactText { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public double? PHoneAlw { get; set; }

        [StringLength(10)]
        public string Conterm { get; set; }

        public int? NUM { get; set; }
    }
}
