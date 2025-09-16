namespace Humica.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("HRBookingCarSchedule")]
	public partial class HRBookingCarSchedule
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int LineItem { get; set; }

		[Key]
		[Column(Order = 1)]
		//[StringLength(25)]
		public int BookingNo { get; set; }

		public bool HaveDriver { get; set; }

		[StringLength(25)]
		public string DriverCode { get; set; }

		[StringLength(25)]
		public string CarID { get; set; }

		[Column(TypeName = "date")]
		public DateTime BookingDate { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		[StringLength(20)]
		public string Status { get; set; }
	}
}