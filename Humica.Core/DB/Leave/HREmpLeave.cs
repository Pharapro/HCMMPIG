namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpLeave")]
    public partial class HREmpLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public int Increment { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public double? NoDay { get; set; }

        public double? NoRest { get; set; }

        public double? NoPH { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(200)]
        public string ResonToCancel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RejectDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        public DateTime? BackToWorkOn { get; set; }

        [StringLength(1000)]
        public string TaskHand_Over { get; set; }

        public bool? Urgent { get; set; }

        public bool TranType { get; set; }

        [StringLength(500)]
        public string Attachment { get; set; }
        [StringLength(20)]
        public string Units { get; set; }

        public bool? IsReqCancel { get; set; }
    }
}
