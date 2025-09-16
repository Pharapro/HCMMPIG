using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class ClsPRExchangeRate : IClsPRExchangeRate
    {
        IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRExchRate Header { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PRExchRate> ListHeader { get; set; }
        public List<PRBiExchangeRate> ListBiHeader { get; set; }
        public ClsPRExchangeRate()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public void OnIndexLoadingExchangeRate()
        {
            ListHeader = unitOfWork.Set<PRExchRate>().OrderByDescending(w => w.PeriodID).ToList();
        }
        public void OnIndexLoadingBiExchangeRate()
        {
            ListBiHeader = unitOfWork.Set<PRBiExchangeRate>().OrderByDescending(w => w.Period).ToList();
        }
        public string OnGridModifyExchangeRate(PRExchRate MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action == "ADD")
                {
                    bool isOverlap = unitOfWork.Set<PRExchRate>().Any(w => w.PeriodID == MModel.PeriodID);
                    if (isOverlap)
                    {
                        return "PER_EX";
                    }
                    MModel.CreateOn = DateTime.Now;
                    MModel.CreateBy = User.UserName;
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    MModel.ChangedOn = DateTime.Now;
                    MModel.ChangedBy = User.UserName;
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<PRExchRate>().FirstOrDefault(w => w.TranNo == MModel.TranNo);
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
        public string OnGridModifyBiExchangeRate(PRBiExchangeRate MModel, string Action)
        {
            OnLoad();
            try
            {
                if (Action == "ADD")
                {
                    bool isOverlap = unitOfWork.Set<PRBiExchangeRate>().Any(w => w.Period == MModel.Period);
                    if (isOverlap)
                    {
                        return "PER_EX";
                    }
                    MModel.CreatedOn = DateTime.Now;
                    MModel.CreatedBy = User.UserName;
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    MModel.ChangedOn = DateTime.Now;
                    MModel.ChangedBy = User.UserName;
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<PRBiExchangeRate>().FirstOrDefault(w => w.TranNo == MModel.TranNo);
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
                if (e.InnerException != null)
                {
                    if (e.InnerException.InnerException != null)
                        return e.InnerException.InnerException.Message;
                    return e.InnerException.Message;
                }
                    return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilter = new ClsFilterJob();
            keyValues.Add("PERIOD_SELECT", clsFilter.LoadPeriod());
            keyValues.Add("CURRENCY_SELECT", unitOfWork.Set<HRCurrency>().ToList());
            return keyValues;
        }
    }
}