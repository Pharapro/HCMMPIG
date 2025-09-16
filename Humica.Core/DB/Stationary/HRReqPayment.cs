namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRReqPayment")]
    public partial class HRReqPayment
    {
        [Key]
        [StringLength(50)]
        public string RPNumber { get; set; }

        [StringLength(50)]
        public string Branch { get; set; }

        [StringLength(20)]
        public string DocumentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(20)]
        public string Requestor { get; set; }

        [StringLength(20)]
        public string BeneName { get; set; }

        [StringLength(20)]
        public string BeneAcc { get; set; }

        [StringLength(20)]
        public string BeneBank { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        public bool Advance { get; set; }

        public bool SettlementAdvance { get; set; }

        public bool PayymenyStaff { get; set; }

        public bool PaymentVendor { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public decimal TotalAmountReq { get; set; }

        public decimal TotalAmountRev { get; set; }

        public decimal? AdvanceAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AdvanceDate { get; set; }

        public decimal? DueToEmployee { get; set; }

        public decimal? DueToCompany { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(100)]
        public string DocumentReference { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeliveryDate { get; set; }
    }
}
