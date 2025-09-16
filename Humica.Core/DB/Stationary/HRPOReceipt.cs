namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPOReceipt")]
    public partial class HRPOReceipt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [StringLength(25)]
        public string ReceiptNo { get; set; }

        [StringLength(5)]
        public string DocumentType { get; set; }

        [StringLength(30)]
        public string DocurementReference { get; set; }

        [StringLength(30)]
        public string VendorReference { get; set; }

        [StringLength(150)]
        public string VendorName { get; set; }

        public DateTime DocumentDate { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        public bool? IsReturn { get; set; }

        [StringLength(15)]
        public string ReStatus { get; set; }

        public decimal TotalQty { get; set; }

        public decimal OpenQty { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(180)]
        public string ReasonDescription { get; set; }

        [StringLength(5)]
        public string Reason { get; set; }

        [StringLength(150)]
        public string Comment { get; set; }

        [StringLength(5)]
        public string ReasonCancel { get; set; }

        [StringLength(180)]
        public string ReasonCacnelDescription { get; set; }

        [StringLength(30)]
        public string RequestedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExtectedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BillingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        public decimal NetAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalPercentageDiscount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal VatTotal { get; set; }

        [StringLength(5)]
        public string CurrencyCode { get; set; }

        public decimal CurrencyRate { get; set; }

        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(10)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public DateTime? LastAppovedDate { get; set; }

        [StringLength(10)]
        public string LastApprovedBy { get; set; }

        public decimal CompletePercentage { get; set; }
    }
}
