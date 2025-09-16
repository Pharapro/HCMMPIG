namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("HRAssetNoneStaffRecord")]
	public partial class HRAssetNoneStaffRecord
	{
		[Key]
		[StringLength(15)]
		public string HandlerCode { get; set; }

		[Required]
		[StringLength(50)]
		public string HandlerName { get; set; }

		[StringLength(100)]
		public string Company { get; set; }

		[StringLength(100)]
		public string Position { get; set; }

		[StringLength(50)]
		public string PhoneNumber { get; set; }
		[StringLength(100)]
		public string Commune { get; set; }
		[StringLength(100)]
		public string District { get; set; }
		[StringLength(100)]
		public string Province { get; set; }
	}
}
