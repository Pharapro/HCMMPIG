using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.EOB
{
    public class OnBoardChkLstObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public EOBEmpChkLst Header { get; set; }
        public HR_STAFF_VIEW staffProfile { get; set; }
        public List<HRStaffProfile> ListAnnounce { get; set; }
        public List<EOBConfirmAlert> ListConfirmed { get; set; }
        public List<HRStaffProfile> ListstaffProfile { get; set; }
        public List<EOBEmpChkLst> ListHeader { get; set; }
        public List<EOBEmpChkLstItem> ListItem { get; set; }
        public List<EOBSChkLstItem> ListCheckItem { get; set; }
        public List<EOBSChkLst> ListCheck { get; set; }
        public string MessageError { get; set; }
        public string CheckedItem { get; set; }
        public OnBoardChkLstObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string OBCheckList(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                string[] Answers = id.Split(';');
                if (id == null)
                {
                    return "CHECKED_EMPTY";
                }

                var Stafff = DBV.HR_STAFF_VIEW.ToList();
                var lstChkLstItem = DB.EOBSChkLstItems.ToList();
                HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (EmpStaff != null)
                {
                    Header.EmployeeName = EmpStaff.AllName;
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    foreach (var r in Answers)
                    {

                        if (r != "")
                        {
                            string[] anstr = r.Split(',');
                            string Line = anstr[0].ToString();
                            string Code = anstr[1].ToString();
                            var obj = new EOBEmpChkLstItem();
                            obj.EmpCode = Header.EmpCode;
                            obj.LineItem = Convert.ToInt32(Line);
                            obj.Code = Code;
                            var check = lstChkLstItem.Where(w => w.Code == Code && w.LineItem == obj.LineItem).ToList();
                            if (check.Count > 0)
                            {
                                obj.Description = check.First().Description;
                            }
                            DB.EOBEmpChkLstItems.Add(obj);
                        }
                    }
                    DB.EOBEmpChkLsts.Add(Header);
                }
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string updChkLst(string id)
        {
            try
            {
                var DBD = new HumicaDBContext();
                DB = new HumicaDBContext();
                ListItem = new List<EOBEmpChkLstItem>();
                var ObjMatch = DB.EOBEmpChkLsts.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                var _listScore = DB.EOBEmpChkLstItems.Where(w => w.EmpCode == Header.EmpCode).ToList();
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                foreach (var read in _listScore)
                {
                    DBD.EOBEmpChkLstItems.Attach(read);
                    DBD.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.Remark = Header.Remark;
                ObjMatch.DocumentDate = Header.DocumentDate;

                HumicaDBContext DBM = new HumicaDBContext();
                var lstChkLstItem = DB.EOBSChkLstItems.ToList();
                string[] Answers = id.Split(';');
                foreach (var r in Answers)
                {
                    if (r != "")
                    {
                        string[] anstr = r.Split(',');
                        string Line = anstr[0].ToString();
                        string Code = anstr[1].ToString();
                        var obj = new EOBEmpChkLstItem();
                        obj.EmpCode = Header.EmpCode;
                        obj.LineItem = Convert.ToInt32(Line);
                        obj.Code = Code;
                        var check = lstChkLstItem.Where(w => w.Code == Code && w.LineItem == obj.LineItem).ToList();
                        if (check.Count > 0)
                        {
                            obj.Description = check.First().Description;
                        }
                        ListItem.Add(obj);
                        DBM.EOBEmpChkLstItems.Add(obj);
                    }
                }
                DBM.EOBEmpChkLsts.Attach(ObjMatch);

                DBM.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
                DBM.Entry(ObjMatch).Property(x => x.DocumentDate).IsModified = true;
                DBM.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DBM.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DBD.SaveChanges();
                int row = DBM.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string deleteChkLst(string EmpCode)
        {
            try
            {
                DB = new HumicaDBContext();

                var ObjMatch = DB.EOBEmpChkLsts.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                var ListEmpD = DB.EOBEmpChkLstItems.Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var item in ListEmpD)
                {
                    DB.EOBEmpChkLstItems.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                DB.EOBEmpChkLsts.Remove(ObjMatch);
                DB.Entry(ObjMatch).State = System.Data.Entity.EntityState.Deleted;
                var row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }

}