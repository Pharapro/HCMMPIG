using Humica.Core;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.UI.WebControls;

namespace Humica.Logic.HR
{
    public class ClsBookingRoom : IClsBookingRoom
    {
        protected IUnitOfWork unitOfWork;

        public SMSystemEntity SMS = new SMSystemEntity();
        public ClsEmail EmailObject { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public string MessageError { get; set; }
        public HRBookingRoom Header { get; set; }
        public List<HRBookingRoom> ListHeader { get; set; }
        public List<ClsListHeader> ListHeaderItem { get; set; }
        public List<HRBookingSchedule> ListBookingSchedule { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        public ClsBookingRoom()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoadingIndex()
        {
            ListHeaderItem = new List<ClsListHeader>();
            DateTime DocDate = DateTime.Now;
            var obj = unitOfWork.Repository<HRBookingRoom>().Queryable().Where(w => w.DocumentDate.Year == DocDate.Year).ToList().OrderByDescending(x => x.DocumentDate).ToList();
            foreach(var read in obj)
            {
                string roomname = "";
                var Item = unitOfWork.Repository<HRBookingSchedule>().Queryable().FirstOrDefault(w => w.BookingNo == read.BookingNo);
                if (Item == null) continue;
                var roomtype = unitOfWork.Repository<HRRoomType>().Queryable().FirstOrDefault(w => w.RoomCode == Item.RoomID);
                if (roomtype != null) 
                {
                    roomname = !string.IsNullOrEmpty(roomtype.SecDescription)
                         ? $"{roomtype.Description} ({roomtype.SecDescription})"
                         : roomtype.Description;
                }
                var List = new ClsListHeader
                {
                    BookingNo = read.BookingNo,
                    EmpCode = read.EmpCode,
                    EmpName = read.EmpName,
                    RoomName = roomname,
                    DocumentDate = Item.BookingDate,
                    StartTime = Item.StartTime,
                    TotalHour = read.TotalHour,
                    EndTime = Item.EndTime,
                    Reason = read.Reason,
                    Status = read.Status
                };
                ListHeaderItem.Add(List);
            }
        }
        public void OnCreated()
        {
            ListBookingSchedule = new List<HRBookingSchedule>();
            Header = new HRBookingRoom();
            Header.DocumentDate = DateTime.Now;
        }
        public void OnDetail(string id)
        {
            ListBookingSchedule = new List<HRBookingSchedule>();
            Header = new HRBookingRoom();
            Header = unitOfWork.Repository<HRBookingRoom>().Queryable().FirstOrDefault(w => w.BookingNo == id);
            ListBookingSchedule = unitOfWork.Repository<HRBookingSchedule>().Queryable().Where(w => w.BookingNo == id).ToList();
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
                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                DocType = objCF.DocType;
                var objNumber = new CFNumberRank(objCF.DocType, ScreenId);

                Header.BookingNo = objNumber.NextNumberRank;

                unitOfWork.Add(Header);
                //Booking Schedule
                int lineItem = 0;
                ListBookingSchedule = ListBookingSchedule.OrderBy(w => w.BookingDate).ToList();
                decimal TotalHour = 0;
                foreach (var read in ListBookingSchedule)
                {
                    lineItem = lineItem + 1;
                    DateTime StartTime = read.BookingDate + read.StartTime.TimeOfDay;
                    DateTime EndTime = read.BookingDate + read.EndTime.TimeOfDay;
                    HRBookingSchedule objBuild = new HRBookingSchedule();
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

                    unitOfWork.Add(objBuild);
                }
               unitOfWork.Save();

                var EmpBooking = unitOfWork.Repository<HR_BooingRoom_View>().Queryable().Where(w => w.BookingNo == Header.BookingNo).ToList();

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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string EditBooking(string id)
        {
            try
            {
                var DBI = new HumicaDBContext();
                var objmatch =unitOfWork.Repository<HRBookingRoom>().Queryable().FirstOrDefault(w => w.BookingNo == id);
                if (objmatch != null)
                {
                    if(Header != null) objmatch.Reason = Header.Reason;
                    objmatch.TotalHour = Header.TotalHour;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    unitOfWork.Update(objmatch);
                }
                //Booking Schedule

                decimal TotalHour = 0;
                foreach (var read in ListBookingSchedule)
                {
                    DateTime StartTime = read.BookingDate + read.StartTime.TimeOfDay;
                    DateTime EndTime = read.BookingDate + read.EndTime.TimeOfDay;
                    var objBuild = new HRBookingSchedule();
                    objBuild.BookingNo = read.BookingNo;
                    objBuild.LineItem = read.LineItem;
                    objBuild.RoomID = read.RoomID;
                    objBuild.StartTime = StartTime;
                    objBuild.EndTime = EndTime;
                    objBuild.BookingDate = read.BookingDate.Date;
                    objBuild.Status = Header.Status;
                    //var interval = read.EndTime.Subtract(read.StartTime);
                    //var Hour = interval.TotalHours;
                    //TotalHour += Convert.ToDecimal(Math.Round(Hour, 2));

                   unitOfWork.Update(objBuild);
                }
                unitOfWork.Save();

                var EmpBooking = unitOfWork.Repository<HR_BooingRoom_View>().Queryable().Where(w => w.BookingNo == objmatch.BookingNo).ToList();
                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);

                if (objCF != null && EmpBooking.Any())
                {
                    #region ---Send To Telegram---
                    if (!string.IsNullOrEmpty(objCF.Telegram))
                    {
                        string str = "Dear team, I would like to edit book the meeting room as below:";
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
                    if (User.UserName != objmatch.CreateedBy)
                    {
                        var Createby = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objmatch.CreateedBy);
                        if (Createby != null && !string.IsNullOrEmpty(Createby.Email) && emailConfig != null)
                        {
                            CFEmailAccount emailAccount = emailConfig;
                            string subject = "Confirmation of Meeting Room Schedule Change";
                            string str_ = $"Dear {Createby.AllName},<br/>Thank you for your request. The booking room:<br/>";
                                   str_ += $"Room has been updated as follows:<br/>";

                            foreach (var booking in EmpBooking)
                            {
                                str_ += "<br/>- <b>" + booking.RoomType + "</b> on <b>" + booking.BookingDate.ToString("dd.MM.yyyy") +
                                       "</b> from <b>" + booking.StartTime.ToString("hh:mm tt") +
                                       "</b> to <b>" + booking.EndTime.ToString("hh:mm tt") + "</b>";
                            }
                            string Emp = User.UserName;
                            var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                            if (staff != null) Emp = staff.AllName;
                            str_ += "<br/><b>Reason:</b> <b>" + EmpBooking.First().Reason + "</b><br/><br/>Thank you.<br/><br/>Yours sincerely,<br/><br/><b>" + Emp + "</b>";
                            string body = str_;
                            EmailObject = new ClsEmail();
                            string[] filePaths = new string[] { "" };
                            int result = EmailObject.SendMails(emailAccount, "", Createby.Email, subject, body, filePaths);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objCF.Email) && emailConfig != null)
                        {
                            CFEmailAccount emailAccount = emailConfig;
                            string subject = "Confirmation of Meeting Room Schedule Change";
                            string str_ = "Dear Admin, <br/>I would like to request an update to the meeting room booking as per the details below:<br/>";

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
                    }
                    #endregion
                }


                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RejctedBooking(string id,string Comment)
        {
            try
            {
                var DBI = new HumicaDBContext();
                var objmatch = unitOfWork.Repository<HRBookingRoom>().Queryable().FirstOrDefault(w => w.BookingNo == id);
                if (objmatch != null)
                {
                    objmatch.Comment = Comment;
                    objmatch.Status = SYDocumentStatus.REJECTED.ToString();
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    unitOfWork.Update(objmatch);
                }
                var Listobjmatch = unitOfWork.Repository<HRBookingSchedule>().Queryable().Where(w => w.BookingNo == objmatch.BookingNo).ToList();
                foreach (var item in Listobjmatch)
                {
                    if (item.BookingDate.Date < DateTime.Now.Date)
                    {
                        return "Booking Date smaller than Date now";
                    }
                    item.Status = SYDocumentStatus.REJECTED.ToString();
                    unitOfWork.Update(item);
                }
                var EmpBooking = unitOfWork.Repository<HR_BooingRoom_View>().Queryable().Where(w => w.BookingNo == id).ToList();
                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF != null && EmpBooking.Any())
                {
                    #region ---Send To Telegram---
                    if (!string.IsNullOrEmpty(objCF.Telegram))
                    {
                        string str = "Dear team, I would like to reject book the meeting room as below:";
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
                    if (User.UserName != objmatch.CreateedBy)
                    {
                        var Createby = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objmatch.CreateedBy);
                        if (Createby != null && !string.IsNullOrEmpty(Createby.Email) && emailConfig != null)
                        {
                            CFEmailAccount emailAccount = emailConfig;
                            string subject = "Booking Room";
                            string str_ = "Dear team,<br/>I would like to reject book the meeting room as below:<br/>";

                            foreach (var booking in EmpBooking)
                            {
                                str_ += "<br/>- <b>" + booking.RoomType + "</b> on <b>" + booking.BookingDate.ToString("dd.MM.yyyy") +
                                       "</b> from <b>" + booking.StartTime.ToString("hh:mm tt") +
                                       "</b> to <b>" + booking.EndTime.ToString("hh:mm tt") + "</b>";
                            }
                            string Emp = User.UserName;
                            var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                            if (staff != null) Emp = staff.AllName;
                            str_ += "<br/><b>Reason:</b> <b>" + EmpBooking.First().Reason + "</b><br/><br/>Thank you.<br/><br/>Yours sincerely,<br/><br/><b>" + Emp + "</b>";
                            string body = str_;
                            EmailObject = new ClsEmail();
                            string[] filePaths = new string[] { "" };
                            int result = EmailObject.SendMails(emailAccount, "", Createby.Email, subject, body, filePaths);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objCF.Email) && emailConfig != null)
                        {
                            CFEmailAccount emailAccount = emailConfig;
                            string subject = "Booking Room";
                            string str_ = "Dear team,<br/>I would like to cancel book the meeting room as below:<br/>";

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
                    }
                    #endregion
                }
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string IsValidBookingTime(HRBookingSchedule objCheck, List<HRBookingSchedule> ListCurrent)
        {
            string RoomID = objCheck.RoomID;
            var LstCheckRoomID = unitOfWork.Repository<HRRoomType>().Queryable().Where(w => w.RoomCode == objCheck.RoomID).ToList();
            var CheckRoomID = LstCheckRoomID.First();

            var checkListCurrent = ListCurrent.Where(w => w.RoomID == RoomID
                      && w.BookingDate == objCheck.BookingDate).ToList();
            if (ListCurrent.Where(w => (w.StartTime > objCheck.StartTime && w.StartTime < objCheck.EndTime) ||
                         (w.EndTime > objCheck.StartTime && w.EndTime < objCheck.EndTime) ||
                         (objCheck.StartTime > w.StartTime && objCheck.StartTime < w.EndTime) ||
                         (objCheck.EndTime > w.StartTime && objCheck.EndTime < w.EndTime)).Any())
            {
                return "DUP_WITH_OTHER";
            }
            var listCheck = unitOfWork.Repository<HRBookingSchedule>().Queryable().Where(w => w.RoomID == RoomID
            && w.BookingDate == objCheck.BookingDate && w.Status != SYDocumentStatus.REJECTED.ToString()).ToList();
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
        public string IsValidBookingTimes(HRBookingSchedule objCheck, List<HRBookingSchedule> ListCurrent)
        {
            string RoomID = objCheck.RoomID;
            var LstCheckRoomID = unitOfWork.Repository<HRRoomType>().Queryable().Where(w => w.RoomCode == objCheck.RoomID).ToList();
            var CheckRoomID = LstCheckRoomID.First();
            var listCheck =unitOfWork.Repository<HRBookingSchedule>().Queryable().Where(w => w.RoomID == RoomID
            && w.BookingDate == objCheck.BookingDate && w.Status != SYDocumentStatus.REJECTED.ToString() && w.BookingNo != objCheck.BookingNo).ToList();
            if (listCheck.Where(w => w.BookingDate.Date == objCheck.BookingDate.Date && (objCheck.StartTime >= w.StartTime && objCheck.StartTime < w.EndTime ||
                objCheck.EndTime >= w.StartTime && objCheck.EndTime < w.EndTime)).Any())
            {
                return "DUP_WITH_OTHER";
            }
            return SYConstant.OK;
        }
        public Dictionary<string, dynamic> OnDataSelector(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            keyValues.Add("STATUS_APPROVAL", new SYDataList(unitOfWork, "STATUS_LEAVE_APPROVAL").ListData);
            keyValues.Add("ROOMTypes_SELECT", unitOfWork.Repository<HRRoomType>().Queryable().ToList());
            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff(true));
            return keyValues;
        }
        public class ClsListHeader
        {
            public string BookingNo { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public string RoomName { get; set; }
            public DateTime DocumentDate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public decimal TotalHour { get; set; }
            public string Reason { get; set; }
            public string Status { get; set; }
        }
    }
}
