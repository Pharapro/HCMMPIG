using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humica.Logic.HR
{
    public class HREmpMultiPositionObject
	{
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public List<HREmpMultiPosition> ListHREmpMultiPosition { get; set; }
        public HREmpMultiPositionObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}