namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_POReceipt_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string ReceiptNo { get; set; }

        [StringLength(30)]
        public string DocurementReference { get; set; }

        [StringLength(150)]
        public string VendorName { get; set; }

        [StringLength(30)]
        public string VendorReference { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string Status { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal TotalQty { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal OpenQty { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal NetAmount { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal TotalAmount { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal TotalDiscount { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(5)]
        public string CurrencyCode { get; set; }

        [StringLength(140)]
        public string Requestor { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        public string DescDetails { get; set; }
    }
}
