using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.TelegramSharp.Telegram;
using System;
using System.IO;

namespace Humica.Logic.PR
{
    public class PRSendTelegram
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }

        public PRSendTelegram()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void Send_PaySlip_Telegram(string ChartID, string FileName, string TokenID, HR_PR_EmpSalary EmpSalary)
        {
            string EmpCode = "";
            try
            {
                DateTime _InMonth = new DateTime((int)EmpSalary.INYear, EmpSalary.INMonth, 1);
                // string FileName = @"D:\Project Master\Telegram Testing\Alter Telegram\Alter Telegram\RC.pdf";
                // TeleSharp Bot = new TeleSharp("835670290:AAGoq8pHBgi0vGHJgCimeMLVGhpNrYzdEfM");
                TeleSharp Bot = new TeleSharp(TokenID);
                MessageSender sender1 = new MessageSender();
                sender1.Id = ChartID;
                string caption = "Hello <b>" + EmpSalary.AllName + "</b>\nThis the Payslip on <b>" + _InMonth.ToString("MMMM yyyy") +"</b>";
                //Bot.SendMessage(new SendMessageParams
                //{
                //    ChatId = sender1.Id.ToString(),
                //    Text = "Hello\nThis the Payslip your month" + DateTime.Now
                //});
                Bot.SendDocument(sender1, File.ReadAllBytes(FileName), Path.GetFileName(FileName), caption);
                //Bot.SendDocument(sender1, System.IO.File.ReadAllBytes(FileName),
                //    Path.GetFileName(FileName));
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = EmpCode;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }

        }

    }
}


