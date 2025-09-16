using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.LM
{
    public class ClaimLeaveObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string MessageError { get; set; }
        public string ScreenId { get; set; }
        public FTINYear FInYear { get; set; }
        public HRClaimLeave Header { get; set; }
        public List<HRClaimLeave> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile Header_Staff { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRClaimLeave> ListEmpLeaveReq { get; set; }
        public ClaimLeaveObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void SetAutoApproval(string EmpCode, List<HRStaffProfile> LstStaffProfiles)
        {
            ListApproval = new List<ExDocApproval>();
            var LstStaff = LstStaffProfiles.Where(w => w.Status == "A");
            HRStaffProfile _StaffReq = LstStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            int line = 0;
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTFirstLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTFirstLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTSecondLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTSecondLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTthirdLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTthirdLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == "REQ_CL").ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);
            return objStaff;
        }
        public string ClaimLeave(string fileName="", bool IsESS = false)
        {
            try
            {
                DB = new HumicaDBContext();
                using (var context = new HumicaDBContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var EmpClaim = DB.HRClaimLeaves.Where(w => w.EmpCode == Header.EmpCode && w.WorkingDate.Year == Header.WorkingDate.Year).ToList();
                            if (EmpClaim.Where(w => w.WorkingDate.Date == Header.WorkingDate.Date).Any())
                            {
                                return "Date " + Header.WorkingDate.ToString("dd-MMM-yyyy") + " already exist";
                            }
                            if (Header.EmpCode == null)
                                return "EMPCODE_EN";
                            if (Header.WorkingType == null)
                                return "WORKTYPE_EN";
                            if (Header.ClaimLeave == null)
                                return "CLAIMLEAVE_EN";
                            Header_Staff = DBV.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                            Header.Expired = Header.WorkingDate.AddDays(7);
                            Header.Status = SYDocumentStatus.APPROVED.ToString();
                            if (IsESS == true)
                            {
                                List<HRStaffProfile> ListStaff = DBV.HRStaffProfiles.ToList();
                                Header.Status = SYDocumentStatus.PENDING.ToString();
                                SetAutoApproval(Header.EmpCode, ListStaff);
                            }
                            if (Header_Staff.IsAutoAppLeave == true)
                                Header.Status = SYDocumentStatus.APPROVED.ToString();
                            Header.IsExpired = false;
                            Header.IsUsed = false;
                            Header.EmpName = Header_Staff.AllName;
                            Header.CreatedBy = User.UserName;
                            Header.CreatedOn = DateTime.Now;

                            context.HRClaimLeaves.Add(Header);
                            int row = context.SaveChanges();
                            int line = 0;
                            foreach (var item in ListApproval)
                            {
                                line += 1;
                                item.ID = 0;
                                item.LastChangedDate = DateTime.Now;
                                item.DocumentNo = Header.TranNo.ToString();
                                item.Status = SYDocumentStatus.OPEN.ToString();
                                item.ApprovedBy = "";
                                item.ApprovedName = "";
                                item.ApproveLevel = line;
                                item.DocumentType = "REQ_CL";
                                context.ExDocApprovals.Add(item);
                                context.SaveChanges();
                            }

                            var _LeaveType = DB.HRLeaveTypes.Find(Header.ClaimLeave);
                            SYHRAnnouncement _announ = new SYHRAnnouncement();
                            var _Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                            var _Lstapp = ListApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                            _announ.Type = "CompensatoryRequest";
                            if (_Lstapp.Count() > 0)
                            {
                                var min = _Lstapp.Min(w => w.ApproveLevel);
                                _announ.Subject = _Staff.AllName;
                                _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                                _announ.Description = @"Claim type of " + _LeaveType.Description +
                                    " from " + Header.WorkingDate.ToString("yyyy.MM.dd") + " My Reason: " + Header.Remark;
                            }
                            if (Header.Status == SYDocumentStatus.APPROVED.ToString())
                            {
                                _announ.Type = "CompensatoryRequested";
                                _announ.Subject = "Request";
                                _announ.UserName = Header.EmpCode;
                                _announ.Description = "Claim type of " + _LeaveType.Description;
                            }
                            _announ.DocumentNo = Header.TranNo.ToString();
                            _announ.DocumentDate = DateTime.Now;
                            _announ.IsRead = false;
                            _announ.CreatedBy = User.UserName;
                            _announ.CreatedOn = DateTime.Now;
                            context.SYHRAnnouncements.Add(_announ);

                            dbContextTransaction.Commit();
                            if (IsESS == true)
                            {
                                #region Notifican on Mobile
                                var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _Staff.EmpCode);
                                if (access != null)
                                {
                                    if (!string.IsNullOrEmpty(access.FirebaseID))
                                    {
                                        string Desc = _announ.Description;
                                        Notification.Notificationf Noti = new Notification.Notificationf();
                                        var clientToken = new List<string>();
                                        clientToken.Add(access.FirebaseID);
                                        var dd = Noti.SendNotification(clientToken, "CompensatoryRequested", Desc);
                                    }
                                }
                                #endregion
                            }
                            if (IsESS == false || Header_Staff.IsAutoAppLeave == true)
                                Generate_Balance_Claim(Header.EmpCode, Header.ClaimLeave, Header.WorkingDate.Year);
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string UpdateClaimLeave(string id)
        {
            try
            {
                if (Header.WorkingType == null)
                    return "WORKTYPE_EN";
                if (Header.ClaimLeave == null)
                    return "CLAIMLEAVE_EN";

                DB = new HumicaDBContext();
                int TranNo = Convert.ToInt32(id);
                var ObjMatch = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);

                if (ObjMatch == null)
                    return "DOC_INV";

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.Expired = Header.WorkingDate.AddDays(7);
                ObjMatch.WorkingDate = Header.WorkingDate;
                ObjMatch.WorkingType = Header.WorkingType;
                ObjMatch.WorkingHour = Header.WorkingHour;
                ObjMatch.ClaimLeave = Header.ClaimLeave;
                ObjMatch.Remark = Header.Remark;
                ObjMatch.IsExpired = false;
                DateTime DateNow = DateTime.Now;
                if (ObjMatch.Expired.Value.Date < DateNow.Date)
                {
                    ObjMatch.IsExpired = true;
                }
                DB.HRClaimLeaves.Attach(ObjMatch);

                DB.Entry(ObjMatch).Property(x => x.WorkingDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.WorkingType).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.WorkingHour).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ClaimLeave).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Expired).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.IsExpired).IsModified = true;

                DB.SaveChanges();
                Generate_Balance_Claim(ObjMatch.EmpCode, ObjMatch.ClaimLeave, ObjMatch.WorkingDate.Year);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteClaimLeav(string id)
        {
            try
            {
                DB = new HumicaDBContext();

                int TranNo = Convert.ToInt32(id);
                var ObjMatch = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                if (ObjMatch == null)
                    return "DOC_INV";
                if (ObjMatch.IsUsed == true)
                {
                    return "PROCESS_EN";
                }
                DB.HRClaimLeaves.Remove(ObjMatch);

                DB.SaveChanges();
                Generate_Balance_Claim(ObjMatch.EmpCode, ObjMatch.ClaimLeave, ObjMatch.WorkingDate.Year);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(string id, string URL)
        {
            try
            {
                DB = new HumicaDBContext();
                int TranNo = Convert.ToInt32(id);
                var objMatch = DB.HRClaimLeaves.Find(TranNo);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                var _LeaveType = DB.HRLeaveTypes.Find(objMatch.ClaimLeave);
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == "REQ_CL"
                                    && w.DocumentNo == id && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
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

                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DB.ExDocApprovals.Attach(read);
                            DB.Entry(read).State = System.Data.Entity.EntityState.Modified;
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

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;


                SYHRAnnouncement _announ = new SYHRAnnouncement();
                var _Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                var _Lstapp = listApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                _announ.Type = "LeaveRequest";
                if (_Lstapp.Count() > 0)
                {
                    var min = _Lstapp.Min(w => w.ApproveLevel);
                    _announ.Subject = _Staff.AllName;
                    _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                    _announ.Description = @"Claim type of " + _LeaveType.Description +
                        " from " + objMatch.WorkingDate.ToString("yyyy.MM.dd") + " My Reason: " + objMatch.Remark;
                }
                if (status == SYDocumentStatus.APPROVED.ToString())
                {
                    _announ.Type = "LeaveApproved";
                    _announ.Subject = "Approved";
                    _announ.UserName = objMatch.EmpCode;
                    _announ.Description = "Leave type of " + _LeaveType.Description;
                }
                _announ.DocumentNo = id;
                _announ.DocumentDate = DateTime.Now;
                _announ.IsRead = false;
                _announ.CreatedBy = User.UserName;
                _announ.CreatedOn = DateTime.Now;
                DB.SYHRAnnouncements.Add(_announ);

                int row = DB.SaveChanges();
                DBV = new HumicaDBViewContext();
                if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    Generate_Balance_Claim(objMatch.EmpCode, objMatch.ClaimLeave, objMatch.WorkingDate.Year);
                    #region Notifican on Mobile
                    var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _Staff.EmpCode);
                    if (access != null)
                    {
                        if (!string.IsNullOrEmpty(access.FirebaseID))
                        {
                            string Desc = _announ.Description;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(access.FirebaseID);
                            var dd = Noti.SendNotification(clientToken, "LeaveApproved", Desc);
                        }
                    }
                    #endregion
                }
                else
                {
                    HRStaffProfile Staff = getNextApprover(id, "");
                    var access = DB.TokenResources.FirstOrDefault(w => w.UserName == Staff.EmpCode);
                    if (access != null)
                    {
                        if (!string.IsNullOrEmpty(access.FirebaseID))
                        {
                            string Desc = _announ.Description;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(access.FirebaseID);
                            var dd = Noti.SendNotification(clientToken, "LeaveApproved", Desc);
                        }
                    }

                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string RejectLeave(string ApprovalID)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                string[] c = ApprovalID.Split(';');
                foreach (var r in c)
                {
                    if (r == "") continue;
                    int TranNo = Convert.ToInt32(r);
                    string Reject = SYDocumentStatus.REJECTED.ToString();
                    HRClaimLeave objmatch = DB.HRClaimLeaves.First(w => w.TranNo == TranNo);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == TranNo.ToString());
                    foreach (var read in _obj)
                    {
                        read.Status = Reject;
                        read.LastChangedDate = DateTime.Now;
                        DB.Entry(read).Property(w => w.Status).IsModified = true;
                        DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                    }
                    objmatch.Status = Reject;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.HRClaimLeaves.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                    DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void Generate_Balance_Claim(string EmpCode, string LeaveType, int Year)
        {
            HumicaDBContext DBI = new HumicaDBContext();
            string Approval = SYDocumentStatus.APPROVED.ToString();
            var EmpClaim = DBI.HRClaimLeaves.Where(w => w.EmpCode == EmpCode && w.WorkingDate.Year == Year && w.Status == Approval).ToList();
            HRStaffProfile _Staff = DBI.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            var Parameter = DBI.PRParameters.FirstOrDefault(w => w.Code == _Staff.PayParam);
            HREmpLeaveB LeaveB = DBI.HREmpLeaveBs.Where(w => w.EmpCode == EmpCode && w.LeaveCode == LeaveType
                  && w.InYear == Year).FirstOrDefault();

            decimal? WHour = 8;
            decimal PH_Edit = 0;
            decimal Rest_Edit = 0;
            if (Parameter != null) WHour = Parameter.WHOUR;
            decimal LDay = 0;
            DateTime DateNow = DateTime.Now;
            foreach (var item in EmpClaim)
            {
                if (item.IsUsed == true)
                {
                    LDay = Math.Round(item.WorkingHour / WHour.Value, 2);
                }
                else
                {
                    if (item.Expired.Value.Date < DateNow.Date)
                    {
                        item.IsExpired = true;
                        DBI.HRClaimLeaves.Attach(item);
                        DBI.Entry(item).Property(x => x.IsExpired).IsModified = true;
                    }
                    else LDay = Math.Round(item.WorkingHour / WHour.Value, 2);
                }
                if (item.WorkingType == "PH") PH_Edit += LDay;
                else if (item.WorkingType == "RD") Rest_Edit += LDay;
            }
            if (LeaveB == null)
            {
                LeaveB = new HREmpLeaveB();
                LeaveB.EmpCode = Header.EmpCode;
                LeaveB.AllName = _Staff.AllName;
                LeaveB.LToDate = 0;
                LeaveB.ForwardUse = 0;
                LeaveB.Forward = 0;
                LeaveB.DayEntitle = 0;
                LeaveB.DayLeave = 0;
                LeaveB.LeaveCode = Header.ClaimLeave;
                LeaveB.InYear = Header.WorkingDate.Year;
                LeaveB.ForWardExp = new DateTime(1900, 01, 01);
                LeaveB.PH_Edit = PH_Edit;
                LeaveB.Rest_Edit = Rest_Edit;
                LeaveB.YTD = (LeaveB.DayEntitle ?? 0) + PH_Edit + Rest_Edit;
                LeaveB.Balance = (LeaveB.DayEntitle ?? 0) + PH_Edit + Rest_Edit;
                LeaveB.CreateBy = User.UserName;
                LeaveB.CreateOn = DateTime.Now;
                DBI.HREmpLeaveBs.Add(LeaveB);
            }
            else
            {
                LeaveB.PH_Edit = PH_Edit;
                LeaveB.Rest_Edit = Rest_Edit;
                LeaveB.YTD = LeaveB.DayEntitle + LeaveB.PH_Edit + LeaveB.Rest_Edit;
                LeaveB.Balance = LeaveB.DayEntitle + LeaveB.PH_Edit + LeaveB.Rest_Edit - LeaveB.DayLeave;
                DBI.HREmpLeaveBs.Attach(LeaveB);
                DBI.Entry(LeaveB).Property(w => w.Balance).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.YTD).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.Rest_Edit).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.PH_Edit).IsModified = true;
            }
            DBI.SaveChanges();
        }
    }
}