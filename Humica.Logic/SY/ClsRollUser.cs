using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Security;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;

namespace Humica.EF.Models.SY
{
    public class ClsRollUser
    {
        public SMSystemEntity DB = new SMSystemEntity();
        HumicaDBContext DBA = new HumicaDBContext();

        public string StorageSelected = "";

        public string RoleSelected = "";

        public string RoleSelectedAPP = "";

        public string LevelSelected = "";

        public string DataSelected = "";

        public string LevelDataSelected = "";

        public SYUser User { get; set; }

        public string ScreenId { get; set; }

        public SYUserBusiness BS { get; set; }

        public SYRole Roles { get; set; }

        public SYRoleItem RolesItem { get; set; }

        public SYRoleGroup RolesGroup { get; set; }

        public SYRoleGroupItem RoleGroupItem { get; set; }

        public SYUserGroup UserGroup { get; set; }

        public List<SYUserGroup> ListUserGroup { get; set; }

        public string UserName { get; set; }

        public SYUser UserObject { get; set; }

        public SYUserBusiness UserBusinessObject { get; set; }

        public List<SYUser> ListHeader { get; set; }

        public string MessageCode { get; set; }
        public string EmpID { get; set; }
        public string Branch { get; set; }
        public string Level { get; set; }

        public SYRole HeaderRole { get; set; }

        public List<SYRole> ListHeaderRole { get; set; }

        public List<SYRoleAPP> ListHeaderRoleAPP { get; set; }

        public List<SYRoleItem> ListRoleItem { get; set; }

        public SYUserDataAcess DataAccess { get; set; }

        public List<SYUserDataAcess> ListDataAccess { get; set; }

        public List<SYUserRole> ListRoleAssigned { get; set; }

        public List<SYUserRoleAPP> ListRoleAPPAssigned { get; set; }

        public List<SYUserDataAcess> ListDataAccessAssigned { get; set; }

        public List<SYUserLevel> ListLevelAssigned { get; set; }

        public List<CFTRCode> ListTRCode { get; set; }

        public List<HRLevel> ListUserLevel { get; set; }

        public List<HRBranch> ListStorage { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }

        public FTFilterEmployee Filter { get; set; }

        public ClsRollUser()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<HRStaffProfile> LoadDataEmp(FTFilterEmployee Filter1)
        {
            DateTime date = DateTime.Now;

            var validUsers = DB.SYUsers
                .Where(w => w.ExpireDate > date)
                .AsNoTracking()
                .Select(s => s.UserName)
                .ToList();

            var staffQuery = DBA.HRStaffProfiles
                .Where(w => w.Status == "A" || (w.DateTerminate > date))
                .AsNoTracking()
                .Select(s => new { s.EmpCode, s.AllName, s.DEPT, s.JobCode, s.Branch, s.LevelCode });

            if (!Filter1.IsIncludeHasUser)
            {
                staffQuery = staffQuery.Where(w => !validUsers.Contains(w.EmpCode));
            }

            if (!string.IsNullOrEmpty(Filter1.Branch))
            {
                var branches = Filter1.Branch.Split(',')
                                             .Select(b => b.Trim())
                                             .Where(b => !string.IsNullOrEmpty(b))
                                             .ToList();
                staffQuery = staffQuery.Where(w => branches.Contains(w.Branch));
            }

            if (!string.IsNullOrEmpty(Filter1.Department))
            {
                staffQuery = staffQuery.Where(w => w.DEPT == Filter1.Department);
            }

            if (!string.IsNullOrEmpty(Filter1.Position))
            {
                staffQuery = staffQuery.Where(w => w.JobCode == Filter1.Position);
            }

            if (!string.IsNullOrEmpty(Filter1.Level))
            {
                var levels = Filter1.Level.Split(',')
                                          .Select(l => l.Trim())
                                          .Where(l => !string.IsNullOrEmpty(l))
                                          .ToList();
                staffQuery = staffQuery.Where(w => levels.Contains(w.LevelCode));
            }

            var filteredStaff = staffQuery.ToList();

            var staffProfiles = filteredStaff.Select(s => new HRStaffProfile
            {
                EmpCode = s.EmpCode,
                AllName = s.AllName
            }).OrderBy(w => w.EmpCode).ToList();

            return staffProfiles;
        }

        public string changePwd(int userID, string pwd)
        {
            try
            {
                SYUser sYUser = DB.SYUsers.Find(userID);
                if (sYUser == null)
                {
                    return "USR_NE";
                }

                sYUser.IsFirstLogChange = false;
                sYUser.ChangedBy = User.UserName;
                sYUser.ChangedOn = DateTime.Now;
                sYUser.Password = PasswordHash.HashPassword(pwd);
                SMSystemEntity sMSystemEntity = new SMSystemEntity();
                sMSystemEntity.SYUsers.Attach(sYUser);
                sMSystemEntity.Entry(sYUser).Property((SYUser w) => w.ChangedBy).IsModified = true;
                sMSystemEntity.Entry(sYUser).Property((SYUser w) => w.ChangedOn).IsModified = true;
                sMSystemEntity.Entry(sYUser).Property((SYUser w) => w.Password).IsModified = true;
                sMSystemEntity.Entry(sYUser).Property((SYUser w) => w.IsFirstLogChange).IsModified = true;
                int num = sMSystemEntity.SaveChanges();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserID.ToString();
                sYEventLog.DocurmentAction = UserObject.UserName;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserID.ToString();
                sYEventLog2.DocurmentAction = UserObject.UserName;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }

        public string createUser()
        {
            try
            {
                bool IsUser = false;
                SMSystemEntity sMSystemEntity = new SMSystemEntity();
                List<SYUser> list = DB.SYUsers.Where((SYUser w) => w.UserName == UserName).ToList();
                if (list.Count > 0)
                {
                    var UserObj = new SYUser();
                    UserObj = list.First();
                    //List<SYEventLog> list2 = DB.SYEventLogs.Where((SYEventLog w) => w.UserId == UserObj.UserName).Take(1).ToList();
                    //if (list2.Count > 0)
                    //{
                    //    return "USER_OPR";
                    //}
                    IsUser = true;
                    UserObject.UserID = UserObj.UserID;
                    //sMSystemEntity.SYUsers.Attach(UserObj);
                    //sMSystemEntity.Entry(UserObj).State = EntityState.Deleted;
                    List<SYUserRole> list3 = DB.SYUserRoles.Where((SYUserRole w) => w.UserID == UserObj.UserID).ToList();
                    foreach (SYUserRole item in list3)
                    {
                        sMSystemEntity.SYUserRoles.Attach(item);
                        sMSystemEntity.Entry(item).State = EntityState.Deleted;
                    }

                    List<SYUserRoleAPP> list4 = DB.SYUserRoleAPPs.Where((SYUserRoleAPP w) => w.UserID == UserObj.UserID).ToList();
                    foreach (SYUserRoleAPP item2 in list4)
                    {
                        sMSystemEntity.SYUserRoleAPPs.Attach(item2);
                        sMSystemEntity.Entry(item2).State = EntityState.Deleted;
                    }

                    List<SYUserLevel> list5 = DB.SYUserLevels.Where((SYUserLevel w) => w.UserName == UserObj.UserName).ToList();
                    foreach (SYUserLevel item3 in list5)
                    {
                        sMSystemEntity.SYUserLevels.Attach(item3);
                        sMSystemEntity.Entry(item3).State = EntityState.Deleted;
                    }

                    List<SYUserDataAcess> list6 = DB.SYUserDataAcesses.Where((SYUserDataAcess w) => w.UserId == UserObj.UserID).ToList();
                    foreach (SYUserDataAcess item4 in list6)
                    {
                        sMSystemEntity.SYUserDataAcesses.Attach(item4);
                        sMSystemEntity.Entry(item4).State = EntityState.Deleted;
                    }
                }

                if (RoleSelected == null || RoleSelected == "")
                {
                    return "EE001";
                }

                if (UserObject.UserName.Length > 15)
                {
                    return "USERNAME_MAXLEN_15";
                }
                SMSystemEntity Entity = new SMSystemEntity();
                SMSystemEntity EntityBusines = new SMSystemEntity();
                int num = 0;
                if (IsUser == false)
                {
                    UserObject.CreatedBy = User.UserID.ToString();
                    UserObject.UserType = UserType.N.ToString();
                    UserObject.IsActive = true;
                    UserObject.CreatedOn = DateTime.Now;
                    UserObject.TokenCode = ClsCrypo.GetUniqueKey(15);
                    UserObject.Password = PasswordHash.HashPassword(UserObject.Password);
                    Entity.SYUsers.Add(UserObject);
                    num = Entity.SaveChanges();
                    var user = DB.SYUsers.FirstOrDefault(w => w.UserName == UserObject.UserName);
                    SYUserBusiness sYUserBusiness = new SYUserBusiness();
                    sYUserBusiness.UserId = user.UserID;
                    sYUserBusiness.ObjectCode = UserObject.CompanyOwner;
                    sYUserBusiness.ObjectType = UserType.N.ToString();
                    sYUserBusiness.BusinessType = UserType.N.ToString();
                    sYUserBusiness.CompanyCode = UserObject.CompanyOwner;
                    sYUserBusiness.DecimalPlace = 2;
                    sYUserBusiness.BaseCurrency = "USD";
                    sYUserBusiness.DecimalSepartor = ".";
                    EntityBusines.SYUserBusinesses.Add(sYUserBusiness);
                }
                var num1 = sMSystemEntity.SaveChanges();
                int num2 = EntityBusines.SaveChanges();
                try
                {
                    string[] array = RoleSelected.Split(';');
                    if (array.Length != 0)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(array[i]))
                            {
                                string value = array[i].Trim();
                                int roleId;

                                if (int.TryParse(value, out roleId))
                                {
                                    SYUserRole sYUserRole = new SYUserRole
                                    {
                                        UserID = UserObject.UserID,
                                        RoleId = roleId
                                    };

                                    Entity.SYUserRoles.Add(sYUserRole);
                                }
                            }
                        }
                    }

                    string[] array2 = RoleSelectedAPP.Split(';');
                    if (array2.Length != 0)
                    {
                        for (int j = 0; j < array2.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(array2[j]))
                            {
                                string value = array2[j].Trim();
                                int roleId;

                                if (int.TryParse(value, out roleId))
                                {
                                    SYUserRoleAPP sYUserRoleAPP = new SYUserRoleAPP
                                    {
                                        UserID = UserObject.UserID,
                                        RoleId = Convert.ToInt32(array2[j])
                                    };
                                    Entity.SYUserRoleAPPs.Add(sYUserRoleAPP);
                                }
                            }
                        }
                    }

                    if (LevelSelected != null && !(LevelSelected == ""))
                    {
                        string[] array3 = LevelSelected.Split(';');
                        if (array3.Length != 0)
                        {
                            string[] array4 = array3;
                            foreach (string text in array4)
                            {
                                if (text.Split(':').Length != 2)
                                {
                                    Entity.SYUserLevels.Add(new SYUserLevel
                                    {
                                        UserName = UserObject.UserName,
                                        LevelCode = text
                                    });
                                }
                            }
                        }
                    }

                    if (StorageSelected != null && !(StorageSelected == ""))
                    {
                        string[] array5 = StorageSelected.Split(';');
                        if (array5.Length != 0)
                        {
                            string[] array6 = array5;
                            foreach (string text2 in array6)
                            {
                                if (text2.Split(':').Length != 2)
                                {
                                    Entity.SYUserDataAcesses.Add(new SYUserDataAcess
                                    {
                                        UserId = UserObject.UserID,
                                        UserType = "S",
                                        CompanyCode = text2
                                    });
                                }
                            }
                        }
                    }

                    if (DataSelected != null && !(DataSelected == ""))
                    {
                        string[] array7 = DataSelected.Split(';');
                        if (array7.Length != 0)
                        {
                            string[] array8 = array7;
                            foreach (string text3 in array8)
                            {
                                string[] array9 = text3.Split(':');
                                if (array9.Length != 2)
                                {
                                    SYUserDataAcess sYUserDataAcess = new SYUserDataAcess();
                                    sYUserDataAcess.UserId = UserObject.UserID;
                                    sYUserDataAcess.UserType = UserObject.UserType;
                                    sYUserDataAcess.CompanyCode = text3;
                                    Entity.SYUserDataAcesses.Add(sYUserDataAcess);
                                }
                            }
                        }
                    }
                    else if (UserObject.UserType == UserType.N.ToString())
                    {
                        SYUserDataAcess sYUserDataAcess2 = new SYUserDataAcess();
                        sYUserDataAcess2.UserId = UserObject.UserID;
                        sYUserDataAcess2.UserType = UserObject.UserType;
                        sYUserDataAcess2.CompanyCode = "*";
                        Entity.SYUserDataAcesses.Add(sYUserDataAcess2);
                    }
                }
                catch
                {
                }

                num = Entity.SaveChanges();
                if (num > 0)
                {
                    return "OK";
                }

                return "EE001";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserID.ToString();
                sYEventLog.DocurmentAction = UserObject.UserName;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserID.ToString();
                sYEventLog2.DocurmentAction = UserObject.UserName;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }

        public string editUser(string tokend)
        {
            DB = new SMSystemEntity();
            try
            {
                List<SYUser> list = DB.SYUsers.Where((SYUser w) => w.TokenCode == tokend).ToList();
                if (list.Count == 0)
                {
                    return "USR_NE";
                }

                SYUser userObj = list.First();
                List<SYUser> list2 = DB.SYUsers.Where((SYUser w) => w.Email == UserObject.Email).ToList();
                if (list2.Count > 0)
                {
                }

                if (RoleSelected == null || RoleSelected == "")
                {
                    return "EE001";
                }

                if (UserObject.UserName.Length > 10)
                {
                    return "USERNAME_MAXLEN_10";
                }

                userObj.CompanyOwner = UserObject.CompanyOwner;
                userObj.ChangedBy = User.UserID.ToString();
                userObj.ChangedOn = DateTime.Now;
                userObj.LoginName = UserObject.LoginName;
                userObj.Email = UserObject.Email;
                userObj.ExpireDate = UserObject.ExpireDate;
                userObj.IsActive = true;
                userObj.IsApprover = UserObject.IsApprover;
                if (UserObject.Password != null)
                {
                    userObj.Password = PasswordHash.HashPassword(UserObject.Password);
                }

                SYUserBusiness sYUserBusiness = DB.SYUserBusinesses.FirstOrDefault((SYUserBusiness w) => w.UserId == userObj.UserID);
                if (sYUserBusiness == null)
                {
                    sYUserBusiness = new SYUserBusiness();
                    sYUserBusiness.UserId = userObj.UserID;
                    sYUserBusiness.ObjectCode = UserObject.CompanyOwner;
                    sYUserBusiness.ObjectType = UserType.N.ToString();
                    sYUserBusiness.BusinessType = UserType.N.ToString();
                    sYUserBusiness.CompanyCode = UserObject.CompanyOwner;
                    sYUserBusiness.DecimalPlace = 2;
                    sYUserBusiness.BaseCurrency = "USD";
                    sYUserBusiness.DecimalSepartor = ".";
                    DB.SYUserBusinesses.Add(sYUserBusiness);
                }

                DB.SYUsers.Attach(userObj);
                DB.Entry(userObj).Property((SYUser w) => w.ChangedBy).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.ChangedOn).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.LoginName).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.Email).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.ExpireDate).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.IsActive).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.IsApprover).IsModified = true;
                DB.Entry(userObj).Property((SYUser w) => w.CompanyOwner).IsModified = true;
                if (userObj.Password != null)
                {
                    DB.Entry(userObj).Property((SYUser w) => w.Password).IsModified = true;
                }

                try
                {
                    string[] array = RoleSelected.Split(';');
                    if (RoleSelected != null && !(RoleSelected == ""))
                    {
                        if (array.Length != 0)
                        {
                            List<SYUserRole> list3 = DB.SYUserRoles.Where((SYUserRole w) => w.UserID == userObj.UserID).ToList();
                            if (list3.Count > 0)
                            {
                                foreach (SYUserRole item in list3)
                                {
                                    DB.SYUserRoles.Remove(item);
                                }
                            }

                            for (int i = 0; i < array.Length; i++)
                            {
                                if (ClsInformation.IsNumeric(array[i]))
                                {
                                    SYUserRole sYUserRole = new SYUserRole();
                                    sYUserRole.UserID = userObj.UserID;
                                    sYUserRole.RoleId = Convert.ToInt32(array[i]);
                                    DB.SYUserRoles.Add(sYUserRole);
                                }
                            }
                        }
                    }
                    else
                    {
                        List<SYUserRole> list4 = DB.SYUserRoles.Where((SYUserRole w) => w.UserID == userObj.UserID).ToList();
                        if (list4.Count > 0)
                        {
                            foreach (SYUserRole item2 in list4)
                            {
                                DB.SYUserRoles.Remove(item2);
                            }
                        }
                    }

                    string[] array2 = RoleSelectedAPP.Split(';');
                    if (RoleSelectedAPP != null && !(RoleSelectedAPP == ""))
                    {
                        if (array2.Length != 0)
                        {
                            List<SYUserRoleAPP> list5 = DB.SYUserRoleAPPs.Where((SYUserRoleAPP w) => w.UserID == userObj.UserID).ToList();
                            if (list5.Count > 0)
                            {
                                foreach (SYUserRoleAPP item3 in list5)
                                {
                                    DB.SYUserRoleAPPs.Remove(item3);
                                }
                            }

                            for (int j = 0; j < array2.Length; j++)
                            {
                                if (ClsInformation.IsNumeric(array[j]) && !string.IsNullOrEmpty(array2[j]))
                                {
                                    SYUserRoleAPP sYUserRoleAPP = new SYUserRoleAPP();
                                    sYUserRoleAPP.UserID = userObj.UserID;
                                    sYUserRoleAPP.RoleId = Convert.ToInt32(array2[j]);
                                    DB.SYUserRoleAPPs.Add(sYUserRoleAPP);
                                }
                            }
                        }
                    }
                    else
                    {
                        List<SYUserRoleAPP> list6 = DB.SYUserRoleAPPs.Where((SYUserRoleAPP w) => w.UserID == userObj.UserID).ToList();
                        if (list6.Count > 0)
                        {
                            foreach (SYUserRoleAPP item4 in list6)
                            {
                                DB.SYUserRoleAPPs.Remove(item4);
                            }
                        }
                    }

                    if (DataSelected != null && !(DataSelected == ""))
                    {
                        string[] array3 = DataSelected.Split(';');
                        if (array3.Length != 0)
                        {
                            List<SYUserDataAcess> list7 = DB.SYUserDataAcesses.Where((SYUserDataAcess w) => w.UserId == userObj.UserID).ToList();
                            if (list7.Count > 0)
                            {
                                foreach (SYUserDataAcess item5 in list7)
                                {
                                    DB.SYUserDataAcesses.Remove(item5);
                                }
                            }

                            string[] array4 = array3;
                            foreach (string text in array4)
                            {
                                if (text != null && !(text == ""))
                                {
                                    SYUserDataAcess sYUserDataAcess = new SYUserDataAcess();
                                    sYUserDataAcess.UserId = userObj.UserID;
                                    sYUserDataAcess.UserType = userObj.UserType;
                                    sYUserDataAcess.CompanyCode = text;
                                    DB.SYUserDataAcesses.Add(sYUserDataAcess);
                                }
                            }
                        }
                    }
                    else
                    {
                        List<SYUserDataAcess> list8 = DB.SYUserDataAcesses.Where((SYUserDataAcess w) => w.UserId == userObj.UserID).ToList();
                        if (list8.Count > 0)
                        {
                            foreach (SYUserDataAcess item6 in list8)
                            {
                                DB.SYUserDataAcesses.Remove(item6);
                            }
                        }
                    }

                    if (StorageSelected != null && !(StorageSelected == ""))
                    {
                        string[] array5 = StorageSelected.Split(';');
                        if (array5.Length != 0)
                        {
                            List<SYUserDataAcess> list9 = DB.SYUserDataAcesses.Where((SYUserDataAcess w) => w.UserId == userObj.UserID && w.UserType == "S").ToList();
                            if (list9.Count > 0)
                            {
                                foreach (SYUserDataAcess item7 in list9)
                                {
                                    DB.SYUserDataAcesses.Remove(item7);
                                }
                            }

                            string[] array6 = array5;
                            foreach (string text2 in array6)
                            {
                                if (text2 != null && !(text2 == ""))
                                {
                                    DB.SYUserDataAcesses.Add(new SYUserDataAcess
                                    {
                                        UserId = userObj.UserID,
                                        UserType = "S",
                                        CompanyCode = text2
                                    });
                                }
                            }
                        }
                    }

                    if (LevelSelected != null && !(LevelSelected == ""))
                    {
                        string[] array7 = LevelSelected.Split(';');
                        if (array7.Length != 0)
                        {
                            List<SYUserLevel> list10 = DB.SYUserLevels.Where((SYUserLevel w) => w.UserName == userObj.UserName).ToList();
                            if (list10.Count > 0)
                            {
                                foreach (SYUserLevel item8 in list10)
                                {
                                    DB.SYUserLevels.Remove(item8);
                                }
                            }

                            string[] array8 = array7;
                            foreach (string text3 in array8)
                            {
                                if (text3 != null && !(text3 == ""))
                                {
                                    DB.SYUserLevels.Add(new SYUserLevel
                                    {
                                        UserName = userObj.UserName,
                                        LevelCode = text3
                                    });
                                }
                            }
                        }
                    }
                }
                catch
                {
                }

                int num = DB.SaveChanges();
                UserObject = userObj;
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserID.ToString();
                sYEventLog.DocurmentAction = UserObject.UserName;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserID.ToString();
                sYEventLog2.DocurmentAction = UserObject.UserName;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }

        public SYUser getUserByToken(string token)
        {
            List<SYUser> list = DB.SYUsers.Where((SYUser w) => w.TokenCode == token).ToList();
            if (list.Count > 0)
            {
                return list.First();
            }

            return null;
        }

        public static async Task<string> isAuthenticated(string userName, string Pwd)
        {
            SMSystemEntity DBA = new SMSystemEntity();
            List<SYUser> usrList = await DBA.SYUsers.Where((SYUser w) => w.UserName == userName).ToListAsync();
            if (usrList.Count > 0)
            {
                SYUser obj = usrList.First();
                if (obj.ExpireDate < DateTime.Now)
                {
                    return "USR_EXP";
                }

                if (!obj.IsActive)
                {
                    return "USR_INC";
                }

                if (PasswordHash.ValidatePassword(Pwd, obj.Password))
                {
                    return "OK";
                }
            }

            return "AUTH_FAIL";
        }

        public string deleteUser(string token)
        {
            try
            {
                SMSystemEntity sMSystemEntity = new SMSystemEntity();
                List<SYUser> list = DB.SYUsers.Where((SYUser w) => w.TokenCode == token).ToList();
                if (list.Count == 0)
                {
                    return "USR_NE";
                }

                UserObject = list.First();
                List<SYEventLog> list2 = DB.SYEventLogs.Where((SYEventLog w) => w.UserId == UserObject.UserName).Take(1).ToList();
                if (list2.Count > 0)
                {
                    return "USER_OPR";
                }

                List<SYUserRole> list3 = DB.SYUserRoles.Where((SYUserRole w) => w.UserID == UserObject.UserID).ToList();
                sMSystemEntity.SYUsers.Attach(UserObject);
                sMSystemEntity.Entry(UserObject).State = EntityState.Deleted;
                foreach (SYUserRole item in list3)
                {
                    sMSystemEntity.SYUserRoles.Attach(item);
                    sMSystemEntity.Entry(item).State = EntityState.Deleted;
                }

                List<SYUserRoleAPP> list4 = DB.SYUserRoleAPPs.Where((SYUserRoleAPP w) => w.UserID == UserObject.UserID).ToList();
                foreach (SYUserRoleAPP item2 in list4)
                {
                    sMSystemEntity.SYUserRoleAPPs.Attach(item2);
                    sMSystemEntity.Entry(item2).State = EntityState.Deleted;
                }

                List<SYUserLevel> list5 = DB.SYUserLevels.Where((SYUserLevel w) => w.UserName == UserObject.UserName).ToList();
                foreach (SYUserLevel item3 in list5)
                {
                    sMSystemEntity.SYUserLevels.Attach(item3);
                    sMSystemEntity.Entry(item3).State = EntityState.Deleted;
                }

                List<SYUserDataAcess> list6 = DB.SYUserDataAcesses.Where((SYUserDataAcess w) => w.UserId == UserObject.UserID).ToList();
                foreach (SYUserDataAcess item4 in list6)
                {
                    sMSystemEntity.SYUserDataAcesses.Attach(item4);
                    sMSystemEntity.Entry(item4).State = EntityState.Deleted;
                }

                int num = sMSystemEntity.SaveChanges();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserID.ToString();
                sYEventLog.DocurmentAction = UserObject.UserName;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserID.ToString();
                sYEventLog2.DocurmentAction = UserObject.UserName;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }

        public string createUserGroup()
        {
            try
            {
                List<SYUserGroup> list = DB.SYUserGroups.Where((SYUserGroup w) => w.Description == UserGroup.Description).ToList();
                if (list.Count > 0)
                {
                    return "USRG_EXS";
                }

                UserGroup.CreatedBy = User.UserID.ToString();
                UserGroup.CreatedOn = DateTime.Now;
                SMSystemEntity sMSystemEntity = new SMSystemEntity();
                sMSystemEntity.SYUserGroups.Add(UserGroup);
                int num = sMSystemEntity.SaveChanges();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserID.ToString();
                sYEventLog.DocurmentAction = UserGroup.Description;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserID.ToString();
                sYEventLog2.DocurmentAction = UserGroup.Description;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }
    }
}