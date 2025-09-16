namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATMealSetup")]
    public partial class ATMealSetup
    {
        [Key]
        public int TranNo { get; set; }

        public string MealAllowanceType { get; set; }
        public DateTime? BreakfastFrom { get; set; }

        public DateTime? BreakfastTo { get; set; }

        public DateTime? LunchFrom { get; set; }

        public DateTime? LunchTo { get; set; }

        public DateTime? DinnerFrom { get; set; }

        public DateTime? DinnerTo { get; set; }
        public DateTime? NightFrom { get; set; }

        public DateTime? NightTo { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string OTAdj { get; set; }

        [StringLength(10)]
        public string WDAdj { get; set; }
    }
}
