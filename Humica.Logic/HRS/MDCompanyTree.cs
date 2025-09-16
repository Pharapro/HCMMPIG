using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humica.Logic.HRS
{
    public class MDCompanyTree
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public List<HRCompanyGroup> ListCompanyGroup { get; set; }
        public List<HRCompanyTree> ListCompanyTree { get; set; }
        public List<HRPositionStructure> ListPositionStructure { get; set; }

        public MDCompanyTree()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public static IEnumerable<HR_CompanyGroup_View> GetCompanyGroupItem()
        {
            HumicaDBViewContext DBB = new HumicaDBViewContext();
            List<HR_CompanyGroup_View> HRList = new List<HR_CompanyGroup_View>();
            if (HttpContext.Current.Session["CompanyGroupGroup"] != null)
            {
                string CompanyGroup = HttpContext.Current.Session["CompanyGroupGroup"].ToString();
                if (CompanyGroup != null && CompanyGroup != "")
                {
                    HRList = DBB.HR_CompanyGroup_View.Where(w => w.CompanyGroup == CompanyGroup).ToList();
                }
            }
            return HRList;
        }
        
        public static IEnumerable<HRCompanyTree> GetParentWorkGroup()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRCompanyTree> HRListTree = new List<HRCompanyTree>();
            if (HttpContext.Current.Session["ParentWorkGroup"] != null)
            {
                string ParentWorkGroup = HttpContext.Current.Session["ParentWorkGroup"].ToString();
                if (ParentWorkGroup != null && ParentWorkGroup != "")
                {
                    var Com = DBB.HRCompanyGroups.FirstOrDefault(w => w.WorkGroup == ParentWorkGroup);
                    //HRList = DBB.HRCompanyTrees.Where(w => w.WorkGroup == ParentWorkGroup).ToList();
                   var _HRList = (from Group in DBB.HRCompanyGroups
                              join tree in DBB.HRCompanyTrees on Group.WorkGroup equals tree.WorkGroup
                              where Group.Level == Com.Level - 1
                              select tree);
                    HRListTree = _HRList.ToList();
                }
            }
            return HRListTree;
        }
        public static IEnumerable<HRCompanyTree> GetSubParentWork()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRCompanyTree> HRListTree = new List<HRCompanyTree>();
            if (HttpContext.Current.Session["ParentWorkGroup"] != null)
            {
                string ParentWorkGroup = HttpContext.Current.Session["ParentWorkGroup"].ToString();
                if (ParentWorkGroup != null && ParentWorkGroup != "")
                {
                    var Com = DBB.HRCompanyGroups.FirstOrDefault(w => w.WorkGroup == ParentWorkGroup);
                    var _HRList = (from Group in DBB.HRCompanyGroups
                                   join tree in DBB.HRCompanyTrees on Group.WorkGroup equals tree.WorkGroup
                                   where Group.Level == Com.Level
                                   select tree);
                    foreach (var item in _HRList)
                    {
                        if (!HRListTree.Where(w => w.CompanyMember == item.CompanyMember).Any())
                        {
                            HRListTree.Add(item);
                        }
                    }
                }
            }
            return HRListTree;
        }
        public static IEnumerable<HRPositionStructure> GetParentPosition()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRPositionStructure> HRListTree = new List<HRPositionStructure>();
            // //if (HttpContext.Current.Session["ParentPosition"] != null)
            // //{
            //     string ParentWorkGroup = HttpContext.Current.Session["ParentPosition"].ToString();
            //     if (!string.IsNullOrEmpty(ParentWorkGroup))
            //     {
            //         HRListTree = DBB.HRPositionStructures.ToList();
            //     }
            //// }
            HRListTree = DBB.HRPositionStructures.ToList();
            return HRListTree;
        }
        public static string GetDescription(string CompanyGroup, string Menmber)
        {
            string Des = "";
            HumicaDBViewContext DBB = new HumicaDBViewContext();
            var Match = DBB.HR_CompanyGroup_View.FirstOrDefault(w => w.CompanyGroup == CompanyGroup && w.Code == Menmber);
            if (Match != null) { Des = Match.Description; }
            return Des;
        }
        public List<HR_OrgChart_View> LoadDatas()
        {
            using (var DBV = new HumicaDBViewContext())
            {
                var Listdata = DB.HRCompanyTrees.ToList();
                List<HR_OrgChart_View> _list = new List<HR_OrgChart_View>();
                foreach (var item in Listdata)
                {
                    _list.Add(new HR_OrgChart_View()
                    {
                        EmpCode = item.CompanyMember,
                        Name=item.WorkGroup,
                        Designation=item.CompanyMemberDesc,
                        ReportingID=item.ParentWorkGroupID,
                    });
                }
                var reportingIds = _list.Select(x => x.ReportingID).Distinct().ToList();

                var hods = DBV.HR_OrgChart_View
                    .Where(h => reportingIds.Contains(h.EmpCode))
                    .ToList();

                // Combine the filtered staff with HODs
                var combinedList = _list.Union(hods).Distinct().ToList(); // Use Union to ensure HODs are included

                return combinedList;
            }
        }
        public List<HR_OrgChart_View> LoadDataPosition()
        {
            using (var DBV = new HumicaDBViewContext())
            {
                var Listdata = DBV.HRPositionStructures.ToList();
                List<HR_OrgChart_View> _list = new List<HR_OrgChart_View>();
                bool IsHOD = false;
                foreach (var item in Listdata)
                {
                    IsHOD = false;
                    _list.Add(new HR_OrgChart_View()
                    {
                        EmpCode = item.ID.ToString(),
                        Designation = item.Description,
                        ReportingID = item.ReportingID,
                    });
                }
                //var reportingIds = _list.Select(x => x.ReportingID).Distinct().ToList();

                //var hods = DBV.HR_OrgChart_View
                //    .Where(h => reportingIds.Contains(h.EmpCode))
                //    .ToList();

                //// Combine the filtered staff with HODs
                //var combinedList = _list.Union(hods).Distinct().ToList(); // Use Union to ensure HODs are included

                return _list;
            }
        }
    }
    public class ClsTbableName
    {
        public string TableName { get; set; }
        public string WorkGroup { get; set; }
        public static List<ClsTbableName> LoadDataGroup()
        {
            List<ClsTbableName> _list = new List<ClsTbableName>();
            _list.Add(new ClsTbableName() {TableName= "HRBranch", WorkGroup="Branch" });
            _list.Add(new ClsTbableName() {TableName= "HRDivision", WorkGroup="Division" });
            _list.Add(new ClsTbableName() {TableName= "HRGroupDepartment", WorkGroup="GroupDepartment" });
            _list.Add(new ClsTbableName() {TableName= "HRDepartment", WorkGroup="Department" });
            _list.Add(new ClsTbableName() {TableName= "HROffice", WorkGroup = "Office" });
            _list.Add(new ClsTbableName() {TableName= "HRPosition", WorkGroup= "Position" });
            _list.Add(new ClsTbableName() {TableName= "HRGroup", WorkGroup = "Team" });
            _list.Add(new ClsTbableName() {TableName= "HRSection", WorkGroup= "Section" });
            _list.Add(new ClsTbableName() {TableName= "HRLevel", WorkGroup="Level" });
            _list.Add(new ClsTbableName() {TableName= "HRJobGrade", WorkGroup= "JobGrade" });

            return _list;
        }
    }
}