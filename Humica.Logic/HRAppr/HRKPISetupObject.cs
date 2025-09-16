using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace Humica.Logic.HR
{
    public class HRKPISetupObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }

        public string ScreenId { get; set; }

        //        public string DocType { get; set; }

        //        public string Doctype_AVT { get; set; }

        //        public string DocumentNo { get; set; }

        //        public string FormType { get; set; }

        //        public string InputType { get; set; }

        //        public string InputText { get; set; }

        //        public string InputAmount { get; set; }

        //        public string InputMonth { get; set; }

        //        public string InputPercentag { get; set; }

        //        public string TargetText { get; set; }

        //        public string TargetAmount { get; set; }

        //        public string TargetPercentag { get; set; }

        //        public string TargentMonth { get; set; }

        //        public string ActualType { get; set; }

        //        public string ErrorMessage { get; set; }

        //        public List<HRApprFactor> ListFactor { get; set; }

        //        public List<MDUploadTemplate> ListTemplate { get; set; }

        public FTDGeneralAccount Filter { get; set; }

        //        public List<HREmpAppraisalItem> ListScore { get; set; }

        public HRKPISetupHeader Header { get; set; }
        public List<HRKPISetupHeader> ListHeader { get; set; }
        public List<HRKPISetupItem> ListItem { get; set; }
        public List<HRKPIEmpRating> ListKPIEmpRating { get; set; }
        //public List<HRKPIRating> ListKPIRating { get; set; }
        public List<HRKPIAssignHeader> listAssignHeader { get; set; }
        //        public List<HRKPIAssignItem> listAssignItem { get; set; }
        //        public HRKPIGroupPlanHeader HeaderGroup { get; set; }

        public List<HRKPIGroupPlanHeader> ListKPIGroupHeader { get; set; }

        //        public HRKPIActivitiesHeader HeaderActivities { get; set; }

        //        public List<HRKPIActivitiesHeader> ListWorkProcess { get; set; }

        //        public List<HRKPIActivitiesItem> ListItemActivities { get; set; }

        //        public List<HRKPIActivitiesItem> ListKPIDetailRecord { get; set; }

        //        public List<HRKPIEmpConcern> ListEmpConcern { get; set; }

        //        public List<HRKPIManagemGuideLine> ListManagementGuidline { get; set; }

        public List<HRKPICategory> ListCategory { get; set; }

        //        public HRKPICategory ListIsEmployee { get; set; }

        //        public List<HRKPIJobFolloUp> ListJobFollowUp { get; set; }

        //        public HRKPIFormHeader HeaderForm { get; set; }

        //        public List<HRKPIFormItem> ListItemForm { get; set; }

        //        public List<HRKPIFormHeader> ListHeaderForm { get; set; }

        //        public List<HRKPIEmployeeAction> listEmpAction { get; set; }

        public List<HRKPISection> ListSection { get; set; }

        //        public string ApprovType { get; set; }

        public HRKPISetupObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        #region KPI Plan
        public string CreateSetupeKPI()
        {
            try
            {
                DB = new HumicaDBContext();
                HumicaDBContext DBM = new HumicaDBContext();
                if (Header.KPICategory == null || Header.KPIType == null)
                {
                    return "EE001";
                }
                if (ListItem.Count == 0)
                {
                    return "EE001";
                }
                var ObjMatch = DB.HRKPISetupHeaders.Where(w => w.KPIGroupCode == Header.KPICategory && w.KPIType == Header.KPIType &&
                w.KPIGroupCode == Header.KPIGroupCode).ToList();
                if (ObjMatch.Count() > 0)
                {
                    return "DUPL_KEY";
                }
                var objCF = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.Code = objNumber.NextNumberRank;

                foreach (var read in ListItem.ToList())
                {
                    read.KPISetupCode = Header.Code;
                    DB.HRKPISetupItems.Add(read);
                }
                foreach (var read in ListKPIEmpRating.ToList())
                {
                    read.AssignCode = Header.Code;
                    DB.HRKPIEmpRatings.Add(read);
                }
                Header.IsActive = true;
                var listCategory = DB.HRKPICategories.FirstOrDefault(w => w.CategoryCode == Header.KPICategory);
                Header.Description = listCategory.CatgoryDescription;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HRKPISetupHeaders.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.EmpID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.EmpID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditSetUpKPI(string id)
        {
            try
            {
                var objMatch = DB.HRKPISetupHeaders.FirstOrDefault(w => w.Code == id);
                var listHRKPIItem = DB.HRKPISetupItems.Where(w => w.KPISetupCode == id).ToList();
                var listHRKPIItemRating = DB.HRKPIEmpRatings.Where(w => w.AssignCode == id).ToList();
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                objMatch.KPIType = Header.KPIType;
                var listCategory = DB.HRKPICategories.FirstOrDefault(w => w.CategoryCode == Header.KPICategory);
                Header.Description = listCategory.CatgoryDescription;
                objMatch.Description = Header.Description;
                objMatch.KPICategory = Header.KPICategory;
                objMatch.TotalWeight = 0;
                foreach (var read in listHRKPIItem)
                {
                    DB.HRKPISetupItems.Remove(read);
                }
                foreach (var read in listHRKPIItemRating)
                {
                    DB.HRKPIEmpRatings.Remove(read);
                }
                decimal? TotalWeight = 0;
                //int lineItem = 0;
                foreach (var read in ListItem.ToList())
                {
                    //lineItem += 1;
                    //read.LineItem = lineItem;
                    TotalWeight += read.Weight.Value;
                    read.KPISetupCode = Header.Code;
                    DB.HRKPISetupItems.Add(read);
                }
                foreach (var read in ListKPIEmpRating.ToList())
                {
                    read.AssignCode = objMatch.Code;
                    DB.HRKPIEmpRatings.Add(read);
                }
                objMatch.TotalWeight = TotalWeight;
                DB.HRKPISetupHeaders.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.KPIType).IsModified = true;
                DB.Entry(objMatch).Property(x => x.TotalWeight).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(objMatch).Property(x => x.KPICategory).IsModified = true;

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
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
        }
        public string DeleteSetupKPI(string id)
        {
            try
            {
                var objHeader = DB.HRKPISetupHeaders.FirstOrDefault(w => w.Code == id);
                var objItem = DB.HRKPISetupItems.Where(w => w.KPISetupCode == id).ToList();
                var ObjRating = DB.HRKPIEmpRatings.Where(w => w.AssignCode == id).ToList();

                if (objHeader == null)
                {
                    return "EDCUATION _NE";
                }
                DB.HRKPISetupHeaders.Remove(objHeader);
                foreach (var read in objItem)
                {
                    DB.HRKPISetupItems.Remove(read);
                }
                foreach (var read in ObjRating)
                {
                    DB.HRKPIEmpRatings.Remove(read);
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
        #endregion

        //        public string DeleteKPIActivities(string id)
        //        {
        //            try
        //            {
        //                //Header = new HREmpEduc();
        //                var objHeader = DB.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == id);

        //                if (objHeader == null)
        //                {
        //                    return "EE001";
        //                }
        //                var objItem = DB.HRKPIActivitiesItems.Where(w => w.AVTCode == objHeader.AVTCode).ToList();
        //                //Header.TranNo = id;
        //                DB.HRKPIActivitiesHeaders.Remove(objHeader);
        //                foreach (var read in objItem)
        //                {
        //                    DB.HRKPIActivitiesItems.Remove(read);
        //                }
        //                int row = DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserID.ToString();
        //                //log.ScreenId = Header.EmpID.ToString();
        //                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (Exception e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserID.ToString();
        //                //log.ScreenId = Header.EmpID.ToString();
        //                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        #region RequestRelease
        //        public string RequestRelease(string id, string Actionname, string ApprovType_)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                string CLOSED = SYDocumentStatus.CLOSED.ToString();
        //                string Inprogress = SYDocumentStatus.PROCESSING.ToString();
        //                string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
        //                string pendding = SYDocumentStatus.PENDING.ToString();
        //                string RELEASED = SYDocumentStatus.RELEASED.ToString();
        //                if (Actionname == "RequestforApp" && ApprovType_ == "")
        //                {
        //                    var AssignH = DBM.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == id && w.Status == open);
        //                    if (AssignH == null)
        //                    {
        //                        return "EE001";
        //                    }
        //                    var AssignI = DBM.HRKPIAssignItems.Where(w => w.AssignCode == AssignH.AssignCode && w.Status == open).ToList();
        //                    if (AssignI.Count == 0)
        //                    {
        //                        return "EE001";
        //                    }
        //                    foreach (var item_ in AssignI)
        //                    {
        //                        item_.Status = pendding;
        //                        DBM.HRKPIAssignItems.Attach(item_);
        //                        DBM.Entry(item_).Property(w => w.Status).IsModified = true;
        //                    }
        //                    AssignH.Status = pendding;
        //                    DBM.HRKPIAssignHeaders.Attach(AssignH);
        //                    DBM.Entry(AssignH).Property(w => w.Status).IsModified = true;
        //                }
        //                else if (Actionname == "Details" && ApprovType_ == "KPIAssign")
        //                {
        //                    var AssignH = DBM.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == id && w.Status == pendding);
        //                    if (AssignH == null)
        //                    {
        //                        return "EE001";
        //                    }
        //                    var AssignI = DBM.HRKPIAssignItems.Where(w => w.AssignCode == AssignH.AssignCode && w.Status == pendding).ToList();
        //                    if (AssignI.Count == 0)
        //                    {
        //                        return "EE001";
        //                    }
        //                    foreach (var item_ in AssignI)
        //                    {
        //                        item_.Status = RELEASED;
        //                        DBM.HRKPIAssignItems.Attach(item_);
        //                        DBM.Entry(item_).Property(w => w.Status).IsModified = true;
        //                    }
        //                    AssignH.Status = RELEASED;
        //                    DBM.HRKPIAssignHeaders.Attach(AssignH);
        //                    DBM.Entry(AssignH).Property(w => w.Status).IsModified = true;
        //                }
        //                else if (Actionname == "Details" && ApprovType_ == "KPIImplement")
        //                {
        //                    var ActivitH = DB.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == id);
        //                    if (ActivitH == null)
        //                    {
        //                        return "EE001";
        //                    }
        //                    var AssignH = DB.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == ActivitH.AssignCode && w.Status != CLOSED);
        //                    if (AssignH == null)
        //                    {
        //                        return "EE001";
        //                    }
        //                    var assignI = DB.HRKPIAssignItems.Where(w => w.AssignCode == AssignH.AssignCode).ToList();
        //                    var ActivitI = DB.HRKPIActivitiesItems.Where(w => w.AVTCode == ActivitH.AVTCode).ToList();
        //                    if (ActivitI.Count > 0)
        //                    {
        //                        foreach (var item in ActivitI)
        //                        {
        //                            var listAi = assignI.Where(w => w.AssignCode == ActivitH.AssignCode && w.KPIItemCode == item.KPIElement).ToList();
        //                            if (listAi.Count > 0)
        //                            {
        //                                //listAi.First().Actual += item.Actual;
        //                                listAi.First().Acheivement += item.Acheivement;
        //                                listAi.First().AcheivementByEachKPI += item.AcheivementByEachKPI;
        //                                DBM.HRKPIAssignItems.Attach(listAi.First());
        //                                DBM.Entry(listAi.First()).Property(w => w.Status).IsModified = true;
        //                                DBM.Entry(listAi.First()).Property(w => w.Actual).IsModified = true;
        //                                DBM.Entry(listAi.First()).Property(w => w.Acheivement).IsModified = true;
        //                                DBM.Entry(listAi.First()).Property(w => w.AcheivementByEachKPI).IsModified = true;
        //                            }

        //                            item.Status = COMPLETED;
        //                            DBM.HRKPIActivitiesItems.Attach(item);
        //                            DBM.Entry(item).Property(w => w.Status).IsModified = true;

        //                        }
        //                        ActivitH.Status = COMPLETED;
        //                        DBM.HRKPIActivitiesHeaders.Attach(ActivitH);
        //                        DBM.Entry(ActivitH).Property(w => w.Status).IsModified = true;
        //                    }
        //                    AssignH.TotalAcheivement += ActivitH.TotalAcheivement;
        //                    AssignH.TotalScore += ActivitH.TotalScore;
        //                    DBM.HRKPIAssignHeaders.Attach(AssignH);
        //                    DBM.Entry(AssignH).Property(w => w.Status).IsModified = true;
        //                    DBM.Entry(AssignH).Property(w => w.TotalAcheivement).IsModified = true;
        //                    DBM.Entry(AssignH).Property(w => w.TotalScore).IsModified = true;
        //                }
        //                int row = DBM.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        public string RejectRequest(string id, string Reason, string Comment)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                string Inprogress = SYDocumentStatus.PROCESSING.ToString();
        //                string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
        //                string pendding = SYDocumentStatus.PENDING.ToString();
        //                string RELEASED = SYDocumentStatus.RELEASED.ToString();

        //                var AssignH = DBM.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == id && w.Status == open);
        //                if (AssignH == null)
        //                {
        //                    return "EE001";
        //                }
        //                var AssignI = DBM.HRKPIAssignItems.Where(w => w.AssignCode == AssignH.AssignCode && w.Status == open).ToList();
        //                if (AssignI.Count == 0)
        //                {
        //                    return "EE001";
        //                }
        //                foreach (var item_ in AssignI)
        //                {
        //                    item_.Status = pendding;
        //                    DBM.HRKPIAssignItems.Attach(item_);
        //                    DBM.Entry(item_).Property(w => w.Status).IsModified = true;
        //                }
        //                AssignH.Status = pendding;
        //                DBM.HRKPIAssignHeaders.Attach(AssignH);
        //                DBM.Entry(AssignH).Property(w => w.Status).IsModified = true;


        //                int row = DBM.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        public string CloseTheDoc(string id)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                string Inprogress = SYDocumentStatus.PROCESSING.ToString();
        //                string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
        //                string pendding = SYDocumentStatus.PENDING.ToString();
        //                string RELEASED = SYDocumentStatus.RELEASED.ToString();
        //                string Close = SYDocumentStatus.CLOSED.ToString();

        //                var AssignH = DBM.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == id && w.Status != open);
        //                if (AssignH == null)
        //                {
        //                    return "EE001";
        //                }
        //                var AssignI = DBM.HRKPIAssignItems.Where(w => w.AssignCode == AssignH.AssignCode && w.Status != open).ToList();
        //                if (AssignI.Count == 0)
        //                {
        //                    return "EE001";
        //                }
        //                foreach (var item_ in AssignI)
        //                {
        //                    item_.Status = Close;
        //                    DBM.HRKPIAssignItems.Attach(item_);
        //                    DBM.Entry(item_).Property(w => w.Status).IsModified = true;
        //                }
        //                AssignH.Status = Close;
        //                DBM.HRKPIAssignHeaders.Attach(AssignH);
        //                DBM.Entry(AssignH).Property(w => w.Status).IsModified = true;

        //                int row = DBM.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        #endregion
        //        public string Approved(string id)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBI = new HumicaDBContext();

        //                var objmatch = DB.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == id);

        //                if (objmatch == null) return "DOC_INV";
        //                if (objmatch.Status != SYDocumentStatus.PENDING.ToString()) return "INV_DOC";

        //                string Open = SYDocumentStatus.OPEN.ToString();
        //                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objmatch.KPIType
        //                                    && w.DocumentNo == objmatch.AssignCode && w.Status == Open).OrderBy(w => w.ApproveLevel).ToList();
        //                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
        //                var b = false;
        //                foreach (var read in listApproval)
        //                {
        //                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
        //                    if (checklist.Count > 0)
        //                    {
        //                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
        //                        {
        //                            return "USER_ALREADY_APP";
        //                        }
        //                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
        //                        if (objStaff != null)
        //                        {
        //                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
        //                            {
        //                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
        //                            }
        //                            //StaffProfile = objStaff;
        //                            read.ApprovedBy = objStaff.EmpCode;
        //                            read.ApprovedName = objStaff.AllName;
        //                            read.LastChangedDate = DateTime.Now.Date;
        //                            read.ApprovedDate = DateTime.Now;
        //                            read.Status = SYDocumentStatus.APPROVED.ToString();
        //                            DB.ExDocApprovals.Attach(read);
        //                            DB.Entry(read).State = System.Data.Entity.EntityState.Modified;
        //                            b = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (listApproval.Count > 0)
        //                {
        //                    if (b == false)
        //                    {
        //                        return "USER_NOT_APPROVOR";
        //                    }
        //                }

        //                var Status = SYDocumentStatus.APPROVED.ToString();

        //                if (listApproval.Where(w => w.Status == Open).ToList().Count > 0)
        //                {
        //                    Status = SYDocumentStatus.PENDING.ToString();
        //                }

        //                objmatch.Status = Status;
        //                objmatch.ApprovedBy = User.UserName;

        //                DB.HRKPIAssignHeaders.Attach(objmatch);

        //                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
        //                DB.Entry(objmatch).Property(w => w.ApprovedBy).IsModified = true;

        //                DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                log.DocurmentAction = id;
        //                log.Action = SYActionBehavior.RELEASE.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                log.DocurmentAction = id;
        //                log.Action = SYActionBehavior.RELEASE.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (Exception e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                log.DocurmentAction = id;
        //                log.Action = SYActionBehavior.RELEASE.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }

        //        public static IEnumerable<HRKPISetupHeader> GetHRKPIHEADER()
        //        {
        //            HumicaDBContext DBB = new HumicaDBContext();
        //            List<HRKPISetupHeader> ListH = new List<HRKPISetupHeader>();
        //            string KPICategory = HttpContext.Current.Session["KPICategory"].ToString();
        //            if (KPICategory != null && KPICategory != "")
        //            {
        //                ListH = DBB.HRKPISetupHeaders.Where(w => w.KPICategory == KPICategory && w.IsActive == true).ToList();
        //            }
        //            else
        //            {
        //                ListH = DBB.HRKPISetupHeaders.Where(w => w.IsActive == true).ToList();
        //            }
        //            return ListH;
        //        }
        //        public static IEnumerable<HRKPISetupItem> GetAllKPIElement()
        //        {
        //            HumicaDBContext DBB = new HumicaDBContext();
        //            List<HRKPISetupItem> HRList = new List<HRKPISetupItem>();
        //            string KPICode = HttpContext.Current.Session["KPICode"].ToString();
        //            if (KPICode != null && KPICode != "")
        //            {
        //                HRList = DBB.HRKPISetupItems.Where(w => w.KPISetupCode == KPICode).ToList();
        //            }
        //            return HRList;
        //        }

        //        //public static IEnumerable<HRKPITypeInput> GetAllKPIInputType()
        //        //{
        //        //    HumicaDBContext DBB = new HumicaDBContext();
        //        //    List<HRKPITypeInput> InputType = new List<HRKPITypeInput>();
        //        //    return DBB.HRKPITypeInputs.ToList();

        //        //}
        //        public static IEnumerable<HRKPISection> GetKPISelect()
        //        {
        //            HumicaDBContext DBI = new HumicaDBContext();
        //            List<HRKPISection> ListSection = new List<HRKPISection>();
        //            string KPICode = HttpContext.Current.Session["KPICode"].ToString();
        //            if (KPICode != null && KPICode != "")
        //            {
        //                var ListSetupItem = DBI.HRKPISetupItems.Where(w => w.KPISetupCode == KPICode).ToList();
        //                ListSection = DBI.HRKPISections.ToList();
        //                ListSection = ListSection.Where(w => ListSetupItem.Where(x => x.Section == w.Code).Any()).ToList();
        //            }
        //            return ListSection;
        //        }

        //        public static IEnumerable<HRKPISetupItem> GetTaskBySection()
        //        {
        //            HumicaDBContext DBI = new HumicaDBContext();
        //            List<HRKPISetupItem> ListSection = new List<HRKPISetupItem>();
        //            string SectionCode = "";
        //            string KPICode = HttpContext.Current.Session["KPICode"].ToString();
        //            if (HttpContext.Current.Session["SectionCode"] != null)
        //            {
        //                SectionCode = HttpContext.Current.Session["SectionCode"].ToString();
        //            }
        //            if (KPICode != null && KPICode != "")
        //            {
        //                ListSection = DBI.HRKPISetupItems.Where(w => w.KPISetupCode == KPICode && w.Section == SectionCode).ToList();
        //            }
        //            return ListSection;
        //        }
        //        #region Implement
        //        public static IEnumerable<HRKPISetupHeader> GetHRKPIASSHEADER()
        //        {
        //            HumicaDBContext DBB = new HumicaDBContext();
        //            List<HRKPISetupHeader> ListH = new List<HRKPISetupHeader>();
        //            string AssignCode = HttpContext.Current.Session["AssignCode"].ToString();
        //            //string KPICategory = HttpContext.Current.Session["KPICategory"].ToString();
        //            if (AssignCode != null && AssignCode != "")
        //            {
        //                var ListA = DBB.HRKPIAssignItems.Where(w => w.AssignCode == AssignCode).ToList();
        //                foreach (var item in ListA.GroupBy(w => w.KPIItemCode).Select(w => new { w.First().KPIItemCode }).ToList())
        //                {

        //                    var ListI = DBB.HRKPISetupHeaders.Where(w => w.Code == item.KPIItemCode && w.IsActive == true).ToList();
        //                    foreach (var item_ in ListI)
        //                    {
        //                        HRKPISetupHeader Obj = new HRKPISetupHeader();
        //                        Obj = item_;
        //                        ListH.Add(Obj);
        //                    }
        //                }
        //            }
        //            return ListH;
        //        }
        //        public static IEnumerable<HRKPIAssignItem> GetAllKPIASSIGNElement()
        //        {
        //            HumicaDBContext DBB = new HumicaDBContext();
        //            List<HRKPIAssignItem> HRList = new List<HRKPIAssignItem>();
        //            string AssignCode = HttpContext.Current.Session["AssignCode"].ToString();
        //            //string KPICode = HttpContext.Current.Session["KPICodeItem"].ToString();
        //            if (AssignCode != null || AssignCode != "")
        //            {
        //                HRList = DBB.HRKPIAssignItems.Where(w => w.AssignCode == AssignCode).ToList();
        //            }
        //            return HRList;
        //        }
        //        #endregion
        //        public string SaveImplement()
        //        {
        //            try
        //            {
        //                DB = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                //var Stafff = DBM.HR_STAFF_VIEW;
        //                if (listAssignInsertItem.Count == 0)
        //                {
        //                    return "DOC_INV";
        //                }
        //                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Doctype_AVT);
        //                if (objCF == null)
        //                {
        //                    return "REQUEST_TYPE_NE";
        //                }
        //                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
        //                HeaderActivities.AVTCode = objNumber.NextNumberRank;
        //                var HeaderActi = DB.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == HeaderActivities.AVTCode);
        //                var AssignHeader = DB.HRKPIAssignHeaders.FirstOrDefault(w => w.AssignCode == HeaderActivities.AssignCode && w.KPICategory == HeaderActivities.KPICategory);
        //                if (HeaderActi == null)
        //                {
        //                    foreach (var read in ListKPIDetailRecord.ToList())
        //                    {
        //                        read.KPICode = HeaderActivities.KPICode;
        //                        read.Status = HeaderActivities.Status;
        //                        read.AVTCode = HeaderActivities.AVTCode;
        //                        read.Status = open;
        //                        DB.HRKPIActivitiesItems.Add(read);
        //                    }
        //                    if (ListJobFollowUp.Count > 0)
        //                    {
        //                        foreach (var read1 in ListJobFollowUp.ToList())
        //                        {
        //                            read1.AVTCode = HeaderActivities.AVTCode;
        //                            read1.KPICode = ListKPIDetailRecord.First().KPICode;
        //                            DB.HRKPIJobFolloUps.Add(read1);
        //                        }
        //                    }
        //                    if (ListEmpConcern.Count > 0)
        //                    {
        //                        foreach (var read2 in ListEmpConcern.ToList())
        //                        {
        //                            read2.AVTCode = HeaderActivities.AVTCode;
        //                            read2.KPICode = ListKPIDetailRecord.First().KPICode;
        //                            DB.HRKPIEmpConcerns.Add(read2);
        //                        }
        //                    }
        //                    if (ListManagementGuidline.Count > 0)
        //                    {
        //                        foreach (var read3 in ListManagementGuidline.ToList())
        //                        {
        //                            read3.AVTCode = HeaderActivities.AVTCode;
        //                            read3.KPICode = ListKPIDetailRecord.First().KPICode;
        //                            DB.HRKPIManagemGuideLines.Add(read3);
        //                        }
        //                    }
        //                    if (listEmpAction.Count > 0)
        //                    {
        //                        foreach (var read4 in listEmpAction.ToList())
        //                        {
        //                            read4.AVTCode = HeaderActivities.AVTCode;
        //                            read4.KPICode = ListKPIDetailRecord.First().KPICode;
        //                            DB.HRKPIEmployeeActions.Add(read4);
        //                        }
        //                    }
        //                    HeaderActivities.Status = open;
        //                    HeaderActivities.EmpCode = AssignHeader.AuthorizePerson;
        //                    HeaderActivities.EmpName = AssignHeader.EmpName;
        //                    HeaderActivities.CreatedBy = User.UserName;
        //                    HeaderActivities.CreatedOn = DateTime.Now;
        //                    DocumentNo = HeaderActivities.AVTCode;
        //                    //var Emp = DB.HRStaffProfiles.Find(HeaderActivities.EmpCode);
        //                    //if (Emp != null)
        //                    //{
        //                    //    HeaderActivities.EmpName = Emp.AllName;
        //                    //}
        //                }
        //                DB.HRKPIActivitiesHeaders.Add(HeaderActivities);
        //                int row = DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        public string SaveImplementEdit(string id)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                //var Stafff = DBM.HR_STAFF_VIEW;
        //                var HeaderAvt = DB.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == id && w.AssignCode == HeaderActivities.AssignCode);

        //                if (HeaderAvt == null)
        //                {
        //                    return "DOC_INV";
        //                }
        //                var ListDetail = DB.HRKPIActivitiesItems.Where(w => w.AVTCode == id).ToList();
        //                var ListJobFollowUp_ = DB.HRKPIJobFolloUps.Where(w => w.AVTCode == id).ToList();
        //                var ListEmpCC = DB.HRKPIEmpConcerns.Where(w => w.AVTCode == id).ToList();
        //                var ListEmpManG = DB.HRKPIManagemGuideLines.Where(w => w.AVTCode == id).ToList();
        //                var ListEmpAction_ = DB.HRKPIEmployeeActions.Where(w => w.AVTCode == id).ToList();
        //                if (ListDetail != null)
        //                {
        //                    foreach (var item in ListDetail)
        //                    {
        //                        DB.HRKPIActivitiesItems.Remove(item);
        //                        DB.SaveChanges();
        //                    }
        //                }
        //                if (ListJobFollowUp_ != null)
        //                {
        //                    foreach (var item1 in ListJobFollowUp_)
        //                    {
        //                        DB.HRKPIJobFolloUps.Remove(item1);
        //                        DB.SaveChanges();
        //                    }
        //                }
        //                if (ListEmpCC != null)
        //                {
        //                    foreach (var item2 in ListEmpCC)
        //                    {
        //                        DB.HRKPIEmpConcerns.Remove(item2);
        //                        DB.SaveChanges();
        //                    }
        //                }
        //                if (ListEmpManG != null)
        //                {
        //                    foreach (var item3 in ListEmpManG)
        //                    {
        //                        DB.HRKPIManagemGuideLines.Remove(item3);
        //                        DB.SaveChanges();

        //                    }
        //                }
        //                if (ListEmpAction_ != null)
        //                {
        //                    foreach (var item4 in ListEmpAction_)
        //                    {
        //                        DB.HRKPIEmployeeActions.Remove(item4);
        //                        DB.SaveChanges();

        //                    }
        //                }
        //                if (ListKPIDetailRecord != null)
        //                {
        //                    foreach (var read in ListKPIDetailRecord)
        //                    {

        //                        read.AVTCode = HeaderActivities.AVTCode;
        //                        read.Status = open;
        //                        DB.HRKPIActivitiesItems.Add(read);
        //                    }
        //                }
        //                if (ListJobFollowUp != null)
        //                {
        //                    foreach (var read1 in ListJobFollowUp)
        //                    {
        //                        read1.AVTCode = HeaderActivities.AVTCode;
        //                        DB.HRKPIJobFolloUps.Add(read1);
        //                    }
        //                }
        //                if (ListEmpConcern != null)
        //                {
        //                    foreach (var read2 in ListEmpConcern)
        //                    {
        //                        read2.AVTCode = HeaderActivities.AVTCode;
        //                        DB.HRKPIEmpConcerns.Add(read2);
        //                    }
        //                }
        //                if (ListManagementGuidline != null)
        //                {
        //                    foreach (var read3 in ListManagementGuidline)
        //                    {
        //                        read3.AVTCode = HeaderActivities.AVTCode;
        //                        DB.HRKPIManagemGuideLines.Add(read3);
        //                    }
        //                }
        //                if (listEmpAction != null)
        //                {
        //                    foreach (var read4 in listEmpAction)
        //                    {
        //                        read4.AVTCode = HeaderActivities.AVTCode;
        //                        DB.HRKPIEmployeeActions.Add(read4);
        //                    }
        //                }
        //                HeaderAvt.ReasonCode = "";
        //                HeaderAvt.Comment = "";
        //                HeaderAvt.Status = open;
        //                HeaderAvt.Description = HeaderActivities.Description;
        //                DB.HRKPIActivitiesHeaders.Attach(HeaderAvt);
        //                DB.Entry(HeaderAvt).State = System.Data.Entity.EntityState.Modified;
        //                int row = DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        public string CalulateKPI(string ID)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                string Inprogress = SYDocumentStatus.PROCESSING.ToString();
        //                string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
        //                if (ID != null)
        //                {


        //                    string[] arr = ID.Split(';');


        //                    foreach (var item in arr)
        //                    {
        //                        var ActiviH = DBM.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == item && w.Status == open);
        //                        if (ActiviH == null)
        //                        {
        //                            return "EE001";
        //                        }
        //                        var ActiviI = DBM.HRKPIActivitiesItems.Where(w => w.AVTCode == item && w.Status == open).ToList();
        //                        if (ActiviI.Count == 0)
        //                        {
        //                            return "EE001";
        //                        }
        //                        var KPIAverage = DBM.HRKPIGroupPlanHeaders.Find(ActiviH.KPIGroupCode);
        //                        if (KPIAverage == null)
        //                        {
        //                            return "EE001";
        //                        }
        //                        Header = DBM.HRKPISetupHeaders.Where(w => w.KPIType == ActiviH.Department && w.IsActive == true).FirstOrDefault();
        //                        if (Header == null)
        //                        {
        //                            return "EE001";
        //                        }
        //                        AssignHeader = DBM.HRKPIAssignHeaders.FirstOrDefault(w => w.KPIGroupCode == ActiviH.KPIGroupCode && w.EmpCode == ActiviH.EmpCode && w.Status != COMPLETED);
        //                        if (AssignHeader == null)
        //                        {
        //                            return "EE001";
        //                        }
        //                        AssignHeader.Status = Inprogress;
        //                        foreach (var item_ in ActiviI)
        //                        {
        //                            var AssigItem = DBM.HRKPIAssignItems.FirstOrDefault(w => w.EmpCode == ActiviH.EmpCode && w.AssignCode == AssignHeader.AssignCode && w.KPIItemCode == item_.KPIElement);
        //                            if (AssigItem != null)
        //                            {
        //                                var weigh = AssigItem.Weight / 100;
        //                                //var archeiv = Math.Round((decimal)(item_.Actual / AssigItem.Target), 2);
        //                                //  AssigItem.Actual += item_.Actual;
        //                                //AssigItem.Acheivement += Math.Round((decimal)(item_.Actual / AssigItem.Target) * 100, 2);
        //                                //AssigItem.AcheivementByEachKPI += weigh * archeiv;
        //                                AssigItem.Status = Inprogress;
        //                                DBM.HRKPIAssignItems.Attach(AssigItem);
        //                                DBM.Entry(AssigItem).Property(w => w.Actual).IsModified = true;
        //                                DBM.Entry(AssigItem).Property(w => w.Status).IsModified = true;
        //                                DBM.Entry(AssigItem).Property(w => w.Acheivement).IsModified = true;
        //                                DBM.Entry(AssigItem).Property(w => w.AcheivementByEachKPI).IsModified = true;
        //                                AssignHeader.TotalAcheivement += AssigItem.AcheivementByEachKPI;
        //                                AssignHeader.TotalScore += AssigItem.AcheivementByEachKPI;
        //                            }
        //                            item_.Status = COMPLETED;
        //                            DBM.HRKPIActivitiesItems.Attach(item_);
        //                            DBM.Entry(item_).Property(w => w.Status).IsModified = true;

        //                        }
        //                        AssignHeader.TotalScore = AssignHeader.TotalScore * KPIAverage.KPIAverage;
        //                        DBM.HRKPIAssignHeaders.Attach(AssignHeader);
        //                        DBM.Entry(AssignHeader).Property(w => w.Status).IsModified = true;
        //                        DBM.Entry(AssignHeader).Property(w => w.TotalAcheivement).IsModified = true;
        //                        DBM.Entry(AssignHeader).Property(w => w.TotalScore).IsModified = true;

        //                        ActiviH.Status = COMPLETED;
        //                        DBM.HRKPIActivitiesHeaders.Attach(ActiviH);
        //                        DBM.Entry(ActiviH).Property(w => w.Status).IsModified = true;
        //                    }

        //                }
        //                else
        //                {
        //                    var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Doctype_AVT);
        //                    if (objCF == null)
        //                    {
        //                        return "REQUEST_TYPE_NE";
        //                    }
        //                    var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
        //                    HeaderActivities.AVTCode = objNumber.NextNumberRank;
        //                    var HeaderActi = DB.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == HeaderActivities.AVTCode);
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemActivities.ToList())
        //                        {
        //                            read.AVTCode = HeaderActivities.AVTCode;
        //                            read.Status = open;
        //                            DBM.HRKPIActivitiesItems.Add(read);
        //                        }
        //                        HeaderActivities.Status = open;
        //                        HeaderActivities.CreatedBy = User.UserName;
        //                        HeaderActivities.CreatedOn = DateTime.Now;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        var Emp = DB.HRStaffProfiles.Find(HeaderActivities.EmpCode);
        //                        if (Emp != null)
        //                        {
        //                            HeaderActivities.EmpName = Emp.AllName;
        //                        }
        //                    }
        //                }
        //                int row = DBM.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }

        //        public string RequesForApp(string id, string Status)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();
        //                string open = SYDocumentStatus.OPEN.ToString();
        //                string Inprogress = SYDocumentStatus.PROCESSING.ToString();
        //                string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
        //                string pendding = SYDocumentStatus.PENDING.ToString();
        //                string RELEASED = SYDocumentStatus.RELEASED.ToString();

        //                var ActivitH = DBM.HRKPIActivitiesHeaders.FirstOrDefault(w => w.AVTCode == id && w.Status == Status);
        //                if (ActivitH == null)
        //                {
        //                    return "EE001";
        //                }
        //                var ActivitI = DBM.HRKPIActivitiesItems.Where(w => w.AVTCode == ActivitH.AVTCode && w.Status == Status).ToList();
        //                if (ActivitI.Count == 0)
        //                {
        //                    return "EE001";
        //                }
        //                foreach (var item_ in ActivitI)
        //                {
        //                    item_.Status = pendding;
        //                    DBM.HRKPIActivitiesItems.Attach(item_);
        //                    DBM.Entry(item_).Property(w => w.Status).IsModified = true;
        //                }
        //                ActivitH.Status = pendding;
        //                DBM.HRKPIActivitiesHeaders.Attach(ActivitH);
        //                DBM.Entry(ActivitH).Property(w => w.Status).IsModified = true;
        //                int row = DBM.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            return SYConstant.OK;
        //        }

        //        #region SaveForm
        //        public string SaveForm(string FormType)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();

        //                string open = SYDocumentStatus.OPEN.ToString();
        //                //var Stafff = DBM.HR_STAFF_VIEW;
        //                if (ListItemForm.Count == 0)
        //                {
        //                    return "DOC_INV";
        //                }
        //                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, DocType);
        //                if (objCF == null)
        //                {
        //                    return "REQUEST_TYPE_NE";
        //                }
        //                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
        //                HeaderForm.Code = objNumber.NextNumberRank;
        //                var HeaderActi = DB.HRKPIFormHeaders.Find(HeaderForm.Code);
        //                HeaderForm.DocumentDate = DateTime.Now.Date;
        //                if (FormType == "EmployeeConcern")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.FormType = FormType;
        //                            read.Status = open;
        //                            DB.HRKPIFormItems.Add(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Add(HeaderForm);
        //                    }
        //                }
        //                if (FormType == "FollowUp")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.FormType = FormType;
        //                            read.Status = open;
        //                            DB.HRKPIFormItems.Add(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Add(HeaderForm);
        //                    }
        //                }
        //                if (FormType == "ManagermentGuideLine")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.Status = open;
        //                            read.FormType = FormType;
        //                            DB.HRKPIFormItems.Add(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.CoachName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Add(HeaderForm);
        //                    }
        //                }
        //                if (FormType == "EmployeeAction")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.Status = open;
        //                            read.FormType = FormType;
        //                            DB.HRKPIFormItems.Add(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Add(HeaderForm);
        //                    }
        //                }
        //                int row = DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }

        //        public string EditForm(string Code, string FormType)
        //        {
        //            try
        //            {
        //                HumicaDBContext DBM = new HumicaDBContext();

        //                string open = SYDocumentStatus.OPEN.ToString();
        //                //var Stafff = DBM.HR_STAFF_VIEW;
        //                if (ListItemForm.Count == 0)
        //                {
        //                    return "DOC_INV";
        //                }
        //                var HeaderActi = DB.HRKPIFormHeaders.Find(Code);
        //                HeaderForm.DocumentDate = DateTime.Now.Date;
        //                if (FormType == "EmployeeConcern")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.FormType = FormType;
        //                            read.Status = open;
        //                            DB.HRKPIFormItems.Attach(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Attach(HeaderForm);
        //                        DB.Entry(HeaderForm).State = System.Data.Entity.EntityState.Modified;
        //                    }
        //                }
        //                if (FormType == "FollowUp")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.FormType = FormType;
        //                            read.Status = open;
        //                            DB.HRKPIFormItems.Attach(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Attach(HeaderForm);
        //                        DB.Entry(HeaderForm).State = System.Data.Entity.EntityState.Modified;
        //                    }
        //                }
        //                if (FormType == "ManagermentGuideLine")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.Status = open;
        //                            read.FormType = FormType;
        //                            DB.HRKPIFormItems.Attach(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.CoachName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Attach(HeaderForm);
        //                        DB.Entry(HeaderForm).State = System.Data.Entity.EntityState.Modified;
        //                    }
        //                }
        //                if (FormType == "EmployeeAction")
        //                {
        //                    if (HeaderActi == null)
        //                    {
        //                        foreach (var read in ListItemForm.ToList())
        //                        {
        //                            read.Status = HeaderActivities.Status;
        //                            read.Code = HeaderForm.Code;
        //                            read.Status = open;
        //                            read.FormType = FormType;
        //                            DB.HRKPIFormItems.Attach(read);
        //                        }
        //                        var Td = DB.HRStaffProfiles.Find(HeaderForm.EmpName);
        //                        HeaderForm.Gender = Td.Sex;
        //                        HeaderForm.ExpectedDate = HeaderForm.DocumentDate;
        //                        DocumentNo = HeaderActivities.AVTCode;
        //                        DB.HRKPIFormHeaders.Attach(HeaderForm);
        //                        DB.Entry(HeaderForm).State = System.Data.Entity.EntityState.Modified;
        //                    }
        //                }
        //                int row = DB.SaveChanges();
        //                return SYConstant.OK;
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = ScreenId;
        //                log.UserId = User.UserName;
        //                //log.DocurmentAction = Header.EmpID;
        //                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                return "EE001";
        //            }
        //        }
        //        #endregion

        //    }
        //    public class ClsKPI_InputType
        //    {
        //        public string Code { get; set; }

        //        public string Description { get; set; }

        //        public static List<ClsKPI_InputType> LoadDataKPIInput()
        //        {
        //            List<ClsKPI_InputType> listData = new List<ClsKPI_InputType>();
        //            listData.Add(new ClsKPI_InputType() { Code = "01", Description = "Number" });
        //            listData.Add(new ClsKPI_InputType() { Code = "03", Description = "Text" });
        //            listData.Add(new ClsKPI_InputType() { Code = "04", Description = "Percentage" });
        //            return listData;
        //        }
    }
}