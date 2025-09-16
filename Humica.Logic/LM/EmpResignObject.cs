using Humica.Core.DB;
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
    public class EmpResignObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HREmpResign EmpResign { get; set; }
        public ExDocApproval DocApproval { get; set; }
        public HR_STAFF_VIEW StaffInfor { get; set; }
        public List<HREmpResign> ListHeader { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public EmpResignObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        #region "Save"
        public string SaveResign()
        {
            try
            {
                EmpResign.EmpCode = User.UserName;
                EmpResign.DocDate = DateTime.Now;
                EmpResign.CreatedBy = User.UserName;
                EmpResign.CreatedOn = DateTime.Now;
                if (EmpResign.Reason == null)
                {
                    return "RREASON";
                }
                DB.HREmpResigns.Add(EmpResign);

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = EmpResign.EmpCode;
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
                log.DocurmentAction = EmpResign.EmpCode;
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
                log.DocurmentAction = EmpResign.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion 'Save'

        #region 'Status'
        public string Request(string id)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                HREmpResign objmatch = DB.HREmpResigns.First(w => w.ID == ID);
                var chkFirstLine = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                //var chkFLName = DB.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == chkFirstLine.FirstLine);

                if (objmatch == null) return "DOC_INV";

                SetAutoApproval(objmatch.EmpCode, chkFirstLine.Branch, "RES");
                if (ListApproval.Count() <= 0)
                {
                    return "NO_LINE_MN";
                }
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = objmatch.ID.ToString();
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    DB.ExDocApprovals.Add(read);
                }

                objmatch.Status = SYDocumentStatus.PENDING.ToString();
                DB.HREmpResigns.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;

                //DocApproval = new ExDocApproval();
                //DocApproval.Approver = chkFirstLine.FirstLine;
                //DocApproval.ApproverName = chkFLName.AllName;
                //DocApproval.ApprovedBy = "";
                //DocApproval.ApprovedName = "";
                //DocApproval.ApproveLevel = 1;
                //DocApproval.DocumentType = "RES";
                //DocApproval.DocumentNo = objmatch.ID.ToString();
                //DocApproval.Status = open;
                //DocApproval.WFObject = "WF02";
                //DB.ExDocApprovals.Add(DocApproval);

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public void SetAutoApproval(string EmpCode, string Branch, string DocType)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var LstStaff = DB.HRStaffProfiles.ToList();
            LstStaff = LstStaff.Where(w => w.DateTerminate.Year == 1900).ToList();
            var ListWorkFlow = DB.HRWorkFlowLeaves.ToList();
            var _staffApp = new HRStaffProfile();
            foreach (var item in ListWorkFlow)
            {
                var Staff = LstStaff.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (item.ApproveBy == "1st")
                {
                    var Read = LstStaff.Where(w => w.JobCode == Staff.FirstLine).ToList();
                    if (Read.Count() > 1)
                    {
                        _staffApp = Read.FirstOrDefault(w => w.Branch == Branch);
                        if (_staffApp == null)
                            _staffApp = Read.FirstOrDefault(w => w.DEPT == Staff.DEPT);
                    }
                    else
                        _staffApp = Read.FirstOrDefault();
                }
                else if (item.ApproveBy == "2nd")
                {
                    if (Staff.SecondLine != null)
                    {
                        var Read = LstStaff.Where(w => w.JobCode == Staff.SecondLine).ToList();
                        if (Read.Count > 1)
                        {
                            _staffApp = Read.FirstOrDefault(w => w.Branch == Branch);
                            if (_staffApp == null)
                                _staffApp = Read.FirstOrDefault(w => w.DEPT == Staff.DEPT);
                        }
                        else
                            _staffApp = Read.FirstOrDefault();
                    }
                }
                else
                {
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy && w.Branch == Branch);
                }
                if (_staffApp == null)
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy);
                if (_staffApp == null) continue;
                if (ListApproval.Where(w => w.Approver == _staffApp.EmpCode).Count() > 0) continue;
                var objApp = new ExDocApproval();
                objApp.Approver = _staffApp.EmpCode;
                objApp.ApproverName = _staffApp.AllName;
                objApp.DocumentType = DocType;
                objApp.ApproveLevel = item.Step;
                objApp.WFObject = "WF02";
                ListApproval.Add(objApp);
            }
        }
        public string Approve(string id)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                var objMatch = DB.HREmpResigns.Find(ID);
                string open = SYDocumentStatus.OPEN.ToString();
                string DocNo = objMatch.ID.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == "RES"
                                    && w.DocumentNo == DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;

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
                            DBI.ExDocApprovals.Attach(read);
                            DBI.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            b = true;
                            break;
                        }
                    }

                }
                if (listApproval.Count > 0 || objMatch.Status == open)
                {
                    if (b == false)
                    {
                        return "USER_CANNOT_APPROVE";
                    }
                }
                var status = SYDocumentStatus.APPROVED.ToString();
                var cancel = SYDocumentStatus.CANCELLED.ToString();
                if (objMatch.Status == cancel)
                {
                    return "Document already cancelled cannot Approve";
                }
                var ListRes = DB.HREmpResigns.Where(w => w.EmpCode == objMatch.EmpCode && w.ID == ID).ToList();
                foreach (var item in ListRes)
                {
                    item.Status = status;
                    DB.HREmpResigns.Attach(item);
                    DB.Entry(item).Property(w => w.Status).IsModified = true;
                }

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Cancel(string id)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                HREmpResign objmatch = DB.HREmpResigns.First(w => w.ID == ID);
                var chkstatusApp = DB.ExDocApprovals.Where(w => w.Status == Open && w.DocumentNo == id && w.DocumentType == "RES").ToList();
                if (objmatch == null)
                {
                    return "DOC_INV";
                }
                if (chkstatusApp != null)
                {
                    foreach (var read in chkstatusApp)
                    {
                        read.Status = Cancel;
                        DB.ExDocApprovals.Attach(read);
                        DB.Entry(read).Property(w => w.Status).IsModified = true;
                    }
                }
                if (objmatch.Status == Open || objmatch.Status == Pending)
                {
                    objmatch.Status = Cancel;
                    DB.HREmpResigns.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                else
                {
                    return "Document is approved cannot cancel";
                }

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion 'Status'
    }
}