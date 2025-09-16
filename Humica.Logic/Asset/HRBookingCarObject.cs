using Humica.Core.DB;
using Humica.Core.FT;
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
    public class HRBookingCarObject
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
        public HRBookingCar Header { get; set; }
        public List<HRBookingCar> ListHeader { get; set; }
        public List<HRBookingCarSchedule> ListBookingSchedule { get; set; }
        public CFDocType DocTypeObject { get; set; }
        public decimal VATRate { get; set; }
        public string PLANT { get; set; }
        public string Token { get; set; }
        public string PenaltyNo { get; set; }
        public bool IsSave { get; set; }
        public HRBookingCarObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
		public string IsValidBookingTime(HRBookingCarSchedule objCheck, List<HRBookingCarSchedule> ListCurrent)
		{
			string ID = objCheck.CarID;

			var car = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == ID);
			if (car == null)
				return "CAR_NOT_FOUND";

			var dbList = DB.HRBookingCarSchedules
				.Where(w => w.CarID == ID && w.BookingDate == objCheck.BookingDate
					&& !(w.BookingNo == objCheck.BookingNo && w.LineItem == objCheck.LineItem)) 
				.ToList();

			var combinedList = ListCurrent
				.Where(w => w.CarID == ID && w.BookingDate == objCheck.BookingDate && w.LineItem != objCheck.LineItem)
				.Concat(dbList)
				.ToList();

			var conflict = combinedList.FirstOrDefault(w =>
				w.StartTime < objCheck.EndTime && w.EndTime > objCheck.StartTime);

			if (conflict != null)
			{
				string carInfo = $"{car.Description}";
				return $"DUP_WITH_OTHER:{carInfo}";
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
                    return "REASON";

                DBI.HRBookingCars.Add(Header);
                int lineItem = 0;
                ListBookingSchedule = ListBookingSchedule.OrderBy(w => w.BookingDate).ToList();
                decimal TotalHour = 0;
                foreach (var read in ListBookingSchedule)
                {
                    lineItem = lineItem + 1;
                    DateTime StartTime = read.BookingDate + read.StartTime.TimeOfDay;
                    DateTime EndTime = read.BookingDate + read.EndTime.TimeOfDay;
                    var objBuild = new HRBookingCarSchedule();
                    objBuild.LineItem = lineItem;
                    objBuild.CarID = read.CarID;
                    objBuild.StartTime = StartTime;
                    objBuild.EndTime = EndTime;
					string checkResult = IsValidBookingTime(read, ListBookingSchedule);
					if (checkResult != SYConstant.OK)
					{
						if (checkResult.StartsWith("DUP_WITH_OTHER:"))
						{
							MessageError = checkResult.Split(':')[1];
							return "DUP_CAR";
						}
						return checkResult;
					}
					objBuild.Status = Header.Status;
                    objBuild.BookingNo = Header.BookingNo;
                    objBuild.BookingDate = read.BookingDate.Date;
					objBuild.HaveDriver = read.HaveDriver;
                    if (read.HaveDriver == true) 
						objBuild.DriverCode = read.DriverCode.Split(':')[0];
					var interval = read.EndTime.Subtract(read.StartTime);
                    var Hour = interval.TotalHours;
                    TotalHour += Convert.ToDecimal(Math.Round(Hour, 2));

                    DBI.HRBookingCarSchedules.Add(objBuild);
                }


                int row = DBI.SaveChanges();
                //#region ---Send To Telegram---
                //// var EmailTemplate = SMS.TPEmailTemplates.Find("BOOKINGROOM");
                //var EmpBooking = DBV.HR_BooingRoom_View.Where(w => w.BookingNo == Header.BookingNo).ToList();
                //// if (EmailTemplate != null)
                //// {
                //if (objCF != null)
                //{
                //    string str = "Dear team, I would like to book the meeting room as below:";
                //    foreach (var read in EmpBooking)
                //    {
                //        str += @"%0A- <b>" + read.RoomType + "</b> on <b>" + read.BookingDate.ToString("dd.MM.yyyy")
                //            + "</b> from <b>" + read.StartTime.ToString("hh:mm tt")
                //            + "</b> to <b>" + read.EndTime.ToString("hh:mm tt") + "</b>";
                //    }
                //    str += "%0A*<b>" + EmpBooking.First().Reason + "</b> Thanks you.";
                //    str += "%0A%0AYours sincerely,%0A%0A<b>" + EmpBooking.First().EmpName + " </b>";
                //    SYSendTelegramObject Tel = new SYSendTelegramObject();
                //    Tel.User = User;
                //    Tel.BS = BS;
                //    List<object> ListObjectDictionary = new List<object>();
                //    // ListObjectDictionary.Add(EmpBooking);
                //    // WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplate.EMTemplateObject, EmailTemplate.RequestContent, objCF.Telegram, ListObjectDictionary,"")
                //    WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, objCF.Telegram, false);
                //    MessageError = Tel.getErrorMessage(result1);
                //}
                ////}
                //#endregion

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
        public string Edit(string id)
        {
            try
            {

                var objMatch = DB.HRBookingCars.FirstOrDefault(w => w.BookingNo.ToString() == id);
                var objMatchItem = DB.HRBookingCarSchedules.FirstOrDefault(w => w.BookingNo.ToString() == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                objMatch.DocumentDate = Header.DocumentDate;
                objMatch.EmpCode = Header.EmpCode;
                objMatch.EmpName = Header.EmpName;
                objMatch.Reason = Header.Reason;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                foreach (var item in ListBookingSchedule)
                {
                    objMatchItem.BookingDate = item.BookingDate;
                    objMatchItem.StartTime = item.StartTime;
                    objMatchItem.EndTime = item.EndTime;
					string checkResult = IsValidBookingTime(item, ListBookingSchedule);
					if (checkResult != SYConstant.OK)
					{
						if (checkResult.StartsWith("DUP_WITH_OTHER:"))
						{
							MessageError = checkResult.Split(':')[1];
							return "DUP_CAR";
						}
						return checkResult;
					}
					DB.HRBookingCarSchedules.Attach(objMatchItem);
                    DB.Entry(objMatchItem).State = System.Data.Entity.EntityState.Modified;
                }
                DB.HRBookingCars.Attach(objMatch);
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
                log.DocurmentAction = Header.BookingNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Delete(string id)
        {
            try
            {
                Header = new HRBookingCar();
                var objMatch = DB.HRBookingCars.FirstOrDefault(w => w.BookingNo.ToString() == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                DB.HRBookingCars.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                var ListBookingSchedule = DB.HRBookingCarSchedules.Where(w => w.BookingNo.ToString() == id).ToList();
                foreach (var item in ListBookingSchedule)
                {
                    DB.HRBookingCarSchedules.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
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
                log.ScreenId = Header.BookingNo.ToString();
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
                log.ScreenId = Header.BookingNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }

    }

}