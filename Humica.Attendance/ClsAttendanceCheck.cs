using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Humica.Attendance
{
    public class ClsAttendanceCheck : IClsAttendanceCheck
    {
        protected IUnitOfWork unitOfWork;
        SYUser User { get; set; }
        public FTFilterAttendance Filter { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public List<VIEW_ATInOut> ListInOut { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsAttendanceCheck()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListInOut = new List<VIEW_ATInOut>();
            Filter = new FTFilterAttendance();
            Filter.FromDate = DateTime.Now;
            Filter.ToDate = DateTime.Now;
        }
        public void OnIndexLoading(FTFilterAttendance _Filter)
        {
            var _ListAtInout = unitOfWork.Set<VIEW_ATInOut>().
                Where(w =>
                (DbFunctions.TruncateTime(w.FullDate) >= _Filter.FromDate.Date && DbFunctions.TruncateTime(w.FullDate) <= _Filter.ToDate.Date)
                && (string.IsNullOrEmpty(_Filter.Department) || w.DEPT == _Filter.Department)
                && (string.IsNullOrEmpty(_Filter.Position) || w.Position == _Filter.Position)
                && (string.IsNullOrEmpty(_Filter.Division) || w.Division == _Filter.Division)
                && (string.IsNullOrEmpty(_Filter.Locations) || w.LOCT == _Filter.Locations)
                && (string.IsNullOrEmpty(_Filter.EmpCode) || w.EmpCode == _Filter.EmpCode)
                 ).ToList();
            List<VIEW_ATInOut> ListAtInout = _ListAtInout.ToList();
            var listBranch = SYConstant.getBranchDataAccess();
            ListAtInout = ListAtInout.Where(w => listBranch.Where(x => x.Code == w.BRANCH).Any()).ToList();
            if (_Filter.Branch != null)
            {
                string[] Branch = _Filter.Branch.Split(',');
                List<string> LstBranch = new List<string>();
                foreach (var read in Branch)
                {
                    if (read.Trim() != "")
                    {
                        LstBranch.Add(read.Trim());
                    }
                }

                ListAtInout = ListAtInout.Where(w => LstBranch.Contains(w.BRANCH)).ToList();
            }
            

            ListInOut = ListAtInout.OrderBy(x => x.FullDate).ToList();
        }
        public void OnFilterStaff(string EmpCode)
        {
            StaffProfile = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (StaffProfile == null)
            {
                StaffProfile = new HRStaffProfile();
            }
        }
        public virtual string OnGridModify(VIEW_ATInOut MModel, string Action)
        {
            OnLoad();
            if (Action == "ADD")
            {
                var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.EmpCode);
                if (Staff != null)
                {
                    var Header = new ATInOut();
                    Header.EmpCode = MModel.EmpCode;
                    Header.CardNo = Staff.CardNo;
                    Header.FullDate = MModel.FullDate;
                    Header.CreateBy = User.UserName;
                    Header.CreateOn = DateTime.Now;
                    Header.STATUS = 3;
                    Header.LCK = 0;
                    Header.FLAG = 0;
                    Header.Remark = MModel.Remark;
                    unitOfWork.Add(Header);
                    unitOfWork.Save();
                    ListInOut.Add(new VIEW_ATInOut { EmpCode = MModel.EmpCode, FullDate = MModel.FullDate });
                }
            }
            else if (Action == "EDIT")
            {
                var objUpdate = ListInOut.First();
                objUpdate.FullDate = MModel.FullDate;
                objUpdate.Remark = MModel.Remark;
                var ObjMatch = unitOfWork.Set<ATInOut>().FirstOrDefault(w=>w.ID== MModel.ID);
                ObjMatch.FullDate = MModel.FullDate;
                ObjMatch.Remark = MModel.Remark;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.ChangedBy = User.UserName;
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
            }
            else if (Action == "DELETE")
            {
                var ObjMatch = unitOfWork.Set<ATInOut>().FirstOrDefault(w => w.ID == MModel.ID);
                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
            }
            return SYConstant.OK;
        }

        public Dictionary<string, dynamic> OnDataSelector(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCodes = ListBranch.Select(b => b.Code).ToList();
            var ListStaff = (from staff in unitOfWork.Set<HRStaffProfile>()
                             join branchCode in ListBranchCodes on staff.Branch equals branchCode
                             select staff).ToList();

            keyValues.Add("STAFF_SELECT", ListStaff);
            keyValues.Add("BRANCHES_SELECT", ListBranch);
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("BUSINESSUNIT_SELECT", ClsFilter.LoadBusinessUnit());
            keyValues.Add("OFFICE_SELECT", ClsFilter.LoadOffice());
            keyValues.Add("SECTION_SELECT", ClsFilter.LoadSection());
            keyValues.Add("GROUP_SELECT", ClsFilter.LoadGroups());
            keyValues.Add("POSITION_SELECT", ClsFilter.LoadPosition());
            keyValues.Add("LEVEL_SELECT", SYConstant.getLevelDataAccess());
            return keyValues;
        }
    }
}
