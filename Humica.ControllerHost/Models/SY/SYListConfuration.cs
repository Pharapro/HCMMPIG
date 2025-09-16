
namespace Humica.Models.SY
{
    public class SYListConfuration
    {
        public static string ListDefaultView
        {
            get { return "~/Views/ListDefault/PartialList.cshtml"; }
        }

        public static string ListDefaultViewProcess
        {
            get { return "~/Views/ListDefault/PartialListProcess.cshtml"; }
        }

        public static string ListDefaultViewSelect
        {
            get { return "~/Views/ListDefault/PartialListSelect.cshtml"; }
        }

        public static string ListPivotSelectView
        {
            get { return "~/Views/ListDefault/PartialPivot.cshtml"; }
        }

        public static string ListRowSelectView
        {
            get { return "~/Views/ListDefault/PartialRow.cshtml"; }
        }

        public static string ListDefaultUpload
        {
            get { return "~/Views/ListDefault/UploadList.cshtml"; }
        }

        public static string FilterGeneralAccounting
        {
            get { return "~/Views/FILTER/FTGernalAccounting.cshtml"; }
        }

        public static string ReportingPathPartial
        {
            get { return "~/Views/ListReportingView/"; }
        }
        public static string ReportingPathView
        {
            get { return "~/Views/ListReportingView/ReportView.cshtml"; }
        }
        public static string ReportingList
        {
            get { return "~/Views/ListDefault/ReportList.cshtml"; }
        }

        public static string FilterGeneralPeriodReport
        {
            get { return "~/Views/FILTER/FTGeneralPeriodReport.cshtml"; }
        }

        public static string FilterDealerSelector
        {
            get { return "~/Views/ListReportingView/PartialDealerSearch.cshtml"; }
        }

        public static string FilterDPostalCode
        {
            get { return "~/Views/ListFilter/PartialPostalSearch.cshtml"; }
        }

    }
}