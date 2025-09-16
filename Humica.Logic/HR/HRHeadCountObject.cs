using Humica.Core.DB;
using Humica.EF.MD;
using Humica.Logic.Att;
using System.Collections.Generic;

namespace Humica.Logic.HR
{
    public class HRHeadCountObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public int INYear { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public HRHeadCount Header { get; set; }
        public HRDepartment Dept { get; set; }
        public List<HRHeadCount> ListHeadCount { get; set; }
        public List<HRDepartment> ListDept { get; set; }
        public List<DayInMonth> ListDayInMonth { get; set; }
        public string MessageCode { get; set; }

        public HRHeadCountObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}