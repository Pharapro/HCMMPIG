using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic
{
    public interface IClsFilter : IClsApplication
    {
        List<HRStaffProfile> OnLoadStaffByHOD();
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        
    }
    public class ClsFilter : IClsFilter
    {
        IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsFilter()
        {
            OnLoad();
        }
        public List<HRBranch> LoadBranch()
        {
            var ListTemp = unitOfWork.Set<HRBranch>().ToList();
            return ListTemp;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("BRANCHES_SELECT", SYConstant.getBranchDataAccess());
            keyValues.Add("DIVISION_SELECT", unitOfWork.Set<HRDivision>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("DEPARTMENT_SELECT", unitOfWork.Set<HRDepartment>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("SECTION_SELECT", unitOfWork.Set<HRSection>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("POSITION_SELECT", unitOfWork.Set<HRPosition>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("LEVEL_SELECT", SYConstant.getLevelDataAccess());

            return keyValues;
        }
        public List<HRStaffProfile> OnLoadStaffByHOD()
        {
            OnLoad();
            List<HRStaffProfile> ListTemp = new List<HRStaffProfile>();
            var User = SYSession.getSessionUser();
            ListTemp = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A" && (w.HODCode == User.UserName || w.FirstLine == User.UserName
            || w.SecondLine == User.UserName || w.FirstLine2 == User.UserName || w.SecondLine2 == User.UserName)
            || w.EmpCode == User.UserName).ToList();

            return ListTemp;
        }
        public static List<HRGroupDepartment> LoadBusinessUnit()
        {
            return new HumicaDBContext().HRGroupDepartments.ToList();
        }
        public static List<HRDivision> LoadDivision()
        {
            return new HumicaDBContext().HRDivisions.Where(w => w.IsActive == true).ToList();
        }
        public static List<HRDepartment> LoadDepartment()
        {
            return new HumicaDBContext().HRDepartments.Where(w => w.IsActive == true).ToList();
        }
        public static List<HROffice> LoadOffice()
        {
            return new HumicaDBContext().HROffices.Where(w => w.IsActive == true).ToList();
        }
        public static List<HRSection> LoadSection()
        {
            return new HumicaDBContext().HRSections.Where(w => w.IsActive == true).ToList();
        }
        public static List<HRGroup> LoadGroups()
        {
            return new HumicaDBContext().HRGroups.Where(w => w.IsActive == true).ToList();
        }
        public static List<HRPosition> LoadPosition()
        {
            return new HumicaDBContext().HRPositions.Where(w => w.IsActive == true).ToList();
        }
        public static List<HRLocation> LoadLocation()
        {
            return new HumicaDBContext().HRLocations.Where(w => w.IsActive == true).ToList();
        }
        public static string GetDepartment_Description(string Department)
        {
            string text = "";
            var Data = new HumicaDBContext().HRDepartments.FirstOrDefault(w => w.Code == Department);
            if (Data != null) text = Data.Description;
            return text;
        }
        public static string GetSYData(string DropdownType, string Value)
        {
            string text = "";
            var Data = new SMSystemEntity().SYDatas.FirstOrDefault(w => w.DropDownType == DropdownType && w.SelectValue == Value);
            if (Data != null) text = Data.SelectText;
            return text;
        }
    }
}
