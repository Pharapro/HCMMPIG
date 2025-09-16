using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HRResgierVehicleObject
    {
        public SMSystemEntity DP = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRResgierVehicle Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HRResgierVehicle> ListHeader { get; set; }
		public List<HR_AssetRegisterVehicle_View> ListHeaderView { get; set; }
		public SYUser UserObject { get; set; }
        public SYUserBusiness UserBusinessObject { get; set; }
        public HRResgierVehicleObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string createVehicle()
        {
            DB = new HumicaDBContext();
            try
            {
				if (string.IsNullOrEmpty(Header.AssetClass) || string.IsNullOrEmpty(Header.AssetCode) || string.IsNullOrEmpty(Header.VehicleType)
                            || string.IsNullOrEmpty(Header.MissionCode))
					return "DOC_INV";
				Header.Status = SYDocumentStatus.OPEN.ToString();
				Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;
                DB.HRResgierVehicles.Add(Header);
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
        public string EditVehicle(string id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                objMatch.Model = Header.Model;
                objMatch.Status = SYDocumentStatus.OPEN.ToString();
                objMatch.Description = Header.Description;
                objMatch.Year = Header.Year;
                objMatch.VehicleType = Header.VehicleType;
				objMatch.Horsepower = Header.Horsepower;
				objMatch.Rate = Header.Rate;
				objMatch.PlateNumber = Header.PlateNumber;
                objMatch.Color = Header.Color;
				objMatch.Chair = Header.Chair;
				objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.HRResgierVehicles.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
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
        }
        public string DeleteVehicle(string id)
        {
            Header = new HRResgierVehicle();
            try
            {
                var objMatch = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == id);
                if (objMatch == null)
                {
                    return "DOC_RM";
                }
                DB.HRResgierVehicles.Attach(objMatch);
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
    }
}