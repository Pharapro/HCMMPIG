using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.LM
{
    public class ClsReqLateEarly : IClsReqLateEarly
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public HRReqLateEarly Header { get; set; }
        public List<HRReqLateEarly> ListHeader { get; set; }
        public FTINYear FInYear { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<ClsReuestLaEa> ListReqPending { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsReqLateEarly()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void LoadData(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var listobj = unitOfWork.Set<HRReqLateEarly>().AsQueryable()
                .Where(w => w.Status == pending)
                .ToList();
            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listobj.AsEnumerable().Where(x => w.DocumentNo == x.ReqLaEaNo
                    && w.DocumentType == DocType && w.Approver == userName).Any())
                .ToList();
            foreach (var item in listApp)
            {
                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listobj.Where(w => w.ReqLaEaNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType 
                            && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listobj.Where(w => w.ReqLaEaNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
            }
            var ListLeaveCreater = unitOfWork.Set<HRReqLateEarly>().AsQueryable().AsEnumerable()
                    .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                        .Any(w => x.ReqLaEaNo == w.DocumentNo
                                   && DocType == w.DocumentType && w.Approver == userName && x.Status != open && x.Status != Cancel)
                        || x.CreatedBy == userName)
                    .ToList();
            ListHeader = ListLeaveCreater;
        }
        private void AddPendingToList(ExDocApproval item, List<HRReqLateEarly> listobj)
        {
            foreach (var read in listobj)
            {
                var obj = listobj.FirstOrDefault(w => w.ReqLaEaNo == item.DocumentNo);
                if (obj != null)
                {
                    var PedingReq = new ClsReuestLaEa
                    {
                        Remark = "PENDING",
                        EmpCode = read.EmpCode,
                        EmpName = read.EmpName,
                        LeaveDate = read.LeaveDate,
                        Reason = read.Reason,
                        Status = read.Status,
                        ReqLaEaNo = read.ReqLaEaNo,
                        Qty = read.Qty
                    };

                    ListReqPending.Add(PedingReq);
                }
            }
        }
        public HRStaffProfile getNextApprover(string id, string DocType)
        {
            var objStaff = new HRStaffProfile();
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == DocType).ToList();
            if (listCanApproval.Count == 0)
                return new HRStaffProfile();
            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == NextApp.Approver);
            return objStaff;
        }
        public string CreateReqLateEarly()
        {
            OnLoad();
            try
            {
                if (string.IsNullOrEmpty(HeaderStaff.EmpCode)) return "INV_STAFF";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_DOC";
                if (Header.RequestType == "MISSSCAN")
                    if (string.IsNullOrEmpty(Header.Remark)) return "INV_REMARK";
                if (Header.Qty <= 0 && Header.RequestType != "MISSSCAN")
                    return "INV_QTY";
                if (Header.LeaveDate == null || Header.LeaveDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON_EN";
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var lstempLeave = unitOfWork.Repository<HRReqLateEarly>().Queryable().Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                var leaveH = lstempLeave.Where(w => w.EmpCode == HeaderStaff.EmpCode && w.Status != Cancel && w.Status != Reject).ToList();
                if (leaveH.Where(w => w.LeaveDate.Date == Header.LeaveDate.Date && w.RequestType == Header.RequestType).Any())
                {
                    if (Header.RequestType == "MISSSCAN")
                    {
                        var tpyereamrk = leaveH.Where(w => w.Remark == Header.Remark).ToList();
                        if (tpyereamrk.Any()) return "INV_DATE";
                    }
                    else
                        return "INV_DATE";
                }
                var objNumber = new CFNumberRank(DocType);
                Header.ReqLaEaNo = objNumber.NextNumberRank.Trim();
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.Status = SYDocumentStatus.APPROVED.ToString();
                Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.EmpName = HeaderStaff.AllName;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string editReqLateEarly(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(DocType)) return "INV_DOCTYPE";
                if (Header.LeaveDate == null || Header.LeaveDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_DOC";
                if (Header.RequestType == "MISSSCAN")
                    if (string.IsNullOrEmpty(Header.Remark)) return "INV_REMARK";
                if (Header.Qty <= 0 && Header.RequestType != "MISSSCAN")
                    return "INV_QTY";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                var objMast = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == id);
                if (objMast == null) return "INV_DOC";
                objMast.Qty = Header.Qty;
                objMast.Reason = Header.Reason;
                objMast.LeaveDate = Header.LeaveDate;
                objMast.RequestType = Header.RequestType;
                objMast.Remark = Header.Remark;
                objMast.ChangedOn = DateTime.Now.Date;
                objMast.ChangedBy = User.UserName;
                unitOfWork.Update(objMast);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string deleteReqLateEarly(string ReqLaEaNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objMast = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == ReqLaEaNo);
                if (objMast == null) return "INV_DOC";
                var approver = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == DocType && w.DocumentNo == ReqLaEaNo).ToList();
                if (approver.Any())
                    unitOfWork.BulkDelete(approver);
                unitOfWork.Delete(objMast);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string ESSRequestLaEa(string URL)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(DocType)) return "INV_DOCTYPE";
                if (string.IsNullOrEmpty(HeaderStaff.EmpCode)) return "INV_STAFF";
                if (Header.LeaveDate == null || Header.LeaveDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_DOC";
                if (Header.RequestType == "MISSSCAN")
                    if (string.IsNullOrEmpty(Header.Remark)) return "INV_REMARK";
                if (Header.Qty <= 0 && Header.RequestType != "MISSSCAN")
                    return "INV_QTY";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";

                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var lstempLeave = unitOfWork.Repository<HRReqLateEarly>().Queryable().Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                var leaveH = lstempLeave.Where(w => w.EmpCode == HeaderStaff.EmpCode && w.Status != Cancel && w.Status != Reject).ToList();
                if (leaveH.Where(w => w.LeaveDate.Date == Header.LeaveDate.Date && w.RequestType == Header.RequestType).Any())
                {
                    if (Header.RequestType == "MISSSCAN")
                    {
                        var tpyereamrk = leaveH.Where(w => w.Remark == Header.Remark).ToList();
                        if (tpyereamrk.Any()) return "INV_DATE";
                    }
                    else
                        return "INV_DATE";
                }
                var Result = leaveH.Where(w => w.LeaveDate.Date == Header.LeaveDate.Date).ToList();
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                DateTime fromdate = Header.LeaveDate.StartDateOfMonth();
                DateTime todate = Header.LeaveDate.EndDateOfMonth();
                var Peroid = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.FromDate <= Header.LeaveDate.Date && w.ToDate >= Header.LeaveDate.Date);
                if (Peroid != null)
                {
                    fromdate = Peroid.FromDate;
                    todate = Peroid.ToDate;
                }
                var Missan = leaveH.Where(w => w.LeaveDate >= fromdate && w.LeaveDate <= todate && !string.IsNullOrEmpty(w.Remark)).ToList();
                if (Policy != null)
                {
                    if (Missan.Count >= Policy.MissScan)
                    {
                        return "OVER_MISSCAN";
                    }
                    if (Policy.IsLate_Early > 0)
                    {
                        var count = leaveH.Where(w => w.LeaveDate >= fromdate && w.LeaveDate <= todate && w.RequestType != "MISSSCAN").Count();
                        if (Policy.IsLate_Early < count)
                        {
                            MessageError = Policy.IsLate_Early.ToString();
                            return "OVER_LATE/EARLY";
                        }
                    }
                    if (Header.RequestType == "LATE" && Header.Qty > Policy.MaxLate) return "OVER_LATE";
                    if (Header.RequestType == "EARLY" && Header.Qty > Policy.MaxEarly) return "OVER_EARLY";
                }
                var objCF = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (objCF == null)
                    return "REQUEST_TYPE_NE";

                SetAutoApproval_(HeaderStaff.EmpCode, DocType, Header.LeaveDate.Date);
                if (!ListApproval.Any())
                    return "NO_LINE_MN";
                string Status = SYDocumentStatus.PENDING.ToString();
                var objNumber = new CFNumberRank(DocType);
                Header.ReqLaEaNo = objNumber.NextNumberRank.Trim();
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.EmpName = HeaderStaff.AllName;
                Header.Status = Status;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);

                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Header.ReqLaEaNo;
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                unitOfWork.Save();
                unitOfWork.Commit();

                #region ---Send To Telegram---
                URL += Header.ReqLaEaNo;
                var EmpBooking = unitOfWork.Repository<HRReqLateEarly>().Queryable().Where(w => w.ReqLaEaNo == Header.ReqLaEaNo).ToList();
                var HOD = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objCF.FirstLine);
                var _Type = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == Header.ReqLaEaNo);
                var Post = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == objCF.JobCode);
                var QTY = "";
                if (_Type.Qty > 0)
                {
                    QTY = _Type.Qty.ToString();
                }
                else
                {
                    QTY = _Type.Remark;
                }
                var Link = URL;
                if (HOD != null)
                {
                    string str = "Dear <b>" + HOD.Title + " . " + HOD.AllName + "</b>, I would like to request " + _Type.RequestType + " for my team as below:";
                    foreach (var read in EmpBooking)
                    {
                        str += @"%0A- <b>" + read.EmpName + "</b> on " + read.LeaveDate.ToString("dd.MMM.yyyy");
                    }
                    str += "<b> Total: </b>" + QTY + "%0A<b> Position: </b>" + Post.Description;
                    str += "%0A*<b>" + EmpBooking.First().Reason + "</b> Thanks you.";
                    str += "%0A%0AYours sincerely,%0A%0A<b>" + EmpBooking.First().EmpName + " </b>";//+ "%0A%0APlease login at: " + Link;
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    Tel.User = User;
                    Tel.BS = BS;
                    List<object> ListObjectDictionary = new List<object>();
                    WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, objCF.TeleGroup, false);
                    MessageError = Tel.getErrorMessage(result1);
                }
                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListApproval.Where(w => w.Approver == Approver.Approver).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }
            return SYConstant.OK;
        }
        public string ApproveOTReq(string ReqLaEaNo, string URL)
        {
            OnLoad();
            try
            {
                string[] c = ReqLaEaNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == ID);
                    if (objMatch == null)
                        return "EE001";
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                        return "INV_DOC";

                    string open = SYDocumentStatus.OPEN.ToString();
                    string DocNo = objMatch.ReqLaEaNo;
                    var listApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == DocType
                                        && w.DocumentNo == DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                    var listUser = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.EmpCode == User.UserName).ToList();
                    var b = false;
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
                    foreach (var read in listApproval)
                    {
                        var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                        if (checklist.Count > 0)
                        {
                            if (read.Status == SYDocumentStatus.APPROVED.ToString())
                            {
                                return "USER_ALREADY_APP";
                            }
                            var objStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == read.Approver);
                            if (objStaff != null)
                            {
                                //New
                                if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                                {
                                    listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                                }
                                read.ApprovedBy = objStaff.EmpCode;
                                read.ApprovedName = objStaff.AllName;
                                read.LastChangedDate = DateTime.Now.Date;
                                read.ApprovedDate = DateTime.Now;
                                read.Status = SYDocumentStatus.APPROVED.ToString();
                                unitOfWork.Update(read);
                                b = true;
                                break;
                            }
                        }

                    }
                    if (listApproval.Count > 0)
                    {
                        if (b == false)
                            return "USER_NOT_APPROVOR";
                    }
                    var status = SYDocumentStatus.APPROVED.ToString();
                    if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                        status = SYDocumentStatus.PENDING.ToString();
                    objMatch.Status = status;
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();

                    var StaffApp = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    if (StaffApp != null)
                    {
                        string Email_Template = "ESSLAEA";
                        if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                            Email_Template = "ESSLAEA_APP";
                        else
                            StaffApp = getNextApprover(objMatch.ReqLaEaNo, DocType);
                        if (StaffApp.Email != "" && StaffApp.Email != null)
                        {
                            #region Send Email
                            //SYWorkFlowEmailObject wfo =
                            //           new SYWorkFlowEmailObject(WFObject, WorkFlowType.REQUESTER,
                            //                UserType.N, SYDocumentStatus.PENDING.ToString());
                            //wfo.SelectListItem = new SYSplitItem(objMatch.ReqLaEaNo);
                            //wfo.User = User;
                            //wfo.BS = BS;
                            //wfo.UrlView = SYUrl.getBaseUrl();
                            //wfo.ScreenId = ScreenId;
                            //wfo.Module = "HR";// CModule.PURCHASE.ToString();
                            //wfo.ListLineRef = new List<BSWorkAssign>();
                            //wfo.DocNo = objMatch.ReqLaEaNo;
                            //wfo.Action = SYDocumentStatus.PENDING.ToString();
                            //wfo.ListObjectDictionary = new List<object>();
                            //wfo.ListObjectDictionary.Add(objMatch);
                            //HRStaffProfile Staff = StaffApp;
                            //wfo.ListObjectDictionary.Add(Staff);
                            //WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                            //MessageError = wfo.getErrorMessage(result1);
                            #endregion
                        }
                        #region ---Send To Telegram---
                        var EmailTemplate = unitOfWork.Repository<TPEmailTemplate>().Queryable().FirstOrDefault(w => w.EMTemplateObject == Email_Template);
                        if (EmailTemplate != null)
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(objMatch);
                            ListObjectDictionary.Add(StaffApp);
                            WorkFlowResult result1 = Tel.Send_SMS_Telegram(Email_Template, EmailTemplate.RequestContent, StaffApp.TeleChartID, ListObjectDictionary, URL);
                            MessageError = Tel.getErrorMessage(result1);
                        }
                        #endregion
                        if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            var Sett = unitOfWork.Repository<SYHRSetting>().Queryable().FirstOrDefault();
                            var EmailTemplateCC = unitOfWork.Repository<TPEmailTemplate>().Queryable().FirstOrDefault(w => w.EMTemplateObject == "ESSLAEA_APP_CC");
                            if (EmailTemplateCC != null)
                            {
                                SYSendTelegramObject Tel = new SYSendTelegramObject();
                                Tel.User = User;
                                Tel.BS = BS;
                                List<object> ListObjectDictionary = new List<object>();
                                ListObjectDictionary.Add(objMatch);
                                ListObjectDictionary.Add(StaffApp);
                                WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplateCC.EMTemplateObject, EmailTemplateCC.RequestContent, Sett.TelegLeave, ListObjectDictionary, URL);
                                MessageError = Tel.getErrorMessage(result1);
                            }
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RejectOTReq(string ID)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == ID);
                if (objMatch == null)
                    return "INV_DOC";
                var listApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == DocType
                                        && w.DocumentNo == ID).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                if (listApproval.Count == 0)
                {
                    return "RESTRICT_ACCESS";
                }
                foreach (var read in listApproval)
                {
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.REJECTED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }
                        var objStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            //New
                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            {
                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            }
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.REJECTED.ToString();
                            unitOfWork.Update(read);
                            b = true;
                            break;
                        }
                    }
                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                        return "USER_NOT_REJECT";
                }
                objMatch.Status = SYDocumentStatus.REJECTED.ToString();
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                var StaffApp = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                if (StaffApp != null)
                {
                    #region ---Send To Telegram---
                    var EmailTemplate = unitOfWork.Repository<TPEmailTemplate>().Queryable().FirstOrDefault(w => w.EMTemplateObject == "ESSLAEA_REJ");
                    if (EmailTemplate != null)
                    {
                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        Tel.User = User;
                        Tel.BS = BS;
                        List<object> ListObjectDictionary = new List<object>();
                        ListObjectDictionary.Add(objMatch);
                        ListObjectDictionary.Add(StaffApp);
                        var URL = "";
                        WorkFlowResult result1 = Tel.Send_SMS_Telegram("ESSLAEA_REJ", EmailTemplate.RequestContent, StaffApp.TeleChartID, ListObjectDictionary, URL);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                    #endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string CancelOTReq(string ReqLaEaNo)
        {
            try
            {
                string[] c = ReqLaEaNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == ID);
                    if (objMatch == null)
                        return "INV_DOC";
                    objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ReqLaEaNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SetAutoApproval_(string empCode, string Doctype, DateTime toDate)
        {
            ListApproval = new List<ExDocApproval>();
            var staffList = unitOfWork.Repository<HRStaffProfile>().Queryable()
                .Where(w => w.Status == "A" || (DbFunctions.TruncateTime(w.DateTerminate) > DbFunctions.TruncateTime(toDate) && w.Status == "I")).ToList();
            var staffRequest = staffList.FirstOrDefault(w => w.EmpCode == empCode);
            if (staffRequest == null) return;
            HRStaffProfile staffApp = GetApprover(staffList, staffRequest.FirstLine) ??
                                      GetApprover(staffList, staffRequest.SecondLine);
            HRStaffProfile staffHOD = staffList.FirstOrDefault(w => w.EmpCode == staffRequest.HODCode);
            AddApproverToApprovalList(staffApp, 1, Doctype, Header.ReqLaEaNo);
            AddApproverToApprovalList(staffHOD, 2, Doctype, Header.ReqLaEaNo);
        }
        private HRStaffProfile GetApprover(List<HRStaffProfile> staffList, string empCode)
        {
            return staffList.FirstOrDefault(w => w.EmpCode == empCode);
        }
        private void AddApproverToApprovalList(HRStaffProfile approver, int level, string documentType, string documentNo)
        {
            if (approver != null && !ListApproval.Any(w => w.Approver == approver.EmpCode))
            {
                var docApproval = new ExDocApproval
                {
                    Approver = approver.EmpCode,
                    ApproverName = approver.AllName,
                    ApprovedBy = "",
                    ApprovedName = "",
                    ApproveLevel = level,
                    DocumentType = documentType,
                    DocumentNo = documentNo,
                    Status = SYDocumentStatus.OPEN.ToString(),
                    WFObject = "WF02"
                };
                ListApproval.Add(docApproval);
            }
        }
    }
    public class ClsReuestLaEa
    {
        public string ReqLaEaNo { get; set; }
        public string Remark { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime LeaveDate { get; set; }
        public string Reason { get; set; }
        public int? Qty { get; set; }
        public string Status { get; set; }
    }
}