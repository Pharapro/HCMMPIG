using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.EOB
{
    public class MDEOBCheckList
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public List<EOBSChkLst> ListOnBoardChkLst { get; set; }
        public List<EOBSChkLstItem> ListOnBoardChkLstItem { get; set; }

        public MDEOBCheckList()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}