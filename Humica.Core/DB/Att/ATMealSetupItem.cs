namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATMealSetupItem")]
    public partial class ATMealSetupItem
    {
        [Key]
        [StringLength(30)]
        public string LevelCode { get; set; }

        public decimal? BreakFast { get; set; }

        public decimal? Lunch { get; set; }
        public decimal? Dinner { get; set; }

        public decimal? NightMeal { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
