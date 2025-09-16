using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.MD
{
    public class SYUserAceessObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        private SMSystemEntity DBA = new SMSystemEntity();
        public SYUser User { get; set; }
        public string ScreenId { get; set; }
        public SYUserBusiness BS { get; set; }
        public SYHRModifySalary HeaderSalary { get; set; }
        public List<SYAccessDepartment> ListHeader { get; set; }
        public List<ClsModifySaalry> ListModiSalary { get; set; }
        public string MessageCode { get; set; }
        public List<HRLevel> ListUserLevel { get; set; }
        public List<SYHRModifySalary> ListLevelAssigned { get; set; }
        public string LevelSelected = "";
        public SYUserAceessObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ClsModifySaalry> LoadDataModify()
        {
            List<ClsModifySaalry> List = new List<ClsModifySaalry>();
            List<SYUser> ListUser = DBA.SYUsers.ToList();
            List<SYHRModifySalary> _listSla = DB.SYHRModifySalarys.ToList();
            ListUser = ListUser.Where(w => _listSla.Where(x => x.UserName == w.UserName).Any()).ToList();
            foreach (var item in ListUser)
            {
                int CountAccess = _listSla.Where(x => x.UserName == item.UserName).Count();
                List.Add(new ClsModifySaalry { UserName = item.UserName, LoginName = item.LoginName, CountAccess = CountAccess });

            }
            return List;
        }
        public string createUser()
        {
            DB = new HumicaDBContext();
            try
            {
                //exist
                List<SYHRModifySalary> objExist = DB.SYHRModifySalarys.Where(w => w.UserName == HeaderSalary.UserName).ToList();
                if (objExist.Count > 0)
                {
                    return "CURR_EXS";
                }

                //UserObject.CreatedBy = User.UserID.ToString();
                //UserObject.UserType = UserType.N.ToString();
                //UserObject.IsActive = true;
                //UserObject.CreatedOn = DateTime.Now;
                //UserObject.TokenCode = ClsCrypo.GetUniqueKey(15);

                try
                {

                    if (LevelSelected != null && !(LevelSelected == ""))
                    {
                        string[] strArray2 = LevelSelected.Split(';');
                        if ((uint)strArray2.Length > 0U)
                        {
                            foreach (string str in strArray2)
                            {
                                if (str.Split(':').Length != 2)
                                    DB.SYHRModifySalarys.Add(new SYHRModifySalary()
                                    {
                                        UserName = HeaderSalary.UserName,
                                        Level = str
                                    });
                            }
                        }
                    }

                }
                catch
                {
                    //return "NO_PER_SET";
                }
                var row = DB.SaveChanges();
                if (row > 0)
                {
                    return SYConstant.OK;
                }
                else
                {
                    return "EE001";
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = HeaderSalary.UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = HeaderSalary.UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string editUser(string _UserName)
        {
            DB = new HumicaDBContext();
            try
            {
                //exist
                List<SYHRModifySalary> objExist = DB.SYHRModifySalarys.Where(w => w.UserName == _UserName).ToList();

                if (objExist.Count == 0)
                {
                    return "USR_NE";
                }
                try
                {
                    if (this.LevelSelected != null && !(this.LevelSelected == ""))
                    {
                        string[] strArray2 = this.LevelSelected.Split(';');
                        if ((uint)strArray2.Length > 0U)
                        {
                            List<SYHRModifySalary> list3 = DB.SYHRModifySalarys.Where(w => w.UserName == _UserName).ToList();
                            if (list3.Count > 0)
                            {
                                foreach (var entity in list3)
                                    DB.SYHRModifySalarys.Remove(entity);
                            }
                            foreach (string str in strArray2)
                            {
                                if (str != null && !(str == ""))
                                    DB.SYHRModifySalarys.Add(new SYHRModifySalary()
                                    {
                                        UserName = _UserName,
                                        Level = str
                                    });
                            }
                        }
                    }
                }
                catch
                {
                    //return "NO_PER_SET";
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
                log.DocurmentAction = _UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = _UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteUser(string _UserName)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                //exist
                List<SYHRModifySalary> listLevel = DB.SYHRModifySalarys.Where(w => w.UserName == _UserName).ToList();
                foreach (var read in listLevel)
                {
                    DBM.SYHRModifySalarys.Attach(read);
                    DBM.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                
                int row = DBM.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = _UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = _UserName;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
    public class ClsModifySaalry
    {
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public int CountAccess { get; set; }
    }
}