namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAssetRegister")]
    public partial class HRAssetRegister
    {
        [Key]
        [StringLength(50)]
        public string AssetCode { get; set; }

        //[StringLength(50)]
        //public string ParentAsset { get; set; }

        [StringLength(20)]
        public string AssetClassCode { get; set; }

        [StringLength(20)]
        public string BranchCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Images { get; set; }

        [StringLength(20)]
        public string PropertyType { get; set; }

        [StringLength(20)]
        public string AssetTypeID { get; set; }

        public decimal? UsefulLifeYear { get; set; }

        public decimal Qty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReceiptDate { get; set; }

        public decimal AcquisitionCost { get; set; }

        [StringLength(30)]
        public string BuildingCD { get; set; }

        [StringLength(150)]
        public string Floor { get; set; }

        [StringLength(150)]
        public string Room { get; set; }

        [StringLength(30)]
        public string DepartmentCD { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(150)]
        public string TagNbr { get; set; }

        [StringLength(150)]
        public string Model { get; set; }

        [StringLength(150)]
        public string SerialNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WarrantyExpirationDate { get; set; }

        [StringLength(50)]
        public string Condition { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(10)]
        public string ChangedBy { get; set; }

        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public bool IsActive { get; set; }
        [StringLength(50)]
        public string AssetReference { get; set; }
        [StringLength(200)]
        public string StatusUse { get; set; }
		[StringLength(50)]
		public string OPNumber { get; set; }
		[StringLength(50)]
		public string ReceiptNumber { get; set; }
		[StringLength(50)]
		public string BuildingNumber { get; set; }
		public bool IsCombone { get; set; }
		[StringLength(100)]
		public string Location { get; set; }
		[StringLength(100)]
		public string ReferenceNum { get; set; }
	}
}
