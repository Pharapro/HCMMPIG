using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsAPPIncreaseSalary : IClsApplication
    {
        FTFilterEmployee Filter { get; set; }
        List<HRAPPMatrixIncreaseSalary> ListMatrixIncrease { get; set; }
        decimal TotalSalary { get; set; }
        decimal SalaryInBgP { get; set; }
        decimal SalaryIncBgUSD { get; set; }
        decimal SalaryIncBgUtilised { get; set; }
        decimal SalaryIncBgBalance { get; set; }
        string MessageError { get; set; }
        string ScreenId { get; set; }

        string CareerMatrix();
        string EditMatrix_Adding(HRAPPMatrixIncreaseSalary MModel);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        string OnGridModifyMaxSalary(HRAPPMatrixIncreaseSalary MModel, string Action);
        void OnIndexLoading(bool IsESS = false);
        void OnLoadingFilter(FTFilterEmployee filter, List<HRBranch> branches);
        void Refreshvalue(List<HRAPPMatrixIncreaseSalary> ListIncrease, int InYear);
    }
}
