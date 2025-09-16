//using System;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;
//using DevExpress.Xpo;
//using DevExpress.Web.Mvc;
//using DevExpress.XtraReports.Native;
//using DevExpress.XtraReports.UI;
//using DevExpress.DataAccess.Sql;
//using DMSPRO.Models.FT;
//using DMSPRO.Models.SY;
//using Humica.EF;
//using DMSPRO.Models.RP;
//using DMSPRO.Models.MD;
//using DMSPRO.REPORT.LOGISTIC;
//using DMSPRO.Models.BS;
//using DMSPRO.Models.RPT;
//using SaleModule.MD;
//using DBSystems.Models.RPT;
//using System.Collections.Generic;
//using DMSPRO.Models.LG;
//using SystemAdmin.SY;
//using DMSPRO.Models.NS;
//using DMSPRO.Models.SV;
//using DMSPRO.Models.ReportPhase2;
//using System.Reflection;
//using DMSPOS.Models.REPORT2.RPT;
//using DMSPRO.Models.RPT_REPORT;
//using System.Data.Entity.Validation;
//using System.Data.Entity.Infrastructure;

//namespace DMSPRO.Models.TRN
//{

//    public class ClsTRTrainingProgramItems
//    {
//        private NServiceEntities DBX = new NServiceEntities();
//        public SYUser User { get; set; }
//        public string ScreenID { get; set; }
//        public const string PARAM_BRANCH = "BRANCH";
//        public SYUserBusiness BS { get; set; }
//        public TRTrainingProgram Header { get; set; }
//        public List<TRTrainingProgram> ListTraining { get; set; }

//        public ClsTRTrainingProgramItems()
//        {
//            User = SYSession.getSessionUser();
//            BS = SYSession.getSessionUserBS();
//        }

//        public string CreateTraining()
//        {
//            try
//            {
//                DBX = new NServiceEntities();
//                CheckValidDate((DateTime)Header.StartDate, Header.EndDate);

//                Header.Plant = "SV";
//                Header.CreatedBy = User.UserName;
//                Header.CreatedOn = DateTime.Now;

//                DBX.TRTrainingProgram.Add(Header);

//                DBX.SaveChanges();

//                return "OK";
//            }
//            catch (DbEntityValidationException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
//                SYEventLogObject.saveEventLog(log, e);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (DbUpdateException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (Exception e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//        }

//        public string EditTraining()
//        {
//            try
//            {
//                DBX = new NServiceEntities();

//                //Check valid period date
//                var msg = CheckValidDate((DateTime)Header.StartDate, Header.EndDate);
//                if (msg != SYConstant.OK)
//                {
//                    return msg;
//                }

//               var program = DBX.TRTrainingProgram.Find("SV", Header.Branch, Header.ProgramCode);

//                      program.ChangedBy = User.UserName;
//                      program.ChangedOn = DateTime.Now;
//                      program.CreatedBy = User.UserName;
//                      program.CreatedOn = DateTime.Now;
//                      program.StartDate = Header.StartDate;
//                      program.EndDate = Header.EndDate;
//                      program.TrainingRequirement = Header.TrainingRequirement;
//                      program.TrainingGroup = Header.TrainingGroup;
//                      program.ProgramName = Header.ProgramName;
//                      program.Venue = Header.Venue;
//                      program.Capacity = Header.Capacity;
//                      program.IsActive = Header.IsActive;
//                      program.Target = Header.Target;

//              DBX.TRTrainingProgram.Attach(program);
//                DBX.Entry(program).Property(x => x.ChangedBy).IsModified = true;
//                DBX.Entry(program).Property(x => x.ChangedOn).IsModified = true;
//                DBX.Entry(program).Property(x => x.CreatedBy).IsModified = true;
//                DBX.Entry(program).Property(x => x.CreatedOn).IsModified = true;
//                DBX.Entry(program).Property(x => x.StartDate).IsModified = true;
//                DBX.Entry(program).Property(x => x.EndDate).IsModified = true;
//                DBX.Entry(program).Property(x => x.TrainingRequirement).IsModified = true;
//                DBX.Entry(program).Property(x => x.TrainingGroup).IsModified = true;
//                DBX.Entry(program).Property(x => x.ProgramName).IsModified = true;
//                DBX.Entry(program).Property(x => x.Venue).IsModified = true;
//                DBX.Entry(program).Property(x => x.Capacity).IsModified = true;
//                DBX.Entry(program).Property(x => x.IsActive).IsModified = true;
//                DBX.Entry(program).Property(x => x.Target).IsModified = true;

//                     DBX.SaveChanges();

//                     return SYConstant.OK;
//            }
//            catch (DbEntityValidationException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
//                SYEventLogObject.saveEventLog(log, e);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (DbUpdateException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (Exception e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.Branch;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//        }

//        public string DeleteTrainig()
//        {
//            try
//            {
//                DBX = new NServiceEntities();
//                var program = DBX.TRTrainingProgram.FirstOrDefault(w=>w.ProgramCode== Header.ProgramCode && w.Branch==Header.Branch);

//                DBX.TRTrainingProgram.Remove(program);

//                DBX.SaveChanges();

//                return SYConstant.OK;
//            }
//            catch (DbEntityValidationException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.ProgramCode;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
//                SYEventLogObject.saveEventLog(log, e);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (DbUpdateException e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.ProgramCode;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//            catch (Exception e)
//            {
//                /*------------------SaveLog----------------------------------*/
//                SYEventLog log = new SYEventLog();
//                log.ScreenId = ScreenID;
//                log.UserId = User.UserName;
//                log.DocurmentAction = Header.ProgramCode;
//                //log.DocurmentAction = ActionName;
//                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

//                SYEventLogObject.saveEventLog(log, e, true);
//                /*----------------------------------------------------------*/
//                return "EE001";
//            }
//        }


//        #region "Validate"
//        public string CheckValidDate(DateTime startDate, DateTime? endDate)
//        {
//            if (endDate == null || startDate >= endDate)
//            {
//                return "EE001";

//            }

//            return SYConstant.OK;
//        }

//        #endregion "Validate"

//    }
//}