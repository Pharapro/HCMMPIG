namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRResgierVehicle")]
    public partial class HRResgierVehicle
    {
        [Key]
        public int ID { get; set; }
		[StringLength(100)]
		public string AssetClass { get; set; }
		[StringLength(100)]
		public string AssetCode { get; set; }

		[StringLength(100)]
		public string MissionCode { get; set; }

        [StringLength(50)]
        public string VehicleType { get; set; }
		[StringLength(50)]
		public string Model { get; set; }
		[StringLength(250)]
		public string Horsepower { get; set; }

		public decimal? Rate { get; set; }

        [StringLength(200)]
        public string PlateNumber { get; set; }

        public int Year { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int Chair { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(250)]
		public string PathFile { get; set; }
		[StringLength(50)]
		public string ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
    }
}
