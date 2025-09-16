namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_PO_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PONumber { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime PromisedDate { get; set; }

        [StringLength(140)]
        public string Requestor { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal Total { get; set; }

        [StringLength(150)]
        public string VendorName { get; set; }

        [StringLength(250)]
        public string VendorAddress { get; set; }

        [StringLength(15)]
        public string VendorPhone { get; set; }

        [StringLength(250)]
        public string VendorEmail { get; set; }

        [StringLength(15)]
        public string ShipPhone { get; set; }

        [StringLength(250)]
        public string ShipEmail { get; set; }

        [StringLength(20)]
        public string PaymentTerm { get; set; }

        [StringLength(20)]
        public string ShipTerm { get; set; }

        public string DescDetails { get; set; }
    }
}
