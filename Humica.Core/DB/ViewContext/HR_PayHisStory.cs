namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_PayHisStory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [StringLength(100)]
        public string BranchDes { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public decimal? Salary { get; set; }

        [StringLength(100)]
        public string RewardDesc { get; set; }

        [StringLength(11)]
        public string RewardType { get; set; }

        public decimal? Reward { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal StaffPensionFundAmount { get; set; }

        public decimal? GrossPay { get; set; }

        public decimal? ADVPay { get; set; }

        public decimal? LOAN { get; set; }

        public decimal? NetWage { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(100)]
        public string MonthDesc { get; set; }
    }
}
