using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.MD
{
    public class ClsExWorkFlowPreference : IClsExWorkFlowPreference
    {
        IUnitOfWork unitOfWork;
        public List<CFWorkFlow> ListWF { get; set; }
        public List<ExCfWFApprover> ListWFApprover { get; set; }
        public List<ExCfWFSalaryApprover> ListSalaryApprover { get; set; }
        public List<ExCfWorkFlowItem> ListWFItem { get; set; }
        public List<ExCFWFDepartmentApprover> ListWFDepartmentApprover { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsExWorkFlowPreference()
        {
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListWF = unitOfWork.Repository< CFWorkFlow>().Queryable().ToList();
            ListWFApprover = unitOfWork.Repository < ExCfWFApprover>().Queryable().ToList();
            ListWFItem = unitOfWork.Repository < ExCfWorkFlowItem>().Queryable().ToList();
            ListSalaryApprover = unitOfWork.Repository < ExCfWFSalaryApprover>().Queryable().ToList();
            ListWFDepartmentApprover = unitOfWork.Repository <ExCFWFDepartmentApprover>().Queryable().ToList();
        }
        public void OnIndexWorkFLow()
        {
            ListWF = unitOfWork.Repository<CFWorkFlow>().Queryable().ToList();
        }
        public string OnGridModifyWorkFLow(CFWorkFlow MModel, string Action)
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
                    var objCheck = unitOfWork.Repository<CFWorkFlow>().Queryable().FirstOrDefault(w => w.WFObject == MModel.WFObject);
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
        public void OnIndexByBranch()
        {
            ListWFApprover = unitOfWork.Repository<ExCfWFApprover>().Queryable().ToList();
        }
        public string OnGridModifyByBranch(ExCfWFApprover MModel, string Action)
        {
            OnLoad();
            try
            {
                if(Action!="DELETE")
                {
                    var objStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.Employee);
                    if (objStaff != null)
                    {
                        MModel.EmployeeName = objStaff.AllName;
                    }
                }
                if (Action == "ADD")
                {
                    if (string.IsNullOrEmpty(MModel.Branch)) return "INV_COMPANY";
                    if (string.IsNullOrEmpty(MModel.WFObject)) return "INV_WFObject";
                    if (string.IsNullOrEmpty(MModel.Employee)) return "INV_EMP";
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Repository<ExCfWFApprover>().Queryable().FirstOrDefault(w => w.ID == MModel.ID);
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
        public void OnIndexByDepartment()
        {
            ListWFDepartmentApprover = unitOfWork.Repository<ExCFWFDepartmentApprover>().Queryable().ToList();
        }
        public string OnGridModifyByDepartment(ExCFWFDepartmentApprover MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action == "ADD")
                {
                    if (string.IsNullOrEmpty(MModel.WFObject)) return "INV_WFObject";
                    if (string.IsNullOrEmpty(MModel.CompanyCode)) return "INV_COMPANY";
                    if (string.IsNullOrEmpty(MModel.Department)) return "INV_DEPARTMENT";
                    if (string.IsNullOrEmpty(MModel.Employee)) return "INV_EMP";
                    var objStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.Employee);
                    if (objStaff != null)
                    {
                        MModel.EmployeeName = objStaff.AllName;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var objStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.Employee);
                    if (objStaff != null)
                    {
                        MModel.EmployeeName = objStaff.AllName;
                    }
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Repository<ExCFWFDepartmentApprover>().Queryable().FirstOrDefault(w => w.ID == MModel.ID);
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
        public void OnIndexWFItem()
        {
            ListWFItem = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().ToList();
        }
        public string OnGridModifyWFItem(ExCfWorkFlowItem MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action != "DELETE")
                {
                    var Num = unitOfWork.Repository<BSNumberRank>().FirstOrDefault(w => w.NumberObject == MModel.NumberRank);
                    MModel.NumberRankItem = Num.Length.ToString();
                    MModel.ScreenID = MModel.ScreenID.Trim();
                    MModel.DocType = MModel.DocType.Trim().ToUpper();
                    MModel.IsRequiredApproval = true;
                }
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
                    var objCheck = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == MModel.ScreenID && w.DocType == MModel.DocType);
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
        public void OnIndexSalaryApprover()
        {
            ListSalaryApprover = unitOfWork.Repository<ExCfWFSalaryApprover>().Queryable().ToList();
        }
        public string OnGridModifySalaryApprover(ExCfWFSalaryApprover MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action == "ADD")
                {
                    var objStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.Employee);
                    if (objStaff != null)
                    {
                        MModel.EmployeeName = objStaff.AllName;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var objStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == MModel.Employee);
                    if (objStaff != null)
                    {
                        MModel.EmployeeName = objStaff.AllName;
                    }
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Repository<ExCfWFSalaryApprover>().Queryable().FirstOrDefault(w => w.ID == MModel.ID);
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

        public Dictionary<string, dynamic> OnDataSelectorLoading()
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilterJob = new ClsFilterJob();
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            keyValues.Add("STAFF_LIST", filterStaff.OnLoandingEmployee());
            keyValues.Add("WF_LIST", unitOfWork.Repository<CFWorkFlow>().Queryable().ToList());
            keyValues.Add("WF_LIST_B", unitOfWork.Repository<CFWorkFlow>().Queryable().Where(w => w.ByDepartment != true).ToList());
            keyValues.Add("BRANCH_LIST", clsFilterJob.LoadBranch());
            keyValues.Add("NUMBER_LIST", unitOfWork.Repository<BSNumberRank>().Queryable().ToList());
            keyValues.Add("TELEGRAM_SELECT", unitOfWork.Repository<TelegramBot>().Queryable().ToList());
            keyValues.Add("DEPARTMENT_LIST", clsFilterJob.LoadDepartment());
            keyValues.Add("WF_LIST_DP", unitOfWork.Repository<CFWorkFlow>().Queryable().Where(w => w.ByDepartment == true).ToList());

            return keyValues;
        }
    }
}
