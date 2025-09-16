using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.Atts
{
    public class DevSettingObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATDevSetting Header { get; set; }
        public List<ATDevSetting> ListHeader { get; set; }
        public ATInOut InOut { get; set; }
        public FTFilterInOut Filter { get; set; }
        public List<ATInOut> ListInOut { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string IPAddress { get; set; }
        public int Progress { get; set; }
        public static List<ListProgress> ListProgresses { get; set; }
        public DevSettingObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateDevSetting()
        {
            DB = new HumicaDBContext();
            try
            {
                if (Header.DeviceID == null) return "DEVICEID_EN";
                if (Header.IPAddress == null) return "IPADDR_EN";

                var chkdp = DB.ATDevSettings.Where(w => w.DeviceID == Header.DeviceID).ToList();
                if (chkdp.Count > 0) return "TR_CODE_EXIST";

                Header.CreateOn = DateTime.Now.Date;
                Header.CreateBy = User.UserName;

                DB.ATDevSettings.Add(Header);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.DeviceID;
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
                log.DocurmentAction = Header.DeviceID;
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
                log.DocurmentAction = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditDev(string id)
        {
            try
            {
                var objMatch = DB.ATDevSettings.Find(id);
                if (id == null)
                {
                    return "DOC_NE";
                }

                objMatch.Description = Header.Description;
                objMatch.IPAddress = Header.IPAddress;
                objMatch.Port = Header.Port;
                objMatch.CPassword = Header.CPassword;
                objMatch.ChangedOn = DateTime.Now.Date;
                objMatch.ChangedBy = User.UserName;
                DB.ATDevSettings.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.IPAddress).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Port).IsModified = true;
                DB.Entry(objMatch).Property(w => w.CPassword).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteDev(string id)
        {
            try
            {

                ATDevSetting objCust = DB.ATDevSettings.Find(id);
                if (objCust == null)
                {
                    return "Att_NE";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                DBM.ATDevSettings.Attach(objCust);
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
                log.ScreenId = Header.DeviceID;
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
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public async Task<string> DownloadData(string IPAddress, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                List<AttendanceRaw> list = new List<AttendanceRaw>();
                List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                ListInOut = new List<ATInOut>();
                ZkfpManager _fingerPrint = new ZkfpManager();
                var objMast = DB.ATDevSettings.ToList();
                // var ATInOuts = DB.ATInOuts.Where(x=>x.from);
                // ListInOut = ATInOuts.ToList();
                FromDate = FromDate.AddDays(-1);
                ToDate = ToDate.AddDays(1);
                var _listINOut = await Task.Run(() =>
                {
                    return (from s in DB.ATInOuts
                            where EntityFunctions.TruncateTime(s.FullDate) >= FromDate.Date
                            && EntityFunctions.TruncateTime(s.FullDate) <= ToDate.Date
                            && s.EmpCode.Trim() != ""
                            select s).ToList();
                });
                ListInOut = _listINOut.ToList();
                List<ATDevSetting> ListATDevSetting = objMast.ToList();

                var Employee = DB.HRStaffProfiles.Where(w => !string.IsNullOrEmpty(w.CardNo));
                _listStaff = Employee.ToList();
                string[] IP = IPAddress.Split(';');
                MessageError = "";
                Progress = IP.Count();
                int i = 0;
                decimal _p = 0;
                ListProgresses.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Progress = Progress);
                foreach (var Code in IP)
                {
                    if (Code.Trim() != "")
                    {
                        var IPAdd = ListATDevSetting.FirstOrDefault(w => w.DeviceID == Code);

                        _fingerPrint.IP_Address = IPAdd.IPAddress;
                        _fingerPrint.Port = IPAdd.Port;
                        if (IPAdd.CPassword != null)
                        {
                            _fingerPrint.CommPort = Convert.ToInt32(IPAdd.CPassword);
                        }
                        _fingerPrint.Connect();

                        if (_fingerPrint.ConnectionState)
                        {
                            var rawDatas = _fingerPrint.GetRawData(FromDate, ToDate).ToArray();
                            list.AddRange(rawDatas);
                        }
                        else
                        {
                            if (MessageError.Length > 0) MessageError += " ;" + _fingerPrint.IP_Address;
                            else
                                MessageError = _fingerPrint.IP_Address;
                            //list.Clear();
                            //return "CANNOT_CON_EN";
                        }
                        _fingerPrint.Disconnect();
                    }
                    i += 1;
                    _p = i;
                    ListProgresses.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Percen = _p / x.Progress * 100);
                }
                //  var DBI = new HumicaDBContext();
                list = list.Where(w =>
                {
                    bool wParsed = long.TryParse(w.EnrollNumber, out long wEnrollNumber);
                    if (!wParsed) return true;

                    return !ListInOut.Any(at =>
                        long.TryParse(at.CardNo, out long cardNo) &&
                        cardNo == wEnrollNumber &&
                        at.FullDate == w.ScanDate
                    );
                }).ToList();
                if (list.Count > 0)
                {
                    //Save Data
                    var ListAtInout = new List<ATInOut>();
                    var u_Name = User.UserName;
                    foreach (var item in list)
                    {
                        InOut = new ATInOut();

                        var EmpCode = _listStaff.FirstOrDefault(w =>
                        long.TryParse(w.CardNo, out long cardNo) && long.TryParse(item.EnrollNumber, out long enrollNumber) &&
                        cardNo == enrollNumber); 
                        if (EmpCode == null) continue;
                        InOut.EmpCode = EmpCode.EmpCode;
                        InOut.STATUS = 3;
                        InOut.CardNo = EmpCode.CardNo;
                        InOut.FullDate = item.ScanDate;
                        InOut.CreateBy = u_Name;
                        InOut.CreateOn = DateTime.Now;
                        DB.ATInOuts.Add(InOut);
                    }
                }
                int row = await DB.SaveChangesAsync();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.DocurmentAction = Header.DeviceID;
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
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string Connect(string ID)
        {
            try
            {
                Header = new ATDevSetting();
                Header.DeviceID = ID;
                var objMast = DB.ATDevSettings.ToList();
                //ZkemClient _fingerPrint = new ZkemClient();

                zkemkeeper.CZKEM _fingerPrint = new zkemkeeper.CZKEM();
                //// ZkfpManager();

                objMast = objMast.Where(x => x.DeviceID == ID).ToList();
                foreach (var fin in objMast)
                {
                    //    IsDeviceConnected = _fingerPrint.Connect_Net(fin.IPAddress, fin.Port);
                    if (fin.CPassword != null)
                    {
                        _fingerPrint.SetCommPassword(Convert.ToInt32(fin.CPassword));
                    }
                    if (_fingerPrint.Connect_Net(fin.IPAddress, fin.Port))
                    {
                        _fingerPrint.Disconnect();
                    }
                    else
                    {
                        return "CANNOT_CON_EN";
                    }
                }
                return SYConstant.OK;
            }

            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.DocurmentAction = Header.DeviceID;
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
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.DeviceID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #region "Upload In/Out" 
        public string uploadInOut()
        {
            try
            {
                if (ListInOut.Count == 0)
                {
                    return "NO_DATA";
                }
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    var _list = new List<ATInOut>();
                    List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                    var ATInOuts = DB.ATInOuts;
                    _list = ATInOuts.ToList();
                    var Employee = DB.HRStaffProfiles;
                    _listStaff = Employee.ToList();

                    var date = DateTime.Now;
                    ListInOut = ListInOut.Where(w => !_list.Where(at => at.EmpCode == w.EmpCode && at.FullDate == w.FullDate).Any()).ToList();
                    foreach (var staffs in ListInOut.ToList())
                    {
                        InOut = new ATInOut();
                        var EmpCode = _listStaff.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                        if (EmpCode.Count() > 0)
                        {
                            InOut.EmpCode = EmpCode.FirstOrDefault().EmpCode;
                            InOut.STATUS = 3;
                            InOut.CardNo = EmpCode.FirstOrDefault().CardNo;
                            InOut.FullDate = staffs.FullDate;
                            InOut.CreateBy = User.UserName;
                            InOut.CreateOn = DateTime.Now;
                            DB.ATInOuts.Add(InOut);
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
                log.DocurmentAction = InOut.CardNo;
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
                log.DocurmentAction = InOut.CardNo;
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
                log.DocurmentAction = InOut.CardNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
    }
    public class ListProgress
    {
        public string UserName { get; set; }
        public int Progress { get; set; }
        public decimal Percen { get; set; }
    }
}