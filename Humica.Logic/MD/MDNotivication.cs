using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic.MD
{
    public class MDNotivication
    {
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public MDNotivication()
        {
            this.User = SYSession.getSessionUser();
            this.BS = SYSession.getSessionUserBS();
        }
        public static List<HR_STAFF_VIEW> CreateData()
        {
            var data = new List<HR_STAFF_VIEW>();
            var DBi = new HumicaDBViewContext();
            data = DBi.HR_STAFF_VIEW.ToList();
            return data.ToList();
        }
    }
}