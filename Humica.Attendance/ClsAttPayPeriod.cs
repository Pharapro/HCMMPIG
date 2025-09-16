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

namespace Humica.Attendance
{
    public class ClsAttPayPeriod: IClsAttPayPeriod
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<ATPayperiod> ListPeriod { get; set; }
        public ATPayperiod Header { get; set; }
        public ClsAttPayPeriod()
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
            ListPeriod = unitOfWork.Repository<ATPayperiod>().Queryable().OrderByDescending(w => w.FromDate).ToList();
        }
        public void OnIndexLoadingPeriod()
        {
            ListPeriod = unitOfWork.Repository<ATPayperiod>().Queryable().OrderByDescending(w => w.FromDate).ToList();
        }
        public string OnGridModifyPeriod(ATPayperiod MModel, string Action)
        {
            OnLoad();
            try
            {
                MModel.PeriodString = MModel.ToDate.ToString("MM-yyyy");
                bool isOverlap = unitOfWork.Repository<ATPayperiod>().Queryable().Any(w => w.FromDate <= MModel.ToDate
                && w.ToDate >= MModel.FromDate && w.PeriodID != MModel.PeriodID);
                if (isOverlap)
                {
                    return "PER_EX";
                }
                if (Action == "ADD")
                {
                    if (MModel.FromDate > MModel.ToDate)
                    {
                        return "IN_DATE_RANK";
                    }
                    var itempr = unitOfWork.Repository<PRPayPeriodItem>().Queryable()
                       .FirstOrDefault(w => w.EndDate.Month == MModel.ToDate.Month && w.EndDate.Year == MModel.ToDate.Year);
                    if (itempr != null)
                    {
                        itempr.ATStartDate = MModel.FromDate;
                        itempr.ATEndDate = MModel.ToDate;
                        unitOfWork.Update(itempr);
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    if (MModel.FromDate > MModel.ToDate)
                    {
                        return "IN_DATE_RANK";
                    }
                    var itempr = unitOfWork.Repository<PRPayPeriodItem>().Queryable()
                   .FirstOrDefault(w => w.EndDate.Month == MModel.ToDate.Month && w.EndDate.Year == MModel.ToDate.Year);
                    if (itempr != null)
                    {
                        itempr.ATStartDate = MModel.FromDate;
                        itempr.ATEndDate = MModel.ToDate;
                        unitOfWork.Update(itempr);
                    }
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Repository<ATPayperiod>().FirstOrDefault(w => w.PeriodID == MModel.PeriodID);
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


