using Humica.Core;
using Humica.Core.DB;
using Humica.Core.Helper;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Humica.Performance
{
    public class ClsKPIConfig : IClsKPIConfig
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<HRKPICategory> ListCategory { get; set; }
        public List<HRKPIType> ListKPIGroupHeader { get; set; }
        public List<HRKPINonFinance> ListKPINonFinance { get; set; }

        public List<HRKPIList> ListKPIList { get; set; }
        public List<HRKPIIndicator> ListIndicatorByBU { get; set; }
        public List<HRKPIList> ListKPIListByBU { get; set; }
        public List<HRKPIList> ListKPIListByDept { get; set; }
        public List<HRKPIIndicator> ListIndicatorByDept { get; set; }
        public List<HRKPIGrade> ListKPIGrade { get; set; }
        public List<HRKPINorm> ListKPINorm { get; set; }
        public List<HRKPIAction> ListKPIAction { get; set; }
        protected IUnitOfWork unitOfWork;
        public ClsKPIConfig()
        {
            OnLoad();
        }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void OnIndexLoading()
        {
            ListIndicatorByDept = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Category == "DPM").ToList();
            ListIndicatorByBU = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Category == "BU").ToList();
            ListCategory = unitOfWork.Set<HRKPICategory>().ToList();
            ListKPIGroupHeader = unitOfWork.Set<HRKPIType>().ToList();
            ListKPIList = unitOfWork.Set<HRKPIList>().Where(w => string.IsNullOrEmpty(w.Department)).ToList();
            ListKPIGrade = unitOfWork.Set<HRKPIGrade>().ToList();
            ListKPINorm = unitOfWork.Set<HRKPINorm>().ToList();
            ListKPIAction = unitOfWork.Set<HRKPIAction>().ToList();
            ListKPINonFinance = unitOfWork.Set<HRKPINonFinance>().ToList();
            ListKPIListByDept = unitOfWork.Set<HRKPIList>().Where(w => !string.IsNullOrEmpty(w.Department) && w.Category == "DPM").ToList();
            ListKPIListByBU = unitOfWork.Set<HRKPIList>().Where(w => !string.IsNullOrEmpty(w.Department) && w.Category == "BU").ToList();
        }
        public void OnIndexLoadingCategory()
        {
            ListCategory = unitOfWork.Set<HRKPICategory>().ToList();
        }
        public void OnIndexLoadingKPIType()
        {
            ListKPIGroupHeader = unitOfWork.Set<HRKPIType>().ToList();
        }
        public void OnIndexLoadingKPIGrade()
        {
            ListKPIGrade = unitOfWork.Set<HRKPIGrade>().ToList();
        }
        public void OnIndexLoadingKPINorm()
        {
            ListKPINorm = unitOfWork.Set<HRKPINorm>().ToList();
        }
        public void OnLoadingAction()
        {
            ListKPIAction = unitOfWork.Set<HRKPIAction>().ToList();
        }
        public void OnLoadingNonFinance()
        {
            ListKPINonFinance = unitOfWork.Set<HRKPINonFinance>().ToList();
        }
        public void OnIndexLoadingKPIList()
        {
            ListKPIList = unitOfWork.Set<HRKPIList>().Where(w => string.IsNullOrEmpty(w.Department)).ToList();
        }
        public string OnLoadingDepartment(string Dept)
        {
            string Department = "";
            if (!string.IsNullOrEmpty(Dept))
            {
                Department = Dept;
            }
            else
            {
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null)
                    Department = Staff.DEPT;
            }
            return Department;
        }
        public string OnLoadingBU(string BU)
        {
            string Department = "";
            if (!string.IsNullOrEmpty(BU))
            {
                Department = BU;
            }
            else
            {
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null)
                    Department = Staff.GroupDept;
            }
            return Department;
        }
        public void OnIndexLoadingBU(bool ESS = false)
        {
            if (ESS)
            {
                List<string> departmentList = new List<string>();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null && !string.IsNullOrEmpty(Staff.DEPT))
                {
                    departmentList.Add(Staff.GroupDept);
                }
                ListKPIListByBU = unitOfWork.Set<HRKPIList>().Where(w => departmentList.Contains(w.Department) && w.Category == "BU").ToList();
            }
            else
                ListKPIListByBU = unitOfWork.Set<HRKPIList>().Where(w => !string.IsNullOrEmpty(w.Department) && w.Category == "BU").ToList();
        }
        public void OnIndexLoadingDept(bool ESS = false)
        {
            if (ESS)
            {
                List<string> departmentList = new List<string>();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null && !string.IsNullOrEmpty(Staff.DEPT))
                {
                    departmentList.Add(Staff.DEPT);
                }
                else
                {
                    List<SYAccessDepartment> ListAssDept = unitOfWork.Set<SYAccessDepartment>().Where(w => w.UserName == User.UserName).ToList();
                    if (ListAssDept.Count > 0)
                    {
                        departmentList = ListAssDept.Select(assDept => assDept.DeptCode).ToList();
                    }
                }
                ListKPIListByDept = unitOfWork.Set<HRKPIList>().Where(w => departmentList.Contains(w.Department)).ToList();
            }
            else
                ListKPIListByDept = unitOfWork.Set<HRKPIList>().Where(w => !string.IsNullOrEmpty(w.Department) && w.Category == "DPM").ToList();
        }
        public void OnIndexIndicatorBU(bool ESS = false)
        {
            if (ESS)
            {
                List<string> departmentList = new List<string>();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null)
                {
                    departmentList.Add(Staff.GroupDept);
                }
                ListIndicatorByBU = unitOfWork.Set<HRKPIIndicator>().Where(w => departmentList.Contains(w.Department) && w.Category == "BU").ToList();
            }
            else
            {
                ListIndicatorByBU = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Category == "BU").ToList();
            }
        }
        public void OnIndexLoadingIndicator(bool ESS = false)
        {
            if (ESS)
            {
                List<string> departmentList = new List<string>();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null && !string.IsNullOrEmpty(Staff.DEPT))
                {
                    departmentList.Add(Staff.DEPT);
                }
                else
                {
                    List<SYAccessDepartment> ListAssDept = unitOfWork.Set<SYAccessDepartment>().Where(w => w.UserName == User.UserName).ToList();
                    if (ListAssDept.Count > 0)
                    {
                        departmentList = ListAssDept.Select(assDept => assDept.DeptCode).ToList();
                    }
                }
                ListIndicatorByDept = unitOfWork.Set<HRKPIIndicator>().Where(w => departmentList.Contains(w.Department)).ToList();
            }
            else
            {
                ListIndicatorByDept = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Category != "BU").ToList();
            }
        }
        public string OnGridModifyKPI(HRKPIList MModel, string Action, bool IsESS = false)
        {
            try
            {
                string Code = "";
                int NextNumberRank = 1;
                MModel.Description = MModel.Description?.Trim() ?? string.Empty;
                MModel.SecDescription = MModel.SecDescription?.Trim() ?? string.Empty;
                MModel.Category = "DPM";
                if (Action == "ADD")
                {
                    if (IsESS)
                    {
                        if (string.IsNullOrEmpty(MModel.Department))
                        {
                            return "DEPARTMENT_EN";
                        }

                        var ListByDepartment = unitOfWork.Set<HRKPIList>().Where(w => w.Department == MModel.Department
                        && w.Category == MModel.Category).ToList();
                        if (ListByDepartment.Count > 0)
                        {
                            var result = ListByDepartment.Max(w => w.Code);
                            NextNumberRank = Convert.ToInt32(StringHelper.ExtractNumberFromString(result));
                            NextNumberRank += 1;
                        }
                        Code = StringHelper.getLenOfPrefix(3, NextNumberRank);
                        MModel.Code = MModel.Department + Code;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIList>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyKPIBU(HRKPIList MModel, string Action, string Category, bool IsESS = false)
        {
            try
            {
                string Code = "";
                int NextNumberRank = 1;
                MModel.Description = MModel.Description?.Trim() ?? string.Empty;
                MModel.SecDescription = MModel.SecDescription?.Trim() ?? string.Empty;
                MModel.Category = Category;
                if (Action == "ADD")
                {
                    if (IsESS)
                    {
                        if (string.IsNullOrEmpty(MModel.Department))
                        {
                            if (Category == "BU")
                            {
                                return "BUSINESSUNIT_EN";
                            }
                            return "DEPARTMENT_EN";
                        }
                        var ListByDepartment = unitOfWork.Set<HRKPIList>().Where(w => w.Department == MModel.Department
                        && w.Category == Category).ToList();
                        if (ListByDepartment.Count > 0)
                        {
                            var result = ListByDepartment.Max(w => w.Code);
                            NextNumberRank = Convert.ToInt32(StringHelper.ExtractNumberFromString(result));
                            NextNumberRank += 1;
                        }
                        Code = StringHelper.getLenOfPrefix(3, NextNumberRank);
                        MModel.Code = MModel.Department + Code;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIList>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyIndicator(HRKPIIndicator MModel, string Action, bool IsESS = false)
        {
            try
            {
                string Code = "";
                int NextNumberRank = 1;
                MModel.Description = MModel.Description?.Trim() ?? string.Empty;
                MModel.SecDescription = MModel.SecDescription?.Trim() ?? string.Empty;
                MModel.Category = "DPM";
                if (Action == "ADD")
                {
                    if (IsESS)
                    {
                        if (string.IsNullOrEmpty(MModel.Department))
                        {
                            return "DEPARTMENT_EN";
                        }
                        var ListByDepartment = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Department == MModel.Department
                        && w.Category == MModel.Category).ToList();
                        if (ListByDepartment.Count > 0)
                        {
                            var result = ListByDepartment.Max(w => w.Code);
                            NextNumberRank = Convert.ToInt32(StringHelper.ExtractNumberFromString(result));
                            NextNumberRank += 1;
                        }
                        Code = StringHelper.getLenOfPrefix(3, NextNumberRank);
                        MModel.Code = "IND" + MModel.Department + Code;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var objCheck = unitOfWork.Set<HRKPIIndicator>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        objCheck.Description = MModel.Description;
                        objCheck.SecDescription = MModel.SecDescription;
                        objCheck.Remark = MModel.Remark;
                        objCheck.Department = MModel.Department;
                        objCheck.IsActive = MModel.IsActive;
                        unitOfWork.Update(objCheck);
                    }
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIIndicator>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyIndicatorBU(HRKPIIndicator MModel, string Action, string Category, bool IsESS = false)
        {
            try
            {
                string Code = "";
                int NextNumberRank = 1;
                MModel.Description = MModel.Description?.Trim() ?? string.Empty;
                MModel.SecDescription = MModel.SecDescription?.Trim() ?? string.Empty;
                MModel.Category = Category;
                if (Action == "ADD")
                {
                    if (IsESS)
                    {
                        if (string.IsNullOrEmpty(MModel.Department))
                        {
                            if (Category == "BU")
                            {
                                return "BUSINESSUNIT_EN";
                            }
                            return "DEPARTMENT_EN";
                        }
                        var ListByDepartment = unitOfWork.Set<HRKPIIndicator>().Where(w => w.Department == MModel.Department
                        && w.Category == Category).ToList();
                        if (ListByDepartment.Count > 0)
                        {
                            var result = ListByDepartment.Max(w => w.Code);
                            NextNumberRank = Convert.ToInt32(StringHelper.ExtractNumberFromString(result));
                            NextNumberRank += 1;
                        }
                        Code = StringHelper.getLenOfPrefix(3, NextNumberRank);
                        if (Category == "BU")
                        {
                            MModel.Code = "BU_" + MModel.Department + Code;
                        }
                        else
                        {
                            MModel.Code = "IND" + MModel.Department + Code;
                        }
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var objCheck = unitOfWork.Set<HRKPIIndicator>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        objCheck.Description = MModel.Description;
                        objCheck.SecDescription = MModel.SecDescription;
                        objCheck.Remark = MModel.Remark;
                        objCheck.Department = MModel.Department;
                        objCheck.IsActive = MModel.IsActive;
                        unitOfWork.Update(objCheck);
                    }
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIIndicator>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyKPIType(HRKPIType MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    MModel.Period = DateTimeHelper.CountMonth(MModel.StartDate.Value, MModel.EndDate.Value);

                    //var ListKPI =unitOfWork.Set<HRKPIType>().Where(w => w.StartDate == MModel.StartDate && w.EndDate == MModel.EndDate).ToList();
                    if (string.IsNullOrEmpty(MModel.Code))
                    {
                        return "KPI_GROUP";
                    }
                    //if (ListKPI.Count > 0)
                    //{
                    //    return "ISDUPLICATED";
                    //}
                    if (MModel.KPIAverage == 0 || MModel.KPIAverage == null)
                    {
                        return "KPI_AVG_ERR";
                    }
                    MModel.CreatedBy = User.UserName;
                    MModel.CreatedOn = DateTime.Now;
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    MModel.Period = DateTimeHelper.CountMonth(MModel.StartDate.Value, MModel.EndDate.Value);
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyCategory(HRKPICategory MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    var ListCategory = unitOfWork.Set<HRKPICategory>().ToList();
                    if (string.IsNullOrEmpty(MModel.CategoryCode))
                    {
                        return "CODE";
                    }
                    if (ListCategory.Where(w => w.CategoryCode == MModel.CategoryCode​​​).Count() > 0)
                    {
                        return "ISDUPLICATED";
                    }
                    var Line = ListCategory.ToList();
                    MModel.LineItem = Line.Count + 1;
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPICategory>().FirstOrDefault(w => w.LineItem == MModel.LineItem);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyGrade(HRKPIGrade MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIGrade>().FirstOrDefault(w => w.TranNo == MModel.TranNo);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyNorm(HRKPINorm MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPINorm>().FirstOrDefault(w => w.TranNo == MModel.TranNo);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyAction(HRKPIAction MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPIAction>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyNonFinance(HRKPINonFinance MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRKPINonFinance>().FirstOrDefault(w => w.ID == MModel.ID);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public IEnumerable<HRKPIList> GetKPIList_BU()
        {
            string INDICATOR = "";
            if (HttpContext.Current.Session[SYConstant.KPI_INDICATOR_BU] != null)
            {
                INDICATOR = HttpContext.Current.Session[SYConstant.KPI_INDICATOR_BU].ToString();
            }
            var dataRead = unitOfWork.Set<HRKPIList>().Where(w => w.Department == INDICATOR).ToList();
            return dataRead;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(bool ESS = false)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            var ListIndicator = unitOfWork.Set<HRKPIIndicator>().ToList();
            if (ESS)
            {
                List<string> departmentList = new List<string>();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null && !string.IsNullOrEmpty(Staff.DEPT))
                {
                    departmentList.Add(Staff.DEPT);
                }
                else
                {
                    List<SYAccessDepartment> ListAssDept = unitOfWork.Set<SYAccessDepartment>().Where(w => w.UserName == User.UserName).ToList();
                    if (ListAssDept.Count > 0)
                    {
                        departmentList = ListAssDept.Select(assDept => assDept.DeptCode).ToList();
                    }
                }
                if (Staff != null)
                {
                    keyValues.Add("BUSINESSUNIT_SELECT", unitOfWork.Set<HRGroupDepartment>().Where(w => w.Code == Staff.GroupDept).ToList());
                }
                if (Staff == null) Staff = new HRStaffProfile();
                keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment().Where(w => departmentList.Contains(w.Code)).ToList());
                keyValues.Add("INDICATOR", ListIndicator.Where(w => departmentList.Contains(w.Department)).ToList());
                keyValues.Add("INDICATOR_BU", ListIndicator.Where(w => w.Department== Staff.GroupDept).ToList());
            }
            else
            {
                keyValues.Add("INDICATOR", ListIndicator.Where(w => w.Category != "BU").ToList());
                keyValues.Add("INDICATOR_BU", ListIndicator.Where(w => w.Category == "BU").ToList());
                keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
                keyValues.Add("BUSINESSUNIT_SELECT", unitOfWork.Set<HRGroupDepartment>().ToList());
                keyValues.Add("KPI_CATEGORY", unitOfWork.Set<SYData>().Where(w => w.DropDownType == "KPI_CATEGORY").ToList());
            }
            return keyValues;
        }
    }
}
