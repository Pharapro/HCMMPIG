using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.EOB
{
    public class MDEOBPreboarding
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public List<EOBPreBoStep> ListPreBoardingStep { get; set; }
        public List<EOBPreBoItem> ListPreBoardingItem { get; set; }
        public List<EOBPreBoStage> ListPreBoardingStage { get; set; }
        public List<EOBSocMedAcc> ListSocialMediaAcc { get; set; }

        public MDEOBPreboarding()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}