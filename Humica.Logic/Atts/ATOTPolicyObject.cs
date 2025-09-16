using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;
namespace Humica.Logic.Atts
{
    public class ATOTPolicyObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public List<ATOTPolicy> ListOTPolicy { get; set; }
        public List<ATLaEaPolicy> ListLaEa { get; set; }
        public List<ATOTSetting> ListOTSetting { get; set; }
        public List<ATPolicyLaEa> ListPolicyLeEa { get; set; }
        public ATOTPolicyObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}