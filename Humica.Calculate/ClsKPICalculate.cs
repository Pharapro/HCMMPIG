using Humica.Core.DB;
using Humica.Core.Helper;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Calculate
{
    public class ClsKPICalculate
    {
        private IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsKPICalculate()
        {
            OnLoad();
        }

        public HRKPIAssignHeader Update_Start_PA(IUnitOfWork _unitOfWork, string ID,DateTime StartDate)
        {
            HRKPIAssignHeader KPI = new HRKPIAssignHeader();
            string Released = SYDocumentStatus.RELEASED.ToString();
            KPI = _unitOfWork.Repository<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ID);
            if (KPI != null)
            {
                KPI.ExpectedDate = StartDate;
                KPI.ReStatus = Released;
            }
            return KPI;
        }
        public List<HRKPITracking> Get_DataTracking(string AssignCode)
        {
            string Open = SYDocumentStatus.PENDING.ToString();
            var ListKPI = unitOfWork.Repository<HRKPITracking>().Queryable().Where(w => w.AssignCode == AssignCode && w.Status == Open);
            return ListKPI.ToList();
        }

        //public string Get_GradeKPI(decimal Score)
        //{
        //    string Grade = "";
        //    var ListKPIGrade = unitOfWork.Set<HRKPIGrade>().Where(w => w.FromScore <= Score && w.ToScore > Score).ToList();
        //    foreach (var G in ListKPIGrade.OrderBy(w => w.Grade))
        //    {
        //        Grade = G.Grade;
        //    }
        //    return Grade;
        //}

        public string Get_GradeKPI(decimal Score)
        {
            var grade = unitOfWork.Set<HRKPIGrade>()
                                  .FirstOrDefault(w => w.FromScore <= Score && w.ToScore >= Score);
            return grade?.Grade ?? ""; // Returns the grade or an empty string if no match is found.
        }

        public HRKPIYearlyTracking Calculate_KPI(HRKPIAssignItem KPI, List<HRKPITracking> ListTrack)
        {
            HRKPIYearlyTracking YearlyTracking = new HRKPIYearlyTracking
            {
                AssignCode = KPI.AssignCode,
                ItemCode = KPI.ItemCode,
                KPI = KPI.KPI,
                Indicator = KPI.Indicator,
                Weight = KPI.Weight,
                Target = KPI.Target,
                Method = KPI.Options
            };
            var MethodMap = new Dictionary<string, Action<decimal>>
            {
                { "AVERAGE", val => YearlyTracking.Method = "Average" },
                { "SUM", val => YearlyTracking.Method = "Sum" },
                { "Latest", val => YearlyTracking.Method = "Latest" },
            };
            var monthMap = new Dictionary<string, Action<decimal>>
            {
                { "JAN", val => YearlyTracking.JAN = val },
                { "FEB", val => YearlyTracking.FEB = val },
                { "MAR", val => YearlyTracking.MAR = val },
                { "APR", val => YearlyTracking.APR = val },
                { "MAY", val => YearlyTracking.MAY = val },
                { "JUN", val => YearlyTracking.JUN = val },
                { "JUL", val => YearlyTracking.JUL = val },  // Fixed duplicate "JUN" issue
                { "AUG", val => YearlyTracking.AUG = val },
                { "SEP", val => YearlyTracking.SEP = val },
                { "OCT", val => YearlyTracking.OCT = val },
                { "NOV", val => YearlyTracking.NOV = val },
                { "DEC", val => YearlyTracking.DECE = val }  // Fixed "DECE" typo
            };


            foreach (var group in ListTrack.GroupBy(w => w.DocumentDate.ToString("MMM").ToUpper()))
            {
                decimal Amount = 0;
                var lstTracking = group.ToList();
                if (KPI.Options == "AVERAGE")
                {
                    Amount = lstTracking.Where(w => w.Actual > 0).Any()
                ? ClsRounding.Rounding(lstTracking.Where(w => w.Actual > 0).Average(w => w.Actual), 4, "N")
                : 0;
                }
                else if (KPI.Options == "Latest")
                {
                    Amount = lstTracking.OrderByDescending(w => w.DocumentDate).FirstOrDefault().Actual;
                }
                else
                {
                    Amount = lstTracking.Sum(w => w.Actual);
                }
                if (monthMap.ContainsKey(group.Key))
                {
                    monthMap[group.Key](Amount);
                }
            }
            switch (YearlyTracking.Method)
            {
                case "AVERAGE": YearlyTracking.Method = "Average"; break;
                case "Latest": YearlyTracking.Method = "Latest"; break;
                case "SUM": YearlyTracking.Method = "Sum"; break;
            }
            return YearlyTracking;
        }

        public List<HRKPIMonthlyTracking> Cal_KPI_Monthly(HRKPIAssignHeader HKPI, DateTime MaxMonth,
            HRKPIAssignItem KPI, List<HRKPITracking> ListTrack)
        {
            List<HRKPIMonthlyTracking> LstMonthly = new List<HRKPIMonthlyTracking>();
            for (DateTime date = HKPI.PeriodFrom.Value; date <= MaxMonth; date = date.AddMonths(1))
            {
                int month = date.Month;
                var monthlyTrackingDict = new Dictionary<int, HRKPIMonthlyTracking>();
                if (!monthlyTrackingDict.ContainsKey(month))
                {
                    monthlyTrackingDict[month] = new HRKPIMonthlyTracking
                    {
                        AssignCode = KPI.AssignCode,
                        ItemCode = KPI.ItemCode,
                        KPI = KPI.KPI,
                        Indicator = KPI.Indicator,
                        Weight = KPI.Weight,
                        Target = KPI.Target,
                        Method = KPI.Options,
                        InMonth = month
                    };
                }
                var _lstTemTacking = ListTrack.Where(w => w.DocumentDate.Month == date.Month).ToList();
                foreach (var group in _lstTemTacking.GroupBy(w => new { w.DocumentDate.Day }))
                {
                    int day = group.Key.Day;
                    var YearlyTracking = monthlyTrackingDict[month];
                    decimal Amount = 0;
                    var lstTracking = group.ToList();
                    Amount = Calcu_Method(KPI.Options, lstTracking);
                    //if (KPI.Options == "AVERAGE")
                    //{
                    //    Amount = lstTracking.Where(w => w.Actual > 0).Any()
                    //        ? ClsRounding.Rounding(lstTracking.Where(w => w.Actual > 0).Average(w => w.Actual), 4, "N")
                    //        : 0;
                    //}
                    //else if (KPI.Options == "Latest")
                    //{
                    //    Amount = lstTracking.OrderByDescending(w => w.DocumentDate).FirstOrDefault()?.Actual ?? 0;
                    //}
                    //else // SUM
                    //{
                    //    Amount = lstTracking.Sum(w => w.Actual);
                    //}

                    // Assign the value to the correct day (D1 - D31)
                    switch (day)
                    {
                        case 1: YearlyTracking.D1 = Amount; break;
                        case 2: YearlyTracking.D2 = Amount; break;
                        case 3: YearlyTracking.D3 = Amount; break;
                        case 4: YearlyTracking.D4 = Amount; break;
                        case 5: YearlyTracking.D5 = Amount; break;
                        case 6: YearlyTracking.D6 = Amount; break;
                        case 7: YearlyTracking.D7 = Amount; break;
                        case 8: YearlyTracking.D8 = Amount; break;
                        case 9: YearlyTracking.D9 = Amount; break;
                        case 10: YearlyTracking.D10 = Amount; break;
                        case 11: YearlyTracking.D11 = Amount; break;
                        case 12: YearlyTracking.D12 = Amount; break;
                        case 13: YearlyTracking.D13 = Amount; break;
                        case 14: YearlyTracking.D14 = Amount; break;
                        case 15: YearlyTracking.D15 = Amount; break;
                        case 16: YearlyTracking.D16 = Amount; break;
                        case 17: YearlyTracking.D17 = Amount; break;
                        case 18: YearlyTracking.D18 = Amount; break;
                        case 19: YearlyTracking.D19 = Amount; break;
                        case 20: YearlyTracking.D20 = Amount; break;
                        case 21: YearlyTracking.D21 = Amount; break;
                        case 22: YearlyTracking.D22 = Amount; break;
                        case 23: YearlyTracking.D23 = Amount; break;
                        case 24: YearlyTracking.D24 = Amount; break;
                        case 25: YearlyTracking.D25 = Amount; break;
                        case 26: YearlyTracking.D26 = Amount; break;
                        case 27: YearlyTracking.D27 = Amount; break;
                        case 28: YearlyTracking.D28 = Amount; break;
                        case 29: YearlyTracking.D29 = Amount; break;
                        case 30: YearlyTracking.D30 = Amount; break;
                        case 31: YearlyTracking.D31 = Amount; break;
                    }
                }
                switch (monthlyTrackingDict[month].Method)
                {
                    case "AVERAGE": monthlyTrackingDict[month].Method = "Average"; break;
                    case "Latest": monthlyTrackingDict[month].Method = "Latest"; break;
                    case "SUM": monthlyTrackingDict[month].Method = "Sum"; break;
                }
                monthlyTrackingDict[month].Actual = Calcu_Method(KPI.Options, _lstTemTacking);
                LstMonthly.AddRange(monthlyTrackingDict.Values.ToList());
            }
            return LstMonthly;
        }
        public decimal Calcu_Method(string Method, List<HRKPITracking> ListTrack)
        {
            decimal total = 0;
            if (Method == "AVERAGE")
            {
                total = ListTrack.Where(w => w.Actual > 0).Any()
                    ? ClsRounding.Rounding(ListTrack.Where(w => w.Actual > 0).Average(w => w.Actual), 4, "N")
                    : 0;
            }
            else if (Method == "Latest")
            {
                total = ListTrack.OrderByDescending(w => w.DocumentDate).FirstOrDefault()?.Actual ?? 0;
            }
            else // SUM
            {
                total = ListTrack.Sum(w => w.Actual);
            }
            return total;
        }
    }
}
