using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;
namespace Humica.Payroll
{
    public interface IClsPRTaxSetting : IClsApplication
    {
        string ScreenId { get; set; }
        List<PRTaxSetting> ListHeader { get; set; }
        PRPayrollSetting PayrollSetting { get; set; }
        List<PRExceptType> ListExceptType { get; set; }

        Dictionary<string, dynamic> OnDataSelectorLoading();
        string OnGridModifyExceptType(PRExceptType MModel, string Action);
        string OnGridModifyTaxSetting(PRTaxSetting MModel, string Action);
        void OnIndexExceptType();
        void OnIndexLoading();
        void OnIndexTaxSetting();
        string UpdateSetting();
    }
}
