using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.TelegramSharp.Telegram;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.CF
{
    public class CFAnnouncementObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRAnnouncement Header { get; set; }
        public List<HRAnnouncement> ListHeader { get; set; }
        public string TeleGroup { get; set; }
        public CFAnnouncementObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string Annount(HRAnnouncement announcement, string TeleGroup, List<string> file, string exten)
        {
            var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == TeleGroup);
            string boldText = announcement.Description;
            string messger = announcement.Message;
            if (Telegram != null)
            {
                Announcement(messger, boldText, TeleGroup, file, Telegram.TokenID);
            }
            return SYConstant.OK;
        }
        public string CreateAnnouncement()
        {
            try
            {
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                DB.HRAnnouncements.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditAnnouncement(int id)
        {
            try
            {
                var objMast = DB.HRAnnouncements.Find(id);
                objMast.Description = Header.Description;
                objMast.ValidDate = Header.ValidDate;
                objMast.Message = Header.Message;
                objMast.AttachFile = Header.AttachFile;
                objMast.Document = Header.Document;
                objMast.ChangedOn = DateTime.Now.Date;
                objMast.ChangedBy = User.UserName;
                objMast.AnnounceType = Header.AnnounceType;

                DB.Entry(objMast).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteAnnouncement(int id)
        {
            try
            {

                var objCust = DB.HRAnnouncements.Find(id);
                if (objCust == null)
                {
                    return "ANNOUN_NE";
                }


                HumicaDBContext DBM = new HumicaDBContext();
                DBM.HRAnnouncements.Attach(objCust);
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
                log.ScreenId = Header.ID.ToString();
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
                log.ScreenId = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public int Announcement(string messger, string TemBody, string ChartID, List<string> _FileName, string TokenID)
        {
            TeleSharp Bot = new TeleSharp(TokenID);
            MessageSender sender1 = new MessageSender();
            sender1.Id = ChartID;
            string text = "<b>" + TemBody + "</b>\n" + messger;

            var s = Bot.SendDocuments(sender1, _FileName, text);
            return 0;
        }

        public List<HRAnnouncement> Announcement()
        {

            var _listAnnouncement = DB.HRAnnouncements.ToList();
            var _Holiday = DB.HRPubHolidays.ToList();
            var _ListAnnouncement = new List<HRAnnouncement>();

            var Dates = DateTime.Now;

            var _checkHoliday = _Holiday.Where(w => w.PDate >= Dates.Date && w.PDate <= Dates.AddDays(7)).ToList();
            var _checkAnnouncement = _listAnnouncement.Where(w => w.ValidDate >= Dates.Date && w.ValidDate <= Dates.AddDays(7)).ToList();

            if (_checkHoliday.Count > 0)
            {
                foreach (var read in _checkHoliday)
                {
                    var obj = new HRAnnouncement();
                    obj.Description = read.Description;
                    obj.ValidDate = read.PDate;
                    obj.Message = read.SecDescription;
                    _ListAnnouncement.Add(obj);
                }
            }
            if (_checkAnnouncement.Count > 0)
            {
                foreach (var read in _checkAnnouncement)
                {
                    var obj = new HRAnnouncement();
                    obj.Description = read.Description;
                    obj.ValidDate = read.ValidDate;
                    obj.AttachFile = read.AttachFile;
                    obj.Message = read.Message;
                    _ListAnnouncement.Add(obj);
                }
            }

            return _ListAnnouncement.OrderBy(w => w.ValidDate).ToList();
        }
    }
}