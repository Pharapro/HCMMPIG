namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATGenMeal")]
    public partial class ATGenMeal
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PayPeriodID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(60)]
        public string EmpName { get; set; }

        public int? BreakFast { get; set; }

        public decimal? BreakFastAM { get; set; }

        public decimal? BreakFastRate { get; set; }

        public int? Lunch { get; set; }

        public decimal? LunchAM { get; set; }

        public decimal? LunchRate { get; set; }

        public int? Dinner { get; set; }

        public decimal? DinnerAM { get; set; }

        public decimal? DinnerRate { get; set; }

        public decimal? Amount { get; set; }

        public decimal? FirstPayment { get; set; }

        public decimal? AmountDed { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public bool? IsTransfer { get; set; }
        public int? OTFood { get; set; }
        public decimal? OTFoodAM { get; set; }
        public decimal? OTFoodRate { get; set; }

    }
}
