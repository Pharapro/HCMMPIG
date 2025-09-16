namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("HRDriver")]
	public partial class HRDriver
	{
		[Key]
		[StringLength(25)]
		public string DriverCode { get; set; }

		[StringLength(200)]
		public string DriverName { get; set; }
		public Boolean IsActive { get; set; }
	}
}