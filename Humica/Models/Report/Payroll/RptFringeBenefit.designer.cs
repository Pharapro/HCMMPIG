namespace HUMICA.Models.Report.Payroll
{
    partial class RptFringeBenefit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptFringeBenefit));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.label3 = new DevExpress.XtraReports.UI.XRLabel();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
            this.Division = new DevExpress.XtraReports.Parameters.Parameter();
            this.Department = new DevExpress.XtraReports.Parameters.Parameter();
            this.Section = new DevExpress.XtraReports.Parameters.Parameter();
            this.Position = new DevExpress.XtraReports.Parameters.Parameter();
            this.Level = new DevExpress.XtraReports.Parameters.Parameter();
            this.InMonth = new DevExpress.XtraReports.Parameters.Parameter();
            this.Company = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 20F;
            this.TopMargin.Name = "TopMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
            this.Detail.HeightF = 28.33333F;
            this.Detail.Name = "Detail";
            // 
            // table2
            // 
            this.table2.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table2.Name = "table2";
            this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(1130F, 28.33333F);
            this.table2.StylePriority.UseFont = false;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.tableCell29,
            this.tableCell30,
            this.tableCell35,
            this.tableCell36,
            this.tableCell48,
            this.tableCell49,
            this.tableCell50,
            this.tableCell52,
            this.tableCell65});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1.2D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell2.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
            this.xrTableCell2.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.xrTableCell2.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseForeColor = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell2.Summary = xrSummary1;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.2152149791000032D;
            // 
            // tableCell29
            // 
            this.tableCell29.BackColor = System.Drawing.Color.Transparent;
            this.tableCell29.BorderColor = System.Drawing.Color.Black;
            this.tableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")});
            this.tableCell29.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.tableCell29.ForeColor = System.Drawing.Color.Black;
            this.tableCell29.Multiline = true;
            this.tableCell29.Name = "tableCell29";
            this.tableCell29.StylePriority.UseBackColor = false;
            this.tableCell29.StylePriority.UseBorderColor = false;
            this.tableCell29.StylePriority.UseBorders = false;
            this.tableCell29.StylePriority.UseFont = false;
            this.tableCell29.StylePriority.UseForeColor = false;
            this.tableCell29.StylePriority.UseTextAlignment = false;
            this.tableCell29.Text = "Associate ID Number ";
            this.tableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell29.Weight = 0.35223548201702948D;
            // 
            // tableCell30
            // 
            this.tableCell30.BackColor = System.Drawing.Color.Transparent;
            this.tableCell30.BorderColor = System.Drawing.Color.Black;
            this.tableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell30.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Employee]")});
            this.tableCell30.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.tableCell30.ForeColor = System.Drawing.Color.Black;
            this.tableCell30.Multiline = true;
            this.tableCell30.Name = "tableCell30";
            this.tableCell30.StylePriority.UseBackColor = false;
            this.tableCell30.StylePriority.UseBorderColor = false;
            this.tableCell30.StylePriority.UseBorders = false;
            this.tableCell30.StylePriority.UseFont = false;
            this.tableCell30.StylePriority.UseForeColor = false;
            this.tableCell30.StylePriority.UseTextAlignment = false;
            this.tableCell30.Text = "Name of Employee";
            this.tableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell30.Weight = 0.83155124094175181D;
            // 
            // tableCell35
            // 
            this.tableCell35.BackColor = System.Drawing.Color.Transparent;
            this.tableCell35.BorderColor = System.Drawing.Color.Black;
            this.tableCell35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell35.BorderWidth = 1F;
            this.tableCell35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]")});
            this.tableCell35.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.tableCell35.ForeColor = System.Drawing.Color.Black;
            this.tableCell35.Multiline = true;
            this.tableCell35.Name = "tableCell35";
            this.tableCell35.StylePriority.UseBackColor = false;
            this.tableCell35.StylePriority.UseBorderColor = false;
            this.tableCell35.StylePriority.UseBorders = false;
            this.tableCell35.StylePriority.UseBorderWidth = false;
            this.tableCell35.StylePriority.UseFont = false;
            this.tableCell35.StylePriority.UseForeColor = false;
            this.tableCell35.StylePriority.UseTextAlignment = false;
            this.tableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell35.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell35.Weight = 0.8925673222566779D;
            // 
            // tableCell36
            // 
            this.tableCell36.BackColor = System.Drawing.Color.Transparent;
            this.tableCell36.BorderColor = System.Drawing.Color.Black;
            this.tableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell36.BorderWidth = 1F;
            this.tableCell36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.tableCell36.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.tableCell36.ForeColor = System.Drawing.Color.Black;
            this.tableCell36.Multiline = true;
            this.tableCell36.Name = "tableCell36";
            this.tableCell36.StylePriority.UseBackColor = false;
            this.tableCell36.StylePriority.UseBorderColor = false;
            this.tableCell36.StylePriority.UseBorders = false;
            this.tableCell36.StylePriority.UseBorderWidth = false;
            this.tableCell36.StylePriority.UseFont = false;
            this.tableCell36.StylePriority.UseForeColor = false;
            this.tableCell36.StylePriority.UseTextAlignment = false;
            this.tableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell36.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell36.Weight = 1.3971372757808924D;
            // 
            // tableCell48
            // 
            this.tableCell48.BackColor = System.Drawing.Color.Transparent;
            this.tableCell48.BorderColor = System.Drawing.Color.Black;
            this.tableCell48.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell48.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AMFRINGTAX]")});
            this.tableCell48.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell48.ForeColor = System.Drawing.Color.Black;
            this.tableCell48.Multiline = true;
            this.tableCell48.Name = "tableCell48";
            this.tableCell48.StylePriority.UseBackColor = false;
            this.tableCell48.StylePriority.UseBorderColor = false;
            this.tableCell48.StylePriority.UseBorders = false;
            this.tableCell48.StylePriority.UseFont = false;
            this.tableCell48.StylePriority.UseForeColor = false;
            this.tableCell48.StylePriority.UseTextAlignment = false;
            this.tableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell48.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell48.Weight = 0.45509236835067129D;
            // 
            // tableCell49
            // 
            this.tableCell49.BackColor = System.Drawing.Color.Transparent;
            this.tableCell49.BorderColor = System.Drawing.Color.Black;
            this.tableCell49.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell49.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell49.ForeColor = System.Drawing.Color.Black;
            this.tableCell49.Multiline = true;
            this.tableCell49.Name = "tableCell49";
            this.tableCell49.StylePriority.UseBackColor = false;
            this.tableCell49.StylePriority.UseBorderColor = false;
            this.tableCell49.StylePriority.UseBorders = false;
            this.tableCell49.StylePriority.UseFont = false;
            this.tableCell49.StylePriority.UseForeColor = false;
            this.tableCell49.StylePriority.UseTextAlignment = false;
            this.tableCell49.Text = "20%";
            this.tableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell49.TextFormatString = "{0:#,#}";
            this.tableCell49.Weight = 0.34971920142181123D;
            // 
            // tableCell50
            // 
            this.tableCell50.BackColor = System.Drawing.Color.Transparent;
            this.tableCell50.BorderColor = System.Drawing.Color.Black;
            this.tableCell50.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell50.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FRINGAM]")});
            this.tableCell50.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell50.ForeColor = System.Drawing.Color.Black;
            this.tableCell50.Multiline = true;
            this.tableCell50.Name = "tableCell50";
            this.tableCell50.StylePriority.UseBackColor = false;
            this.tableCell50.StylePriority.UseBorderColor = false;
            this.tableCell50.StylePriority.UseBorders = false;
            this.tableCell50.StylePriority.UseFont = false;
            this.tableCell50.StylePriority.UseForeColor = false;
            this.tableCell50.StylePriority.UseTextAlignment = false;
            this.tableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell50.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell50.Weight = 0.47309140842331321D;
            // 
            // tableCell52
            // 
            this.tableCell52.BackColor = System.Drawing.Color.Transparent;
            this.tableCell52.BorderColor = System.Drawing.Color.Black;
            this.tableCell52.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AMFRINGTAX]-[FRINGAM] ")});
            this.tableCell52.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell52.ForeColor = System.Drawing.Color.Black;
            this.tableCell52.Multiline = true;
            this.tableCell52.Name = "tableCell52";
            this.tableCell52.StylePriority.UseBackColor = false;
            this.tableCell52.StylePriority.UseBorderColor = false;
            this.tableCell52.StylePriority.UseBorders = false;
            this.tableCell52.StylePriority.UseFont = false;
            this.tableCell52.StylePriority.UseForeColor = false;
            this.tableCell52.StylePriority.UseTextAlignment = false;
            this.tableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell52.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell52.Weight = 0.46869227826005272D;
            // 
            // tableCell65
            // 
            this.tableCell65.BackColor = System.Drawing.Color.Transparent;
            this.tableCell65.BorderColor = System.Drawing.Color.Black;
            this.tableCell65.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell65.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.tableCell65.ForeColor = System.Drawing.Color.Black;
            this.tableCell65.Multiline = true;
            this.tableCell65.Name = "tableCell65";
            this.tableCell65.StylePriority.UseBackColor = false;
            this.tableCell65.StylePriority.UseBorderColor = false;
            this.tableCell65.StylePriority.UseBorders = false;
            this.tableCell65.StylePriority.UseFont = false;
            this.tableCell65.StylePriority.UseForeColor = false;
            this.tableCell65.StylePriority.UseTextAlignment = false;
            this.tableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell65.Weight = 0.51981767980580884D;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 20F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label3,
            this.label1});
            this.ReportHeader.HeightF = 57.66668F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // label3
            // 
            this.label3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'In Month:\'+ FormatString(\'{0:MMMM-yyy}\',?InMonth)")});
            this.label3.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28.00001F);
            this.label3.Multiline = true;
            this.label3.Name = "label3";
            this.label3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label3.SizeF = new System.Drawing.SizeF(387.8727F, 29.66667F);
            this.label3.StylePriority.UseFont = false;
            this.label3.StylePriority.UseTextAlignment = false;
            this.label3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new DevExpress.Drawing.DXFont("Times New Roman", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.label1.Multiline = true;
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(387.8727F, 28.00001F);
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Benefit and Tax on Fringe Benefit-Internal";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table3});
            this.ReportFooter.HeightF = 28.33333F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // table3
            // 
            this.table3.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table3.Name = "table3";
            this.table3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table3.SizeF = new System.Drawing.SizeF(1129F, 28.33333F);
            this.table3.StylePriority.UseFont = false;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell127,
            this.tableCell27,
            this.tableCell33,
            this.tableCell51,
            this.tableCell53,
            this.tableCell66});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 1.2D;
            // 
            // tableCell127
            // 
            this.tableCell127.BackColor = System.Drawing.Color.Transparent;
            this.tableCell127.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell127.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell127.ForeColor = System.Drawing.Color.Black;
            this.tableCell127.Multiline = true;
            this.tableCell127.Name = "tableCell127";
            this.tableCell127.StylePriority.UseBackColor = false;
            this.tableCell127.StylePriority.UseBorders = false;
            this.tableCell127.StylePriority.UseFont = false;
            this.tableCell127.StylePriority.UseForeColor = false;
            this.tableCell127.StylePriority.UseTextAlignment = false;
            this.tableCell127.Text = "SumTotal:";
            this.tableCell127.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell127.Weight = 3.2413192484990212D;
            // 
            // tableCell27
            // 
            this.tableCell27.BackColor = System.Drawing.Color.Transparent;
            this.tableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([AMFRINGTAX])")});
            this.tableCell27.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell27.ForeColor = System.Drawing.Color.Black;
            this.tableCell27.Multiline = true;
            this.tableCell27.Name = "tableCell27";
            this.tableCell27.StylePriority.UseBackColor = false;
            this.tableCell27.StylePriority.UseBorders = false;
            this.tableCell27.StylePriority.UseFont = false;
            this.tableCell27.StylePriority.UseForeColor = false;
            this.tableCell27.StylePriority.UseTextAlignment = false;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell27.Summary = xrSummary2;
            this.tableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell27.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell27.Weight = 0.39989606890245355D;
            // 
            // tableCell33
            // 
            this.tableCell33.BackColor = System.Drawing.Color.Transparent;
            this.tableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell33.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell33.ForeColor = System.Drawing.Color.Black;
            this.tableCell33.Multiline = true;
            this.tableCell33.Name = "tableCell33";
            this.tableCell33.StylePriority.UseBackColor = false;
            this.tableCell33.StylePriority.UseBorders = false;
            this.tableCell33.StylePriority.UseFont = false;
            this.tableCell33.StylePriority.UseForeColor = false;
            this.tableCell33.StylePriority.UseTextAlignment = false;
            this.tableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell33.Weight = 0.30730330812363604D;
            // 
            // tableCell51
            // 
            this.tableCell51.BackColor = System.Drawing.Color.Transparent;
            this.tableCell51.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([FRINGAM])")});
            this.tableCell51.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell51.ForeColor = System.Drawing.Color.Black;
            this.tableCell51.Multiline = true;
            this.tableCell51.Name = "tableCell51";
            this.tableCell51.StylePriority.UseBackColor = false;
            this.tableCell51.StylePriority.UseBorders = false;
            this.tableCell51.StylePriority.UseFont = false;
            this.tableCell51.StylePriority.UseForeColor = false;
            this.tableCell51.StylePriority.UseTextAlignment = false;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell51.Summary = xrSummary3;
            this.tableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell51.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell51.Weight = 0.41571233730069079D;
            // 
            // tableCell53
            // 
            this.tableCell53.BackColor = System.Drawing.Color.Transparent;
            this.tableCell53.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell53.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([AMFRINGTAX]-[FRINGAM])")});
            this.tableCell53.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell53.ForeColor = System.Drawing.Color.Black;
            this.tableCell53.Multiline = true;
            this.tableCell53.Name = "tableCell53";
            this.tableCell53.StylePriority.UseBackColor = false;
            this.tableCell53.StylePriority.UseBorders = false;
            this.tableCell53.StylePriority.UseFont = false;
            this.tableCell53.StylePriority.UseForeColor = false;
            this.tableCell53.StylePriority.UseTextAlignment = false;
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell53.Summary = xrSummary4;
            this.tableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell53.TextFormatString = "{0:$#,0.00;($#,0);\'-\';}";
            this.tableCell53.Weight = 0.41184670257699024D;
            // 
            // tableCell66
            // 
            this.tableCell66.BackColor = System.Drawing.Color.Transparent;
            this.tableCell66.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell66.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell66.ForeColor = System.Drawing.Color.Black;
            this.tableCell66.Multiline = true;
            this.tableCell66.Name = "tableCell66";
            this.tableCell66.StylePriority.UseBackColor = false;
            this.tableCell66.StylePriority.UseBorders = false;
            this.tableCell66.StylePriority.UseFont = false;
            this.tableCell66.StylePriority.UseForeColor = false;
            this.tableCell66.StylePriority.UseTextAlignment = false;
            this.tableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell66.Weight = 0.45214035702225913D;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.PageHeader.HeightF = 46.66667F;
            this.PageHeader.Name = "PageHeader";
            // 
            // table1
            // 
            this.table1.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table1.Name = "table1";
            this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(1130F, 46.66667F);
            this.table1.StylePriority.UseFont = false;
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.tableCell1,
            this.tableCell11,
            this.tableCell8,
            this.tableCell4,
            this.tableCell19,
            this.tableCell20,
            this.tableCell59,
            this.tableCell21,
            this.tableCell26});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1.2D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "No.";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.22448647731823837D;
            // 
            // tableCell1
            // 
            this.tableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell1.ForeColor = System.Drawing.Color.White;
            this.tableCell1.Multiline = true;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseBackColor = false;
            this.tableCell1.StylePriority.UseBorders = false;
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.StylePriority.UseForeColor = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "ID";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.36740978814052444D;
            // 
            // tableCell11
            // 
            this.tableCell11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell11.ForeColor = System.Drawing.Color.White;
            this.tableCell11.Multiline = true;
            this.tableCell11.Name = "tableCell11";
            this.tableCell11.StylePriority.UseBackColor = false;
            this.tableCell11.StylePriority.UseBorders = false;
            this.tableCell11.StylePriority.UseFont = false;
            this.tableCell11.StylePriority.UseForeColor = false;
            this.tableCell11.StylePriority.UseTextAlignment = false;
            this.tableCell11.Text = "Name of Employee";
            this.tableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell11.Weight = 0.86737457168288346D;
            // 
            // tableCell8
            // 
            this.tableCell8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell8.ForeColor = System.Drawing.Color.White;
            this.tableCell8.Multiline = true;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StylePriority.UseBackColor = false;
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.StylePriority.UseForeColor = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.Text = "Department";
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell8.Weight = 0.93101932066274073D;
            // 
            // tableCell4
            // 
            this.tableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell4.ForeColor = System.Drawing.Color.White;
            this.tableCell4.Multiline = true;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UseForeColor = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "Position";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell4.Weight = 1.457326808611763D;
            // 
            // tableCell19
            // 
            this.tableCell19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell19.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell19.ForeColor = System.Drawing.Color.White;
            this.tableCell19.Multiline = true;
            this.tableCell19.Name = "tableCell19";
            this.tableCell19.StylePriority.UseBackColor = false;
            this.tableCell19.StylePriority.UseBorders = false;
            this.tableCell19.StylePriority.UseFont = false;
            this.tableCell19.StylePriority.UseForeColor = false;
            this.tableCell19.StylePriority.UseTextAlignment = false;
            this.tableCell19.Text = "Fringe Benefit";
            this.tableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell19.Weight = 0.47469796734692971D;
            // 
            // tableCell20
            // 
            this.tableCell20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell20.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell20.ForeColor = System.Drawing.Color.White;
            this.tableCell20.Multiline = true;
            this.tableCell20.Name = "tableCell20";
            this.tableCell20.StylePriority.UseBackColor = false;
            this.tableCell20.StylePriority.UseBorders = false;
            this.tableCell20.StylePriority.UseFont = false;
            this.tableCell20.StylePriority.UseForeColor = false;
            this.tableCell20.StylePriority.UseTextAlignment = false;
            this.tableCell20.Text = "Tax Rate 20%";
            this.tableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell20.Weight = 0.36478518858611969D;
            // 
            // tableCell59
            // 
            this.tableCell59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell59.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell59.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell59.ForeColor = System.Drawing.Color.White;
            this.tableCell59.Multiline = true;
            this.tableCell59.Name = "tableCell59";
            this.tableCell59.StylePriority.UseBackColor = false;
            this.tableCell59.StylePriority.UseBorders = false;
            this.tableCell59.StylePriority.UseFont = false;
            this.tableCell59.StylePriority.UseForeColor = false;
            this.tableCell59.StylePriority.UseTextAlignment = false;
            this.tableCell59.Text = "Tax on Fringe Benefit";
            this.tableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell59.Weight = 0.49347230607951054D;
            // 
            // tableCell21
            // 
            this.tableCell21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell21.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell21.ForeColor = System.Drawing.Color.White;
            this.tableCell21.Multiline = true;
            this.tableCell21.Name = "tableCell21";
            this.tableCell21.StylePriority.UseBackColor = false;
            this.tableCell21.StylePriority.UseBorders = false;
            this.tableCell21.StylePriority.UseFont = false;
            this.tableCell21.StylePriority.UseForeColor = false;
            this.tableCell21.StylePriority.UseTextAlignment = false;
            this.tableCell21.Text = "Net Fringe benefit";
            this.tableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell21.Weight = 0.48888358851749758D;
            // 
            // tableCell26
            // 
            this.tableCell26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.tableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell26.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell26.ForeColor = System.Drawing.Color.White;
            this.tableCell26.Multiline = true;
            this.tableCell26.Name = "tableCell26";
            this.tableCell26.StylePriority.UseBackColor = false;
            this.tableCell26.StylePriority.UseBorders = false;
            this.tableCell26.StylePriority.UseFont = false;
            this.tableCell26.StylePriority.UseForeColor = false;
            this.tableCell26.StylePriority.UseTextAlignment = false;
            this.tableCell26.Text = "Remarks";
            this.tableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell26.Weight = 0.54221095340701531D;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ReportConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "HR_RPT_FringBenifit";
            queryParameter1.Name = "@Company";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?Company", typeof(string));
            queryParameter2.Name = "@Branch";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?Branch", typeof(string));
            queryParameter3.Name = "@Division";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("?Division", typeof(string));
            queryParameter4.Name = "@Department";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("?Department", typeof(string));
            queryParameter5.Name = "@Section";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?Section", typeof(string));
            queryParameter6.Name = "@Position";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("?Position", typeof(string));
            queryParameter7.Name = "@Level";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("?Level", typeof(string));
            queryParameter8.Name = "@InMonth";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("?InMonth", typeof(System.DateTime));
            storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4,
            queryParameter5,
            queryParameter6,
            queryParameter7,
            queryParameter8});
            storedProcQuery1.StoredProcName = "HR_RPT_FringBenifit";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Branch
            // 
            this.Branch.Name = "Branch";
            // 
            // Division
            // 
            this.Division.Name = "Division";
            // 
            // Department
            // 
            this.Department.Name = "Department";
            // 
            // Section
            // 
            this.Section.Name = "Section";
            // 
            // Position
            // 
            this.Position.Name = "Position";
            // 
            // Level
            // 
            this.Level.Name = "Level";
            // 
            // InMonth
            // 
            this.InMonth.Name = "InMonth";
            this.InMonth.Type = typeof(System.DateTime);
            this.InMonth.ValueInfo = "2023-08-01";
            // 
            // Company
            // 
            this.Company.Name = "Company";
            // 
            // RptFringeBenefit
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "HR_RPT_FringBenifit";
            this.DataSource = this.sqlDataSource1;
            this.DisplayName = "RPTFringBenifit";
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(19F, 20F, 20F, 20F);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Branch,
            this.Division,
            this.Department,
            this.Section,
            this.Position,
            this.Level,
            this.InMonth,
            this.Company});
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell29;
        private DevExpress.XtraReports.UI.XRTableCell tableCell30;
        private DevExpress.XtraReports.UI.XRTableCell tableCell35;
        private DevExpress.XtraReports.UI.XRTableCell tableCell36;
        private DevExpress.XtraReports.UI.XRTableCell tableCell48;
        private DevExpress.XtraReports.UI.XRTableCell tableCell49;
        private DevExpress.XtraReports.UI.XRTableCell tableCell50;
        private DevExpress.XtraReports.UI.XRTableCell tableCell52;
        private DevExpress.XtraReports.UI.XRTableCell tableCell65;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel label3;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell127;
        private DevExpress.XtraReports.UI.XRTableCell tableCell27;
        private DevExpress.XtraReports.UI.XRTableCell tableCell33;
        private DevExpress.XtraReports.UI.XRTableCell tableCell51;
        private DevExpress.XtraReports.UI.XRTableCell tableCell53;
        private DevExpress.XtraReports.UI.XRTableCell tableCell66;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell19;
        private DevExpress.XtraReports.UI.XRTableCell tableCell20;
        private DevExpress.XtraReports.UI.XRTableCell tableCell59;
        private DevExpress.XtraReports.UI.XRTableCell tableCell21;
        private DevExpress.XtraReports.UI.XRTableCell tableCell26;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter Branch;
        private DevExpress.XtraReports.Parameters.Parameter Division;
        private DevExpress.XtraReports.Parameters.Parameter Department;
        private DevExpress.XtraReports.Parameters.Parameter Section;
        private DevExpress.XtraReports.Parameters.Parameter Position;
        private DevExpress.XtraReports.Parameters.Parameter Level;
        private DevExpress.XtraReports.Parameters.Parameter InMonth;
        private DevExpress.XtraReports.Parameters.Parameter Company;
    }
}
