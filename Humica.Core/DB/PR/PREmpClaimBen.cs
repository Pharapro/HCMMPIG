namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpClaimBen")]
    public partial class PREmpClaimBen
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(15)]
        public string BenType { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime PayDate { get; set; }

        public decimal Amount { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
