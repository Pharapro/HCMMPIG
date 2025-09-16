using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Humica.Logic.MD
{
    public class MDOrgChart
    {
        IUnitOfWork unitOfWork;
        public static HumicaDBViewContext DBV = new HumicaDBViewContext();
        public FTFilterEmployee Filter { get; set; }
        public List<HR_OrgChart_View> ListOrg { get; set; }

        public MDOrgChart()
        {
            ListOrg = new List<HR_OrgChart_View>();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        public List<ClsOrgChart> LoadDatas(string Branch, string Department)
        {
            var ListPositonSt = unitOfWork.Repository<HRPositionStructure>().Queryable();

            //var relevantPositions = GetHierarchyWithLevels(ListPositonSt.ToList(), 37);
            //var relevantPositions = GetHierarchyWithLevels(ListPositonSt.ToList(), 1);
            var relevantPositions=GetAllHierarchiesWithLevels(ListPositonSt.ToList());
            var ListStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.Status == "A");
            ListStaff = ListStaff.Where(w => ListPositonSt.Where(x => x.Code == w.JobCode).Any());
            var ListEmpPosit = unitOfWork.Repository<HREmpMultiPosition>().Queryable();
            List<ClsOrgChart> ListOrgChart = new List<ClsOrgChart>();
            //ListPositonSt = ListPositonSt.Where(w => w.ID > 36).ToList();
            int ID = 0;
            int EmpCode = ListPositonSt.Max(w => w.ID);
            int Level = 0;
            string PostCode = "";
            Dictionary<string, string> ListEmpPost = new Dictionary<string, string>();
            foreach (var item in relevantPositions.OrderBy(w => w.Level).ThenBy(w => w.Code))
            {
                if (item.Level != 0 && Level == item.Level && PostCode == item.Code) continue;
                ID++;
                var ListReport = ListPositonSt.Where(w => w.Code == item.Code);
                var DataReport = ListStaff.Where(w => ListReport.Where(x => x.ID == w.PositionID).Any()).ToList();
                if (ListReport.Count() > 1)
                {
                    int count = ListReport.Count();
                    List<string> lstTemEmp = new List<string>();
                    int i = 0;
                    foreach (var R in ListReport)
                    {
                        bool IsContine = false;
                        i++;
                        if (count == i) IsContine = true;

                        var LstStaff = new List<HRStaffProfile>();
                        if (ListStaff.Where(w => w.PositionID == R.ID).Count() > 0)
                        {
                            LstStaff = ListStaff.Where(w => w.PositionID == R.ID).ToList();
                            LstStaff = LstStaff.Where(w => !lstTemEmp.Contains(w.EmpCode)).ToList();
                            if (LstStaff.Count == 0)
                            {
                                LstStaff = ListStaff.Where(w => w.JobCode == R.Code).ToList();
                            }
                        }
                        else
                        {
                            LstStaff = ListStaff.Where(w => w.JobCode == R.Code).ToList();
                            if (DataReport.Count() > 0)
                            {
                                LstStaff = LstStaff.Where(w => !DataReport.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                                if (LstStaff.Count == 0)
                                {
                                    LstStaff = ListStaff.Where(w => w.JobCode == R.Code).ToList();
                                }
                            }                            
                        }
                        LstStaff = LstStaff.Where(w => !lstTemEmp.Contains(w.EmpCode)).ToList();
                        if (LstStaff.Count > 0)
                        {
                            foreach (var emp in LstStaff)
                            {
                                if (ListEmpPost.Where(w => w.Key == emp.EmpCode && w.Value == item.Code).Any())
                                    continue;
                                ClsOrgChart orgChart = new ClsOrgChart();
                                SwapOrg(orgChart, emp, ID, R);
                                if (IsContine && LstStaff.Count() > 1)
                                {
                                    EmpCode++;
                                    orgChart.EmpCode = EmpCode.ToString();
                                }
                                orgChart.Branch = emp.Branch;
                                orgChart.Dept = emp.DEPT;
                                orgChart.Level = item.Level;
                                //if (!ListOrgChart.Where(w => w.ReportingManager == emp.EmpCode).Any())
                                //{
                                ListOrgChart.Add(orgChart);
                                //}
                                lstTemEmp.Add(emp.EmpCode);
                                ID++;
                                ListEmpPost.Add(emp.EmpCode, item.Code);
                                if (!IsContine)
                                    break;
                            }
                        }
                        else if (LstStaff.Count == 0)
                        {
                            ClsOrgChart orgChart = new ClsOrgChart();
                            HRStaffProfile emp = new HRStaffProfile();
                            SwapOrg(orgChart, emp, ID, R);
                            orgChart.Name = "ទំនេរ";
                            orgChart.Images = "";
                            orgChart.Level = item.Level;
                            var objDept = ListOrgChart.FirstOrDefault(w => w.EmpCode == item.ReportingID);
                            if (objDept != null)
                            {
                                orgChart.Dept = objDept.Dept;
                                orgChart.Branch = objDept.Branch;
                            }
                            ListOrgChart.Add(orgChart);
                            ID++;
                        }
                    }
                }
                else
                {
                    var LstStaff = new List<HRStaffProfile>();
                    if (ListStaff.Where(w => w.PositionID == item.ID).Count() > 0)
                    {
                        LstStaff = ListStaff.Where(w => w.PositionID == item.ID).ToList();
                        if (LstStaff.Count == 0)
                        {
                            LstStaff = ListStaff.Where(w => w.JobCode == item.Code).ToList();
                        }
                    }
                    else
                    {
                        LstStaff = ListStaff.Where(w => w.JobCode == item.Code).ToList();
                        if (DataReport.Count() > 0)
                        {
                            LstStaff = LstStaff.Where(w => !DataReport.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                            if (LstStaff.Count == 0)
                            {
                                LstStaff = ListStaff.Where(w => w.JobCode == item.Code).ToList();
                            }
                        }
                    }
                    if (LstStaff.Count > 0)
                    {
                        int j = 0;
                        foreach (var emp in LstStaff)
                        {
                            if (ListEmpPost.Where(w => w.Key == emp.EmpCode && w.Value == item.Code).Any())
                                continue;
                            ClsOrgChart orgChart = new ClsOrgChart();
                            SwapOrg(orgChart, emp, ID, item);
                            orgChart.Level = item.Level;
                            orgChart.Branch = emp.Branch;
                            orgChart.Dept = emp.DEPT;
                            if (j > 0 && LstStaff.Count > 1)
                            {
                                EmpCode++;
                                orgChart.EmpCode = EmpCode.ToString();
                            }
                            ListOrgChart.Add(orgChart);
                            j++;
                            ID++;
                            ListEmpPost.Add(emp.EmpCode, item.Code);
                        }
                    }
                    else if (LstStaff.Count == 0)
                    {
                        ClsOrgChart orgChart = new ClsOrgChart();
                        HRStaffProfile emp = new HRStaffProfile();
                        SwapOrg(orgChart, emp, ID, item);
                        orgChart.Name = "ទំនេរ";
                        orgChart.Images = "";
                        orgChart.Level = item.Level;
                        var objDept = ListOrgChart.FirstOrDefault(w => w.EmpCode == item.ReportingID);
                        if(objDept!=null)
                        {
                            orgChart.Dept = objDept.Dept;
                            orgChart.Branch = objDept.Branch;
                        }
                        ListOrgChart.Add(orgChart);

                        ID++;
                    }
                    //var ObjMP = new HREmpMultiPosition();
                    //bool IsMP = false;
                    //IsHOD = false;
                    //var ListEmp = ListStaff.Where(w => w.JobCode == item.Code).ToList();
                    //if (item.BaseHOD == true)
                    //{
                    //    IsHOD = true;
                    //    var emp = ListOrgChart.Where(w => w.EmpCode == item.ReportingID).ToList();
                    //    ListEmp = ListStaff.ToList().Where(w => emp.Where(x => x.ReportingManager == w.HODCode).Any()).ToList();
                    //}
                    //else if (!string.IsNullOrEmpty(item.ReportingID) && ListEmpPosit.Where(w => w.PositionID == item.ID).Any())
                    //{
                    //    var MP = ListEmpPosit.ToList().FirstOrDefault(w => w.PositionID == Convert.ToInt32(item.ID));
                    //    if (MP != null)
                    //    {
                    //        ObjMP = MP;
                    //        IsMP = true;
                    //        ListEmp = ListStaff.Where(w => w.EmpCode == MP.EmpCode).ToList();
                    //    }
                    //}
                    //else if (ListEmp.Count > 1)
                    //{
                    //    if (ListPositonSt.Where(w => w.Code == item.Code).Count() == ListEmp.Count())
                    //    {
                    //        if (ListEmp.Where(w => w.PositionID == item.ID).Any())
                    //        {
                    //            ListEmp = ListEmp.Where(w => w.PositionID == item.ID).ToList();
                    //        }
                    //        else
                    //        {

                    //        }
                    //        IsHOD = true;
                    //    }
                    //    //    ListEmp = ListStaff.Where(w => emp.Where(x => x.ReportingManager == w.HODCode).Any()).ToList();
                    //}

                    //if (ListEmp.Count() > 0)
                    //{
                    //    foreach (var emp in ListEmp)
                    //    {
                    //        ID++;
                    //        //if (ListOrgChart.Where(w => w.EmpCode == item.ID.ToString()).Any())
                    //        //{
                    //        //    ListOrgChart.Add(new ClsOrgChart()
                    //        //    {
                    //        //        ID = ID,
                    //        //        EmpCode = EmpCode.ToString(),
                    //        //        Name = emp.AllName,
                    //        //        Designation = item.Description,
                    //        //        ReportingID = item.ReportingID,
                    //        //        ReportingManager = emp.EmpCode,
                    //        //        Images = emp.Images,
                    //        //        Level = item.Level
                    //        //    });
                    //        //    EmpCode++;

                    //        //}
                    //        if (!ListOrgChart.Where(w => w.ReportingManager == emp.EmpCode).Any())
                    //        {
                    //            ListOrgChart.Add(new ClsOrgChart()
                    //            {
                    //                ID = ID,
                    //                EmpCode = item.ID.ToString(),
                    //                Name = emp.AllName,
                    //                Designation = item.Description,
                    //                ReportingID = item.ReportingID,
                    //                ReportingManager = emp.EmpCode,
                    //                Images = emp.Images,
                    //                Level = item.Level
                    //            });
                    //            if (IsHOD) break;
                    //        }
                    //        else if (IsMP)
                    //        {
                    //            ListOrgChart.Add(new ClsOrgChart()
                    //            {
                    //                ID = ID,
                    //                EmpCode = item.ID.ToString(),
                    //                Name = emp.AllName,
                    //                Designation = item.Description,
                    //                ReportingID = ObjMP.PositionStructureID.ToString(),
                    //                ReportingManager = emp.EmpCode,
                    //                Images = emp.Images,
                    //                Level = item.Level
                    //            });
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    ListOrgChart.Add(new ClsOrgChart()
                    //    {
                    //        ID = ID,
                    //        EmpCode = item.ID.ToString(),
                    //        Name = "ទំនេរ",
                    //        Designation = item.Description,
                    //        ReportingID = item.ReportingID,
                    //        Images = "",
                    //        Level = item.Level
                    //    });
                    //}
                }
                    PostCode = item.Code;
                Level = item.Level;
            }
            if(!string.IsNullOrEmpty(Department))
            {
                ListOrgChart = ListOrgChart.Where(w => w.Dept == Department).ToList();
            }
            return ListOrgChart;
        }
        public void SwapOrg(ClsOrgChart S,HRStaffProfile D,int ID,HRPositionStructure Post)
        {
            S.ID = ID;
            S.EmpCode = Post.ID.ToString();
            S.Name = D.AllName;
            S.Designation = Post.Description;
            S.ReportingID = Post.ReportingID;
            S.ReportingManager = D.EmpCode;
            S.Images = D.Images;
        }

        public List<ClsPositionNode> GetHierarchyWithLevels(List<HRPositionStructure> allPositions, int rootId)
        {
            var result = new List<ClsPositionNode>();
            TraverseWithLevel(allPositions, rootId, 0, result, new HashSet<int>());
            //return allPositions.Where(w => result.Where(x => x.ID == w.ID).Any()).ToList();
            return result;

        }
        private void TraverseWithLevel(List<HRPositionStructure> positions, int currentId, int level, List<ClsPositionNode> result, HashSet<int> visited)
        {
            if (visited.Contains(currentId)) return;

            visited.Add(currentId);
            result.Add(new ClsPositionNode
            {
                ID = currentId,
                Code = positions.FirstOrDefault(p => p.ID == currentId)?.Code,
                Description = positions.FirstOrDefault(p => p.ID == currentId)?.Description,
                BaseHOD = positions.FirstOrDefault(p => p.ID == currentId)?.BaseHOD,
                ReportingID = positions.FirstOrDefault(p => p.ID == currentId)?.ReportingID,
                Level = level
            });

            var children = positions
                .Where(p => p.ReportingID == currentId.ToString())
                .Select(p => p.ID)
                .ToList();

            foreach (var childId in children)
            {
                TraverseWithLevel(positions, childId, level + 1, result, visited);
            }
        }

        public List<HRPositionStructure> GetReportingHierarchy(List<HRPositionStructure> positions, int rootId, int depth = 5)
        {
            var result = new HashSet<int>();
            TraverseHierarchy(positions, rootId, depth, result);

            return positions.Where(p => result.Contains(p.ID)).ToList();
        }
        private void TraverseHierarchy(List<HRPositionStructure> positions, int currentId, int depth, HashSet<int> result)
        {
            if (depth == 0) return;

            // Get direct children of the current ID or anyone reporting to it
            var children = positions
                .Where(p => p.ReportingID == currentId.ToString() || p.ID == currentId)
                .Select(p => p.ID)
                .ToList();

            foreach (var childId in children)
            {
                if (!result.Contains(childId))
                {
                    result.Add(childId);
                    TraverseHierarchy(positions, childId, depth - 1, result);
                }
            }
        }
        public List<ClsPositionNode> GetAllHierarchiesWithLevels(List<HRPositionStructure> allPositions)
        {
            var result = new List<ClsPositionNode>();
            var visited = new HashSet<int>();

            var rootNodes = allPositions
                .Where(p => p.ReportingID == null || !allPositions.Any(ap => ap.ID.ToString() == p.ReportingID))
                .ToList();

            foreach (var root in rootNodes)
            {
                TraverseWithLevel(allPositions, root.ID, 0, result, visited);
            }

            return result;
        }
    }
    public class ClsOrgChart
    {
        public int ID { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ReportingID { get; set; }
        public string ReportingManager { get; set; }
        public string Images { get; set; }
        public int Level { get; set; }
        public string Branch { get; set; }
        public string Dept { get; set; }
    }

    public class ClsPositionNode : HRPositionStructure
    {
        public int Level { get; set; }
    }
}