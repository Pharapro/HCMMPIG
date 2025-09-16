using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Attendance
{
	public class TransferAbsenceObject : ITransferAbsenceObject
	{
		protected IUnitOfWork unitOfWork;
		public string ScreenId { get; set; }
		public string EmpID { get; set; }
		public int Progress { get; set; }
		public SYUser User { get; set; }
		public FTFilterAttendance Attendance { get; set; }
		public HRStaffProfile StaffProfile { get; set; }
		public ATEmpSchedule Header { get; set; }
		public List<HRDepartment> ListDepartment { get; set; }
		public List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
		public List<ATEmpCountABS> ListEmpCountABS { get; set; }
		public List<ATEmpSchedule> ListHeader { get; set; }
		public void OnLoad()
		{
			unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
		}

		public TransferAbsenceObject()
		{
			User = SYSession.getSessionUser();
			OnLoad();
		}

		public void OnIndexLoading()
		{
			ListDepartment = unitOfWork.Set<HRDepartment>().Where(s => s.IsActive == true).ToList();
		}
		public string GenrateAttendance(string TranNo, DateTime FromDate, DateTime ToDate)
		{
			string Status_Error = "";
			try
			{
				string[] deptCodes = TranNo.Split(';');
				if (deptCodes.Length == 0)
					return "DOC_INV";

				// Get valid departments
				var departments = unitOfWork.Set<HRDepartment>()
					.Where(d => d.IsActive==true && deptCodes.Contains(d.Code))
					.Select(d => new { d.Code, d.Description })
					.ToList();

				if (!departments.Any())
					return "DOC_INV";

				// Get active employees in those departments
				var empDeptMapping = unitOfWork.Set<HRStaffProfile>()
					.Where(s => s.Status == "A" && deptCodes.Contains(s.DEPT))
					.Select(s => new { s.EmpCode, s.DEPT })
					.ToList();

				var empCodes = empDeptMapping.Select(e => e.EmpCode).Distinct().ToList();
				if (!empCodes.Any())
					return "NO_EMP";

				var existingRecords = unitOfWork.Set<ATEmpCountABS>().ToList();
				if (existingRecords.Any())
				{
					unitOfWork.Set<ATEmpCountABS>().RemoveRange(existingRecords);
					unitOfWork.Save();
				}

				// Preload all relevant schedules
				var scheduleData = unitOfWork.Set<ATEmpSchedule>()
					.Where(s => s.LeaveDesc == "ABS"
							 && s.TranDate >= FromDate
							 && s.TranDate <= ToDate
							 && empCodes.Contains(s.EmpCode))
					.Select(s => new { s.EmpCode, s.TranDate })
					.ToList();

				// Prepare attendance summary
				ListEmpCountABS = new List<ATEmpCountABS>();

				foreach (var dept in departments)
				{
					var deptEmpCodes = empDeptMapping
						.Where(e => e.DEPT == dept.Code)
						.Select(e => e.EmpCode)
						.ToHashSet();

					if (!deptEmpCodes.Any())
						continue;

					var deptSchedules = scheduleData
						.Where(s => deptEmpCodes.Contains(s.EmpCode))
						.ToList();

					var absData = new ATEmpCountABS
					{
						DeptCode = dept.Code,
						DepartmentName = dept.Description
					};

					// Group by month
					var groupedByMonth = deptSchedules
						.GroupBy(s => s.TranDate.Month);

					foreach (var group in groupedByMonth)
					{
						int month = group.Key;
						int absenceCount = group.Count();
						int missScanCount = group.Select(g => g.EmpCode).Distinct().Count();

						SetAbsenceDay(absData, month, absenceCount);
						SetMissScanStaff(absData, month, missScanCount);
					}

					ListEmpCountABS.Add(absData);
				}

				if (ListEmpCountABS.Any())
				{
					unitOfWork.Set<ATEmpCountABS>().AddRange(ListEmpCountABS);
					unitOfWork.Save();
				}

				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
			}
			catch (DbUpdateException e)
			{
				return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
			}
			catch (Exception e)
			{
				return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
			}
		}


		// Helper method to set absence days
		private void SetAbsenceDay(ATEmpCountABS data, int month, int count)
		{
			switch (month)
			{
				case 1: data.Jan_ABS_Day = count; break;
				case 2: data.Feb_ABS_Day = count; break;
				case 3: data.Mar_ABS_Day = count; break;
				case 4: data.Apr_ABS_Day = count; break;
				case 5: data.May_ABS_Day = count; break;
				case 6: data.Jun_ABS_Day = count; break;
				case 7: data.Jul_ABS_Day = count; break;
				case 8: data.Aug_ABS_Day = count; break;
				case 9: data.Sep_ABS_Day = count; break;
				case 10: data.Oct_ABS_Day = count; break;
				case 11: data.Nov_ABS_Day = count; break;
				case 12: data.Dec_ABS_Day = count; break;
			}
		}

		// Helper method to set miss-scan staff counts
		private void SetMissScanStaff(ATEmpCountABS data, int month, int count)
		{
			switch (month)
			{
				case 1: data.Jan_Miss_Staff = count; break;
				case 2: data.Feb_Miss_Staff = count; break;
				case 3: data.Mar_Miss_Staff = count; break;
				case 4: data.Apr_Miss_Staff = count; break;
				case 5: data.May_Miss_Staff = count; break;
				case 6: data.Jun_Miss_Staff = count; break;
				case 7: data.Jul_Miss_Staff = count; break;
				case 8: data.Aug_Miss_Staff = count; break;
				case 9: data.Sep_Miss_Staff = count; break;
				case 10: data.Oct_Miss_Staff = count; break;
				case 11: data.Nov_Miss_Staff = count; break;
				case 12: data.Dec_Miss_Staff = count; break;
			}
		}
	}
}
