using Humica.EF.MD;
using System.Collections.Generic;
using Humica.EF;
using Humica.Core.DB;

namespace Humica.Logic.RCM
{
    public interface IClsRCMERecruit : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string DocType { get; set; }
        string ID { get; set; }
        RCMERecruit Header { get; set; }
        List<RCMERecruit> ListHeader { get; set; }
        List<RCMERecruitD> ListDetails { get; set; }

        string createERecruit();
        string updERecruit(string JobID);
        string deleteERecruit(string JobID);
        string UpdateAfferPost(string JobID);
    }
}
