using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.Inquiry
{
    public class ClsInqLeave
    {
        public char SplitString = ',';
        public HumicaDBViewContext DB = new HumicaDBViewContext();
        public FTFilterEmployee Filter { get; set; }
        public string TemplateType { get; set; }
        public string CompanyCode { get; set; }
        public string Branch { get; set; }
        public string ScreenID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ClsSalary> ListSalary { get; set; }

        public async Task SetDataReportHRPayHistoryFilter()
        {
            ListSalary = new List<ClsSalary>();
            var Staff =await DB.HRStaffProfiles.ToListAsync();
            var Salary = await DB.HISGenSalaries.Where(w => w.ToDate >= Filter.FromDate
            && w.ToDate <= Filter.ToDate).ToListAsync();
            int Inorder = 1;
            Salary = Salary.Where(w => Staff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            foreach (var item in Salary.OrderBy(w=>w.FromDate))
            {
                var obj = new ClsSalary();
                obj.EmpCode = item.EmpCode;
                obj.EmployeeName = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).AllName;
                obj.InYear = item.ToDate.Value.ToString("MMMyy");
                obj.FromDate = item.FromDate.Value;
                obj.ToDate = item.ToDate.Value;
                obj.Salary = item.NetWage;
                obj.GroupNo = 1;
                obj.Order = Inorder;
                if (!ListSalary.Where(w => w.InYear == obj.InYear).Any())
                {
                    Inorder++;
                }
                else if (ListSalary.Where(w => w.InYear == obj.InYear).Any())
                {
                    obj.Order = ListSalary.FirstOrDefault(w => w.InYear == obj.InYear).Order;
                }
                ListSalary.Add(obj);
            }

            var groupedByEmpCode = ListSalary.GroupBy(item => item.EmpCode)
                                             .Select(group => new
                                             {
                                                 EmpCode = group.Key,
                                                 TotalSalary = group.Sum(item => item.Salary ?? 0),
                                                 CountMonth = group.Count()
                                             }).ToList();
            foreach(var item in groupedByEmpCode)
            {
                var obj = new ClsSalary();
                obj.EmpCode = item.EmpCode;
                obj.EmployeeName = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).AllName;
                obj.InYear = "Total";
                obj.Salary = item.TotalSalary;
                obj.GroupNo = 2;
                obj.Order = Inorder;
                ListSalary.Add(obj);
            }
            Inorder += 1;
            foreach (var item in groupedByEmpCode)
            {
                var obj = new ClsSalary();
                obj.EmpCode = item.EmpCode;
                obj.EmployeeName = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).AllName;
                obj.InYear = "Average";
                obj.Salary = ClsRounding.RoundNormal(item.TotalSalary / item.CountMonth, 2);
                obj.GroupNo = 3;
                obj.Order = Inorder;
                ListSalary.Add(obj);
            }
            Inorder += 1;
            foreach (var item in groupedByEmpCode)
            {
                var obj = new ClsSalary();
                obj.EmpCode = item.EmpCode;
                obj.EmployeeName = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).AllName;
                obj.InYear = "Avg 50%/month";
                obj.Salary = ClsRounding.RoundNormal(item.TotalSalary / item.CountMonth, 2) * 0.5M;
                obj.GroupNo = 4;
                obj.Order = Inorder;
                ListSalary.Add(obj);
            }
        }
    }
    public class ClsSalary
    {
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string InYear { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Total { get; set; }
        public int GroupNo { get; set; }
        public int Order { get; set; }
    }
}
