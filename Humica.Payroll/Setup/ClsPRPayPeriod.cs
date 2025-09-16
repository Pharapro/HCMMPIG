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
    public class ClsPRPayPeriod : IClsPRPayPeriod
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<PRPayPeriodItem> ListPeriod { get; set; }
        protected IUnitOfWork unitOfWork;
        public ClsPRPayPeriod()
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
            ListPeriod = unitOfWork.Set<PRPayPeriodItem>().OrderByDescending(w => w.StartDate).ToList();
        }
        public void OnIndexLoadingPeriod()
        {
            ListPeriod = unitOfWork.Set<PRPayPeriodItem>().OrderByDescending(w => w.StartDate).ToList();
        }
        public string OnGridModifyPeriod(PRPayPeriodItem MModel, string Action)
        {
            OnLoad();
            try
            {
                MModel.PeriodString = MModel.EndDate.ToString("MM-yyyy");
                MModel.FiscalYear = MModel.EndDate.Year;
                bool isOverlap = unitOfWork.Set<PRPayPeriodItem>().Any(w => w.StartDate <= MModel.EndDate
                && w.EndDate >= MModel.StartDate && w.PeriodID != MModel.PeriodID);
                if (isOverlap)
                {
                    return "PER_EX";
                }
                if (Action == "ADD")
                {
                    if (MModel.StartDate > MModel.EndDate)
                    {
                        return "IN_DATE_RANK";
                    }
                    MModel.ATStartDate = MModel.StartDate;
                    MModel.ATEndDate = MModel.EndDate;
                    var att = unitOfWork.Repository<ATPayperiod>().Queryable()
                         .FirstOrDefault(w => w.ToDate.Month == MModel.EndDate.Month && w.ToDate.Year == MModel.EndDate.Year);
                    if (att != null)
                    {
                        MModel.ATStartDate = att.FromDate;
                        MModel.ATEndDate = att.ToDate;
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    if (MModel.StartDate > MModel.EndDate)
                    {
                        return "IN_DATE_RANK";
                    }
                    MModel.ATStartDate = MModel.StartDate;
                    MModel.ATEndDate = MModel.EndDate;
                    var att = unitOfWork.Repository<ATPayperiod>().Queryable()
                         .FirstOrDefault(w => w.ToDate.Month == MModel.EndDate.Month && w.ToDate.Year == MModel.EndDate.Year);
                    if (att != null)
                    {
                        MModel.ATStartDate = att.FromDate;
                        MModel.ATEndDate = att.ToDate;
                    }
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<PRPayPeriodItem>().FirstOrDefault(w => w.PeriodID == MModel.PeriodID);
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
    }
}