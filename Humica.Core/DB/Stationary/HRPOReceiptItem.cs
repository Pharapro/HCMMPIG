namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPOReceiptItem")]
    public partial class HRPOReceiptItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string ReceiptNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(30)]
        public string DocumentReference { get; set; }

        [StringLength(150)]
        public string LineDescription { get; set; }

        public int? LineReference { get; set; }

        [StringLength(35)]
        public string ItemCode { get; set; }

        [StringLength(150)]
        public string ItemDescription { get; set; }

        [StringLength(5)]
        public string BaseUnit { get; set; }

        [StringLength(5)]
        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal OpenQuantity { get; set; }

        public decimal ReceiptQuantity { get; set; }

        public decimal CurrentStock { get; set; }

        public decimal TotalExpectCost { get; set; }

        public decimal QuantityReply { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PromisedDate { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public decimal NetAmount { get; set; }

        public decimal OpenAmount { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public decimal PercentageDiscount { get; set; }

        public decimal VatExemtion { get; set; }

        public decimal VatTaxable { get; set; }

        public decimal VatTotal { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }
    }
}
