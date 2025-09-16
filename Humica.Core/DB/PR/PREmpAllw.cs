namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpAllw")]
    public partial class PREmpAllw
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string AllwCode { get; set; }

        [StringLength(100)]
        public string AllwDescription { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int? Status { get; set; }

        public DateTime? EnterDate { get; set; }

        [StringLength(20)]
        public string Hospital { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateClinic { get; set; }

        [StringLength(20)]
        public string InvoceNo { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }
        [StringLength(20)]
        public string Currency { get; set; }
    }
}
