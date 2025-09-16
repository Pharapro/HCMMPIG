using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.EOB
{
    public class MDEOBOrientation
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public List<EOBOrienChList> ListEOBOrienChList { get; set; }
        public List<EOBOrienChListItem> ListEOBOrienChListItem { get; set; }
        public List<EOBTypiDurType> ListEOBTypiDurType { get; set; }
        public List<EOBOrienTypeDur> ListEOBOrienTypeDur { get; set; }
        public List<EOBOrienDesc> ListEOBOrienDesc { get; set; }

        public MDEOBOrientation()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}