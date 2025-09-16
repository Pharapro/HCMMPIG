using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Performance
{
    public class ClsKPIPlanIndividual
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRKPIPlanIndividual Header { get; set; }
        public List<HRKPIPlanIndividual> ListHeader { get; set; }
        //public List<HRKPIPlanHeader> ListPlanPending { get; set; }
        public List<HRKPIPlanIndividual> ListPlanPending { get; set; }
        public List<HRKPIPlanIndivItem> ListItem { get; set; }
        public ClsKPIPlanIndividual()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public async Task<List<HRKPIList>> LoadDataHRKPIList(string Department)
        {
            List<HRKPIList> _list = new List<HRKPIList>();
            _list = await DB.HRKPILists.Where(w => string.IsNullOrEmpty(w.Department)
            || w.Department == Department).ToListAsync();
            return _list;
        }
        public async Task<List<HRKPIIndicator>> LoadDataIndicator(string Department)
        {
            List<HRKPIIndicator> _list = new List<HRKPIIndicator>();
            _list = await DB.HRKPIIndicators.Where(w => (string.IsNullOrEmpty(w.Department)
            || w.Department == Department) && w.IsActive == true).ToListAsync();
            return _list;
        }
        public async Task<List<HRKPIPlanHeader>> LoadDataPlanPending(bool IsESS = false)
        {
            string Release = SYDocumentStatus.RELEASED.ToString();
            DB = new HumicaDBContext();
            List<HRKPIPlanHeader> _lstTemp = new List<HRKPIPlanHeader>();
            if (IsESS)
            {
                string UserName = User.UserName;
                _lstTemp = await DB.HRKPIPlanHeaders.Where(w => w.PlanerCode == UserName && w.Status == Release).ToListAsync();
            }
            else
            {
                _lstTemp = await DB.HRKPIPlanHeaders.Where(w => w.Status == Release).ToListAsync();
            }
            return _lstTemp;
        }
        public async Task<List<HRKPIPlanIndividual>> LoadDataPlanInPending(bool IsESS = false)
        {
            string PENDING = SYDocumentStatus.PENDING.ToString();
            DB = new HumicaDBContext();
            List<HRKPIPlanIndividual> _lstTemp = new List<HRKPIPlanIndividual>();
            if (IsESS)
            {
                string UserName = User.UserName;
                _lstTemp = await DB.HRKPIPlanIndividuals.Where(w => w.PlanerCode == UserName && w.ReStatus == PENDING).ToListAsync();
            }
            else
            {
                _lstTemp = await DB.HRKPIPlanIndividuals.Where(w => w.ReStatus == PENDING).ToListAsync();
            }
            return _lstTemp;
        }
        public async Task<List<HRKPIPlanIndividual>> LoadData(bool IsESS = false)
        {
            DB = new HumicaDBContext();
            List<HRKPIPlanIndividual> _lstTemp = new List<HRKPIPlanIndividual>();
            if (IsESS)
            {
                string UserName = User.UserName;
                _lstTemp = await DB.HRKPIPlanIndividuals.Where(w => w.PlanerCode == UserName ||
                w.PICCode == UserName || w.DirectedByCode == UserName).ToListAsync();
            }
            else
            {
                _lstTemp = await DB.HRKPIPlanIndividuals.ToListAsync();
            }
            return _lstTemp;
        }
        public async Task<List<HRKPIPlanIndivItem>> LoadDataPlanItem(string Code)
        {
            var DBI = new HumicaDBContext();
            List<HRKPIPlanIndivItem> _lstTemp = new List<HRKPIPlanIndivItem>();
            var _lstTempPlan = await DB.HRKPIPlanItems.Where(w => w.KPIPlanCode == Code).ToListAsync();
            foreach (var item in _lstTempPlan)
            {
                var obj = new HRKPIPlanIndivItem();
                obj.Indicator = item.Indicator;
                obj.Measure = item.Measure;
                obj.ItemCode = item.ItemCode;
                obj.KPI = item.KPI;
                obj.ActionPlan = item.ActionPlan;
                obj.Remark = item.Remark;
                obj.Target = item.Target;
                obj.Weight = item.Weight.Value;
                obj.StartDate = item.StartDate;
                obj.EndDate = item.EndDate;
                _lstTemp.Add(obj);
            }
            return _lstTemp;
        }
        public HRKPIPlanIndividual getSetupHeader(string id)
        {
            Header = DB.HRKPIPlanIndividuals.FirstOrDefault(w => w.Code == id);
            if (Header != null)
            {
                return Header;
            }
            return null;
        }
        public async Task<HRKPIPlanIndividual> GetDataPlanIndivid(string KPICode)
        {
            Header = new HRKPIPlanIndividual();
            ListItem = new List<HRKPIPlanIndivItem>();
            Header = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == KPICode);
            //ListItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == KPICode).ToListAsync();

            return Header;
        }
        public async Task<HRKPIPlanIndividual> GetDataPlan(string KPICode)
        {
            Header = new HRKPIPlanIndividual();
            ListItem = new List<HRKPIPlanIndivItem>();
            var Plan = await DB.HRKPIPlanHeaders.FirstOrDefaultAsync(w => w.Code == KPICode);
            if (Plan != null)
            {
                Header.PlanRef = KPICode;
                Header.DocumentDate = DateTime.Now;
                Header.PeriodFrom = Plan.PeriodFrom;
                Header.PeriodTo = Plan.PeriodTo;
                Header.TotalWeight = Plan.TotalWeight;
                Header.KPIAverage = Plan.KPIAverage;
                Header.CriteriaType = Plan.CriteriaType;
                Header.CriteriaName = Plan.CriteriaName;
                Header.KPIType = Plan.KPIType;
                Header.PlanerCode = Plan.PlanerCode;
                Header.PlanerName = Plan.PlanerName;
                Header.PlanerPosition = Plan.PlanerPosition;

                var ListPlanItem = await DB.HRKPIPlanItems.Where(w => w.KPIPlanCode == KPICode).ToListAsync();

                foreach (var item in ListPlanItem)
                {
                    var obj = new HRKPIPlanIndivItem();
                    obj.Indicator = item.Indicator;
                    obj.Measure = item.Measure;
                    obj.Symbol = item.Symbol;
                    obj.ItemCode = item.ItemCode;
                    obj.KPI = item.KPI;
                    obj.ActionPlan = item.ActionPlan;
                    obj.Remark = item.Remark;
                    obj.Target = item.Target;
                    obj.Weight = item.Weight.Value;
                    obj.StartDate = item.StartDate;
                    obj.EndDate = item.EndDate;
                    ListItem.Add(obj);
                }
            }
            Header.Status = SYDocumentStatus.OPEN.ToString();

            return Header;
        }
        public async Task<string> CreateKPIPlan(string _Screen_ID)
        {
            DB = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                if (string.IsNullOrEmpty(Header.PlanRef))
                {
                    return "CATEGORY_EN";
                }
                var Staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == Header.PlanerCode);
                var objCF = await DB.ExCfWorkFlowItems.FirstOrDefaultAsync(w => w.ScreenID == _Screen_ID);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.NumberRank, Staff.CompanyCode, Header.DocumentDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.Code = objNumber.NextNumberRank;

                foreach (var read in ListItem.ToList())
                {
                    read.KPIPlanCode = Header.Code;
                    DB.HRKPIPlanIndivItems.Add(read);
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HRKPIPlanIndividuals.Add(Header);
                int row = await DB.SaveChangesAsync();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
            }
        }
        public async Task<string> CreateKPIPlanIndvid(string id, bool IS_ESS = false)
        {
            DB = new HumicaDBContext();
            using (var DBM = new HumicaDBContext())
            {
                using (var transaction = DBM.Database.BeginTransaction())
                {
                    try
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = false;
                        var objMatch = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == id);
                        var listHRKPIItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == id).ToListAsync();
                        if (objMatch == null)
                        {
                            return "INV_DOC";
                        }
                        objMatch.CriteriaType = Header.CriteriaType;

                        foreach (var read in listHRKPIItem)
                        {
                            DBM.HRKPIPlanIndivItems.Attach(read);
                            DBM.HRKPIPlanIndivItems.Remove(read);
                        }

                        decimal? TotalWeight = 0;
                        foreach (var read in ListItem.ToList())
                        {
                            TotalWeight += read.Weight.Value;
                            read.KPIPlanCode = Header.Code;
                            DBM.HRKPIPlanIndivItems.Add(read);
                        }
                        Header.TotalWeight = TotalWeight;
                        if (IS_ESS)
                        {
                            Header.ReStatus = SYDocumentStatus.PROCESSING.ToString();
                        }
                        Header.CreatedOn = objMatch.CreatedOn;
                        Header.CreatedBy = objMatch.CreatedBy;
                        Header.ChangedBy = User.UserName;
                        Header.ChangedOn = DateTime.Now;

                        DBM.Entry(Header).State = System.Data.Entity.EntityState.Modified;
                        int row = await DBM.SaveChangesAsync();
                        transaction.Commit();
                        return SYConstant.OK;
                    }
                    catch (DbEntityValidationException e)
                    {
                        transaction.Rollback();
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = ScreenId;
                        log.UserId = User.UserName;
                        log.DocurmentAction = Header.Code;
                        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        return "EE001";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log the exception
                        return "EE001";
                    }
                    finally
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }
        public async Task<string> EditSetUpKPI(string id, bool IS_ESS = false)
        {
            DB = new HumicaDBContext();
            using (var DBM = new HumicaDBContext())
            {
                using (var transaction = DBM.Database.BeginTransaction())
                {
                    try
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = false;
                        var objMatch = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == id);
                        var listHRKPIItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == id).ToListAsync();
                        if (objMatch == null)
                        {
                            return "INV_DOC";
                        }
                        objMatch.CriteriaType = Header.CriteriaType;
                        //var listCategory = await DB.HRKPICategories.FirstOrDefaultAsync(w => w.CategoryCode == Header.KPICategory);
                        //Header.Category = listCategory.CatgoryDescription;
                        //objMatch.Category = Header.Category;
                        //objMatch.KPICategory = Header.KPICategory;
                        //objMatch.TotalWeight = 0;

                        foreach (var read in listHRKPIItem)
                        {
                            DBM.HRKPIPlanIndivItems.Attach(read);
                            DBM.HRKPIPlanIndivItems.Remove(read);
                        }

                        decimal? TotalWeight = 0;
                        foreach (var read in ListItem.ToList())
                        {
                            TotalWeight += read.Weight.Value;
                            read.KPIPlanCode = Header.Code;
                            DBM.HRKPIPlanIndivItems.Add(read);
                        }
                        Header.TotalWeight = TotalWeight;
                        if (IS_ESS)
                        {
                            Header.Status = SYDocumentStatus.PROCESSING.ToString();
                        }
                        Header.CreatedOn = objMatch.CreatedOn;
                        Header.CreatedBy = objMatch.CreatedBy;
                        Header.ChangedBy = User.UserName;
                        Header.ChangedOn = DateTime.Now;

                        DBM.Entry(Header).State = System.Data.Entity.EntityState.Modified;
                        int row = await DBM.SaveChangesAsync();
                        transaction.Commit();
                        return SYConstant.OK;
                    }
                    catch (DbEntityValidationException e)
                    {
                        transaction.Rollback();
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = ScreenId;
                        log.UserId = User.UserName;
                        log.DocurmentAction = Header.Code;
                        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        return "EE001";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log the exception
                        return "EE001";
                    }
                    finally
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }
        public string DeleteSetupKPI(string id)
        {
            try
            {
                var objHeader = DB.HRKPIPlanIndividuals.FirstOrDefault(w => w.Code == id);
                var objItem = DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == id).ToList();

                if (objHeader == null)
                {
                    return "EDCUATION _NE";
                }
                DB.HRKPIPlanIndividuals.Remove(objHeader);
                foreach (var read in objItem)
                {
                    DB.HRKPIPlanIndivItems.Remove(read);
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                //log.ScreenId = Header.EmpID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                //log.ScreenId = Header.EmpID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public async Task<string> ReleaseTheDoc(string id)
        {
            string ID = id;
            try
            {
                var objMatch = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public async Task<string> RequestRelease(string id)
        {
            string ID = id;
            try
            {
                var objMatch = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.PENDING.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }

        public async Task<string> ModifyItem(HRKPIPlanIndivItem MModel, string Action)
        {
            MessageError = "";
            decimal? TotalWeight = 0;

            if (Action == "ADD")
            {
                TotalWeight = ListItem.Sum(w => w.Weight) + MModel.Weight;
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListItem.Where(w => w.ItemCode == MModel.ItemCode).FirstOrDefault();
                if (objCheck != null)
                {
                    ListItem.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
                TotalWeight = ListItem.Sum(w => w.Weight) + MModel.Weight;
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListItem.Where(w => w.ItemCode == MModel.ItemCode).FirstOrDefault();
                if (objCheck != null)
                {
                    ListItem.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            if (MModel.Weight == 0)
            {
                return "WEIGHT_NOT_0";
            }
            else if (MModel.Weight <= 0 || MModel.Weight > 1)
            {
                return "INV_AMOUNT";
            }
            else if (TotalWeight > 1)
            {
                return "KPI_Percent";
            }
            var check = ListItem.Where(w => w.ItemCode == MModel.ItemCode).ToList();
            if (check.Count == 0)
            {
                ListItem.Add(MModel);
            }
            else
            {
                return "ISDUPLICATED";
            }
            return SYConstant.OK;
        }

    }
}