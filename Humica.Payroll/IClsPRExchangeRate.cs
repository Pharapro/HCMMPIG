using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;
namespace Humica.Payroll
{
    public interface IClsPRExchangeRate : IClsApplication
    {
        List<PRBiExchangeRate> ListBiHeader { get; set; }
        List<PRExchRate> ListHeader { get; set; }

        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        string OnGridModifyBiExchangeRate(PRBiExchangeRate MModel, string Action);
        string OnGridModifyExchangeRate(PRExchRate MModel, string Action);
        void OnIndexLoadingBiExchangeRate();
        void OnIndexLoadingExchangeRate();
    }
}
