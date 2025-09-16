using Humica.EF;
using Humica.Training.DB;
using Humica.Core.DB;
using Humica.EF.Models;
using Humica.EF.Models.SY;
using System.Collections.Generic;

namespace Humica.Training
{
    public interface IClsEFSPortal : IClsApplication
    {
        string ScreenId { get; set; }
        List<HRApprSelfAssessment> ListSelfAssessment { get; set; }
        HRApprSelfAssessment Header { get; set; }
        List<HRApprSelfAssQCM> ListSelfAssQCM { get; set; }

        string Create();
        string Delete(params object[] keys);
        void OnCreatingLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading();
        void OnDetailLoading(params object[] keys);
        string OnGridModify(HRApprSelfAssQCM MModel, string Action);
        void OnIndexLoading();
        string Update(string AppraiselType, string QuestionCode);
    }
}
