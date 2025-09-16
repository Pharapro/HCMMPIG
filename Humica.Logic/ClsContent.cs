using Humica.Core;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF.MD;
using Humica.EF.Repo;
using Humica.Notification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Humica.Logic
{
    public class ClsSendNotification
    {
        public async static Task<int> SendEmail(ClsContent _content)
        {
            //if (_content.FirebaseID != null && _content.FirebaseID.Count() > 0)
            //{
            //    await SendNoticeMoblie(_content.FirebaseID, _content);
            //}
            if (!string.IsNullOrEmpty(_content.TokenID) && !string.IsNullOrEmpty(_content.ChatID))
            {
               await Send_SMS_Telegram(_content);
            }
            return 0;
        }
        public async static Task<string> Send_SMS_Telegram(ClsContent _Content)
        {
            //_Content.TokenID = "835670290:AAGoq8pHBgi0vGHJgCimeMLVGhpNrYzdEfM";
            //_Content.ChatID = "504467938";
            try
            {

                    if (_Content.Subject == "Request On-Site")
                        _Content.Subject = "ស្នើសុំបំពេញការងារក្រៅការិយាល័យ";
                    else if (_Content.Subject == "Approval On-Site")
                        _Content.Subject = "អនុម័តលើការបំពេញការងារក្រៅការិយាល័យ";
                    else if (_Content.Subject == "Rejected On-Site")
                        _Content.Subject = "បដិសេធលើសំណើរបំពេញការងារក្រៅការិយាល័យ";
                    _Content = await SendNotificationTG_KH(_Content);

                string text = _Content.BodyTG;
                string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                string apiToken = _Content.TokenID;
                string chatId = _Content.ChatID;
                if (!string.IsNullOrEmpty(_Content.ChatIDLM))
                    chatId = _Content.ChatIDLM;
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
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async static Task<ClsContent> SendNotificationTG_KH(ClsContent _Content)
        {
            string SubjectType = _Content.SubjectTypeTG;
            string str = "<b>កម្មវត្ថុ៖</b> " + _Content.Subject;

            if (_Content.SubjectTypeTG == "Requested")
            {
                SubjectType = "ស្នើសុំ";
                str += "%0A<b>អ្នកស្នើសុំ៖</b> " + _Content.SubmitBy;
                str += "%0A<b>តួនាទី៖</b> " + _Content.Position;
                str += "%0A<b>ទីតាំងបំរើការងារ៖</b> " + _Content.Location;
                str += "%0A<b>រង់ចាំបញ្ជាក់ដោយ៖</b> " + _Content.SubmitTo;
                if (!string.IsNullOrEmpty(_Content.SubmitToF))
                    str += "%0A<b>រង់ចាំអនុម័តដោយ៖</b> " + _Content.SubmitToF;
            }
            else if (_Content.SubjectTypeTG == "Approved")
            {
                SubjectType = "";
                str += "%0A<b>អនុម័តដោយ៖</b> " + _Content.SubmitBy;
                str += "%0A<b>ទៅកាន់៖</b> " + _Content.SubmitTo;
            }
            else if (_Content.SubjectTypeTG == "Rejected")
            {
                SubjectType = "";
                str += "%0A<b>បដិសេធទៅកាន់៖</b> " + _Content.SubmitTo;
                str += "%0A<b>ដោយ៖</b> " + _Content.SubmitBy;
            }
            str += "%0A<b>កាលបរិច្ឆេទ" + SubjectType + "៖</b> " + _Content.SubmitDate;
            str += _Content.BodyTG;
            _Content.BodyTG = str;
            return _Content;
        }
    }
    public class ClsTemplateAlert
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsTemplateAlert()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public string GetTokenID(string ChatID)
        {
            string TokenID = "";
            TelegramBot token = unitOfWork.Set<TelegramBot>().FirstOrDefault(w => w.ChatID == ChatID);
            if (token != null)
            {
                TokenID = token.TokenID;
            }
            return TokenID;
        }
        public List<string> GetFirebaseID(string UserApp)
        {
            List<string> clientToken = new List<string>();
            List<TokenResource> Listtoken = unitOfWork.Set<TokenResource>().ToList();
            string[] Emp = UserApp.Split(';');
            foreach (var Code in Emp)
            {
                if (Code.Trim() != "")
                {
                    TokenResource token = Listtoken.FirstOrDefault(b => b.UserName == UserApp);
                    if (token != null)
                    {
                        if (!string.IsNullOrEmpty(token.FirebaseID))
                        {
                            clientToken.Add(token.FirebaseID);
                        }
                    }
                }
            }

            return clientToken;
        }
        public ClsContent GetSubjectMessage(string Subject, string SubjectType, string SubmitBy,
            string SubmitTo, string ChatID)
        {
            ClsContent _content = new ClsContent();
            _content.Subject = Subject;
            _content.SubjectTypeTG = SubjectType;
            _content.SubmitBy = SubmitBy;
            _content.SubmitTo = SubmitTo;
            _content.SubmitDate = DateTime.Now.ToString("dd-MM-yyyy");
            _content.ChatID = ChatID;
            _content = Get_JobInfor(_content);
            return _content;
        }
        public ClsContent Get_JobInfor(ClsContent _content)
        {
            var Staff =  unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == _content.Requester);
            if (Staff != null)
            {
                ClsFilterStaff filterStaff = new ClsFilterStaff();
                _content.Position = filterStaff.Get_Positon(Staff.JobCode);
                _content.Location = filterStaff.Get_Location(Staff.LOCT);
            }
            return _content;
        }

        public SYHRAnnouncement AddAnnouncement(string Subject, string SubjectType,
            string Description, string Receiver, string DocReference)
        {
            SYHRAnnouncement _announ = new SYHRAnnouncement();
            _announ.Subject = Subject;
            _announ.Type = SubjectType;
            _announ.Description = Description;
            _announ.DocumentNo = DocReference;
            _announ.DocumentDate = DateTime.Now;
            _announ.IsRead = false;
            _announ.UserName = Receiver;
            _announ.CreatedBy = User.UserName;
            _announ.CreatedOn = DateTime.Now;
            return _announ;
        }
    }
    public class ClsContent
    {
        public string CompanyCode { get; set; }
        public string Subject { get; set; }
        public string SubjectEmail { get; set; }
        public string SubjectTypeTG { get; set; }
        public string SubjectTypeEA { get; set; }
        public string SubjectNumber { get; set; }
        public string SubmitBy { get; set; }
        public string SubmitTo { get; set; }
        public string SubmitToF { get; set; }
        public string SubmitDate { get; set; }
        public string BodyTG { get; set; }
        public string BodyAPP { get; set; }
        public string EmailBody { get; set; }
        public string URL { get; set; }
        public string TokenID { get; set; }
        public string ChatID { get; set; }
        public string ChatIDLM { get; set; }
        public string EmailTo { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public string Requester { get; set; }
        public decimal? Blaance { get; set; }
        public decimal? BlaanceAct { get; set; }
        public List<string> FirebaseID { get; set; }
    }
}
