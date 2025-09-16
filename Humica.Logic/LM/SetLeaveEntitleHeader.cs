using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.LM
{
    public class SetLeaveEntitleHeader
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRSetEntitleH Header { get; set; }
        public List<HRSetEntitleH> ListHeader { get; set; }
        public HRSetEntitleD SetEntitleDetail { get; set; }
        public List<HRSetEntitleD> ListSetEntitleDetail { get; set; }
        public SetLeaveEntitleHeader()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateSetEntitle()
        {
            try
            {
                if (Header.Code == null)
                {
                    return "INVALID_CODE";
                }
                Header.Code = Header.Code.ToUpper().Trim();
                int lineItem = 0;
                foreach (var read in ListSetEntitleDetail)
                {
                    var objCheckL = DB.HRLeaveTypes.Find(read.LeaveCode);
                    if (objCheckL == null)
                    {
                        return "LEAVE_NE";
                    }
                    lineItem++;
                    var objLeave = new HRSetEntitleD();
                    objLeave.CodeH = Header.Code;
                    objLeave.LeaveCode = read.LeaveCode;
                    objLeave.FromMonth = read.FromMonth;
                    objLeave.ToMonth = read.ToMonth;
                    objLeave.Entitle = read.Entitle;
                    objLeave.Remark = read.Remark;
                    objLeave.IsProRate = read.IsProRate;
                    objLeave.LineItem = lineItem;
                    DB.HRSetEntitleDs.Add(objLeave);
                }


                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;

                DB.HRSetEntitleHs.Add(Header);

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
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Code;
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
                log.DocurmentAction = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditSetEntitle(string id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HRSetEntitleHs.Find(id);

                if (id == null)
                {
                    return "DOC_NE";
                }
                var objMatchD = DB.HRSetEntitleDs.Where(w => w.CodeH == id).ToList();
                foreach (var item in objMatchD)
                {
                    DBM.HRSetEntitleDs.Attach(item);
                    DBM.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                objMatch.Description = Header.Description;
                objMatch.OthDesc = Header.OthDesc;
                objMatch.Remark = Header.Remark;
                objMatch.ChangedOn = DateTime.Now.Date;
                objMatch.ChangedBy = User.UserName;
                int line = 1;
                foreach (var read in ListSetEntitleDetail.OrderBy(w => w.LeaveCode).ThenBy(w => w.FromMonth))
                {
                    var objCheckL = DB.HRLeaveTypes.Find(read.LeaveCode);
                    if (objCheckL == null)
                    {
                        return "LEAVE_NE";
                    }
                    read.CodeH = Header.Code;
                    read.LineItem = line;
                    DB.HRSetEntitleDs.Add(read);

                    line += 1;
                }
                DB.HRSetEntitleHs.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.OthDesc).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;

                int row1 = DBM.SaveChanges();
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteHRSetEntitleH(string id)
        {
            try
            {

                HRSetEntitleH objCust = DB.HRSetEntitleHs.Find(id);
                if (objCust == null)
                {
                    return "LEAVE_NE";
                }
                DB = new HumicaDBContext();
                var EntitleItem = DB.HRSetEntitleDs.Where(w => w.CodeH == id).ToList();
                foreach (var item in EntitleItem.ToList())
                {
                    DB.HRSetEntitleDs.Remove(item);
                }
                DB.HRSetEntitleHs.Attach(objCust);
                DB.Entry(objCust).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id;
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
                log.ScreenId = id;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}