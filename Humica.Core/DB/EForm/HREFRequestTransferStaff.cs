namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFRequestTransferStaff")]
    public partial class HREFRequestTransferStaff
    {
        [Key]
        [StringLength(30)]
        public string DocNo { get; set; }

        [Required]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string Department { get; set; }

        [StringLength(10)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        public decimal? Increase { get; set; }

        public decimal? Salary { get; set; }

        public decimal? NewSalary { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        [StringLength(10)]
        public string CompanyCode { get; set; }

        [StringLength(70)]
        public string TransferCode { get; set; }

        [StringLength(10)]
        public string Section { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [StringLength(400)]
        public string Reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string GroupDEPT { get; set; }

        public string AttachFile { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectDate { get; set; }

        public string CurrentRespon { get; set; }

        public string Achievement { get; set; }

        public string NewRespon { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        public bool IsPro { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
