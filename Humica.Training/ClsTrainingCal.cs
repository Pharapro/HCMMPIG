using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Humica.EF.MD;
using System.Data.Entity.Validation;
using System.Linq;
using Humica.Logic.CF;
using Humica.EF.Models.SY;
using Humica.EF;
using Humica.Training.DB;

namespace Humica.Training
{
    public class ClsTrainingCal
    {
        HumicaDBContext DB = new HumicaDBContext();
        Humica.Core.DB.HumicaDBContext DBX = new Core.DB.HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public TRTrainingCal Header { get; set; }
        public string ScreenId { get; set; }
        // public TRTrainingCalOverview TROverview { get; set; }

        public List<TRTrainingCal> ListTraingCal { get; set; }
        public List<TRTrainingCalEvent> ListCalenderEvent { get; set; }
        public ClsTrainingCal()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateCalendar()
        {
            try
            {
                DB = new HumicaDBContext();
                var objCF = DBX.ExCfWorkFlowItems.Find(ScreenId, Header.TrainingType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.DocType, ScreenId);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.TrainingCalNo = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                int lineItem = 1;
                foreach(var item in ListCalenderEvent)
                {
                    item.LineItem = lineItem;
                    item.TrainingCalNo = Header.TrainingCalNo;
                    DB.TRTrainingCalEvent.Add(item);
                    lineItem++;
                }

                DB.TRTrainingCal.Add(Header);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TrainingCalNo;
                //log.DocurmentAction = ActionName;
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
                log.DocurmentAction = Header.TrainingCalNo;
                //log.DocurmentAction = ActionName;
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
                log.DocurmentAction = Header.TrainingCalNo;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string EditCalendar(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                Header.TrainingCalNo = id;
                var Cal = DB.TRTrainingCal.FirstOrDefault(w => w.TrainingCalNo == id);
                Cal.ToDate = Header.ToDate;
                Cal.FromDate = Header.FromDate;
                Cal.MinTrainee = Header.MinTrainee;
                Cal.MaxTrainee = Header.MaxTrainee;
                Cal.Venue = Header.Venue;
                Cal.Comment = Header.Comment;
                Cal.DateCloseR = Header.DateCloseR;
                Cal.TargetAudience = Header.TargetAudience;
                Cal.Duration = Header.Duration;
                Cal.TrainingCost = Header.TrainingCost;
                Cal.Facilitator = Header.Facilitator;
                Cal.TrainingType = Header.TrainingType;
                Cal.Status = Header.Status;
                Cal.ChangedBy = User.UserName;
                Cal.ChangedOn = DateTime.Now;
                var ListEvent = DB.TRTrainingCalEvent.Where(w => w.TrainingCalNo == Cal.TrainingCalNo).ToList();
                foreach (var item1 in ListEvent)
                {
                    DB.TRTrainingCalEvent.Remove(item1);
                    DB.SaveChanges();
                }
                int lineItem = 1;
                foreach (var item in ListCalenderEvent)
                {
                    item.LineItem = lineItem;
                    item.TrainingCalNo = Cal.TrainingCalNo;
                    DB.TRTrainingCalEvent.Add(item);
                    lineItem++;
                }
                DB.TRTrainingCal.Attach(Cal);


                DB.Entry(Cal).Property(x => x.FromDate).IsModified = true;
                DB.Entry(Cal).Property(x => x.ToDate).IsModified = true;
                DB.Entry(Cal).Property(x => x.DateCloseR).IsModified = true;
                DB.Entry(Cal).Property(x => x.Comment).IsModified = true;
                DB.Entry(Cal).Property(x => x.Duration).IsModified = true;
                DB.Entry(Cal).Property(x => x.Facilitator).IsModified = true;
                DB.Entry(Cal).Property(x => x.MaxTrainee).IsModified = true;
                DB.Entry(Cal).Property(x => x.MinTrainee).IsModified = true;
                DB.Entry(Cal).Property(x => x.TargetAudience).IsModified = true;
                DB.Entry(Cal).Property(x => x.TrainingCost).IsModified = true;
                DB.Entry(Cal).Property(x => x.Status).IsModified = true;
                DB.Entry(Cal).Property(x => x.TrainingType).IsModified = true;
                DB.Entry(Cal).Property(x => x.Venue).IsModified = true;
                DB.Entry(Cal).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(Cal).Property(x => x.ChangedOn).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TrainingCalNo;
                //log.DocurmentAction = ActionName;
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
                log.DocurmentAction = Header.TrainingCalNo;
                //log.DocurmentAction = ActionName;
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

                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteCalendar(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                var program = DB.TRTrainingCal.FirstOrDefault(w => w.TrainingCalNo == id);
                var ListEvent = DB.TRTrainingCalEvent.Where(w => w.TrainingCalNo == id).ToList();
                foreach (var item1 in ListEvent)
                {
                    DB.TRTrainingCalEvent.Remove(item1);
                    DB.SaveChanges();
                }
                DB.TRTrainingCal.Remove(program);

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
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();

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

        public string requestToApprove(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.TRTrainingCal.FirstOrDefault(w => w.TrainingCalNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                DB.TRTrainingCal.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.TrainingCalNo;
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
                log.DocurmentAction = Header.TrainingCalNo;
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
                log.DocurmentAction = Header.TrainingCalNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(string id)
        {
            try
            {
                Humica.Core.DB.HumicaDBContext DBX = new Humica.Core.DB.HumicaDBContext();
                var objMatch = DB.TRTrainingCal.FirstOrDefault(w => w.TrainingCalNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objMatch.TrainingType
                                    && w.DocumentNo == objMatch.TrainingCalNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
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
                        var objStaff = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
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
                //var open = SYDocumentStatus.OPEN.ToString();
                // objMatch.IsApproved = true;
                if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                {
                    status = SYDocumentStatus.PENDING.ToString();
                    // objMatch.IsApproved = false;
                }

                objMatch.Status = status;
                DB.TRTrainingCal.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.TrainingCalNo;
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
                log.DocurmentAction = Header.TrainingCalNo;
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
                log.DocurmentAction = Header.TrainingCalNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelDoc(string ApprovalID)
        {
            try
            {
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.TRTrainingCal.Find(ApprovalID);
                if (objmatch == null)
                {
                    return "REQUEST_NE";
                }
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }

}
