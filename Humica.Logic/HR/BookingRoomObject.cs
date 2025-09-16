using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class BookingRoomObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity SMS = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string SaleOrderNo { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public string MessageError { get; set; }
        public FTDGeneralPeriod Filter { get; set; }
        public HRBookingRoom Header { get; set; }
        public List<HRBookingRoom> ListHeader { get; set; }
        public List<HRBookingSchedule> ListBookingSchedule { get; set; }
        public CFDocType DocTypeObject { get; set; }
        public ClsEmail EmailObject { get; set; }
        public decimal VATRate { get; set; }
        public string PLANT { get; set; }
        public string Token { get; set; }
        public string PenaltyNo { get; set; }
        public bool IsSave { get; set; }
        public BookingRoomObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string IsValidBookingTime(HRBookingSchedule objCheck, List<HRBookingSchedule> ListCurrent)
        {
            string RoomID = objCheck.RoomID;
            var LstCheckRoomID = DB.HRRoomTypes.Where(w => w.RoomCode == objCheck.RoomID).ToList();
            var CheckRoomID = LstCheckRoomID.First();

            var checkListCurrent = ListCurrent.Where(w => w.RoomID == RoomID
                      && w.BookingDate == objCheck.BookingDate).ToList();
            if (ListCurrent.Where(w => ((w.StartTime > objCheck.StartTime && w.StartTime < objCheck.EndTime) ||
                         (w.EndTime > objCheck.StartTime && w.EndTime < objCheck.EndTime) ||
                         (objCheck.StartTime > w.StartTime && objCheck.StartTime < w.EndTime) ||
                         (objCheck.EndTime > w.StartTime && objCheck.EndTime < w.EndTime))).Any())
            {
                return "DUP_WITH_OTHER";
            }
            var listCheck = DB.HRBookingSchedules.Where(w => w.RoomID == RoomID
            && w.BookingDate == objCheck.BookingDate).ToList();
            var listCheck1 = listCheck.Where(w => ((w.StartTime > objCheck.StartTime && w.StartTime < objCheck.EndTime) ||
                         (w.EndTime > objCheck.StartTime && w.EndTime < objCheck.EndTime) ||
                         (objCheck.StartTime > w.StartTime && objCheck.StartTime < w.EndTime) ||
                         (objCheck.EndTime > w.StartTime && objCheck.EndTime < w.EndTime))).ToList();
            if (listCheck1.Count > 0)
            {
                return "DUP_WITH_OTHER";
            }
            if (listCheck.Where(w => w.StartTime == objCheck.StartTime && w.EndTime == objCheck.EndTime).Any())
            {
                return "DUP_WITH_OTHER";
            }
            return SYConstant.OK;
        }
        public string createBooking()
        {
            try
            {
                var DBI = new HumicaDBContext();
                if (ListBookingSchedule.Count == 0)
                {
                    return "LIST_NE";
                }
                if (Header.TotalHour <= 0)
                {
                    return "INVALID_HOUR";
                }
                Header.Status = SYDocumentStatus.BOOKING.ToString();
                Header.DocumentDate = DateTime.Now.Date;
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreateedBy = User.UserName;
                if (Header.Reason == "")
                    return "REASON_EN";
                var objCF = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                DocType = objCF.DocType;
                var objNumber = new CFNumberRank(objCF.DocType, ScreenId);

                Header.BookingNo = objNumber.NextNumberRank;

                DBI.HRBookingRooms.Add(Header);
                //Booking Schedule
                int lineItem = 0;
                ListBookingSchedule = ListBookingSchedule.OrderBy(w => w.BookingDate).ToList();
                decimal TotalHour = 0;
                foreach (var read in ListBookingSchedule)
                {
                    //var objCheckB = DB.HRRoomTypes.Find(read.RoomID);
                    //if (objCheckB == null)
                    //{
                    //    return "ROOM_NE";
                    //}
                    lineItem = lineItem + 1;
                    DateTime StartTime = read.BookingDate + read.StartTime.TimeOfDay;
                    DateTime EndTime = read.BookingDate + read.EndTime.TimeOfDay;
                    var objBuild = new HRBookingSchedule();
                    objBuild.LineItem = lineItem;
                    objBuild.RoomID = read.RoomID;
                    objBuild.StartTime = StartTime;
                    objBuild.EndTime = EndTime;
                    objBuild.Status = Header.Status;
                    objBuild.BookingNo = Header.BookingNo;
                    objBuild.BookingDate = read.BookingDate.Date;
                    var interval = read.EndTime.Subtract(read.StartTime);
                    var Hour = interval.TotalHours;
                    TotalHour += Convert.ToDecimal(Math.Round(Hour, 2));

                    DBI.HRBookingSchedules.Add(objBuild);
                }


                int row = DBI.SaveChanges();
                
                var EmpBooking = DBV.HR_BooingRoom_View.Where(w => w.BookingNo == Header.BookingNo).ToList();

                if (objCF != null && EmpBooking.Any())
                {
                    #region ---Send To Telegram---
                    if (!string.IsNullOrEmpty(objCF.Telegram))
                    {
                        string str = "Dear team, I would like to book the meeting room as below:";
                        foreach (var read in EmpBooking)
                        {
                            str += @"%0A- <b>" + read.RoomType + "</b> on <b>" + read.BookingDate.ToString("dd.MM.yyyy")
                                + "</b> from <b>" + read.StartTime.ToString("hh:mm tt")
                                + "</b> to <b>" + read.EndTime.ToString("hh:mm tt") + "</b>";
                        }
                        str += "%0A*<b>" + EmpBooking.First().Reason + "</b> Thanks you.";
                        str += "%0A%0AYours sincerely,%0A%0A<b>" + EmpBooking.First().EmpName + " </b>";
                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        Tel.User = User;
                        Tel.BS = BS;
                        List<object> ListObjectDictionary = new List<object>();
                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, objCF.Telegram, false);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                    #endregion

                    #region ---Email---
                    var emailConfig = SMS.CFEmailAccounts.FirstOrDefault();
                    if (!string.IsNullOrEmpty(objCF.Email) && emailConfig != null)
                    {
                        CFEmailAccount emailAccount = emailConfig;
                        string subject = "Booking Room";
                        string str_ = "Dear team,<br/>I would like to book the meeting room as below:<br/>";

                        foreach (var booking in EmpBooking)
                        {
                            str_ += "<br/>- <b>" + booking.RoomType + "</b> on <b>" + booking.BookingDate.ToString("dd.MM.yyyy") +
                                   "</b> from <b>" + booking.StartTime.ToString("hh:mm tt") +
                                   "</b> to <b>" + booking.EndTime.ToString("hh:mm tt") + "</b>";
                        }

                        str_ += "<br/><b>Reason:</b> <b>" + EmpBooking.First().Reason + "</b><br/><br/>Thank you.<br/><br/>Yours sincerely,<br/><br/><b>" + EmpBooking.First().EmpName + "</b>";
                        string body = str_;
                        EmailObject = new ClsEmail();
                        string[] filePaths = new string[] { "" };
                        int result = EmailObject.SendMails(emailAccount, "", objCF.Email, subject, body, filePaths);
                    }
                    #endregion
                }


                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e);
                var objNumber = new CFNumberRank(DocType, ScreenId, true);
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
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                var objNumber = new CFNumberRank(DocType, ScreenId, true);
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
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                var objNumber = new CFNumberRank(DocType, ScreenId, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }

}