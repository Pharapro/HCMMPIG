using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.FT;
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

namespace Humica.Performance
{
    public class CLsKPIPlan : IClsKPIPlan
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRKPIPlanHeader Header { get; set; }
        public FTFilerIndex Filter { get; set; }
        public List<HRKPIPlanHeader> ListHeader { get; set; }
        public List<HRKPIPlanHeader> ListHeaderPending { get; set; }
        public List<HRKPIPlanItem> ListItem { get; set; }
        public CLsKPIPlan()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public void OnIndexLoading(bool IsESS = false)
        {
            OnLoad();
            if (Filter == null)
            {
                Filter = new FTFilerIndex();
                Filter.InYear = DateTime.Now.Year;
            }
            if (IsESS)
            {
                string pending = SYDocumentStatus.PENDING.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string UserName = User.UserName;
                ListHeader = unitOfWork.Set<HRKPIPlanHeader>().Where(w => w.InYear == Filter.InYear && (w.PlanerCode == UserName
                || w.PICCode == UserName) && (w.Status != pending && w.Status != Open)).ToList();
            }
            else
            {
                ListHeader = unitOfWork.Set<HRKPIPlanHeader>().Where(w => w.InYear == Filter.InYear).ToList();
            }
            ListHeader = ListHeader.OrderByDescending(w => w.DocumentDate).ToList();
        }
        public void OnIndexLoadingPending()
        {
            string pending = SYDocumentStatus.PENDING.ToString();
            string UserName = User.UserName;
            ListHeaderPending = unitOfWork.Set<HRKPIPlanHeader>().Where(w => (w.PlanerCode == UserName || w.PICCode == UserName) && w.Status == pending).ToList();
            ListHeaderPending = ListHeaderPending.OrderByDescending(w => w.DocumentDate).ToList();
        }
        public virtual void OnCreatingLoading()
        {
            Header = new HRKPIPlanHeader();
            Header.TotalWeight = 0;
            Header.KPIAverage = 0;
            Header.DocumentDate = DateTime.Now;
            Header.Dateline = DateTime.Now;
            Header.Status = SYDocumentStatus.OPEN.ToString();
            Header.InYear = DateTime.Now.Year;
            ListItem = new List<HRKPIPlanItem>();
        }
        public string Create()
        {
            OnLoad();
            try
            {
                BeforSave(Header);

                var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Header.PlanerCode);
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                //var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                var objNumber = new CFNumberRank(objCF.NumberRank, Staff.CompanyCode, Header.DocumentDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.Code = objNumber.NextNumberRank;

                foreach (var read in ListItem.ToList())
                {
                    read.KPIPlanCode = Header.Code;
                    unitOfWork.Add(read);
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string OnGridModify(HRKPIPlanItem MModel, string Action)
        {
            MessageError = "";
            decimal? TotalWeight = 0;

            if (Action == "ADD")
            {
                TotalWeight = ListItem.Sum(w => w.Weight) + MModel.Weight;
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListItem.Where(w => w.Indicator == MModel.Indicator).FirstOrDefault();
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
                var objCheck = ListItem.Where(w => w.Indicator == MModel.Indicator).FirstOrDefault();
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
            var check = ListItem.Where(w => w.Indicator == MModel.Indicator).ToList();
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
        public string BeforSave(HRKPIPlanHeader PlanHeader)
        {
            if (PlanHeader.KPICategory == null || PlanHeader.CriteriaType == null)
            {
                return "CATEGORY_EN";
            }
            var ObjMatch = unitOfWork.Set<HRKPIPlanHeader>().Where(w => w.KPIType == PlanHeader.KPICategory && w.CriteriaType == PlanHeader.CriteriaType &&
            w.KPIType == PlanHeader.KPIType && w.PlanerCode == PlanHeader.PlanerCode && w.PICCode == PlanHeader.PICCode).ToList();
            if (ObjMatch.Count() > 0)
            {
                return "DUPL_KEY";
            }
            var KPIType = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == PlanHeader.KPIType);
            if (KPIType == null)
            {
                return "DOC_INV";
            }
            PlanHeader.KPIAverage = KPIType.KPIAverage;
            var listCategory = unitOfWork.Set<HRKPICategory>().FirstOrDefault(w => w.CategoryCode == PlanHeader.KPICategory);
            PlanHeader.Category = listCategory.CatgoryDescription;
            PlanHeader.CreatedBy = User.UserName;
            PlanHeader.CreatedOn = DateTime.Now;
            return SYConstant.OK;
        }
        public string Update(string id, bool IS_ESS = false)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
                var listHRKPIItem = unitOfWork.Set<HRKPIPlanItem>().Where(w => w.KPIPlanCode == id).ToList();
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                objMatch.CriteriaType = Header.CriteriaType;
                var listCategory = unitOfWork.Set<HRKPICategory>().FirstOrDefault(w => w.CategoryCode == Header.KPICategory);
                objMatch.Category = listCategory.CatgoryDescription;
                //objMatch.Category = Header.Category;
                //objMatch.KPICategory = Header.KPICategory;
                //objMatch.TotalWeight = 0;

                foreach (var read in listHRKPIItem)
                {
                    unitOfWork.Delete(read);
                }
                decimal? TotalWeight = 0;
                foreach (var read in ListItem.ToList())
                {
                    TotalWeight += read.Weight.Value;
                    read.KPIPlanCode = Header.Code;
                    unitOfWork.Add(read);
                }
                if (Header.ExpectedDate.HasValue)
                {
                    if (Header.ExpectedDate.Value > Header.Dateline.Value)
                    {
                        return "EXPECTED_DATE";
                    }
                }
                objMatch.TotalWeight = TotalWeight;
                if (IS_ESS)
                {
                    objMatch.Status = SYDocumentStatus.PROCESSING.ToString();
                }
                objMatch.Description = Header.Description;
                objMatch.ExpectedDate = Header.ExpectedDate;
                objMatch.PICCode = Header.PICCode;
                objMatch.PICName = Header.PICName;
                objMatch.PICPosition = Header.PICPosition;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string Delete(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
                var listHRKPIItem = unitOfWork.Set<HRKPIPlanItem>().Where(w => w.KPIPlanCode == id).ToList();

                if (objMatch == null)
                {
                    return "EDCUATION _NE";
                }

                foreach (var read in listHRKPIItem)
                {
                    unitOfWork.Delete(read);
                }
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string Code = (string)keys[0];
            this.Header = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == Code);
            if (this.Header != null)
            {
                ListItem = unitOfWork.Set<HRKPIPlanItem>().Where(w => w.KPIPlanCode == Code).ToList();
            }
        }
        public string ReleaseTheDoc(string id, bool IS_ESS = false)
        {
            string ID = id;
            try
            {
                var objMatch = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (IS_ESS && objMatch.Status != SYDocumentStatus.PROCESSING.ToString())
                {
                    return "INV_DOC";
                }
                else if (IS_ESS == false && objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }

        public string RequestRelease(string id)
        {
            string ID = id;
            try
            {
                var objMatch = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
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

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public HRKPIPlanHeader GetDataCopy(string id)
        {
            Header = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
            if (Header != null)
            {
                ListItem = unitOfWork.Set<HRKPIPlanItem>().Where(w => w.KPIPlanCode == id).ToList();
                return Header;
            }
            return null;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("LIST_Indicator", unitOfWork.Set<HRKPIIndicator>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("LIST_GROUPKPI", unitOfWork.Set<HRKPIType>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("KPI_LIST", unitOfWork.Set<HRKPIList>().ToList());
            keyValues.Add("LIST_KPI_CATEGORY", unitOfWork.Set<HRKPICategory>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("KPI_Measure", ClsMeasure.LoadDataMeasure());
            keyValues.Add("KPI_Symbol", ClsMeasure.LoadDataSymbol());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataSelectorKPI(params object[] keys)
        {
            string Department = (string)keys[0];
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("KPI_LIST", unitOfWork.Set<HRKPIList>().Where(w => string.IsNullOrEmpty(w.Department) || w.Department == Department).ToList());
            keyValues.Add("LIST_Indicator", unitOfWork.Set<HRKPIIndicator>().Where(w => (string.IsNullOrEmpty(w.Department) || w.Department == Department) && w.IsActive == true).ToList());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataList(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("STAFF_LIST", unitOfWork.Set<HR_STAFF_VIEW>().Where(w => w.StatusCode == "A").ToList());

            return keyValues;
        }
    }
    public class ClsKPI_InputType
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public static List<ClsKPI_InputType> LoadDataKPIInput()
        {
            List<ClsKPI_InputType> listData = new List<ClsKPI_InputType>();
            listData.Add(new ClsKPI_InputType() { Code = "01", Description = "Currency" });
            listData.Add(new ClsKPI_InputType() { Code = "02", Description = "Quantity" });
            listData.Add(new ClsKPI_InputType() { Code = "03", Description = "Percentage" });
            return listData;
        }
    }
}