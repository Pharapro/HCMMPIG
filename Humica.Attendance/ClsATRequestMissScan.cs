using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Humica.Logic.CF;
using Humica.Core.SY;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Humica.Logic.LM;
using System.Runtime.Remoting;
using System.Security.Policy;
using Humica.Core;

namespace Humica.Attendance
{
    public class ClsATRequestMissScan : IClsATRequestMissScan
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public ATEmpMissscan Header { get; set; }
        public List<ATEmpMissscan> ListHeader { get; set; }
        public List<ATEmpMissscan> ListHeaderPending { get; set; }
        public FTINYear FInYear { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<RequestMissScan> ListPending { get; set; }
        public DateTime Time { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsATRequestMissScan()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public void OnIndexLoading(bool IsESS = false)
        {
            if (IsESS)
            {
                string Open = SYDocumentStatus.OPEN.ToString();
                ListHeader = unitOfWork.Set<ATEmpMissscan>().Where(w => w.EmpCode == User.UserName
                && w.Status != Open).ToList();
            }
            else
            {
                ListHeader = unitOfWork.Set<ATEmpMissscan>().ToList();
            }
        }
        public virtual string OnEditLoading(params object[] keys)
        {
            string DocumentNo = (string)keys[0];
            this.Header = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == DocumentNo);
            HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
            string pending = SYDocumentStatus.PENDING.ToString();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            ListHeader = unitOfWork.Set<ATEmpMissscan>().Where(w => w.DocumentNo == Header.DocumentNo).ToList();
            ListApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentNo == DocumentNo && w.DocumentType == Header.RequestType).ToList();
            bool anyApproved = unitOfWork.Set<ExDocApproval>().Any(w => w.DocumentNo == DocumentNo && w.DocumentType == Header.RequestType && w.Status == Approve);

            if (anyApproved)
            {
                return "DOC_INV";
            }
            if (Header.Status != pending)
            {
                return "DOC_INV";
            }

            return SYConstant.OK;
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string DocumentNo = (string)keys[0];
            Header = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == DocumentNo);
            ListHeader = new List<ATEmpMissscan>();
            if (Header != null)
            {
                HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                ListApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentNo == DocumentNo && w.DocumentType == Header.RequestType).ToList();
            }
            ListHeader = unitOfWork.Set<ATEmpMissscan>().Where(w => w.DocumentNo == DocumentNo).ToList();
        }
        public void ProcessMissScan(string userName)
        {
            ListPending = new List<RequestMissScan>();
            var staff = unitOfWork.Set<HRStaffProfile>().Where(w => w.EmpCode == userName).ToList();
            if (staff.Any())
            {
                string pending = SYDocumentStatus.PENDING.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var listReqMissScan = unitOfWork.Set<ATEmpMissscan>().Where(w => w.Status == pending).ToList();
                var ListApp = unitOfWork.Set<ExDocApproval>().AsEnumerable().Where(w => listReqMissScan.Where(x => w.DocumentNo == x.DocumentNo && w.DocumentType == x.RequestType && w.Approver == userName).Any()).ToList();
                foreach (var read in ListApp)
                {
                    var objemp = new RequestMissScan();
                    if (read.ApproveLevel > 1 && read.Status == Open)
                    {
                        var level = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        if (level != null) continue;
                        var EmpStaff = listReqMissScan.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.EmpName = EmpStaff.EmpName;
                        objemp.RequestType = EmpStaff.RequestType;
                        objemp.MissscanDate = EmpStaff.MissscanDate;
                        objemp.RequestDate = EmpStaff.RequestDate;
                        objemp.Time = EmpStaff.Time;
                        objemp.Reason = EmpStaff.Reason;
                        objemp.Status = EmpStaff.Status;
                        objemp.CreatedBy = EmpStaff.CreatedBy;
                        objemp.CreatedOn = EmpStaff.CreatedOn;
                        objemp.ChangedBy = EmpStaff.ChangedBy;
                        objemp.ChangedOn = EmpStaff.ChangedOn;
                        ListPending.Add(objemp);
                    }
                    else if (read.ApproveLevel == 1 && read.Status == Open)
                    {
                        var EmpStaff = listReqMissScan.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.EmpName = EmpStaff.EmpName;
                        objemp.RequestType = EmpStaff.RequestType;
                        objemp.MissscanDate = EmpStaff.MissscanDate;
                        objemp.RequestDate = EmpStaff.RequestDate;
                        objemp.Time = EmpStaff.Time;
                        objemp.Reason = EmpStaff.Reason;
                        objemp.Status = EmpStaff.Status;
                        objemp.CreatedBy = EmpStaff.CreatedBy;
                        objemp.CreatedOn = EmpStaff.CreatedOn;
                        objemp.ChangedBy = EmpStaff.ChangedBy;
                        objemp.ChangedOn = EmpStaff.ChangedOn;
                        ListPending.Add(objemp);
                    }
                }
                var ListLeaveCreater = unitOfWork.Set<ATEmpMissscan>().AsEnumerable()
                    .Where(x => unitOfWork.Set<ExDocApproval>()
                        .Any(w => x.DocumentNo == w.DocumentNo  && w.Approver == userName) || x.CreatedBy == userName)
                    .ToList();
                //var ListLeaveCreater = unitOfWork.Set<ATEmpMissscan>().AsEnumerable()
                //                 .Where(x => unitOfWork.Set<ExDocApproval>().Any(w => x.DocumentNo == w.DocumentNo
                //                  && w.DocumentType == "MISSSCAN" && w.Approver == userName && x.Status != Open) || x.CreatedBy == userName).OrderByDescending(w => w.DocumentNo).ToList();
                ListHeader = ListLeaveCreater.ToList();
            }
        }
        public HRStaffProfile getNextApprover(string id, string DocType)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == DocType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == NextApp.Approver);
            return objStaff;
        }
        public string Create(string URL)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(DocType)) return "INV_DOCTYPE";
                if (Header.MissscanDate == null || Header.MissscanDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_REQUESTTYPE";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.MissscanDate.Date + Header.Time.TimeOfDay == DateTime.MinValue) return "INV_TIME";
                if (Header.MissscanDate.Date + Header.Time.TimeOfDay > DateTime.Now) return "INV_TIME";
                var StaffReq = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (StaffReq == null) return "INVALID_EMP";
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                DateTime fromdate = Header.MissscanDate.StartDateOfMonth();
                DateTime todate = Header.MissscanDate.EndDateOfMonth();
                var Peroid = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.FromDate <= Header.MissscanDate.Date && w.ToDate >= Header.MissscanDate.Date);
                if (Peroid != null)
                {
                    fromdate = Peroid.FromDate;
                    todate = Peroid.ToDate;
                }
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var lstempLeave = unitOfWork.Repository<ATEmpMissscan>().Queryable().Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                var leaveH = lstempLeave.Where(w => w.EmpCode == HeaderStaff.EmpCode && w.Status != Cancel && w.Status != Reject).ToList();
                var Missan = leaveH.Where(w => w.MissscanDate >= fromdate && w.MissscanDate <= todate).ToList();
                if (Policy != null)
                {
                    if (Missan.Count >= Policy.MissScan)
                    {
                        return "OVER_MISSCAN";
                    }
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var existingRecord = unitOfWork.Repository<ATEmpMissscan>().Queryable() .FirstOrDefault(w => w.EmpCode == Header.EmpCode
                       && DbFunctions.TruncateTime(w.MissscanDate) == Header.MissscanDate.Date
                       && w.RequestType.Trim().ToUpper() == Header.RequestType.Trim().ToUpper());
                if (existingRecord != null)
                    return "INV_DATE";
                SetAutoApproval_(Header.EmpCode, Header.RequestType, Header.MissscanDate.Date);
                if (!ListApproval.Any())
                    return "NO_LINE_MN";
                string Pending = SYDocumentStatus.PENDING.ToString();
                var objNumber = new CFNumberRank(DocType);
                if (objNumber == null) return "NUMBER_RANK_NE";
                Header.DocumentNo = objNumber.NextNumberRank.Trim();
                Header.CompanyCode = StaffReq.CompanyCode;
                Header.Time = Header.MissscanDate + Header.Time.TimeOfDay;
                Header.Status = Pending;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);

                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Header.DocumentNo;
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                unitOfWork.Save();
                unitOfWork.Commit();

                #region ---Send To Telegram---
                if (StaffReq != null && !string.IsNullOrEmpty(StaffReq.TeleGroup))
                {
                    string str = $"Subject: <b> MISS SCAN </b> %0A";
                    str += $" %0A Requested By: <b>{StaffReq.Title} {StaffReq.AllName}</b>";
                    str += $" %0A Requested Date: <b>{Header.CreatedOn}</b>";
                    str += $" %0A Miss Scan Type: <b>{Header.RequestType} </b>";
                    str += $" %0A Miss Scan Date: <b>{Header.MissscanDate} </b>";
                    str += $" %0A Reason: <b>{Header.Reason} </b>";
                    str += $" %0A Phone No: <b> {StaffReq.Phone1} </b>";
                    str += "%0A<b>Please login at:</b> <a href=\"@LINK\\\">Link</a>";
                    str = str.Replace("@LINK", URL);
                    SYSendTelegramObject Tel = new SYSendTelegramObject
                    {
                        User = User,
                        BS = BS
                    };

                    WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, StaffReq.TeleGroup, false);
                    MessageError = Tel.getErrorMessage(result1);
                }
                #endregion
                #region Notifican on Mobile
                SYHRAnnouncement _announ = new SYHRAnnouncement();
                var access = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.UserName == _announ.UserName);
                if (access != null)
                {
                    if (!string.IsNullOrEmpty(access.FirebaseID))
                    {
                        string Desc = StaffReq.AllName + @" to request Miss Scan of " + Header.RequestType +
                            " from " + Header.MissscanDate.ToString("yyyy.MM.dd");
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(access.FirebaseID);
                        var dd = Noti.SendNotification(clientToken, "MissScan Request", Desc);
                    }
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
        public string ESSCreate(string URL)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(DocType)) return "INV_DOCTYPE";
                if (string.IsNullOrEmpty(HeaderStaff.EmpCode)) return "INV_STAFF";
                if (Header.RequestDate == null || Header.RequestDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "REQUEST_TYPE_NE";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.MissscanDate.Date + Header.Time.TimeOfDay > DateTime.Now) return "INV_TIME";
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                DateTime fromdate = Header.MissscanDate.StartDateOfMonth();
                DateTime todate = Header.MissscanDate.EndDateOfMonth();
                var Peroid = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.FromDate <= Header.MissscanDate.Date && w.ToDate >= Header.MissscanDate.Date);
                if (Peroid != null)
                {
                    fromdate = Peroid.FromDate;
                    todate = Peroid.ToDate;
                }
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var lstempLeave = unitOfWork.Repository<ATEmpMissscan>().Queryable().Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                var leaveH = lstempLeave.Where(w => w.EmpCode == HeaderStaff.EmpCode && w.Status != Cancel && w.Status != Reject).ToList();
                var Missan = leaveH.Where(w => w.MissscanDate >= fromdate && w.MissscanDate <= todate).ToList();
                if (Policy != null)
                {
                    if (Missan.Count >= Policy.MissScan)
                    {
                        return "OVER_MISSCAN";
                    }
                }
                var objCF = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (objCF == null) return "INV_STAFF";
                var existingRecord = unitOfWork.Repository<ATEmpMissscan>().Queryable()
                    .FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode
                                      && DbFunctions.TruncateTime(w.MissscanDate) == Header.MissscanDate.Date
                                      && w.RequestType.Trim().ToUpper() == Header.RequestType.Trim().ToUpper());
                if (existingRecord != null)
                    return "REQUEST_TYPE_NE";
                SetAutoApproval_(HeaderStaff.EmpCode, Header.RequestType, Header.MissscanDate.Date);
                if (!ListApproval.Any())
                    return "NO_LINE_MN";
                string Status = SYDocumentStatus.PENDING.ToString();
                var objNumber = new CFNumberRank(DocType);
                if (objNumber == null) return "NUMBER_RANK_NE";
                Header.DocumentNo = objNumber.NextNumberRank.Trim();
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.EmpName = HeaderStaff.AllName;
                Header.CompanyCode = HeaderStaff.CompanyCode;
                Header.Time = Header.MissscanDate.Date + Header.Time.TimeOfDay;
                Header.Status = Status;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);

                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Header.DocumentNo;
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                unitOfWork.Save();
                unitOfWork.Commit();

                #region ---Send To Telegram---
                if (objCF != null && !string.IsNullOrEmpty(objCF.TeleGroup))
                {
                    string str = $"Subject: <b> MISS SCAN </b> %0A";
                    str += $" %0A Requested By: <b>{objCF.Title} {objCF.AllName}</b>";
                    str += $" %0A Requested Date: <b>{Header.CreatedOn}</b>";
                    str += $" %0A Miss Scan Type: <b>{Header.RequestType} </b>";
                    str += $" %0A Miss Scan Date: <b>{Header.MissscanDate} </b>";
                    str += $" %0A Reason: <b>{Header.Reason} </b>";
                    str += $" %0A Phone No: <b> {objCF.Phone1} </b>";
                    str += "%0A<b>Please login at:</b> <a href=\"@LINK\\\">Link</a>";
                    str = str.Replace("@LINK", URL);
                    SYSendTelegramObject Tel = new SYSendTelegramObject
                    {
                        User = User,
                        BS = BS
                    };

                    WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, objCF.TeleGroup, false);
                    MessageError = Tel.getErrorMessage(result1);
                }
                #endregion
                #region Notifican on Mobile
                SYHRAnnouncement _announ = new SYHRAnnouncement();
                var access = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.UserName == _announ.UserName);
                if (access != null)
                {
                    if (!string.IsNullOrEmpty(access.FirebaseID))
                    {
                        string Desc = objCF.AllName + @" to request Miss Scan of " + Header.RequestType +
                            " from " + Header.MissscanDate.ToString("yyyy.MM.dd");
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(access.FirebaseID);
                        var dd = Noti.SendNotification(clientToken, "MissScan Request", Desc);
                    }
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
        public string HRCreate(string DocNo)
        {
            OnLoad();
            try
            {
                var staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(x => x.EmpCode == Header.EmpCode);
                string Status = SYDocumentStatus.APPROVED.ToString();
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.MissscanDate.Date + Header.Time.TimeOfDay > DateTime.Now) return "INV_TIME";
                if (staff == null) return "INVALID_EMP";
                if (string.IsNullOrEmpty(DocType)) return "INV_DOCTYPE";
                if (Header.RequestDate == null || Header.RequestDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_DOC";
                var existingRecord = unitOfWork.Repository<ATEmpMissscan>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode
                      && DbFunctions.TruncateTime(w.MissscanDate) == Header.MissscanDate.Date
                      && w.RequestType.Trim().ToUpper() == Header.RequestType.Trim().ToUpper());
                if (existingRecord != null)
                    return "REQUEST_TYPE_NE";
                string Approve = SYDocumentStatus.APPROVED.ToString();
                var objNumber = new CFNumberRank(DocType);
                if (objNumber == null) return "NUMBER_RANK_NE";
                Header.DocumentNo = objNumber.NextNumberRank.Trim();
                Header.CompanyCode = staff.CompanyCode;
                Header.Time = Header.MissscanDate + Header.Time.TimeOfDay;
                Header.Status = Approve;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);
                if (Header.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    var Attcheck = new ATInOut();
                    Attcheck.EmpCode = Header.EmpCode;
                    Attcheck.CardNo = " ";
                    Attcheck.FullDate = Header.MissscanDate + Header.Time.TimeOfDay;
                    Attcheck.CreateBy = User.UserName;
                    Attcheck.CreateOn = DateTime.Now;
                    Attcheck.STATUS = 3;
                    Attcheck.LCK = 0;
                    Attcheck.FLAG = 0;
                    Attcheck.Remark = Header.Reason;
                    unitOfWork.Set<ATInOut>().Add(Attcheck);
 
                }
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
        public string Update(string DocNo, bool IsESS = false)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == DocNo);
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.MissscanDate.Date + Header.Time.TimeOfDay > DateTime.Now) return "INV_TIME";
                if (Header.RequestDate == null || Header.RequestDate == DateTime.MinValue) return "INV_DATE";
                if (string.IsNullOrEmpty(Header.RequestType))
                    return "INV_DOC";
                var existingRecord = unitOfWork.Repository<ATEmpMissscan>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode
                      && DbFunctions.TruncateTime(w.MissscanDate) == Header.MissscanDate.Date
                      && w.RequestType.Trim().ToUpper() == Header.RequestType.Trim().ToUpper() && w.DocumentNo != objMatch.DocumentNo);
                if (existingRecord != null)
                    return "REQUEST_TYPE_NE";

                ListApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentNo == DocNo && w.DocumentType == Header.RequestType).ToList();

                objMatch.MissscanDate = Header.MissscanDate;
                objMatch.Reason = Header.Reason;
                objMatch.Time = Header.MissscanDate + Header.Time.TimeOfDay;
                objMatch.RequestType = Header.RequestType;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(string DocumentNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objMast = unitOfWork.Repository<ATEmpMissscan>().Queryable().FirstOrDefault(w => w.DocumentNo == DocumentNo);
                if (objMast == null) return "INV_DOC";
                var approver = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == objMast.RequestType && w.DocumentNo == DocumentNo).ToList();
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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo, SYActionBehavior.DELETE.ToString(), e, true);
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
        public string Approve(string DocumentNo, string URL, string comment)
        {
            try
            {
                string[] c = DocumentNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                    {
                        return "INV_DOC";
                    }

                    string open = SYDocumentStatus.OPEN.ToString();
                    string DocNo = objMatch.DocumentNo;
                    var listApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == objMatch.RequestType
                                        && w.DocumentNo == DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                    var listUser = unitOfWork.Set<HRStaffProfile>().Where(w => w.EmpCode == User.UserName).ToList();
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
                            var objStaff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == read.Approver);
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
                                unitOfWork.Save();
                                b = true;
                                break;
                            }
                        }

                    }
                    if (listApproval.Count > 0)
                    {
                        if (b == false)
                        {
                            return "USER_NOT_APPROVOR";
                        }
                    }

                    var status = SYDocumentStatus.APPROVED.ToString();
                    if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                    {
                        status = SYDocumentStatus.PENDING.ToString();
                    }
                    objMatch.Status = status;
                    if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                    {
                        var Attcheck = new ATInOut();
                        Attcheck.EmpCode = objMatch.EmpCode;
                        Attcheck.FullDate = objMatch.MissscanDate.Date + objMatch.Time.TimeOfDay;
                        Attcheck.CreateBy = objMatch.EmpCode;
                        Attcheck.CreateOn = DateTime.Now;
                        Attcheck.STATUS = 3;
                        Attcheck.Remark = objMatch.Reason;
                        unitOfWork.Set<ATInOut>().Add(Attcheck);
                        unitOfWork.Save();

                        HRStaffProfile Staff = getNextApprover(DocNo, objMatch.RequestType);
                        var access = unitOfWork.Repository<TokenResource>().Queryable().FirstOrDefault(w => w.UserName == Staff.EmpCode);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                string Desc = objMatch.EmpName + @" to request Miss Scan of " + objMatch.RequestType +
                           " from " + objMatch.MissscanDate.ToString("yyyy.MM.dd");
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "LeaveRequest", Desc);
                            }
                        }
                        //var Sett = unitOfWork.Set<SYHRSetting>().First();
                        //var EmailTemplateCC = unitOfWork.Set<TPEmailTemplate>().Find("ESSLAEA_APP_CC");
                        //var StaffApp = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                        //if (EmailTemplateCC != null && StaffApp != null)
                        //{
                        //    SYSendTelegramObject Tel = new SYSendTelegramObject();
                        //    Tel.User = User;
                        //    Tel.BS = BS;
                        //    List<object> ListObjectDictionary = new List<object>();
                        //    ListObjectDictionary.Add(objMatch);
                        //    ListObjectDictionary.Add(StaffApp);
                        //    WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplateCC.EMTemplateObject, EmailTemplateCC.RequestContent, Sett.TelegLeave, ListObjectDictionary, URL);
                        //    MessageError = Tel.getErrorMessage(result1);
                        //}
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string HRApprove(string DocumentNo)
        {
            try
            {
                string[] c = DocumentNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == ID);
                    if (objMatch == null)
                        return "INV_DOC";
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                        return "CANNOTAPRROVE";

                    string Approve = SYDocumentStatus.APPROVED.ToString();
                    objMatch.Status = Approve;
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;

                    unitOfWork.Update(objMatch);
                    if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                    {
                        var Attcheck = new ATInOut();
                        Attcheck.EmpCode = objMatch.EmpCode;
                        Attcheck.FullDate = objMatch.MissscanDate + objMatch.Time.TimeOfDay;
                        Attcheck.CreateBy = User.UserName;
                        Attcheck.CreateOn = DateTime.Now;
                        Attcheck.STATUS = 3;
                        Attcheck.Remark = objMatch.Reason;
                        unitOfWork.Set<ATInOut>().Add(Attcheck);
                        unitOfWork.Save();
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string HRReject(string DocumentNo)
        {
            try
            {
                string[] c = DocumentNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == ID);
                    if (objMatch == null)
                        return "INV_DOC";
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                        return "CANNOTAPRROVE";

                    string Reject = SYDocumentStatus.REJECTED.ToString();
                    objMatch.Status = Reject;
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;

                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string Reject(string DocumentNo, string Remark, bool ESS = false)
        {
            try
            {
                string[] c = DocumentNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    var open = SYDocumentStatus.OPEN.ToString();
                    objMatch.Status = SYDocumentStatus.REJECTED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;
                    string DocNo = objMatch.DocumentNo;
                    var listApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == objMatch.RequestType
                                        && w.DocumentNo == DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                    var listUser = unitOfWork.Set<HRStaffProfile>().Where(w => w.EmpCode == User.UserName).ToList();
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
                            var objStaff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == read.Approver);
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
                                unitOfWork.Set<ExDocApproval>().Attach(read);
                                b = true;
                                break;
                            }
                        }

                    }
                    if (listApproval.Count > 0)
                    {
                        if (b == false)
                        {
                            return "USER_NOT_REJECT";
                        }
                    }
                    unitOfWork.Save();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocumentNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Cancel(string DocumentNo)
        {
            try
            {
                string[] c = DocumentNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    var listApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == objMatch.RequestType
                                        && w.DocumentNo == ID).OrderBy(w => w.ApproveLevel).ToList();
                    var listUser = unitOfWork.Set<HRStaffProfile>().Where(w => w.EmpCode == User.UserName).ToList();
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
                            if (read.Status == SYDocumentStatus.CANCELLED.ToString())
                            {
                                return "USER_ALREADY_APP";
                            }
                            var objStaff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == read.Approver);
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
                                read.Status = SYDocumentStatus.CANCELLED.ToString();
                                unitOfWork.Set<ExDocApproval>().Attach(read);
                                b = true;
                                break;
                            }
                        }

                    }
                    if (listApproval.Count > 0)
                    {
                        if (b == false)
                        {
                            return "USER_NOT_CANCELLED";
                        }
                    }
                    objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;
                    unitOfWork.Save();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
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
            AddApproverToApprovalList(staffApp, 1, Doctype, Header.DocumentNo);
            AddApproverToApprovalList(staffHOD, 2, Doctype, Header.DocumentNo);
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

    public class RequestMissScan
    {
        public string DocumentNo { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string RequestType { get; set; }
        public DateTime MissscanDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reason { get; set; }
        public DateTime? Time { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
    
    }
}
