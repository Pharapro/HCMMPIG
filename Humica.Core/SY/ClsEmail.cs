using Hangfire;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace Humica.Core.SY
{
    public class ClsEmail
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ScreenId { get; set; }

        public string MailFrom { get; set; }
        public string MailCC { get; set; }
        public string MailTo { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpName { get; set; }
        public string SmtpPassword { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        private string _Attach;
        // private string _smtp_mail_id;

        public string SmtpPort { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<Job> ListJobs { get; set; }
        public Job Job { get; set; }
        public List<State> ListJobStates { get; set; }
        public string SendBoxDescription { get; set; }
        public ClsEmail()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string Attach
        {
            get { return _Attach; }
            set { _Attach = value; }
        }

        public bool IsEnableSSL { get; set; }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public Int16 SendEmail()
        {
            try
            {
                MailMessage Mail = new MailMessage();
                // Set the to and from addresses

                Mail.IsBodyHtml = true;
                Mail.From = new MailAddress(MailFrom);
                string[] sp = MailTo.Split(';');
                if (sp.Length > 0)
                {
                    foreach (string esp in sp)
                    {
                        if (IsValidEmail(esp))
                        {
                            Mail.To.Add(esp);
                        }
                    }
                }
                else
                {
                    Mail.To.Add(MailTo);
                }
                if (MailCC != null)
                {
                    string[] spc = MailCC.Split(';');
                    foreach (string esp in spc)
                    {
                        if (IsValidEmail(esp))
                        {
                            Mail.CC.Add(esp);
                        }
                    }
                }
                // Set the subject and body

                Mail.Subject = Subject;
                Mail.Body = Body;
                if (Attach != null)
                {
                    Mail.Attachments.Add(new Attachment(_Attach, System.Net.Mime.MediaTypeNames.Application.Octet));
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = SmtpHost;
                smtp.Port = Convert.ToInt32(SmtpPort);
                smtp.EnableSsl = this.IsEnableSSL;
                smtp.Credentials = new NetworkCredential(SmtpName, SmtpPassword);
                smtp.Send(Mail);
                return 0;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = "EMAIL";
                log.UserId = "EMAIL";
                log.DocurmentAction = "EMAIL";
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return 102;
            }
        }

        public StringBuilder GetMailBody(string text, Hashtable param)
        {
            StringBuilder sb = new StringBuilder();
            if (param != null)
            {
                if (param.Count > 0)
                {
                    foreach (string key in param.Keys)
                    {
                        text = text.Replace("@" + key + "@", param[key].ToString());
                    }
                }
            }
            sb.Append(text);
            return sb;
        }

        public StringBuilder GetEmailTitle(string cre)
        {
            return new StringBuilder(cre);
        }

        public StringBuilder GetMailBody(StringBuilder cre, Hashtable param)
        {
            StringBuilder sb = new StringBuilder();
            string text = cre.ToString();
            if (param != null)
            {
                if (param.Count > 0)
                {
                    foreach (string key in param.Keys)
                    {
                        text = text.Replace("@" + key + "@", param[key].ToString());
                    }
                }
            }
            sb.Append(text);
            return sb;
        }

        //public int SendGmail()
        //{
        //    MailMessage msg = new MailMessage();
        //    SmtpClient client = new SmtpClient();
        //    _mailfrom = MailFrom;
        //    _smtp_mail_id = _mailfrom;
        //    _smtp_password = SmtpPassword;
        //    try
        //    {
        //        msg.Subject = _subject;
        //        msg.Body = _body;
        //        msg.From = new MailAddress(_mailfrom);
        //        msg.To.Add(_mailto);
        //        if (_mailcc != null)
        //        {
        //            msg.CC.Add(_mailcc);
        //        }
        //        msg.IsBodyHtml = true;
        //        client.Host = "smtp.gmail.com";
        //        NetworkCredential basicauthenticationinfo = new NetworkCredential(_smtp_mail_id, _smtp_password);
        //        client.Port = 587;
        //        client.EnableSsl = true;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = basicauthenticationinfo;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.Send(msg);
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //        return 102;
        //    }
        //}
        public string GetAllList(int jobId = 0)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                //GridItems1
                ListJobs = new List<Job>();
                ListJobs = DBX.Jobs.OrderByDescending(o => o.CreatedAt).ToList();

                if (jobId != 0)
                {
                    Job = DBX.Jobs.FirstOrDefault(f => f.Id == jobId);
                    ListJobStates = DBX.States.Where(w => w.JobId == jobId).OrderByDescending(o => o.CreatedAt).ToList();
                }
                else
                {
                    Job = null;
                    ListJobStates = new List<State>();
                }



                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteGridItems1(string jobId)
        {
            try
            {

                bool a = BackgroundJob.Delete(jobId);

                GetAllList();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog
                {
                    ScreenId = ScreenId,
                    UserId = User.UserName,
                    Action = SYActionBehavior.ADD.ToString()
                };
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #region this can send more file to email
        public int SendMails(CFEmailAccount EmailAccount, string ToAddress, string CcAddress,
                    string Subject, string Body, string[] AttachFile)
        {
            // Arrange data
            string SMTPHostName = EmailAccount.SMTPHostName;
            string UserName = EmailAccount.UserName;
            string Password = EmailAccount.Password;
            string SMTPPort = EmailAccount.SMTPPort.Value.ToString();
            bool IsEnableSSL = EmailAccount.IsEnableSSL.Value;
            string FromAddress = EmailAccount.EmailAccount;

            // Queue the actual email sending job
            SendMailJob(
                SMTPHostName, UserName, Password, SMTPPort, IsEnableSSL,
                FromAddress, ToAddress, CcAddress,
                Subject, Body, AttachFile);

            return 0; // Consider improving return type for success/failure
        }
        public void SendMailJob(string SMTPHostName, string UserName, string Password, string SMTPPort,
                 bool IsEnableSSL, string FromAddress, string ToAddress, string CcAddress,
                 string Subject, string Body, string[] AttachFiles)
        {
            try
            {
                // Setup SMTP client
                using (var smtpClient = new SmtpClient(SMTPHostName, int.Parse(SMTPPort)))
                {
                    smtpClient.EnableSsl = IsEnableSSL;
                    smtpClient.Credentials = new NetworkCredential(UserName, Password);

                    // Create the email
                    var mail = new MailMessage();
                    mail.From = new MailAddress(FromAddress);
                    if (!string.IsNullOrEmpty(ToAddress))
                        mail.To.Add(ToAddress);

                    if (!string.IsNullOrEmpty(CcAddress))
                    {
                        mail.CC.Add(CcAddress);
                    }

                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true; // If the body contains HTML content

                    // Attach files if provided
                    if (AttachFiles != null && AttachFiles.Any())
                    {
                        foreach (var filePath in AttachFiles)
                        {
                            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                            {
                                Attachment attachment = new Attachment(filePath);
                                mail.Attachments.Add(attachment);
                            }
                        }
                    }

                    // Send the email
                    smtpClient.Send(mail);
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log it as necessary
                Console.WriteLine("Error sending email: " + ex.Message);
                // You can implement logging or retry logic here.
            }
        }
        #endregion
        #region send by one file with run Background
        public int SendMail(CFEmailAccount EmailAccount, string ToAddress, string CcAddress,
                    string Subject, string Body, string AttachFile, string FileName)
        {
            // Arrange data
            string SMTPHostName = EmailAccount.SMTPHostName;
            string UserName = EmailAccount.UserName;
            string Password = EmailAccount.Password;
            string SMTPPort = EmailAccount.SMTPPort.Value.ToString();
            bool IsEnableSSL = EmailAccount.IsEnableSSL.Value;
            string FromAddress = EmailAccount.EmailAccount;

            // Todo: Let's also create a sample background job
            BackgroundJob.Enqueue(() => SendMail(
                SMTPHostName, UserName, Password, SMTPPort, IsEnableSSL,
                FromAddress, ToAddress, CcAddress,
                Subject, Body, AttachFile, FileName));

            return 0;
        }
        public void SendMail(string SmtpServer, string SmtpUserName, string SmtpUserPass, string SmtpPort, bool EnableSsl,
          string fromAddress, string toAddress, string ccAddress,
          string subject, string body, string attachFile, string fileName)
        {
            MailAddress addr = new MailAddress(fromAddress);
            string mailFrom = addr.Address;
            string displayName = string.IsNullOrEmpty(addr.DisplayName) ? addr.User : addr.DisplayName;
            MailAddress fromEmail = new MailAddress(mailFrom, displayName);

            //Read SMTP Server Name or IP from Config xml file
            //string SmtpServer = SMTPHostName;
            //ConfigSettings.GetProperty(Constants.SMTP_SERVER);
            //Read User Name from Config xml file
            //string SmtpUserName = UserName;
            //ConfigSettings.GetProperty(Constants.SMTP_USERNAME);
            //Read User Password from Config xml file
            //string SmtpUserPass = Password;
            //ConfigSettings.GetProperty(Constants.SMTP_PASSWORD);
            //Read port setting from Config xml file
            //string smtpPort = SMTPPort;
            //ConfigSettings.GetProperty(Constants.SMTP_PORT);

            SmtpClient smtpSend = new SmtpClient(SmtpServer);

            using (MailMessage emailMessage = new MailMessage())
            {
                if (!string.IsNullOrEmpty(toAddress))
                {
                    //emailMessage.To.Add(toAddress);
                    string[] spc = toAddress.Replace(" ", string.Empty).Split(';');
                    foreach (string esp in spc)
                    {
                        if (IsValidEmail(esp))
                        {
                            emailMessage.To.Add(esp);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ccAddress))
                {
                    //emailMessage.CC.Add(ccAddress);
                    string[] spc = ccAddress.Replace(" ", string.Empty).Split(';');
                    foreach (string esp in spc)
                    {
                        if (IsValidEmail(esp))
                        {
                            emailMessage.CC.Add(esp);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(attachFile) && File.Exists(@attachFile))
                {
                    ContentType contentType = new ContentType
                    {
                        MediaType = MediaTypeNames.Application.Octet,
                        Name = fileName
                    };
                    emailMessage.Attachments.Add(new Attachment(attachFile, contentType));
                }

                emailMessage.From = fromEmail;
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                if (!Regex.IsMatch(emailMessage.Body, @"^([0-9a-z!@#\$\%\^&\*\(\)\-=_\+])", RegexOptions.IgnoreCase) ||
                    !Regex.IsMatch(emailMessage.Subject, @"^([0-9a-z!@#\$\%\^&\*\(\)\-=_\+])", RegexOptions.IgnoreCase))
                {
                    emailMessage.BodyEncoding = Encoding.Unicode;
                }

                if (SmtpUserName != null && SmtpUserPass != null && SmtpPort != null)
                {
                    smtpSend.Port = Convert.ToInt32(SmtpPort);
                    smtpSend.UseDefaultCredentials = false;
                    smtpSend.Credentials = new NetworkCredential(SmtpUserName, SmtpUserPass);
                    smtpSend.EnableSsl = EnableSsl;
                }

                try
                {
                    smtpSend.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }
        #endregion
        #region send by one file and no run Background
        public int SendMailNew(CFEmailAccount emailAccount, string toAddress, string ccAddress, string subject, string body, string attachFile, string fileName)
        {
            // Arrange data
            string smtpHostName = emailAccount.SMTPHostName;
            string userName = emailAccount.UserName;
            string password = emailAccount.Password;
            string smtpPort = emailAccount.SMTPPort.Value.ToString();
            bool isEnableSSL = emailAccount.IsEnableSSL.Value;
            string fromAddress = emailAccount.EmailAccount;

            // Enqueue the background job
            SendMailInternal(
          smtpHostName, userName, password, smtpPort, isEnableSSL,
          fromAddress, toAddress, ccAddress,
          subject, body, attachFile, fileName);

            return 0;
        }

        public void SendMailInternal(string smtpServer, string smtpUserName, string smtpUserPass, string smtpPort, bool enableSsl,
            string fromAddress, string toAddress, string ccAddress,
            string subject, string body, string attachFile, string fileName)
        {
            using (var smtpSend = new SmtpClient(smtpServer))
            using (var emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(fromAddress);
                AddEmailAddresses(emailMessage.To, toAddress);
                AddEmailAddresses(emailMessage.CC, ccAddress);
                AddAttachment(emailMessage, attachFile, fileName);

                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                SetSmtpClientConfiguration(smtpSend, smtpUserName, smtpUserPass, smtpPort, enableSsl);

                try
                {
                    smtpSend.Send(emailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                    throw new InvalidOperationException("Failed to send email: " + ex.Message);
                }
            }
        }

        private void AddEmailAddresses(MailAddressCollection collection, string addresses)
        {
            if (!string.IsNullOrEmpty(addresses))
            {
                string[] spc = addresses.Replace(" ", string.Empty).Split(';');
                foreach (string addr in spc)
                {
                    if (IsValidEmail_(addr))
                    {
                        collection.Add(addr);
                    }
                }
            }
        }

        private void AddAttachment(MailMessage emailMessage, string attachFile, string fileName)
        {
            if (!string.IsNullOrEmpty(attachFile) && File.Exists(attachFile))
            {
                emailMessage.Attachments.Add(new Attachment(attachFile, new ContentType
                {
                    MediaType = MediaTypeNames.Application.Octet,
                    Name = fileName
                }));
            }
        }

        private void SetSmtpClientConfiguration(SmtpClient smtpSend, string smtpUserName, string smtpUserPass, string smtpPort, bool enableSsl)
        {
            if (!string.IsNullOrEmpty(smtpUserName) && !string.IsNullOrEmpty(smtpUserPass) && !string.IsNullOrEmpty(smtpPort))
            {
                smtpSend.Port = Convert.ToInt32(smtpPort);
                smtpSend.UseDefaultCredentials = false;
                smtpSend.Credentials = new NetworkCredential(smtpUserName, smtpUserPass);
                smtpSend.EnableSsl = enableSsl;
            }
        }

        private bool IsValidEmail_(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }
        #endregion
    }
}