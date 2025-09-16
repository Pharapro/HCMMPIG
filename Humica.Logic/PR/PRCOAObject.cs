using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.PR
{
    public class PRCOAObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRChartofAccount Header { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public List<PRChartofAccount> ListHeader { get; set; }
        public List<HRMaterialAccount> ListMaterailMaster { get; set; }
        public List<HRJournalType> ListJournalType { get; set; }
        public List<CFExVendor> ListCFExVendor { get; set; }
        public List<SYSHRBuiltinAcc> ListSetting { get; set; }

        public PRCOAObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}


