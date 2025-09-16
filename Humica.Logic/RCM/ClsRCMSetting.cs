using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Repo;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public class ClsRCMSetting: IClsRCMSetting
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public List<RCMAdvType> ListAdvType { get; set; }
        public List<RCMSAdvertise> ListAds { get; set; }
        public List<RCMSLang> ListLanguage { get; set; }
        public List<RCMSJobDesc> ListJD { get; set; }
        public List<RCMSRefQuestion> ListSRefQues { get; set; }
        public List<RCMSQuestionnaire> ListQuestionnaire { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsRCMSetting()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
    }
}