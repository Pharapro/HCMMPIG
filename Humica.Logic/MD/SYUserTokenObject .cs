using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.MD
{
    public class SYUserTokenObject
    {
        
        public string StorageSelected = "";
        public SYUser User { get; set; }
        public TokenResource header { get; set; }
        public string ScreenId { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<TokenResource> ListHeader { get; set; }
        public string MessageCode { get; set; }
        public SYRole HeaderRole { get; set; }
        public SYUserTokenObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}