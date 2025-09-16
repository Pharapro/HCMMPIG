namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATShift")]
    public partial class ATShift
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public DateTime? CheckInBefore1 { get; set; }

        public DateTime? CheckInAfter1 { get; set; }

        public DateTime? CheckOutBefore1 { get; set; }

        public DateTime? CheckOutAfter1 { get; set; }

        public DateTime? CheckInBefore2 { get; set; }

        public DateTime? CheckOutAfter2 { get; set; }

        public DateTime? CheckOutBefore2 { get; set; }

        public DateTime? CheckIn1 { get; set; }

        public DateTime? CheckOut1 { get; set; }

        public bool? OverNight1 { get; set; }

        public bool SplitShift { get; set; }

        public DateTime? CheckIn2 { get; set; }

        public DateTime? CheckOut2 { get; set; }

        public bool? OverNight2 { get; set; }

        public bool BreakFast { get; set; }

        public bool Lunch { get; set; }

        public bool Dinner { get; set; }

        public bool NightMeal { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public DateTime? BreakStart { get; set; }

        public DateTime? BreakEnd { get; set; }

        public DateTime? CheckInAfter2 { get; set; }
		public bool IsActive { get; set; }
	}
}
