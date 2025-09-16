using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.MD
{
    public class MDRoomType
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRRoomType Header { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRRoomType> ListHeader { get; set; }
        public MDRoomType()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


    }

}