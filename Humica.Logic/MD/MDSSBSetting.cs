using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.MD
{
    public class MDSSBSetting
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public SSBSetting Header { get; set; }
        public string ScreenId { get; set; }
        public string BranchCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        //public FTDGeneralPeriod Filter { get; set; }
        //public MDCustomerControl ControlData { get; set; }
        public List<SSBSetting> ListHeader { get; set; }
        //public List<MDDepreSetting> ListDepreSetting { get; set; }
        public MDSSBSetting()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


    }

}