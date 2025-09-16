using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.TelegramSharp.Telegram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web.Routing;

namespace Humica.Core.SY
{
    public class SYSendTelegramObject
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public WorkFlowType WFType { get; set; }
        public SLevel LevelAction { get; set; }
        public string ScreenId { get; set; }
        public object ObjectDictionary { get; set; }
        public List<object> ListObjectDictionary { get; set; }
        public string DocumentNo { get; set; }
        SMSystemEntity DBB = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public string DocType { get; set; }
        public CFWorkFlow CFHeader { get; set; }
        public CFWorkFlowItem CFItem { get; set; }
        public string DocNo { get; set; }
        public TPEmailTemplate EmailTemplate { get; set; }
        public SYSendTelegramObject()
        {
        }
        public SYSendTelegramObject(string WFObject, WorkFlowType WFType, string actionProcess)
        {
            CFHeader = DBB.CFWorkFlows.Find(WFObject);
            if (CFHeader != null)
            {
                CFItem = DBB.CFWorkFlowItems.Find(WFObject, WFType.ToString(), actionProcess);
                this.WFType = WFType;
                if (CFItem != null)
                {
                    EmailTemplate = DBB.TPEmailTemplates.Find(CFItem.EMTemplate);
                }
            }
        }
        public void Send_SMS_TelegramLeaveApproval(string ChatID, string URL,string Tembody)
        {
            try
            {
                string err = null;
                if (CFHeader == null)
                {
                    err = "NO_WF";
                }
                if (CFItem == null)
                {
                    err = "NO_WF_ITEM";
                }
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                if (Telegram != null)
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;
                    string chatId = Telegram.ChatID;
                    string text = getEmailContentByParam(Tembody, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                }
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }

        }
        public void Send_SMS_Telegram(string ChatID, string URL)
        {
            try
            {
                string err = null;
                if (CFHeader == null)
                {
                    err = "NO_WF";
                }
                if (CFItem == null)
                {
                    err = "NO_WF_ITEM";
                }
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                if (Telegram != null)
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;
                    string chatId = Telegram.ChatID;
                    string text = getEmailContentByParam(EmailTemplate.RequestContent, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                }
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }

        }
        public WorkFlowResult Send_SMS_Telegram(string Template, string ChatID, bool IsAuto)
        {
            try
            {
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                if (Telegram != null|| IsAuto)
                {
                    if (Telegram == null) Telegram = new TelegramBot();
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID ?? "835670290:AAGoq8pHBgi0vGHJgCimeMLVGhpNrYzdEfM";
                    if (string.IsNullOrEmpty(apiToken)) return WorkFlowResult.TELEGRAM_NOT_SEND;
                    string chatId = ChatID;// Telegram.ChatID;
                    string text = Template;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                    // dynamic array = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    return WorkFlowResult.COMPLETED;

                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }

        }
        public WorkFlowResult Send_SMS_Telegrams(TPEmailTemplate Template, string ChatID, List<object> ListObjectDictionary, string URL, string _Company, string Body)
        {
            try
            {
                // var Telegram = DB.TelegramBots.FirstOrDefault(w=>w.Description== "LeaveRequest");
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                EmailTemplate = new TPEmailTemplate();
                EmailTemplate = Template;
                if (Telegram != null && !string.IsNullOrEmpty(Telegram.TokenID))
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;// "835670290:AAGoq8pHBgi0vGHJgCimeMLVGhpNrYzdEfM";
                                                       // "872155850:AAHlcg1gcH6MjZaKtzaPhtQu03PxHQN4ZZU";
                                                       //  string chatId = "504467938"; -1001405576397,-1001429819055 
                    string chatId = Telegram.ChatID;//1001446018688
                    string TemBody = Template.RequestContent;
                    //string text = "";
                    //if (_Company == "TELA")
                    //{
                    string text = Body;
                    //}
                    //else text = getEmailContentByParam(TemBody, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                    return WorkFlowResult.COMPLETED;
                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }

        }
        public WorkFlowResult Send_SMS_Telegram(string Template, string TemBody, string ChatID, List<object> ListObjectDictionary, string URL)
        {
            try
            {
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                if (Telegram != null)
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;
                    string chatId = ChatID;// Telegram.ChatID;
                    string text = getEmailContentByParam(Template, TemBody, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                    // dynamic array = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    return WorkFlowResult.COMPLETED;

                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }

        }

        public WorkFlowResult Send_Telegram(string Template, string TemBody, string ChatID, List<object> ListObjectDictionary, string URL)
        {
            try
            {
                var Telegram = DB.TelegramBots.ToList().First();
                if (ChatID != null)
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;
                    string chatId = ChatID;// Telegram.ChatID;
                    string text = getEmailContentByParam(Template, TemBody, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                    // dynamic array = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    return WorkFlowResult.COMPLETED;

                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }

        }

        public string getEmailContentByParam(string text, List<object> ListObjectDictionary, string URL)
        {
            string[] textsp = text.Split(' ');
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = DBB.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == EmailTemplate.EMTemplateObject);
                        if (objParam != null)
                        {

                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName, param);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);
                                }
                            }
                        }
                    }

                }

            }

            string link = URL;
            // text = text.Replace("@NUMBER", number);
            text = text.Replace("@LINK", link);
            // text = text.Replace("@BATCH_NUMBER", batch);

            return text;
        }
        public string getEmailContentByParam(string Template, string text, List<object> ListObjectDictionary, string URL)
        {
            string[] textsp = text.Split(' ');
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = DBB.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == Template);//ESSOT_TELEGRAM
                        if (objParam != null)
                        {

                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName, param);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);

                                }
                            }
                        }
                    }

                }

            }

            string link = URL;
            // text = text.Replace("@NUMBER", number);
            text = text.Replace("@LINK", link);
            // text = text.Replace("@BATCH_NUMBER", batch);

            return text;
        }
        public string getErrorMessage(WorkFlowResult result)
        {
            if (result == WorkFlowResult.ERROR)
            {
                return SYMessages.getMessage("WF_ERR");
            }
            if (result == WorkFlowResult.TELEGRAM_NOT_SEND)
            {
                return SYMessages.getMessage("WF_TEL");
            }
            return "";
        }

        public WorkFlowResult Send_SMS_TelegramOT(string requestContent, string teleChartID, string fileName, List<object> listObjectDictionary, string uRL)
        {
            try
            {
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == teleChartID);
                if (Telegram != null)
                {
                    string apiToken = Telegram.TokenID;
                    string chatId = Telegram.ChatID;
                    string text = getEmailContentByParam(chatId, ListObjectDictionary, uRL); //TemBody;
                    TeleSharp Bot = new TeleSharp(apiToken);
                    MessageSender sender1 = new MessageSender();
                    sender1.Id = chatId;
                    Bot.SendPhoto(sender1, File.ReadAllBytes(fileName), Path.GetFileName(fileName), text);
                    return WorkFlowResult.COMPLETED;
                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
        }
    }
}