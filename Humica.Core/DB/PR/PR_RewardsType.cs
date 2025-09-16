namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_RewardsType
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ReCode { get; set; }

        [StringLength(100)]
        public string RewardType { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        public bool Tax { get; set; }

        public bool FTax { get; set; }

        public double Amount { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        public bool? IsBIMonthly { get; set; }
        public decimal BIPercentageAm { get; set; }
        [StringLength(20)]
        public string IncomeType { get; set; }
        [StringLength(20)]
        public string TaxType { get; set; }
    }
}
