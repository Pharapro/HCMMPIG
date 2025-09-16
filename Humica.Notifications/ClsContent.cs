using System;
using System.Collections.Generic;

namespace Humica.Notifications
{
    public class ClsTemplateAlert
    {
        public static ClsContent GetSubjectMessage(string Subject, string SubjectType, string SubmitBy,
            string SubmitTo, string ChatID, DateTime SummitDate)
        {
            ClsContent _content = new ClsContent();
            _content.Subject = Subject;
            _content.SubjectTypeTG = SubjectType;
            _content.SubmitBy = SubmitBy;
            _content.SubmitTo = SubmitTo;
            _content.SubmitDate = SummitDate.ToString("dd-MM-yyyy");
            _content.ChatID = ChatID;

            return _content;
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
