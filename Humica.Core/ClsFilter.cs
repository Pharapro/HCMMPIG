
using System.Collections.Generic;

namespace Humica.Core
{
    public class ClsFilterGeneral
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public static List<ClsFilterGeneral> OnLoadDataValue()
        {
            return new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "AMOUNT", Description = "Amount" },
                new ClsFilterGeneral { Code = "RATE", Description = "Rate" },
            };
        }
        public static List<ClsFilterGeneral> OnLoadTaxBase()
        {
            return new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "Monthly", Description = "Monthly" },
                new ClsFilterGeneral { Code = "Yearly", Description = "Yearly" },
            };
        }
        public static List<ClsFilterGeneral> OnLoadRewardType()
        {
            List<ClsFilterGeneral> _list = new List<ClsFilterGeneral>();
            _list.Add(new ClsFilterGeneral { Code = "ALLW", Description = "Allowance" });
            _list.Add(new ClsFilterGeneral { Code = "DED", Description = "Deduction" });
            _list.Add(new ClsFilterGeneral { Code = "BON", Description = "Bonus" });
            return _list;
        }
        public static List<ClsFilterGeneral> OnLoadTaxType()
        {
            List<ClsFilterGeneral> _list = new List<ClsFilterGeneral>();
            _list.Add(new ClsFilterGeneral { Code = "TX-001", Description = "Taxable Income" });
            _list.Add(new ClsFilterGeneral { Code = "TX-002", Description = "Fringe Benefit Tax" });
            return _list;
        }
        public static List<ClsFilterGeneral> LoadDataSalaryType()
        {
            List<ClsFilterGeneral> _lst = new List<ClsFilterGeneral>();
            _lst.Add(new ClsFilterGeneral { Code = "BS", Description = "Basic Salary" });
            _lst.Add(new ClsFilterGeneral { Code = "NP", Description = "Total Taxable Income" });
            _lst.Add(new ClsFilterGeneral { Code = "GP", Description = "Gross Pay" });
            return _lst;
        }
        public static List<ClsFilterGeneral> LoadDataBalanceType()
        {
            List<ClsFilterGeneral> _lst = new List<ClsFilterGeneral>();
            _lst.Add(new ClsFilterGeneral { Code = "CB", Description = "Current Balance" });
            _lst.Add(new ClsFilterGeneral { Code = "QB", Description = "Quarterly Balance" });
            _lst.Add(new ClsFilterGeneral { Code = "SB", Description = "Semester Balance" });
            return _lst;
        }
        public static List<ClsFilterGeneral> LoadDataLeavePolicy()
        {
            List<ClsFilterGeneral> _lst = new List<ClsFilterGeneral>();
            _lst.Add(new ClsFilterGeneral { Code = "NEWJOIN", Description = "New Join" });
            _lst.Add(new ClsFilterGeneral { Code = "RESIGN", Description = "Resignation" });
            return _lst;
        }
        public static List<ClsFilterGeneral> LoadDataOTCategory()
        {
            var ListOTCategory = new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "OT", Description = "Overtime" },
                new ClsFilterGeneral { Code = "OBS", Description = "OT Before Shift"},
                new ClsFilterGeneral { Code = "OBT", Description = "OT Break Time" },
                new ClsFilterGeneral { Code = "PH", Description = "PH or OFF" }
            };
            return ListOTCategory;
        }
        public static List<ClsFilterGeneral> LoadDataKPI_Status()
        {
            var ListOTCategory = new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "KEEP", Description = "KPI Keep" },
                new ClsFilterGeneral { Code = "CLOSED", Description = "KPI Closed"},
                //new ClsFilterGeneral { Code = "TRANSFER", Description = "KPI Transfer" },
                new ClsFilterGeneral { Code = "CHANGED", Description = "KPI Changed" }
            };
            return ListOTCategory;
        }
        public static List<ClsFilterGeneral> LoadData_RoleMapping()
        {
            return new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "HODCode", Description = "Head of Department" },
                new ClsFilterGeneral { Code = "FirstLine", Description = "First Line" },
                new ClsFilterGeneral { Code = "FirstLine2", Description = "First Line 2" },
                new ClsFilterGeneral { Code = "SecondLine", Description = "Second Line" },
                new ClsFilterGeneral { Code = "SecondLine2", Description = "Second Line 2" },
                new ClsFilterGeneral { Code = "OTFirstLine", Description = "OT First Line" },
                new ClsFilterGeneral { Code = "OTSecondLine", Description = "OT Second Line" },
                new ClsFilterGeneral { Code = "OTthirdLine", Description = "OT Third Line" },
                new ClsFilterGeneral { Code = "APPAppraisal", Description = "Appraisal 1" },
                new ClsFilterGeneral { Code = "APPAppraisal2", Description = "Appraisal 2" },
                new ClsFilterGeneral { Code = "APPEvaluator", Description = "Evaluator" },
                new ClsFilterGeneral { Code = "APPTracking", Description = "Tracking" }
            };
        }
        public static List<ClsFilterGeneral> LoadDataLevel_Eval()
        {
            var ListOTCategory = new List<ClsFilterGeneral>
            {
                new ClsFilterGeneral { Code = "0", Description = "Self Eva" },
                new ClsFilterGeneral { Code = "1", Description = "1st EV"},
                new ClsFilterGeneral { Code = "2", Description = "2nd EV" }
            };
            return ListOTCategory;
        }
    }
}
