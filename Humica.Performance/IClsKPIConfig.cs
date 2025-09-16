using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsKPIConfig : IClsApplication
    {
        List<HRKPICategory> ListCategory { get; set; }
        List<HRKPIType> ListKPIGroupHeader { get; set; }
        List<HRKPIList> ListKPIListByBU { get; set; }
        List<HRKPIList> ListKPIListByDept { get; set; }
        List<HRKPIIndicator> ListIndicatorByDept { get; set; }
        List<HRKPIIndicator> ListIndicatorByBU { get; set; }
        List<HRKPIAction> ListKPIAction { get; set; }
        List<HRKPINonFinance> ListKPINonFinance { get; set; }
        List<HRKPIList> ListKPIList { get; set; }
        List<HRKPIGrade> ListKPIGrade { get; set; }
        List<HRKPINorm> ListKPINorm { get; set; }
        void OnIndexLoading();
        void OnIndexLoadingKPIType();
        void OnIndexLoadingCategory();
        void OnIndexLoadingBU(bool ESS = false);
        void OnIndexLoadingDept(bool ESS = false);
        void OnIndexIndicatorBU(bool ESS = false);
        void OnIndexLoadingIndicator(bool ESS = false);
        void OnIndexLoadingKPIList();
        void OnIndexLoadingKPIGrade();
        void OnIndexLoadingKPINorm();
        void OnLoadingAction();
        void OnLoadingNonFinance();
        string OnLoadingDepartment(string Department);
        string OnLoadingBU(string BU);
        string OnGridModifyKPI(HRKPIList MModel, string Action, bool IsESS = false);
        string OnGridModifyKPIBU(HRKPIList MModel, string Action, string Category, bool IsESS = false);
        string OnGridModifyIndicator(HRKPIIndicator MModel, string Action, bool IsESS = false);
        string OnGridModifyIndicatorBU(HRKPIIndicator MModel, string Action, string Category, bool IsESS = false);
        string OnGridModifyKPIType(HRKPIType MModel, string Action);
        string OnGridModifyCategory(HRKPICategory MModel, string Action);
        string OnGridModifyGrade(HRKPIGrade MModel, string Action);
        string OnGridModifyNorm(HRKPINorm MModel, string Action);
        string OnGridModifyAction(HRKPIAction MModel, string Action);
        string OnGridModifyNonFinance(HRKPINonFinance MModel, string Action);

        Dictionary<string, dynamic> OnDataSelectorLoading(bool ESS = false);
    }
}
