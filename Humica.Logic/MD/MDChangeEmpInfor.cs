using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic.MD
{
    public class MDChangeEmpInfor
    {
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string ChangeInforType { get; set; }
        public string FromCode { get; set; }
        public string ToCode { get; set; }
        public string EmpCode { get; set; }
        public List<HRStaffProfile> ListEmp { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public MDChangeEmpInfor()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<HRStaffProfile> LoadDataEmpGen(FTFilterEmployee Filter1, List<HRBranch> _ListBranch)
        {
            var _List = new List<HRStaffProfile>();
            var _listStaff = DBV.HRStaffProfiles.Where(w => w.Status == "A"
             && (string.IsNullOrEmpty(Filter1.Branch) || w.Branch == Filter1.Branch)
                           && (string.IsNullOrEmpty(Filter1.Division) || w.Division == Filter1.Division)
                           && (string.IsNullOrEmpty(Filter1.BusinessUnit) || w.GroupDept == Filter1.BusinessUnit)
                           && (string.IsNullOrEmpty(Filter1.Department) || w.DEPT == Filter1.Department)
                           && (string.IsNullOrEmpty(Filter1.Office) || w.Office == Filter1.Office)
                           && (string.IsNullOrEmpty(Filter1.Section) || w.SECT == Filter1.Section)
                           && (string.IsNullOrEmpty(Filter1.Group) || w.Groups == Filter1.Group)
                           && (string.IsNullOrEmpty(Filter1.Position) || w.JobCode == Filter1.Position)
                           && (string.IsNullOrEmpty(Filter1.Level) || w.LevelCode == Filter1.Level)
                           && (string.IsNullOrEmpty(Filter1.Category) || w.CATE == Filter1.Category)).ToList();
            _List = _listStaff.ToList();

            return _List.OrderBy(w => w.EmpCode).ToList();
        }
    }
}