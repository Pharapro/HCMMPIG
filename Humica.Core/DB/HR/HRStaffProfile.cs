namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRStaffProfile")]
    public partial class HRStaffProfile
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(70)]
        public string FirstName { get; set; }

        [StringLength(70)]
        public string LastName { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(70)]
        public string OthFirstName { get; set; }

        [StringLength(70)]
        public string OthLastName { get; set; }

        [StringLength(140)]
        public string OthAllName { get; set; }

        [StringLength(10)]
        public string Costcent { get; set; }

        [StringLength(10)]
        public string SubCostCent { get; set; }

        [StringLength(10)]
        public string EmpType { get; set; }

        [StringLength(10)]
        public string EmpSource { get; set; }

        [StringLength(5)]
        public string Title { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(10)]
        public string Marital { get; set; }

        [StringLength(200)]
        public string POB { get; set; }

        [StringLength(10)]
        public string Country { get; set; }

        [StringLength(10)]
        public string Nation { get; set; }

        [StringLength(10)]
        public string Race { get; set; }

        public string State { get; set; }

        [StringLength(10)]
        public string Religion { get; set; }

        [StringLength(10)]
        public string TranType { get; set; }

        [StringLength(50)]
        public string SOCSO { get; set; }

        public bool ISNSSF { get; set; }

        [StringLength(100)]
        public string Phone1 { get; set; }

        [StringLength(100)]
        public string Phone2 { get; set; }

        [StringLength(20)]
        public string PhoneExt { get; set; }

        [StringLength(200)]
        public string FAX { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(10)]
        public string BankName { get; set; }

        [StringLength(140)]
        public string BankBranch { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        [StringLength(140)]
        public string BankAccName { get; set; }

        [StringLength(20)]
        public string Heigth { get; set; }

        [StringLength(20)]
        public string Weigth { get; set; }

        [StringLength(5)]
        public string BloodType { get; set; }

        [StringLength(100)]
        public string Performance { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }
        [StringLength(10)]
        public string Office { get; set; }

        [StringLength(10)]
        public string Line { get; set; }

        [StringLength(10)]
        public string CATE { get; set; }

        [StringLength(10)]
        public string SECT { get; set; }
        [StringLength(10)]
        public string Groups { get; set; }
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
        public string JobCode { get; set; }

        [StringLength(100)]
        public string POSTDESC { get; set; }

        [StringLength(500)]
        public string JOBSPEC { get; set; }

        public decimal Salary { get; set; }

        public DateTime? EffectDate { get; set; }

        [StringLength(10)]
        public string HODCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Probation { get; set; }

        public DateTime? LeaveConf { get; set; }

        [StringLength(10)]
        public string LEAVESCHM { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateTerminate { get; set; }

        [StringLength(10)]
        public string TerminateStatus { get; set; }

        [StringLength(200)]
        public string TerminateRemark { get; set; }

        [StringLength(500)]
        public string ConAddress { get; set; }

        [StringLength(500)]
        public string ConAddressOth { get; set; }

        [StringLength(100)]
        public string ConPhone { get; set; }

        [StringLength(500)]
        public string Peraddress { get; set; }

        [StringLength(500)]
        public string PeraddressOth { get; set; }

        [StringLength(100)]
        public string PerPhone { get; set; }

        [StringLength(10)]
        public string CardNo { get; set; }

        public bool IsAtten { get; set; }

        [StringLength(50)]
        public string CareerDesc { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string PayParam { get; set; }

        [StringLength(2000)]
        public string EMPSKILL { get; set; }

        [StringLength(2000)]
        public string EMPHOBBIE { get; set; }

        [StringLength(50)]
        public string ESalary { get; set; }

        [StringLength(10)]
        public string ROSTER { get; set; }

        [StringLength(10)]
        public string BONUSTYPE { get; set; }

        [StringLength(50)]
        public string Email2 { get; set; }

        [StringLength(500)]
        public string PAEMAIL { get; set; }

        [StringLength(10)]
        public string ALERTLEAVE { get; set; }

        [StringLength(1)]
        public string TXPayType { get; set; }

        public bool? IsMealAllowance { get; set; }

        [StringLength(50)]
        public string PostFamily { get; set; }

        [StringLength(30)]
        public string StaffType { get; set; }

        [StringLength(15)]
        public string GRPLVAP { get; set; }

        [StringLength(15)]
        public string GRPOTAP { get; set; }

        [StringLength(30)]
        public string GrpGLAcc { get; set; }

        public double? SalaryProb { get; set; }

        public bool? ISHealth { get; set; }

        [StringLength(30)]
        public string HealthNo { get; set; }

        [StringLength(500)]
        public string Images { get; set; }

        public bool IsCalSalary { get; set; }

        [StringLength(10)]
        public string SalaryType { get; set; }

        [StringLength(100)]
        public string TeleGroup { get; set; }

        public decimal BankFee { get; set; }

        public bool IsResident { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReSalary { get; set; }

        public bool SalaryFlag { get; set; }

        [StringLength(20)]
        public string FirstLine { get; set; }

        [StringLength(20)]
        public string FirstLine2 { get; set; }

        [StringLength(20)]
        public string SecondLine { get; set; }

        [StringLength(20)]
        public string SecondLine2 { get; set; }

        public bool Supervisory { get; set; }

        [StringLength(20)]
        public string HODPost { get; set; }

        [StringLength(20)]
        public string ProbationType { get; set; }

        [StringLength(500)]
        public string Signature { get; set; }

        [StringLength(20)]
        public string CheckSum { get; set; }

        [StringLength(20)]
        public string BankAccType { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public bool IsOTApproval { get; set; }

        [StringLength(20)]
        public string Province { get; set; }

        [StringLength(20)]
        public string District { get; set; }

        [StringLength(20)]
        public string Commune { get; set; }

        [StringLength(50)]
        public string Village { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(20)]
        public string TID { get; set; }

        [StringLength(50)]
        public string TeleChartID { get; set; }

        public bool IsHold { get; set; }

        public decimal SalaryTax { get; set; }

        public decimal SalaryNSSF { get; set; }

        public bool IsAnnouncement { get; set; }

        public bool IsIntegrate { get; set; }

        [StringLength(15)]
        public string NSSFContributionType { get; set; }

        [StringLength(15)]
        public string TemLeave { get; set; }

        public bool? IsPayPartial { get; set; }
        [StringLength(10)]
        public string GroupDept { get; set; }
        public string PassPayslip { get; set; }

        public bool? IsAutoAppLeave { get; set; }
        public bool? IsAutoAppKPITraing { get; set; }
        [StringLength(20)]
        public string OTFirstLine { get; set; }
        [StringLength(20)]
        public string OTSecondLine { get; set; }
        [StringLength(20)]
        public string OTthirdLine { get; set; }
        public string EmailGroup { get; set; }
        public string Attachment { get; set; }
        public string AttachFile { get; set; }
        public string GPSData { get; set; }
        public bool? IsOnsite { get; set; }
        [StringLength(30)]
        public string Manager { get; set; }
        [StringLength(10)]
        public string APPTracking { get; set; }
        [StringLength(10)]
        public string APPEvaluator { get; set; }
        [StringLength(10)]
        public string APPAppraisal { get; set; }
        [StringLength(10)]
        public string APPAppraisal2 { get; set; }
        [StringLength(20)]
        public string Currency { get; set; }
        public string TeleGroupScan { get; set; }
        public string TeleGroupOT { get; set; }

        public int? PositionID { get; set; }
        public bool? IsExceptScan { get; set; }
		public string ImgQr { get; set; }
	}
}
