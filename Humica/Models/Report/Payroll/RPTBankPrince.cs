namespace Humica.Models.Report.Payroll
{
    public partial class RPTBankPrince : DevExpress.XtraReports.UI.XtraReport
    {
        public RPTBankPrince()
        {
            InitializeComponent();
        }
        private void lblNetText_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string Total = "";
            //if (GetCurrentColumnValue("Total") == null)
            //{
            //    Total = "";
            //}
            //else
            //{
            //    Total = CONVERT_TO_LETTER_ENG(GetCurrentColumnValue("Total").ToString());
            //}
            //lblNetText.Text = Total;
        }
    }
}
