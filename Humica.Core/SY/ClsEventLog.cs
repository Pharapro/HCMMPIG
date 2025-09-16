using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Threading.Tasks;

namespace Humica.Core.SY
{
    public class ClsEventLog
    {
        public static async Task  SaveEventLog(string screenId, string userId, string documentAction, string action, Exception e, bool isUpdateException = false)
        {
            SYEventLog log = new SYEventLog
            {
                ScreenId = screenId,
                UserId = userId,
                DocurmentAction = documentAction,
                Action = action
            };

            SYEventLogObject.saveEventLog(log, e, isUpdateException);
        }
        public static void SaveEventLogs(string screenId, string userId, string documentAction, string action, Exception e, bool isUpdateException = false)
        {
            SYEventLog log = new SYEventLog
            {
                ScreenId = screenId,
                UserId = userId,
                DocurmentAction = documentAction,
                Action = action
            };

            SYEventLogObject.saveEventLog(log, e, isUpdateException);
        }
        public static string Save_EventLog(string screenId, string userId, string documentAction, string action, Exception e, bool isUpdateException = false)
        {
            string Error = "";
            Error = e.Message;
            if (e.InnerException != null)
            {
                if (e.InnerException.InnerException != null)
                {
                    Error = e.InnerException.InnerException.Message.Replace("\n", "\n--");
                }
                else
                {
                    Error = e.InnerException.Message.Replace("\n", "\n--");
                }
            }
            SYEventLog log = new SYEventLog
            {
                ScreenId = screenId,
                UserId = userId,
                DocurmentAction = documentAction,
                Action = action
            };

            SYEventLogObject.saveEventLog(log, e, isUpdateException);
            return "EE001";
        }
        public static string Save_EventLogs(string screenId, string userId, string documentAction, string action, Exception e, bool isUpdateException = false)
        {
            string Error = "";
            Error = e.Message;
            if (e.InnerException != null)
            {
                if (e.InnerException.InnerException != null)
                {
                    Error = e.InnerException.InnerException.Message.Replace("\n", "\n--");
                }
                else
                {
                    Error = e.InnerException.Message.Replace("\n", "\n--");
                }
            }
            SYEventLog log = new SYEventLog
            {
                ScreenId = screenId,
                UserId = userId,
                DocurmentAction = documentAction,
                Action = action
            };

            SYEventLogObject.saveEventLog(log, e, isUpdateException);
            return Error;
        }
    }
}
