using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Repo;
using System;
using System.IO;

namespace Humica.Core.SY
{
    public class ClsEmailTemplate
    {
        IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsEmailTemplate() {
            OnLoad();
        }
        public string CheckEmail(HRStaffProfile Staff)
        {
            string Email = "";
            if (!string.IsNullOrEmpty(Staff.Email))
                Email = Staff.Email;
            return Email;
        }
        public ZEmailSentLog Insert_EmailLog(string EmpCode, string Subject, string EmailTo, string EmailBody)
        {
            OnLoad();
            var emailLog = new ZEmailSentLog();
            try
            {
                ClsEmailAccount emailAccount = GetEmail();
                if (emailAccount != null)
                {
                    emailAccount.Subject = Subject;
                    emailLog = Get_EmailLog(emailAccount, EmailTo, EmailBody, EmpCode);
                }
                return emailLog;
            }
            catch (Exception ex)
            {
                return new ZEmailSentLog();
            }
        }
        public ClsEmailAccount GetEmail()
        {
            ClsEmailAccount _Email = new ClsEmailAccount();
            var Email = unitOfWork.Repository<CFEmailAccount>().FirstOrDefault();
            if (Email != null)
            {
                _Email.StmpObject = Email.SMTPObject;
                _Email.EmailAccount = Email.EmailAccount;
                _Email.UserName = Email.UserName;
                _Email.Password = Email.Password;
                _Email.SMTPHostName = Email.SMTPHostName;
                _Email.SMTPPort = Email.SMTPPort.ToString();
                _Email.IsEnableSSL = Email.IsEnableSSL.Value;
                _Email.EmailFrom = Email.EmailAccount;
            }
            return _Email;
        }
        public ZEmailSentLog Get_EmailLog(ClsEmailAccount EmailAccount, string EmailTo, string EmailBody, string CreateBy)
        {
            var mailLog = new ZEmailSentLog()
            {
                SMTPHostName = EmailAccount.SMTPHostName,
                SMTPPort = 0,
                IsEnableSSL = false,
                FromAddress = EmailAccount.EmailAccount,
                ToAddress = EmailTo,
                CcAddress = EmailAccount.EmailCC,
                Subject = EmailAccount.Subject,
                Body = EmailBody,
                AttachFile = EmailAccount.AttachFile,
                FileName = Path.GetFileName(EmailAccount.AttachFile),
                TryAttemp = 1,
                CompanyCode = "",
                RefNumber = "",
                StmpObject = EmailAccount.StmpObject,
                ScreenID = EmailAccount.ScreenId,
                CreatedBy = CreateBy,
                CreatedOn = DateTime.Now,
                Status = false,
                StateName = "Scheduled"
            };
            return mailLog;
        }


    }
    public class ClsEmailAccount
    {
        public string StmpObject { get; set; }
        public string SMTPHostName { get; set; }
        public string EmailAccount { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SMTPPort { get; set; }
        public bool IsEnableSSL { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string AttachFile { get; set; }
        public string ScreenId { get; set; }

    }
}
