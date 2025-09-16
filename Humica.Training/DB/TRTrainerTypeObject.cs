using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training
{
    public class TRTrainerTypeObject
    {
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public TRTrainerType Header { get; set; }
        public List<TRTrainerType> ListHeader { get; set; }
        public List<TRTrainerInfo> ListTrainerInfor { get; set; }
        public Humica.Core.DB.HR_STAFF_VIEW HeaderStaff { get; set; }
        public string InputType { get; set; }
        public string ApprovType { get; set; }
        public TRTrainerTypeObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateTrainerType()
        {
            DBX = new HumicaDBContextTR();
            try
            {
                if (string.IsNullOrEmpty(Header.Code))
                {
                    return "ALERT";
                }
                else
                {
                    List<TRTrainerType> items = DBX.TRTrainerType.Where(x => x.Code == Header.Code).ToList();
                    if (items.Count > 0)
                        return "ALERT_TR";
                }
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                string trainerTypeCode = string.Empty;
                trainerTypeCode = Header.Code;
                DBX.TRTrainerType.Add(Header);
                int row = DBX.SaveChanges();
                // insert trainer info
                var trainerType = DBX.TRTrainerType.OrderByDescending(x => x.TrainNo).FirstOrDefault();
                int TrainerTypeID = 0;
                if (trainerType != null)
                    TrainerTypeID = trainerType.TrainNo;
                if (TrainerTypeID > 0)
                {
                    foreach (var read in ListTrainerInfor.ToList())
                    {
                        read.TrainerTypeID = TrainerTypeID.ToString();
                        read.CreatedOn = DateTime.Now;
                        read.CreatedBy = User.UserName;
                        DBX.TRTrainerInfo.Add(read);
                    }
                    DBX.SaveChanges();
                }
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
        public string EditTrainerInfo(int TrainNo)
        {
            try
            {
                var objMatch = DBX.TRTrainerType.FirstOrDefault(x => x.TrainNo == TrainNo);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                string TrainerTypeID = objMatch.TrainNo.ToString();
                var objMatchItem = DBX.TRTrainerInfo.Where(x => x.TrainerTypeID == TrainerTypeID).ToList();
                //if (objMatchItem.ToList().Count == 0)
                //{
                //    return "DOC_NE";
                //}
                foreach (var item in objMatchItem)
                {
                    DBX.TRTrainerInfo.Remove(item);
                    DBX.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var read in ListTrainerInfor.ToList())
                {
                    read.TrainerTypeID = objMatch.TrainNo.ToString();
                    read.ChangedOn = DateTime.Now;
                    read.ChangedBy = User.UserName;
                    DBX.TRTrainerInfo.Add(read);
                }
                int row1 = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.EmpID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteTrainerInfo(int TrainNo)
        {
            try
            {
                var objMatch = DBX.TRTrainerType.FirstOrDefault(x => x.TrainNo == TrainNo);

                if (objMatch == null)
                {
                    return "EE001";
                }
                string TrainerTypeID = objMatch.TrainNo.ToString();
                var objMatchItem = DBX.TRTrainerInfo.Where(x => x.TrainerTypeID == TrainerTypeID).ToList();
                DBX.TRTrainerType.Remove(objMatch);
                foreach (var read in objMatchItem)
                {
                    DBX.TRTrainerInfo.Remove(read);
                }
                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                //log.ScreenId = Header.EmpID.ToString();
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
                //log.ScreenId = Header.EmpID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }
    public class TrainerType
    {
        public const string EXT = "EXTERNAL";
        public const string INT = "INTERNAL";
    }
}