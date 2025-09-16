using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Core.CF
{
    public class CFNumberRankObject
    {
        public List<BSNumberRank> ListNumberRankHeader { get; set; }
        public List<BSNumberRankItemN> ListNumberRankHofCom { get; set; }
        public List<BSNumberRankItem> ListNumberSofComYear { get; set; }
        public List<HREmpCode> ListNumEmpCode { get; set; }

    }
}