namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAssetStaff")]
    public partial class HRAssetStaff
    {
		[StringLength(30)]
		public string ReferenceNum { get; set; }
		public int ID { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmployeName { get; set; }

        [StringLength(50)]
        public string AssetCode { get; set; }
        [StringLength(200)]
        public string AssetDescription { get; set; }
        [Column(TypeName = "date")]
        public DateTime AssignDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }
        public string Attachment { get; set; }
        public string Condition { get; set; }
		public DateTime? RequestDate { get; set; }
	}
}
