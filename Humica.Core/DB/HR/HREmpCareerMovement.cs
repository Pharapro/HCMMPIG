namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpCareerMovement")]
    public partial class HREmpCareerMovement
    {
        public int ID { get; set; }

        [StringLength(15)]
        public string CompanyCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }
        [StringLength(200)]
        public string EmployeeName { get; set; }
        public string CareerType { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        [StringLength(15)]
        public string CareerCode { get; set; }

        public decimal OldSalary { get; set; }

        public decimal Increase { get; set; }

        public decimal NewSalary { get; set; }

        [Column(TypeName = "date")]
        public DateTime EffectDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(15)]
        public string BusinessUnit { get; set; }

        [StringLength(15)]
        public string Division { get; set; }

        [StringLength(15)]
        public string BranchCode { get; set; }

        [StringLength(15)]
        public string DepartmentCode { get; set; }

        [StringLength(15)]
        public string Office { get; set; }

        [StringLength(15)]
        public string Section { get; set; }

        [StringLength(15)]
        public string Groups { get; set; }

        [StringLength(15)]
        public string PositionCode { get; set; }

        [StringLength(15)]
        public string JobLevel { get; set; }

        [StringLength(15)]
        public string JobGrade { get; set; }

        [StringLength(15)]
        public string Category { get; set; }

        [StringLength(15)]
        public string EmployeeType { get; set; }

        [StringLength(15)]
        public string Location { get; set; }

        [StringLength(15)]
        public string StaffType { get; set; }
        [StringLength(15)]
        public string NewCompanyCode { get; set; }

        [StringLength(15)]
        public string NewBusinessUnit { get; set; }

        [StringLength(15)]
        public string NewDivision { get; set; }

        [StringLength(15)]
        public string NewBranch { get; set; }

        [StringLength(15)]
        public string NewDepartment { get; set; }

        [StringLength(15)]
        public string NewOffice { get; set; }

        [StringLength(15)]
        public string NewSection { get; set; }

        [StringLength(15)]
        public string NewGroups { get; set; }

        [StringLength(15)]
        public string NewPosition { get; set; }

        [StringLength(15)]
        public string NewJobLevel { get; set; }

        [StringLength(15)]
        public string NewJobGrade { get; set; }

        [StringLength(15)]
        public string NewCategory { get; set; }

        [StringLength(15)]
        public string NewEmployeeType { get; set; }

        [StringLength(15)]
        public string NewLocation { get; set; }

        [StringLength(15)]
        public string NewStaffType { get; set; }

        
        [StringLength(10)]
        public string Resigntype { get; set; }
        public string Reason { get; set; }
        [StringLength(15)]
        public string NewEmpCode { get; set; }
        public string Attachfile { get; set; }
        [StringLength(15)]
        public string Status { get; set; }
        [StringLength(15)]
        public string PerformanceID { get; set; }
        [StringLength(15)]
        public string KPI { get; set; }
        [StringLength(15)]
        public string KPIStatus { get; set; }
        [StringLength(50)]
        public string AppraisalType { get; set; }
        public bool? IsClosedPA { get; set; }
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        [StringLength(15)]
        public string DocReference { get; set; }
        [StringLength(20)]
        public string HOD { get; set; }
        [StringLength(20)]
        public string FirstLine { get; set; }
        [StringLength(20)]
        public string FirstLine2 { get; set; }
        [StringLength(20)]
        public string SecondLine { get; set; }
        [StringLength(20)]
        public string SecondLine2 { get; set; }
        [StringLength(20)]
        public string OTFirstLine { get; set; }
        [StringLength(20)]
        public string OTSecondLine { get; set; }
        [StringLength(20)]
        public string OTthirdLine { get; set; }
        [StringLength(20)]
        public string APPAppraisal { get; set; }
        [StringLength(20)]
        public string APPAppraisal2 { get; set; }
        [StringLength(20)]
        public string APPTracking { get; set; }
        [StringLength(20)]
        public string APPEvaluator { get; set; }

        public int? PositionID { get; set; }
        [StringLength(50)]
        public string KPIType { get; set; }
        public bool? IsEvalPA { get; set; }
        public bool? IsEvalKPI { get; set; }
        public DateTime? KPIStartDate { get; set; }
        public DateTime? KPIDeadline { get; set; }
        public DateTime? PAStartDate { get; set; }
        public DateTime? PADeadline1 { get; set; }
        public DateTime? PADeadline2 { get; set; }
        [StringLength(100)]
        public string TeleGroup { get; set; }
        public DateTime? SubordEffectDate { get; set; }
    }
}
