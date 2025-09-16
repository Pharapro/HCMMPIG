namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPOHeader")]
    public partial class HRPOHeader
    {
        [Key]
        [StringLength(20)]
        public string PONumber { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(20)]
        public string DocumentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime PromisedDate { get; set; }

        [StringLength(20)]
        public string Requestor { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public decimal Total { get; set; }

        [StringLength(150)]
        public string VendorName { get; set; }

        [StringLength(250)]
        public string VendorAddress { get; set; }

        [StringLength(15)]
        public string VendorPhone { get; set; }

        [StringLength(250)]
        public string VendorEmail { get; set; }

        [StringLength(150)]
        public string ShipName { get; set; }

        [StringLength(250)]
        public string ShipAddress { get; set; }

        [StringLength(15)]
        public string ShipPhone { get; set; }

        [StringLength(250)]
        public string ShipEmail { get; set; }

        [StringLength(20)]
        public string PaymentTerm { get; set; }

        [StringLength(20)]
        public string ShipTerm { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(100)]
        public string DocumentReference { get; set; }

        [StringLength(500)]
        public string AttachFile { get; set; }

        [StringLength(20)]
        public string ContactPerson { get; set; }
    }
}
