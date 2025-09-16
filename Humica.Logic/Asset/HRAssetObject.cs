using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.Asset
{
    public class HRAssetObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public List<HRAssetType> ListAssetType { get; set; }
        public List<HRAssetClass> ListAssetClass { get; set; }
		public List<HRAssetLocation> ListAssetLocation { get; set; }
		public List<HRDriver> ListDriver { get; set; }
		public HRAssetObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}