using Hangfire;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.TelegramSharp.Telegram;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Humica.Logic.EOB
{

    public class EOBAnnouncementObject
    {

        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public List<HRStaffProfile> ListAnnounce { get; set; }
        public CFEmailAccount EmailAccount { get; set; }
        public ClsEmail EmailObject { get; set; }
        public TPEmailTemplate EmailTemplate { get; set; }
        public string Action { get; set; }
        public SYSplitItem SelectListItem { get; set; }
        public List<object> ListObjectDictionary { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public string MessageError { get; set; }
        public string TeleGroup { get; set; }
        public string EmpCode { get; set; }
        public EOBAnnouncementObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        #region 'Create'
        public string Announce(string EmpCode, string TeleGroup, string FileName)
        {
            try
            {
                string[] Emp = EmpCode.Split(',');
                string[] _FileName = FileName.Split(',');
                for (int i = 0; i < Emp.Length; i++)
                {
                    var _EmpCode = Emp[i];
                    var ObjMatch = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == _EmpCode);
                    if (ObjMatch != null)
                    {
                        ObjMatch.IsAnnouncement = true;
                        DB.HRStaffProfiles.Attach(ObjMatch);
                        DB.Entry(ObjMatch).Property(x => x.IsAnnouncement).IsModified = true;
                        DB.SaveChanges();
                    }

                    #region ---Send To Telegram---

                    EOB_EMP_ANNOUCEMENT AnnounceEmp = new EOB_EMP_ANNOUCEMENT();
                    AnnounceEmp = DBV.EOB_EMP_ANNOUCEMENT.FirstOrDefault(w => w.EmpCode == _EmpCode);

                    var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == TeleGroup);
                    var EmailTemplate = SMS.TPEmailTemplates.Find("HRANNOUNCEMENT");
                    var staff = DB.HRStaffProfiles.Where(w => w.EmpCode == _EmpCode).ToList();
                    string Chart_ID = staff.FirstOrDefault().TeleChartID?.ToString();
                    if (Telegram == null)
                        return "telegram";
                    if (EmailTemplate == null) return "emailtemplate";
                    if (Telegram != null && EmailTemplate != null)
                    {
                        Chart_ID = Telegram.ChatID;
                        List<object> ListObjectDictionary = new List<object>();
                        ListObjectDictionary.Add(AnnounceEmp);
                        string _File;

                        if (_FileName.Length == 1) { _File = _FileName[i]; }
                        else
                        {
                            int _i = 0; _i += i; _File = _FileName[_i];
                        }
                        Announcement(EmailTemplate.RequestContent, _EmpCode, Chart_ID, _File, Telegram.TokenID, ListObjectDictionary);
                    }

                }

                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public int Announcement(string TemBody, string id, string ChartID, string _FileName, string TokenID, List<object> ListObjectDictionary)
        {
            BackgroundJob.Enqueue(() =>
            Send_Announce_Telegram(TemBody, id, ChartID, _FileName, TokenID, ListObjectDictionary));

            return 0;
        }
        public void Send_Announce_Telegram(string TemBody, string id, string ChartID, string _FileName, string TokenID, List<object> ListObjectDictionary)
        {
            string EmpCode = "";
            var _Name = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == id);
            if (_Name != null)
            {
                try
                {
                    TeleSharp Bot = new TeleSharp(TokenID);
                    MessageSender sender1 = new MessageSender();
                    sender1.Id = ChartID;
                    string text = getEmailContentByParam(TemBody, ListObjectDictionary);
                    Bot.SendPhoto(sender1, System.IO.File.ReadAllBytes(_FileName), Path.GetFileName(_FileName), text);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserID.ToString();
                    log.ScreenId = EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }

        }

        public string getEmailContentByParam(string text, List<object> ListObjectDictionary)
        {
            string[] textsp = text.Split(' ');
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = SMS.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == "HRANNOUNCEMENT");
                        if (objParam != null)
                        {

                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);
                                }
                            }
                        }
                    }

                }

            }
            return text;
        }
        #endregion 
    }
}