using System;

namespace Humica.Core.FT
{
    public class FTFilterData
    {
        public string EmpCode { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }
        public string Section { get; set; }
        public string LevelCode { get; set; }
        public string LeaveType { get; set; }
        public string EntitleType { get; set; }
        public int INYear { get; set; }
        public int FYear { get; set; }
        public int TYear { get; set; }
        public DateTime ForwardExp { get; set; }
        public decimal MaxForward { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string JournalType { get; set; }
        public string Remark { get; set; }
        public string TimeShift { get; set; }

    }
    public class FTINYear
    {
        public int INYear { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class Filters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string CurStage { get; set; }
        public string Status { get; set; }
    }
    public class FTFilterEmployee
    {
        public string CompanyCode { get; set; }
        public string Company { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string BusinessUnit { get; set; }
        public string Office { get; set; }
        public string Locations { get; set; }
        public string EmpType { get; set; }
        public string StaffType { get; set; }
        public string Division { get; set; }
        public string Section { get; set; }
        public string Group { get; set; }
        public string Level { get; set; }
        public string Category { get; set; }
        public string Position { get; set; }
        public string SalaryType { get; set; }
        public string RunPayrollAs { get; set; }
        public bool CalculateTax { get; set; }
        public DateTime? StartDate { get; set; }
        public int INYear { get; set; }
        public DateTime InMonth { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public int ReportType { get; set; }
        public string ReportTypes { get; set; }
        public DateTime EndProbation { get; set; }
        public string Status { get; set; }
        public string ResignDate { get; set; }
        public decimal Amount { get; set; }
        public string RewardsType { get; set; }
        public decimal Rate { get; set; }
        public string BankName { get; set; }
        public string ListDivision { get; set; }
        public decimal Day { get; set; }
        public string CareerCode { get; set; }
        public string TemplateType { get; set; }
        public bool IsNewJoin { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime ToMonth { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Shift { get; set; }
        public bool IsLate { get; set; }
        public bool IsEalary { get; set; }
        public bool IsMissScan { get; set; }
        public string BranchAccount { get; set; }
        public string LeaveType { get; set; }
        public int PayPeriodId { get; set; }
        public bool IsFirstPay { get; set; }
        public string InsuranceType { get; set; }
        public string Reference { get; set; }
        public string STReportType { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<decimal> TotalHours { get; set; }
        public decimal BreakTime { get; set; }
        public string Remark { get; set; }
        public string RequestBy { get; set; }
        public DateTime InDate { get; set; }
        public decimal? Percentage { get; set; }
        public string UserName { get; set; }
        public string ContractType { get; set; }
        public bool IsIncludeBatch { get; set; }
        public bool IsIncludeHasUser { get; set; }
        public string Team { get; set; }
        public string OTCategory { get; set; }
    }

    public class FTFilterInOut
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string EmpType { get; set; }
        public string Division { get; set; }
        public string Section { get; set; }
        public string Level { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CareerCode { get; set; }
        public string Location { get; set; }
    }
    public class FTAttendance
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Division { get; set; }
        public string Shift { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class FTMonthlySum
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string Division { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Location { get; set; }
        public DateTime InMonth { get; set; }
        public int PayPeriodId { get; set; }
    }
    public class FTDGeneralAccount
    {
        public string DealerCode { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string PeriodString { get; set; }
        public string Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReportType { get; set; }
        public string DocType { get; set; }
        public string MovementType { get; set; }
        public string TemplateType { get; set; }
        public bool IsDisplayZero { get; set; }
    }
    public class FTTraining
    {
        public int? INYear { get; set; }
        public string Course { get; set; }
    }
	public class FTAssetMaster
	{
		public string AssetCode { get; set; }
		public string Branch { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
	}
	public class FTDGeneralPeriod
    {
        public string DealerCode { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string TRCode { get; set; }
        public string Addition1 { get; set; }
        public string DocType { get; set; }
        public string Status { get; set; }
        public string ReportType { get; set; }
        public string DealerType { get; set; }
        public string RequestType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromMonth { get; set; }
        public int ToMonth { get; set; }
        public string Ki { get; set; }
        public int FromQuater { get; set; }
        public int ToQuater { get; set; }
        public string Period { get; set; }
        public int YearInJp { get; set; }
        public string HouseNo { get; set; }
        public string CustomerNo { get; set; }
    }

    public class FTDFromDateToDate
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class FTFilterReport
    {
        public string UserName { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Division { get; set; }
        public string BusinessUnit { get; set; }
        public string Department { get; set; }
        public string Office { get; set; }
        public string Section { get; set; }
        public string Group { get; set; }
        public string Position { get; set; }
        public string Level { get; set; }
        public string Locations { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string CareerCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime InMonth { get; set; }
        public int InYear { get; set; }
        public bool IsLate { get; set; }
        public bool IsEalary { get; set; }
        public bool IsMissScan { get; set; }
        public string CreatedBy { get; set; }
    }
    public class FTFilterAttendance : FTFilerPayroll
    {
        public bool IsLate { get; set; }
        public bool IsEalary { get; set; }
        public bool IsMissScan { get; set; }
        public string Shift { get; set; }
        public bool IsIncludeBatch { get; set; }
    }
    public class FTFilerLeave : FTFilterReport
    {
        public string LeaveType { get; set; }
    }
    public class FTFilerPayroll : FTFilterReport
    {
        public string RewardsType { get; set; }
        public string SalaryType { get; set; }
        public int Period { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValueDate { get; set; }
        public int PeriodID { get; set; }
        public string ReportTypes { get; set; }
    }
    public class FTFilerPerformance : FTFilterReport
    {
        public bool IsKPI { get; set; }
    }
    public class FTFilerIndex
    {
        public int InYear { get; set; }
        public string Status { get; set; }
    }
}