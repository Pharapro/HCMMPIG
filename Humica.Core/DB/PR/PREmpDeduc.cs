namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpDeduc")]
    public partial class PREmpDeduc
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string DedCode { get; set; }

        [StringLength(100)]
        public string DedDescription { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int? Status { get; set; }
        public string StatusAssetDed { get; set; }
        public int? AssetTransferID { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }
        [StringLength(20)]
        public string Currency { get; set; }
    }
}
