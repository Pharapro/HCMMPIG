namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenSalary")]
    public partial class HISGenSalary
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(10)]
        public string TermStatus { get; set; }

        [StringLength(200)]
        public string TermRemark { get; set; }

        public DateTime? TermDate { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? WorkHour { get; set; }

        public decimal? ExchRate { get; set; }

        [StringLength(10)]
        public string CostCenter { get; set; }

        [Key]
        [Column(Order = 2)]
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
        public string Office { get; set; }

        [StringLength(10)]
        public string LINE { get; set; }

        [StringLength(10)]
        public string CATE { get; set; }

        [StringLength(10)]
        public string Sect { get; set; }
        [StringLength(10)]
        public string Groups { get; set; }
        [StringLength(10)]
        public string POST { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(10)]
        public string JobGrade { get; set; }

        [StringLength(10)]
        public string PersGrade { get; set; }

        [StringLength(10)]
        public string HomeFunction { get; set; }

        [StringLength(10)]
        public string Functions { get; set; }

        [StringLength(10)]
        public string SubFunction { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string ICNO { get; set; }

        [StringLength(50)]
        public string EPF { get; set; }

        [StringLength(100)]
        public string SOCSO { get; set; }

        [StringLength(10)]
        public string TaxNo { get; set; }

        [StringLength(10)]
        public string BankName { get; set; }

        [StringLength(140)]
        public string BankBranch { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        public int? Spouse { get; set; }

        public int? Child { get; set; }

        public DateTime? DateJoin { get; set; }

        public decimal? ActWorkDay { get; set; }

        public decimal? Salary { get; set; }

        public decimal? ADVPay { get; set; }

        public decimal? LOAN { get; set; }

        public decimal? OTAM { get; set; }

        public decimal? ShiftPay { get; set; }

        public decimal? PayBack { get; set; }

        public decimal? LeaveDeduct { get; set; }

        public decimal? TIP { get; set; }

        public decimal? TaxALWAM { get; set; }

        public decimal? UTAXALWAM { get; set; }

        public decimal? TAXDEDAM { get; set; }

        public decimal? UTAXDEDAM { get; set; }

        public decimal? TAXBONAM { get; set; }

        public decimal? UTAXBONAM { get; set; }

        public decimal? UTAXSP { get; set; }

        public decimal? UTAXCH { get; set; }

        public decimal? AMTOBETAX { get; set; }

        public decimal? AMUNTAX { get; set; }

        public decimal? TAXAM { get; set; }

        public decimal? GrossNoTIP { get; set; }

        public decimal? GrossPay { get; set; }

        public decimal? NetNoTIP { get; set; }

        public decimal? NetWage { get; set; }

        public decimal? SOSEC { get; set; }

        public int? LCK { get; set; }

        [StringLength(15)]
        public string USERGEN { get; set; }

        public DateTime? DATEGEN { get; set; }

        public decimal? TAXRATE { get; set; }

        public decimal? TAXTYPE { get; set; }

        public decimal? NWAM { get; set; }

        public decimal? AVGGrSOSC { get; set; }

        public decimal? NSSFRate { get; set; }

        public decimal? AMFRINGTAX { get; set; }

        public decimal? FRINGRATE { get; set; }

        public decimal? FRINGAM { get; set; }

        [StringLength(50)]
        public string ESalary { get; set; }

        [StringLength(50)]
        public string EAmtoBrTax { get; set; }

        [StringLength(50)]
        public string EGrossNoTIP { get; set; }

        [StringLength(50)]
        public string EGrossPay { get; set; }

        [StringLength(50)]
        public string ENetNoTIP { get; set; }

        [StringLength(50)]
        public string ENetWage { get; set; }
        public decimal? SeniorityTaxable { get; set; }

        [StringLength(120)]
        public string SMSTele { get; set; }

        [StringLength(50)]
        public string RECKEY { get; set; }

        [StringLength(10)]
        public string TXPayType { get; set; }

        [StringLength(15)]
        public string CareerCode { get; set; }

        public decimal? StaffHealth { get; set; }

        public decimal? CompHealth { get; set; }

        public decimal? ActWorkHours { get; set; }

        public decimal BankFee { get; set; }

        public decimal AmtoBeTaxKH { get; set; }

        public decimal TaxKH { get; set; }

        public decimal? Increased { get; set; }

        public decimal? SFT_Salary { get; set; }

        public decimal? SFT_GrossPay { get; set; }

        public decimal? SFT_AmToBeTax { get; set; }

        public decimal? SFT_Tax { get; set; }

        public decimal? SFT_TaxRate { get; set; }

        public decimal? SFT_NetPay { get; set; }

        public decimal? StaffPensionFundRate { get; set; }

        public decimal? StaffPensionFundAmount { get; set; }

        public decimal? CompanyPensionFundRate { get; set; }

        public decimal? CompanyPensionFundAmount { get; set; }

        public decimal? StaffPensionFundAmountKH { get; set; }

        public decimal? CompanyPensionFundAmountKH { get; set; }
        public decimal FirstPaymentAmount { get; set; }

        public decimal? RetrolPayment { get; set; }
        public decimal? AlwBeforTax { get; set; }
        public decimal? Seniority { get; set; }
        public decimal? StaffRisk { get; set; }
        public decimal? StaffRiskKH { get; set; }
        public decimal? TotalRisk { get; set; }
        public decimal? StaffHealthCareUSD { get; set; }
        public decimal? TotalHealthCare { get; set; }
        public int PeriodID { get; set; }
        public decimal? TotalTaxableIncome { get; set; }
    }
}
