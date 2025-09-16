namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("HR_AssetRegisterVehicle_View")]
	public partial class HR_AssetRegisterVehicle_View
	{
		[Key]
		public int ID { get; set; }

		[StringLength(50)]
		public string MissionCode { get; set; }

		[StringLength(50)]
		public string AssetClass { get; set; }

		[StringLength(50)]
		public string AssetCode { get; set; }

		[StringLength(50)]
		public string CarType { get; set; }

		[StringLength(150)]
		public string Model { get; set; }

		[StringLength(50)]
		public string Horsepower { get; set; }

		public decimal? Rate { get; set; }

		[StringLength(50)]
		public string PlateNumber { get; set; }

		public int? Year { get; set; }

		[StringLength(50)]
		public string Color { get; set; }

		[StringLength(500)]
		public string Description { get; set; }

		public int? Chair { get; set; }

		[StringLength(50)]
		public string Status { get; set; }

		[StringLength(50)]
		public string CreatedBy { get; set; }

		public DateTime? CreatedOn { get; set; }

		[StringLength(50)]
		public string ChangedBy { get; set; }

		public DateTime? ChangedOn { get; set; }

		[StringLength(500)]
		public string PathFile { get; set; }
	}
}
