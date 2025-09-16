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

namespace Humica.Payroll
{
    public class ClsPRConfig : IClsPRConfig
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public List<PRIncomeType> ListIncomeType { get; set; }
        public List<PRSocialSecurity> ListSocialSecurity { get; set; }
        public List<SYRoundingRule> ListRoundingRule { get; set; }
        public List<PRFringeBenefitSetting> ListFringeBenefit { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsPRConfig()
        {
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListIncomeType = unitOfWork.Repository<PRIncomeType>().Queryable().OrderBy(w => w.InOrder).ToList();
            ListSocialSecurity = unitOfWork.Repository<PRSocialSecurity>().Queryable().ToList();
            ListRoundingRule = unitOfWork.Repository<SYRoundingRule>().Queryable().ToList();
            ListFringeBenefit = unitOfWork.Repository<PRFringeBenefitSetting>().Queryable().ToList();
        }
        public void OnIndexLoadingConfig()
        {
            ListIncomeType = unitOfWork.Repository<PRIncomeType>().Queryable().OrderBy(w => w.InOrder).ToList();
        }
        public void OnIndexLoadingSoSe()
        {
            ListSocialSecurity = unitOfWork.Repository<PRSocialSecurity>().Queryable().ToList();
        }
        public void OnIndexLoadingRounding()
        {
            ListRoundingRule = unitOfWork.Repository<SYRoundingRule>().Queryable().ToList();
        }
        public void OnIndexLoadingFringeBenefit()
        {
            ListFringeBenefit = unitOfWork.Repository<PRFringeBenefitSetting>().Queryable().ToList();
        }
        public string OnGridModifyConfig(PRIncomeType MModel, string Action)
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
                    var objCheck = unitOfWork.Repository<PRIncomeType>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
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
        public string OnGridModifySoSe(PRSocialSecurity MModel, string Action)
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
                    var objCheck = unitOfWork.Repository<PRSocialSecurity>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
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
        public string OnGridModifyRounding(SYRoundingRule MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action != "DELETE")
                {
                    if (string.IsNullOrEmpty(MModel.Type))
                    {
                        return "INVALIED_TYPE";
                    }
                    if (string.IsNullOrEmpty(MModel.Currency))
                    {
                        return "INVALIED_CURRENCY";
                    }
                    if (string.IsNullOrEmpty(MModel.RoundMethod))
                    {
                        return "INVALIED_RoundMethod";
                    }
                    if (MModel.EffectiveDate == null || MModel.EffectiveDate <= DateTime.MinValue)
                    {
                        return "INVALIED_RoundMethod";
                    }
                    if (MModel.EndDate == null || MModel.EndDate <= DateTime.MinValue)
                    {
                        return "INVALIED_EndDate";
                    }
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
                    var objCheck = unitOfWork.Repository<SYRoundingRule>().Queryable().FirstOrDefault(w => w.ID == MModel.ID);
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
        public string OnGridModifyFringeBenefit(PRFringeBenefitSetting MModel, string Action)
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
                    var objCheck = unitOfWork.Repository<PRFringeBenefitSetting>().Queryable().FirstOrDefault(w => w.EmployeeType == MModel.EmployeeType);
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
            keyValues.Add("SELECT_INCOME_TYPE", unitOfWork.Repository<PRIncomeType>().Queryable().ToList());
            keyValues.Add("SELECT_VALUES", ClsFilterGeneral.OnLoadDataValue());
            keyValues.Add("SALARTYPE_SELECT", ClsFilterGeneral.LoadDataSalaryType().ToList());
            keyValues.Add("SELECT_CURRENCY", unitOfWork.Repository<HRCurrency>().Queryable().ToList());
            keyValues.Add("SELECT_EMPLOYEE_TYPE", unitOfWork.Repository<HREmpType>().Queryable().ToList());
            return keyValues;
        }
    }
}


