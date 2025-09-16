using Humica.EF.MD;

namespace Humica.Logic.MD
{
    public class MDChangeEmpCode
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string EmpCodeFrom { get; set; }
        public string EmpCodeTo { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public MDChangeEmpCode()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}