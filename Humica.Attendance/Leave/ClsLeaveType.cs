using Humica.Core;
using Humica.Core.DB;
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
    public class ClsLeaveType : IClsLeaveType
    {
        public HumicaDBContext DB = new HumicaDBContext();
        IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRLeaveType Header { get; set; }
        public List<HRLeaveType> ListHeader { get; set; }
        public IQueryable ListLHourPolicy { get; set; }
        public IQueryable ListLeavePolicy { get; set; }
        public IQueryable ListLeaveProRate { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsLeaveType()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListHeader = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();
        }
        public virtual void OnCreatingLoading()
        {
            Header = new HRLeaveType();
            Header.Soperand = 1;
            Header.BeforeDay = 0;
            Header.Operator = "*";
            Header.SVC = false;
            Header.IncPub = true;
            Header.InRest = true;
            Header.Probation = false;
            Header.CUT = false;
            Header.IsCurrent = true;
            Header.IsOverEntitle = true;
            Header.NumDay = 0;
            Header.Gender = "B";
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string Code = (string)keys[0];
            Header = unitOfWork.Repository<HRLeaveType>().Queryable().FirstOrDefault(w => w.Code == Code);
        }
        public string Create()
        {
            OnLoad();
            try
            {
                if (string.IsNullOrEmpty(Header.Code))
                {
                    return "CODE_EN";
                }
                Header.Code = Header.Code.ToUpper().Trim();
                var Count = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();
                if (Count.Where(w => w.Code == Header.Code).ToList().Count() > 0)
                {
                    return "DUP_CODE_EN";
                }
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                Header.CUTTYPE = 1;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName,Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Update(string id)
        {
            OnLoad();
            try
            {
                HRLeaveType objMast = unitOfWork.Repository<HRLeaveType>().Queryable().FirstOrDefault(w => w.Code == id);
                objMast.Description = Header.Description;
                objMast.OthDesc = Header.OthDesc;
                objMast.Remark = Header.Remark;
                objMast.InRest = Header.InRest;
                objMast.IncPub = Header.IncPub;
                objMast.Probation = Header.Probation;
                objMast.BeforeDay = Header.BeforeDay;
                objMast.ReqDocument = Header.ReqDocument;
                objMast.NumDay = Header.NumDay;
                objMast.Allowbackward = Header.Allowbackward;
                objMast.Beforebackward = Header.Beforebackward;
                objMast.IsOverEntitle = Header.IsOverEntitle;
                objMast.IsCurrent = Header.IsCurrent;
                objMast.Gender = Header.Gender;
                objMast.BalanceType = Header.BalanceType;
                objMast.CUT = Header.CUT;
                objMast.Foperand = Header.Foperand;
                objMast.Operator = Header.Operator;
                objMast.Soperand = Header.Soperand;
                objMast.IsParent = Header.IsParent;
                objMast.Parent = Header.Parent;
                objMast.NotAllowRequest = Header.NotAllowRequest;
                objMast.Amount = Header.Amount;
                objMast.MinTaken = Header.MinTaken;
                objMast.ChangedOn = DateTime.Now;
                objMast.ChangedBy = User.UserName;
                unitOfWork.Update(objMast);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string Delete(string id)
        {
            OnLoad();
            try
            {
                HRLeaveType objMast = unitOfWork.Repository<HRLeaveType>().Queryable().FirstOrDefault(w => w.Code == id);
                if (objMast == null)
                {
                    return "LEAVE_NE";
                }
                var objEmp = unitOfWork.Repository<HREmpLeaveB>().Queryable().Where(w => w.LeaveCode == id).ToList();
                if (objEmp.Count() > 0)
                {
                    return "DATA_USE";
                }
                unitOfWork.Delete(objMast);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public static decimal GetUnitLeaveDeductionAmoun(HRLeaveType leaveType, decimal? salary, decimal numDayInMonth, decimal workingHour)
        {
            if (!salary.HasValue)
            {
                return 0m;
            }

            decimal result = 0;
            if (leaveType.Foperand == "B")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary - leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary + leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary * leaveType.Soperand);
                }
            }
            else if (leaveType.Foperand == "B/W")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth - leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth + leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth * leaveType.Soperand);
                }
            }
            else if (leaveType.Foperand == "B/D*H")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth * workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth - workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / leaveType.Soperand);
                }
            }

            return result;
        }

        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("BALANCE_TYPE_SELECT", ClsFilterGeneral.LoadDataBalanceType());
            keyValues.Add("GENDER_SELECT", new SYDataList(unitOfWork, "GENDER_SELECT").ListData);
            keyValues.Add("LEAVETYPE_SELECT", new SYDataList(unitOfWork, "LEAVETYPE").ListData);
            keyValues.Add("Operator_SELECT",new SYDataList(unitOfWork, "Operator").ListData);
            keyValues.Add("Leave_SELECT", unitOfWork.Repository<HRLeaveType>().Queryable().ToList());

            return keyValues;
        }

        #region Leave Policy
        public void OnIndexLoadingPolicy()
        {
            ListLeaveProRate = unitOfWork.Repository<HRLeaveProRate>().Queryable();
            ListLHourPolicy = unitOfWork.Repository<HRLeaveHourPolicy>().Queryable();
            ListLeavePolicy = unitOfWork.Repository<HRLeaveDedPolicy>().Queryable();
        }
        public void OnIndexPolicy()
        {
            ListLeaveProRate = unitOfWork.Repository<HRLeaveProRate>().Queryable();
        }
        public void OnIndexHourPolicy()
        {
            ListLHourPolicy = unitOfWork.Repository<HRLeaveHourPolicy>().Queryable();
        }
        public void OnIndexCondition()
        {
            ListLeavePolicy = unitOfWork.Repository<HRLeaveDedPolicy>().Queryable();
        }
        public string OnGridModifyPolicy(HRLeaveProRate MModel, string Action)
        {
            OnLoad();
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
                    unitOfWork.Delete(MModel);
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
        public string OnGridModifyHour(HRLeaveHourPolicy MModel, string Action)
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
                    unitOfWork.Delete(MModel);
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
        public string OnGridModifyLeaveCondition(HRLeaveDedPolicy MModel, string Action)
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
                    unitOfWork.Delete(MModel);
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
        public Dictionary<string, dynamic> OnDataSelectorPolicy(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("RESIGN_SELECT", ClsFilterGeneral.LoadDataLeavePolicy());
            keyValues.Add("ENTITLE_SELECT", unitOfWork.Repository<HRSetEntitleH>().Queryable().ToList());
            keyValues.Add("LEAVE_SELECT", unitOfWork.Repository<HRLeaveType>().Queryable().ToList());
            keyValues.Add("LEAVE_WORK_TYPE", new SYDataList(unitOfWork, "LEAVE_WORKTYPE").ListData);

            return keyValues;
        }

        #endregion
    }
}