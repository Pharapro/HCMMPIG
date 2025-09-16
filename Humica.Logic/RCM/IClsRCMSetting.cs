using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public interface IClsRCMSetting: IClsApplication
    {
         SYUser User { get; set; }
         SYUserBusiness BS { get; set; }
         string ScreenId { get; set; }
         string Code { get; set; }
         List<RCMAdvType> ListAdvType { get; set; }
         List<RCMSAdvertise> ListAds { get; set; }
         List<RCMSLang> ListLanguage { get; set; }
         List<RCMSJobDesc> ListJD { get; set; }
         List<RCMSRefQuestion> ListSRefQues { get; set; }
         List<RCMSQuestionnaire> ListQuestionnaire { get; set; }
    }
}