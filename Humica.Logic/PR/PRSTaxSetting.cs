using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.PR
{
    public class PRSTaxSetting
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRTaxSetting Header { get; set; }
        public string ScreenId { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PRTaxSetting> ListHeader { get; set; }


        public PRSTaxSetting()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}


