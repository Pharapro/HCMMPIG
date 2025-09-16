using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.CF
{
    public class CFEmail
    {
        public SMSystemEntity DB = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public SYEmailConf Header { get; set; }
        public string ScreenId { get; set; }
        public string BranchCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        //public FTDGeneralPeriod Filter { get; set; }
        //public MDCustomerControl ControlData { get; set; }
        public List<SYEmailConf> ListHeader { get; set; }
        //public List<MDDepreSetting> ListDepreSetting { get; set; }
        public CFEmail()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


    }

}