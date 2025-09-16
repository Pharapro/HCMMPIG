namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRAssetTransfer")]
    public partial class HRAssetTransfer
    {
		public int ID { get; set; }
		[StringLength(50)]
		public string TransferType { get; set; }
		[StringLength(50)]
		public string ReferenceNum { get; set; }

		[Required]
        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmployeName { get; set; }

        [Required]
        [StringLength(50)]
        public string AssetCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime AssignDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public bool IsDedSalary { get; set; }

        [StringLength(200)]
        public string AssetDescription { get; set; }

        public string Attachment { get; set; }

        [StringLength(50)]
        public string Condition { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Period { get; set; }
        public decimal? DedAmount { get; set; }
        public decimal? Amount { get; set; }
        public string DedType { get; set; }
		[StringLength(50)]
		public string Receiver { get; set; }
		public string Location { get; set; }
		public DateTime? RemarkDate { get; set; }
		[StringLength(250)]
		public string RemarkDateDes { get; set; }
	}
}
