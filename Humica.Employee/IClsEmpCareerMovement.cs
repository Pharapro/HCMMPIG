using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Employee
{
    public interface IClsEmpCareerMovement : IClsApplication
    {
        FTFilterEmployee Filter { get; set; }
        List<ClsEmpCarrer> ListEmpCareerMove { get; set; }
        HREmpCareerMovement Header { get; set; }
        IQueryable<ClsStaff> ListStaff { get; set; }
        string NewSalary { get; set; }
        string OldSalary { get; set; }
        string Increase { get; set; }
        List<HREmpUnderManager> ListUnderManager { get; set; }
        string ScreenId { get; set; }
        bool IsSalary { get; set; }
        bool IsInAtive { get; set; }
        List<HREmpUnderManager> ListAddUManager { get; set; }
        List<HRStaffProfile> ListAddEmp { get; set; }
        List<HRStaffProfile> ListEmpByDept { get; set; }
        string EmpID { get; set; }

        string Create();
        void OnCreatingLoading(string ID);
        Dictionary<string, dynamic> OnDataJobLoading(params object[] keys);
        void OnDetailLoading(params object[] keys);
        string OnGridModify(HREmpUnderManager MModel, string Action);
        string OnGridModifyAdd(string DocumentType);
        string OnGridModifyAddEmp(string EmpCode, string Action);
        string OnGridModifyTransfer(string EmpCode, string Transfer);
        void OnLoadingEmpByDept(string Dept);
        void OnLoandCareerMovement();
        void OnLoandindEmployee();
        string Update(int ID);
    }
}
