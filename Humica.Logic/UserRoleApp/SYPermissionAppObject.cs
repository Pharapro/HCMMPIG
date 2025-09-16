using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic
{
    public class SYPermissionAppObject
    {
        public string ScreenId { get; set; }
        public string TemplateType { get; set; }
        public string ErrorMessage { get; set; }
        SMSystemEntity DB = new SMSystemEntity();
        public List<SYRoleAPP> ListRoleApp { get; set; }
        public SYRoleAPP HeaderRoleApp { get; set; }
        public List<SYRoleItemAPP> ListRoleItemApp { get; set; }
        public List<SYActionTemplateAPP> ListActionName { get; set; }
        public List<string> SelectedItem { get; set; }
        public List<SYActionTemplateAPP> ActionTemplateSelected { get; set; }
        public SYUser User { get; set; }
        public SYPermissionAppObject()
        {
            User = SYSession.getSessionUser();
        }
        public string createRole()
        {
            try
            {
                var tblSYRoleApp = DB.SYRoleAPPs;
                int count = 0;
                string description = string.Empty;
                description = HeaderRoleApp.Description;
                description = description.Trim();
                count = tblSYRoleApp.Where(x => x.Description == description).Count();
                if (count > 0)
                {
                    return "EXIST";
                }
                if (string.IsNullOrEmpty(HeaderRoleApp.Description))
                {
                    return "INV_DOC";
                }
                using (var con = new SMSystemEntity())
                {
                    using (DbContextTransaction transaction = con.Database.BeginTransaction())
                    {
                        try
                        {
                            SYRoleAPP roleApp = new SYRoleAPP();

                            roleApp = HeaderRoleApp;
                            roleApp.CreatedBy = User.UserName;
                            roleApp.CreatedOn = DateTime.Now;
                            roleApp.IsActive = true;
                            con.SYRoleAPPs.Add(roleApp);
                            con.SaveChanges();
                            int roleId = roleApp.RoleID;
                            foreach (var item in ActionTemplateSelected)
                            {
                                SYRoleItemAPP sYRoleItemAPP = new SYRoleItemAPP();
                                sYRoleItemAPP.RoleId = roleId;
                                sYRoleItemAPP.ScreenId = item.ScreenID;
                                sYRoleItemAPP.Description = item.ScreenName;
                                con.SYRoleItemAPPs.Add(sYRoleItemAPP);
                                con.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            transaction.Rollback();
                            SYEventLog sYEventLog = new SYEventLog();
                            sYEventLog.ScreenId = ScreenId;
                            sYEventLog.Action = SYActionBehavior.ADD.ToString();
                            SYEventLogObject.saveEventLog(sYEventLog, e);
                            return "EE001";
                        }

                    }
                }
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                //sYEventLog.UserId = User.UserID.ToString();
                //sYEventLog.DocurmentAction = HeaderRole.RoleID.ToString();
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }

        }
        public string editRole(int id)
        {
            try
            {
                var tblSYRoleApp = DB.SYRoleAPPs;

                string description = HeaderRoleApp.Description?.Trim();
                bool isEdit = tblSYRoleApp.Find(id)?.Description.Trim() == description;
                int count = tblSYRoleApp.Count(x => x.Description.Trim() == description);

                if (count > 0 && !isEdit)
                {
                    return "EXIST";
                }

                if (string.IsNullOrEmpty(HeaderRoleApp.Description))
                {
                    return "INV_DOC";
                }
                using (var con = new SMSystemEntity())
                {
                    using (DbContextTransaction transaction = con.Database.BeginTransaction())
                    {
                        try
                        {
                            var objMatch = con.SYRoleAPPs.FirstOrDefault(w => w.RoleID == id);
                            List<SYRoleItemAPP> roleItemAPPs = new List<SYRoleItemAPP>();
                            roleItemAPPs = ListRoleItemApp;
                            if (objMatch == null)
                            {
                                return "INV_DOC";
                            }
                            objMatch.Description = HeaderRoleApp.Description;
                            objMatch.IsActive = HeaderRoleApp.IsActive;
                            objMatch.ChangedBy = User.UserName;
                            objMatch.ChangedON = DateTime.Now;
                            con.SYRoleAPPs.Attach(objMatch);
                            con.Entry(objMatch).Property(w => w.Description).IsModified = true;
                            con.Entry(objMatch).Property(w => w.IsActive).IsModified = true;
                            con.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                            con.Entry(objMatch).Property(w => w.ChangedON).IsModified = true;
                            con.SaveChanges();
                            List<SYRoleItemAPP> items = new List<SYRoleItemAPP>();
                            items = roleItemAPPs.Where(x => x.RoleId == id).ToList();
                            foreach (var item in items)
                            {
                                con.SYRoleItemAPPs.Attach(item);
                                con.Entry(item).State = EntityState.Deleted;
                                con.SaveChanges();
                            }

                            foreach (var item in ActionTemplateSelected)
                            {
                                SYRoleItemAPP sYRoleItemAPP = new SYRoleItemAPP();
                                sYRoleItemAPP.RoleId = id;
                                sYRoleItemAPP.ScreenId = item.ScreenID;
                                sYRoleItemAPP.Description = item.ScreenName;
                                con.SYRoleItemAPPs.Add(sYRoleItemAPP);
                                con.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            transaction.Rollback();
                            SYEventLog sYEventLog = new SYEventLog();
                            sYEventLog.ScreenId = ScreenId;
                            sYEventLog.Action = SYActionBehavior.ADD.ToString();
                            SYEventLogObject.saveEventLog(sYEventLog, e);
                            return "EE001";
                        }

                    }
                }
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                //sYEventLog.UserId = User.UserID.ToString();
                //sYEventLog.DocurmentAction = HeaderRole.RoleID.ToString();
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
        }

        public string deleteRole(int id)
        {

            try
            {
                DB = new SMSystemEntity();
                HeaderRoleApp = DB.SYRoleAPPs.Find(id);
                if (HeaderRoleApp == null)
                {
                    return "ROLE_NE";
                }
                DB.SYRoleAPPs.Attach(HeaderRoleApp);
                DB.Entry(HeaderRoleApp).State = EntityState.Deleted;
                DB.SaveChanges();
                var tblSYRoleAppItem = DB.SYRoleItemAPPs;
                var items = tblSYRoleAppItem.Where(x => x.RoleId == id);
                DB = new SMSystemEntity();
                foreach (var item in items)
                {
                    DB.SYRoleItemAPPs.Attach(item);
                    DB.Entry(item).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {

                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                //sYEventLog.UserId = User.UserID.ToString();
                //sYEventLog.DocurmentAction = HeaderRole.RoleID.ToString();
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";

            }
            return "OK";
        }


    }
}
