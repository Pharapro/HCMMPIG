using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRCostCenterObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public PREmpCCCharge Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<PREmpCCCharge> ListCCCharge { get; set; }
        public List<PRCostCenter> ListCosCenterType { get; set; }
        public string Code { get; set; }
        public PRCostCenterGroup HeaderCCGroup { get; set; }
        public List<PRCostCenterGroup> ListHeaderCCGroup { get; set; }
        public List<PRCostCenterGroupItem> ListCCGroupItem { get; set; }
        public string MessageCode { get; set; }

        public PRCostCenterObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateCCGroup()
        {
            try
            {
                if (HeaderCCGroup.Code == null)
                {
                    return "INVALID_CODE";
                }
                if (ListCCGroupItem.Sum(w => w.Charge) != 100)
                {
                    return "INV_AMOUNT";
                }
                HeaderCCGroup.Code = HeaderCCGroup.Code.ToUpper().Trim();
                foreach (var read in ListCCGroupItem)
                {
                    read.CodeCCGoup = HeaderCCGroup.Code;
                    DB.PRCostCenterGroupItems.Add(read);
                }

                HeaderCCGroup.CreatedOn = DateTime.Now.Date;
                HeaderCCGroup.CreatedBy = User.UserName;

                DB.PRCostCenterGroups.Add(HeaderCCGroup);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderCCGroup.Code;
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
                log.DocurmentAction = HeaderCCGroup.Code;
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
                log.DocurmentAction = HeaderCCGroup.Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditCCGroup(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                var DBM = new HumicaDBContext();
                var objMatch = DB.PRCostCenterGroups.Find(id);
                if (id == null)
                {
                    return "DOC_NE";
                }
                if (ListCCGroupItem.Sum(w => w.Charge) != 100)
                {
                    return "INV_AMOUNT";
                }
                var objMatchD = DB.PRCostCenterGroupItems.Where(w => w.CodeCCGoup == id).ToList();
                foreach (var item in objMatchD)
                {
                    DB.PRCostCenterGroupItems.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                objMatch.Description = HeaderCCGroup.Description;
                objMatch.Remark = HeaderCCGroup.Remark;
                objMatch.ChangedOn = DateTime.Now.Date;
                objMatch.ChangedBy = User.UserName;
                foreach (var read in ListCCGroupItem)
                {
                    var CCItem = new PRCostCenterGroupItem();
                    CCItem = read;
                    CCItem.CodeCCGoup = HeaderCCGroup.Code;
                    DB.PRCostCenterGroupItems.Add(CCItem);
                }
                DB.PRCostCenterGroups.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;

                //int row1 = DBM.SaveChanges();
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderCCGroup.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.DocurmentAction = HeaderCCGroup.Code;
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
                log.DocurmentAction = HeaderCCGroup.Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteCCGroup(string id)
        {
            try
            {

                var objCust = DB.PRCostCenterGroups.Find(id);
                if (objCust == null)
                {
                    return "LEAVE_NE";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                var objMatchD = DB.PRCostCenterGroupItems.Where(w => w.CodeCCGoup == id).ToList();
                foreach (var item in objMatchD)
                {
                    DBM.PRCostCenterGroupItems.Attach(item);
                    DBM.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                DBM.PRCostCenterGroups.Attach(objCust);
                DBM.Entry(objCust).State = System.Data.Entity.EntityState.Deleted;
                int row = DBM.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderCCGroup.Code;
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
                log.ScreenId = HeaderCCGroup.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}
