namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenFirstPay")]
    public partial class HISGenFirstPay
    {
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }
        public int PeriodID { get; set; }

        [StringLength(10)]
        public string TermStatus { get; set; }

        [StringLength(200)]
        public string TermRemark { get; set; }

        public DateTime? TermDate { get; set; }

        public int? INYear { get; set; }

        public int? INMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? WorkHour { get; set; }

        public decimal? ExchRate { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string EmpName { get; set; }

        [StringLength(10)]
        public string EmpType { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string LINE { get; set; }

        [StringLength(10)]
        public string CATE { get; set; }

        [StringLength(10)]
        public string Sect { get; set; }

        [StringLength(10)]
        public string POST { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(10)]
        public string JobGrade { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string ICNO { get; set; }

        [StringLength(10)]
        public string BankName { get; set; }

        [StringLength(140)]
        public string BankBranch { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        public DateTime? DateJoin { get; set; }

        public decimal? ActWorkDay { get; set; }

        public decimal? Salary { get; set; }

        public decimal? ADVPay { get; set; }

        public decimal? LOAN { get; set; }

        public decimal? OTAM { get; set; }

        public decimal? LeaveDeduct { get; set; }

        public decimal? DedAM { get; set; }

        public decimal? PayBack { get; set; }

        public decimal? GrossPay { get; set; }

        public decimal? NetWage { get; set; }

        public decimal? AmountKH { get; set; }

        [StringLength(15)]
        public string CareerCode { get; set; }

        public decimal? ActWorkHours { get; set; }

        public decimal? BankFee { get; set; }

        public decimal? Increased { get; set; }

        [StringLength(20)]
        public string PayStatus { get; set; }

        [StringLength(15)]
        public string USERGEN { get; set; }

        public DateTime? DATEGEN { get; set; }

        public decimal? CashBonus { get; set; }

        public decimal? NSBonus { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PayFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PayTo { get; set; }

        public int? Child { get; set; }

        public int? Spouse { get; set; }

        public decimal? UTAXCH { get; set; }

        public decimal? UTAXSP { get; set; }

        [StringLength(1)]
        public string TXPayType { get; set; }

        public decimal? TAXTYPE { get; set; }

        public decimal? AMTOBETAX { get; set; }

        public decimal? TaxAmountKH { get; set; }

        public decimal? TaxAmountUSD { get; set; }

        [Key]
        public int TrainNo { get; set; }

        public decimal? BasicSalary { get; set; }

        public decimal? TaxALWAM { get; set; }

        public decimal? UTaxALWAM { get; set; }

        public decimal AMFRINGTAX { get; set; }
        public decimal FRINGRATE { get; set; }
        public decimal FRINGAM { get; set; }

        public decimal? TaxDEDAM { get; set; }

        public decimal? UTaxDEDAM { get; set; }

        public decimal? TaxBONAM { get; set; }

        public decimal? UTaxBONAM { get; set; }
    }
}
