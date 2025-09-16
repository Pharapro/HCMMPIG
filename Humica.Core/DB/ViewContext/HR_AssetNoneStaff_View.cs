namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class HR_AssetNoneStaff_View
	{
		public int ID { get; set; }

		[StringLength(50)]
		public string ReferenceNum { get; set; }

		[StringLength(10)]
		public string HandlerCode { get; set; }

		[StringLength(100)]
		public string HandlerName { get; set; }

		[StringLength(100)]
		public string Position { get; set; }

		[StringLength(20)]
		public string PhoneNumber { get; set; }

		[StringLength(50)]
		public string SerialNumber { get; set; }

		[StringLength(100)]
		public string Model { get; set; }

		[StringLength(50)]
		public string AssetCode { get; set; }

		[StringLength(200)]
		public string AssetDescription { get; set; }

		[Column(TypeName = "date")]
		public DateTime? AssignDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? ReturnDate { get; set; }

		[StringLength(50)]
		public string Status { get; set; }

		[StringLength(50)]
		public string Condition { get; set; }

		[StringLength(500)]
		public string Remark { get; set; }

		[StringLength(500)]
		public string Attachment { get; set; }

		[StringLength(100)]
		public string Company { get; set; }

		[Column(TypeName = "date")]
		public DateTime? RemarkDate { get; set; }

		[StringLength(200)]
		public string RemarkDateDes { get; set; }
		[StringLength(100)]
		public string Commune { get; set; }
		[StringLength(100)]
		public string District { get; set; }
		[StringLength(100)]
		public string Province { get; set; }
	}
}
