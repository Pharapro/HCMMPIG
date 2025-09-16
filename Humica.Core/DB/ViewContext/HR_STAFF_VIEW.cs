namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_STAFF_VIEW
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(140)]
        public string OthAllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Probation { get; set; }

        [StringLength(100)]
        public string Phone1 { get; set; }

        [StringLength(10)]
        public string EmpType { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }
        [StringLength(10)]
        public string Office { get; set; }

        [StringLength(10)]
        public string Groups { get; set; }

        [StringLength(10)]
        public string CardNo { get; set; }

        [StringLength(5)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Section { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Level { get; set; }

        [StringLength(100)]
        public string EmployeeType { get; set; }

        [StringLength(10)]
        public string TerminateStatus { get; set; }

        [StringLength(10)]
        public string PayParam { get; set; }

        [StringLength(10)]
        public string BranchID { get; set; }
        [StringLength(10)]
        public string JobCode { get; set; }

        [StringLength(500)]
        public string Peraddress { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateTerminate { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [StringLength(100)]
        public string PayParameter { get; set; }

        [StringLength(50)]
        public string PostFamily { get; set; }

        [StringLength(10)]
        public string GroupDept { get; set; }

        [StringLength(10)]
        public string SECT { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string CATE { get; set; }

        [StringLength(100)]
        public string DivisionDesc { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }

        [StringLength(100)]
        public string Nationality { get; set; }

        [StringLength(500)]
        public string PeraddressOth { get; set; }

        [StringLength(140)]
        public string BankBranch { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(140)]
        public string BankAccName { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        [StringLength(50)]
        public string NSSF { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(29)]
        public string TXPayType { get; set; }

        [StringLength(100)]
        public string ServicesLenght { get; set; }

        [StringLength(10)]
        public string ROSTER { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Status { get; set; }

        [StringLength(20)]
        public string StatusCode { get; set; }

        [StringLength(20)]
        public string IDCard { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDCard_IssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDCard_ExpiryDate { get; set; }

        [StringLength(20)]
        public string PassportNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string SexKH { get; set; }

        public int? Age { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Commune { get; set; }

        public string Village { get; set; }

        public int? Warning { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsHold { get; set; }

        [StringLength(100)]
        public string SecBranch { get; set; }
        public string ComENG { get; set; }
        public string BU { get; set; }

        [StringLength(100)]
        public string SecDepartment { get; set; }

        [StringLength(250)]
        public string SecLocation { get; set; }

        [StringLength(100)]
        public string SecDivision { get; set; }

        [StringLength(100)]
        public string SecPostion { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string TeleChartID { get; set; }
        [StringLength(50)]
        public string TeleGroup { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OpenDateShirt { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsIntegrate { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string TitleKH { get; set; }
        public bool? IsPayPartial { get; set; }
        [StringLength(10)]
        public string HODCode { get; set; }
        [StringLength(20)]
        public string FirstLine { get; set; }
        public string FirstLine2 { get; set; }

        [StringLength(20)]
        public string SecondLine { get; set; }
        public string SecondLine2 { get; set; }
        public string JobGradeDes { get; set; }
        public string SalaryEN { get; set; }
        public decimal Salary { get; set; }
    }
}
