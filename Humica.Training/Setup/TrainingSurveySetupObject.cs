using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Training.Setup
{
    public class TrainingSurveySetupObject
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<TRSurveyRegion> ListSurveyRegion { get; set; }
        public List<TRSurveyFactor> ListSurveyFactor { get; set; }
        public List<TRSurveyRating> ListSurveyRating { get; set; }
        public TrainingSurveySetupObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}