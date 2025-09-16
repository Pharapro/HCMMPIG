namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class HR_AssetStaff_View
	{
		public int ID { get; set; }
		public string ReferenceNum { get; set; }

		[StringLength(10)]
		public string EmpCode { get; set; }

		[StringLength(100)]
		public string EmployeName { get; set; }

		[StringLength(50)]
		public string AssetCode { get; set; }

		[StringLength(200)]
		public string AssetDescription { get; set; }
		[StringLength(150)]
		public string SerialNumber { get; set; }

		[StringLength(150)]
		public string Model { get; set; }

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
		[Column(TypeName = "date")]
		public DateTime? RequestDate { get; set; }
	}
}
