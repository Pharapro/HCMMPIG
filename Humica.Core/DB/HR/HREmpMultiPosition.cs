namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("HREmpMultiPosition")]
	public partial class HREmpMultiPosition
	{
		[Key, Column(Order = 0)]
		[StringLength(15)]
		public string EmpCode { get; set; }

		[Key, Column(Order = 1)]
		[StringLength(20)]
		public string PositionCode { get; set; }

		[StringLength(500)]
		public string PositionDescription { get; set; }
		public int? PositionID { get; set; }
		public int? PositionStructureID { get; set; }

		[Required]
		public DateTime EffectiveDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}
