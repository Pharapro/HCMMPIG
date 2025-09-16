using Humica.Core.DB;
using Humica.Core.FT;
using System;
using System.Collections.Generic;

namespace Humica.Logic.Inquiry
{
    public class ClsInqAttendance
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
        public List<VIEW_ATEmpSchedule> List_EmpSchedule { get; set; }

        public void SetDataReportHRPayHistoryFilter()
        {
            //// Branch
            //List<SYDocumentNo> filterData = new SYSplitItem(FilterReport.Branch, SplitString).ListDocument;
            //if (filterData != null)
            //{
            //    foreach (CRMContactView read in ListContact.OrderBy(w => w.Branch))
            //    {
            //        List<SYDocumentNo> checkList = filterData.Where(w => w.DocumentNo == read.Branch).ToList();
            //        if (checkList.Count == 0)
            //        {
            //            _ = ListContact.Remove(read);
            //        }
            //    }
            //}

            //// PIC
            //filterData = new SYSplitItem(FilterReport.StaffID, SplitString).ListDocument;
            //if (filterData != null)
            //{
            //    foreach (CRMContactView read in ListContact.OrderBy(w => w.Owner))
            //    {
            //        List<SYDocumentNo> checkList = filterData.Where(w => w.DocumentNo == read.Owner).ToList();
            //        if (checkList.Count == 0)
            //        {
            //            _ = ListContact.Remove(read);
            //        }
            //    }
            //}
        }
    }
}
