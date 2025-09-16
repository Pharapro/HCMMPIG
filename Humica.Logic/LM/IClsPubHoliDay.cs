using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.LM
{
    public interface IClsPubHoliDay : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string MessageError { get; set; }
        HRPubHoliday Header { get; set; }
        List<HRPubHoliday> ListHeader { get; set; }
        List<MDUploadTemplate> ListTemplate { get; set; }
        FTFilterEmployee Filter { get; set; }

        string uploadPubHoliday();
    }
}
