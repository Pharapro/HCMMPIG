namespace Humica.Core.SY
{
    public class SYWorkFlowObject
    {
    }
    public enum WorkFlowType
    {
        REQUESTER, APPROVER, REJECTOR, RECEIVER, COLLECTOR
    }
    public enum WorkFlowAction
    {
        REQUESTED, APPROVED
    }
    public enum WorkFlowResult
    {
        ERROR, COMPLETED, EMAIL_NOT_SEND, TELEGRAM_NOT_SEND
    }
}