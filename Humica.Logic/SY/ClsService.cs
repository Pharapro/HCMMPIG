using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Humica.Logic.SY
{
    public class ClsService
    {
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public SyBackgroundService BackgroundService { get; set; }
        public ClsService()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<string> UpdateSetting()
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = await DB.SyBackgroundServices.FirstOrDefaultAsync();
                if (ObjMatch == null)
                {
                    DB.SyBackgroundServices.Add(BackgroundService);
                    int row = DB.SaveChanges();
                }
                else
                {
                    BackgroundService.ID = ObjMatch.ID;
                    HumicaDBContext DBM = new HumicaDBContext();
                    DBM.Entry(BackgroundService).State = System.Data.Entity.EntityState.Modified;
                    int row = DBM.SaveChanges();

                    //ClsAuditLog.AuditLog(ScreenId, ObjMatch.ID.ToString(), "Header", ObjMatch, BackgroundService);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = BackgroundService.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.DocurmentAction = BackgroundService.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
    public class ClsDownload
    {
        public string Type { get; set; }
        public static List<ClsDownload> LoadData()
        {
            List < ClsDownload > _list= new List<ClsDownload>();
            _list.Add(new ClsDownload() { Type = "API" });
            _list.Add(new ClsDownload() { Type = "IP" });
            _list.Add(new ClsDownload() { Type = "Other" });

            return _list;
        }
    }
    public class ClsSchedule
    {
        public string Type { get; set; }
        public static List<ClsSchedule> LoadData()
        {
            List<ClsSchedule> _list = new List<ClsSchedule>();
            _list.Add(new ClsSchedule() { Type = "Monday" });
            _list.Add(new ClsSchedule() { Type = "Tuesday" });
            _list.Add(new ClsSchedule() { Type = "Wednesday" });
            _list.Add(new ClsSchedule() { Type = "Thursday" });
            _list.Add(new ClsSchedule() { Type = "Friday" });
            _list.Add(new ClsSchedule() { Type = "Saturday" });
            _list.Add(new ClsSchedule() { Type = "Sunday" });

            return _list;
        }
    }
}
