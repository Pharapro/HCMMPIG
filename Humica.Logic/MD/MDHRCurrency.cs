using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.MD
{
    public class MDHRCurrency
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRCurrency Header { get; set; }
        public string ScreenId { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRCurrency> ListHeader { get; set; }
        public MDHRCurrency()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


    }

}