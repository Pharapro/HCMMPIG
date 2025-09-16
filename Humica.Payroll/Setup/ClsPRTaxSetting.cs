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

namespace Humica.Payroll
{
    public class ClsPRTaxSetting : IClsPRTaxSetting
    {
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        protected IUnitOfWork unitOfWork;
        public List<PRTaxSetting> ListHeader { get; set; }
        public List<PRExceptType> ListExceptType { get; set; }
        public PRPayrollSetting PayrollSetting { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public ClsPRTaxSetting()
        {
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListHeader = unitOfWork.Set<PRTaxSetting>().OrderBy(w => w.TaxFrom).ToList();
            ListExceptType = unitOfWork.Set<PRExceptType>().ToList();
            PayrollSetting = unitOfWork.Set<PRPayrollSetting>().FirstOrDefault();
        }
        public void OnIndexTaxSetting()
        {
            ListHeader = unitOfWork.Set<PRTaxSetting>().OrderBy(w => w.TaxFrom).ToList();
        }
        public void OnIndexExceptType()
        {
            ListExceptType = unitOfWork.Set<PRExceptType>().ToList();
        }
        public string OnGridModifyTaxSetting(PRTaxSetting MModel, string Action)
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
                    var objCheck = unitOfWork.Set<PRTaxSetting>().FirstOrDefault(w => w.TranNo == MModel.TranNo);
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

        public string UpdateSetting()
        {
            OnLoad();
            try
            {
                var ObjMatch = unitOfWork.Set<PRPayrollSetting>().FirstOrDefault();
               
                if (ObjMatch == null)
                {
                    unitOfWork.Add(PayrollSetting);
                }
                else
                {
                    ObjMatch.BaseCurrency = PayrollSetting.BaseCurrency;
                    ObjMatch.Spouse = PayrollSetting.Spouse;
                    ObjMatch.Child = PayrollSetting.Child;
                    ObjMatch.TaxBase = PayrollSetting.TaxBase;
                    unitOfWork.Update(ObjMatch);
                }
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string OnGridModifyExceptType(PRExceptType MModel, string Action)
        {
            OnLoad();
            try
            {
                MModel.Code = MModel.Code.Trim();
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
                    var objCheck = unitOfWork.Set<PRExceptType>().FirstOrDefault(w => w.Code == MModel.Code);
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
            keyValues.Add("SELECT_CURRENCY", unitOfWork.Set<HRCurrency>().ToList());
            keyValues.Add("SELECT_VALUES", ClsFilterGeneral.OnLoadDataValue());
            keyValues.Add("SELECT_TAXBASE", ClsFilterGeneral.OnLoadTaxBase());
            return keyValues;
        }
    }
}
