using Humica.Core;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Payroll
{
    public interface IClsPRPayPeriod : IClsApplication
    {
        List<PRPayPeriodItem> ListPeriod { get; set; }

        string OnGridModifyPeriod(PRPayPeriodItem MModel, string Action);
        void OnIndexLoading();
        void OnIndexLoadingPeriod();
    }
}
