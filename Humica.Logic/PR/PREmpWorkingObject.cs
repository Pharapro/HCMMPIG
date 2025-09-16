using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PREmpWorkingObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; private set; }
        public PREmpWorking Header { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public List<PREmpWorking> ListHeader { get; set; }

        public List<PREmpWorking> listImport { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string MessageError { get; set; }

        public PREmpWorkingObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateEmp()
        {
            try
            {
                Header.EmpCode = Header.EmpCode;
                Header.InMonth = Header.FromDate.Value.Month;
                Header.InYear = Header.FromDate.Value.Year;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.PREmpWorkings.Add(Header);
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
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteEmp(int id)
        {
            try
            {
                Header = new PREmpWorking();
                var objMatch = DB.PREmpWorkings.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "FAMILY_NE";
                }
                // Header = id;
                DB.PREmpWorkings.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                // log.ScreenId = tr.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

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
                //log.ScreenId = EmpCode.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";

            }
            //finally
            //{
            //    DB.Configuration.AutoDetectChangesEnabled = true;
            //}
        }
        public string EditEmp(int id)
        {
            try
            {

                var objMatch = DB.PREmpWorkings.SingleOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "FAMILY_NE";
                }
                Header.EmpCode = objMatch.EmpCode;
                objMatch.InMonth = Header.FromDate.Value.Month;
                objMatch.InYear = Header.FromDate.Value.Year;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Hours = Header.Hours;
                objMatch.Remark = Header.Remark;


                DB.PREmpWorkings.Attach(objMatch);

                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string uploadEmpW()
        {
            try
            {
                if (listImport.Count == 0)
                {
                    return "NO_DATA";
                }
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    var _list = new List<PREmpWorking>();
                    List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                    var Working = DB.PREmpWorkings;
                    _list = Working.ToList();
                    var Employee = DB.HRStaffProfiles;
                    _listStaff = Employee.ToList();

                    var date = DateTime.Now;
                    foreach (var staffs in listImport.ToList())
                    {
                        Header = new PREmpWorking();
                        var EmpCode = _listStaff.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                        Header.EmpCode = "";
                        if (EmpCode.Count <= 1)
                        {
                            if (EmpCode.Count == 1)
                            {
                                Header.EmpCode = EmpCode.FirstOrDefault().EmpCode;
                            }
                            Header.PayPeriodId = staffs.PayPeriodId;
                            Header.InMonth = staffs.InMonth;
                            Header.InYear = staffs.InYear;
                            Header.FromDate = staffs.FromDate;
                            Header.ToDate = staffs.ToDate;
                            Header.ActWorkDay = staffs.ActWorkDay;
                            Header.Hours = staffs.Hours;
                            Header.Remark = staffs.Remark;
                            Header.CreatedBy = User.UserName;
                            Header.CreatedOn = DateTime.Now;
                            DB.PREmpWorkings.Add(Header);
                        }
                        else
                        {
                            foreach (var item1 in EmpCode)
                            {
                                if (item1 != null)
                                {
                                    Header.EmpCode = item1.EmpCode;
                                }
                                Header.PayPeriodId = staffs.PayPeriodId;
                                Header.InMonth = staffs.InMonth;
                                Header.InYear = staffs.InYear;
                                Header.FromDate = staffs.FromDate;
                                Header.ToDate = staffs.ToDate;
                                Header.ActWorkDay = staffs.ActWorkDay;
                                Header.Hours = staffs.Hours;
                                Header.Remark = staffs.Remark;
                                Header.CreatedBy = User.UserName;
                                Header.CreatedOn = DateTime.Now;
                                DB.PREmpWorkings.Add(Header);
                            }
                        }
                    }

                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();
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
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}


