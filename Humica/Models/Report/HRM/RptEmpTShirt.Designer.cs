
namespace HUMICA.Models.Report.HRM
{
    partial class RptEmpTShirt
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
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptEmpTShirt));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCompany = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
            this.Division = new DevExpress.XtraReports.Parameters.Parameter();
            this.Department = new DevExpress.XtraReports.Parameters.Parameter();
            this.Section = new DevExpress.XtraReports.Parameters.Parameter();
            this.Position = new DevExpress.XtraReports.Parameters.Parameter();
            this.Level = new DevExpress.XtraReports.Parameters.Parameter();
            this.InMonth = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 25F;
            this.TopMargin.Name = "TopMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SortPos", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 25F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table3});
            this.PageHeader.HeightF = 53.62501F;
            this.PageHeader.Name = "PageHeader";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label1,
            this.lblCompany});
            this.ReportHeader.HeightF = 60.33331F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table1.Name = "table1";
            this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(1119F, 25F);
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2,
            this.tableCell3,
            this.tableCell4,
            this.tableCell5,
            this.tableCell6,
            this.tableCell7,
            this.tableCell8,
            this.tableCell9,
            this.tableCell10});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
            this.tableCell1.Multiline = true;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseBackColor = false;
            this.tableCell1.StylePriority.UseBorders = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell1.Summary = xrSummary1;
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.48235598934667767D;
            // 
            // tableCell2
            // 
            this.tableCell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")});
            this.tableCell2.Multiline = true;
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StylePriority.UseBackColor = false;
            this.tableCell2.StylePriority.UseBorders = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell2.Weight = 0.48117275678221094D;
            // 
            // tableCell3
            // 
            this.tableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OthAllName]")});
            this.tableCell3.Multiline = true;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseBackColor = false;
            this.tableCell3.StylePriority.UseBorders = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell3.Weight = 1.0684731001688474D;
            // 
            // tableCell4
            // 
            this.tableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AllName]")});
            this.tableCell4.Multiline = true;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell4.Weight = 1.1814249172686373D;
            // 
            // tableCell5
            // 
            this.tableCell5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Sex]=\'M\',\'ប្រុស\',\'ស្រី\')")});
            this.tableCell5.Multiline = true;
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StylePriority.UseBackColor = false;
            this.tableCell5.StylePriority.UseBorders = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell5.Weight = 0.31950979246489514D;
            // 
            // tableCell6
            // 
            this.tableCell6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.tableCell6.Multiline = true;
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StylePriority.UseBackColor = false;
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.StylePriority.UseTextAlignment = false;
            this.tableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell6.Weight = 1.3707551271029403D;
            // 
            // tableCell7
            // 
            this.tableCell7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
            this.tableCell7.Multiline = true;
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StylePriority.UseBackColor = false;
            this.tableCell7.StylePriority.UseBorders = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell7.TextFormatString = "{0:d-MMM-yy}";
            this.tableCell7.Weight = 0.71688235084457608D;
            // 
            // tableCell8
            // 
            this.tableCell8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OpenDate_Befor]")});
            this.tableCell8.Multiline = true;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StylePriority.UseBackColor = false;
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell8.TextFormatString = "{0:d-MMM-yy}";
            this.tableCell8.Weight = 0.74410397593418764D;
            // 
            // tableCell9
            // 
            this.tableCell9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OpenDate]")});
            this.tableCell9.Multiline = true;
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StylePriority.UseBackColor = false;
            this.tableCell9.StylePriority.UseBorders = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell9.TextFormatString = "{0:d-MMM-yy}";
            this.tableCell9.Weight = 0.74408932750890111D;
            // 
            // tableCell10
            // 
            this.tableCell10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.tableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell10.Multiline = true;
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StylePriority.UseBackColor = false;
            this.tableCell10.StylePriority.UseBorders = false;
            this.tableCell10.StylePriority.UseTextAlignment = false;
            this.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell10.Weight = 0.74408932750890111D;
            // 
            // table3
            // 
            this.table3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table3.Name = "table3";
            this.table3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table3.SizeF = new System.Drawing.SizeF(1119F, 53.62501F);
            this.table3.StylePriority.UseFont = false;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell58,
            this.tableCell59,
            this.tableCell60,
            this.tableCell61,
            this.tableCell62,
            this.tableCell65,
            this.tableCell66,
            this.tableCell68,
            this.tableCell69,
            this.tableCell70});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 1D;
            // 
            // tableCell58
            // 
            this.tableCell58.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell58.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell58.ForeColor = System.Drawing.Color.White;
            this.tableCell58.Multiline = true;
            this.tableCell58.Name = "tableCell58";
            this.tableCell58.StylePriority.UseBackColor = false;
            this.tableCell58.StylePriority.UseBorders = false;
            this.tableCell58.StylePriority.UseForeColor = false;
            this.tableCell58.StylePriority.UseTextAlignment = false;
            this.tableCell58.Text = "លេខរៀង";
            this.tableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell58.Weight = 0.48235598934667767D;
            // 
            // tableCell59
            // 
            this.tableCell59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell59.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell59.ForeColor = System.Drawing.Color.White;
            this.tableCell59.Multiline = true;
            this.tableCell59.Name = "tableCell59";
            this.tableCell59.StylePriority.UseBackColor = false;
            this.tableCell59.StylePriority.UseBorders = false;
            this.tableCell59.StylePriority.UseForeColor = false;
            this.tableCell59.StylePriority.UseTextAlignment = false;
            this.tableCell59.Text = "អត្តលេខ";
            this.tableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell59.Weight = 0.48117275678221094D;
            // 
            // tableCell60
            // 
            this.tableCell60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell60.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell60.ForeColor = System.Drawing.Color.White;
            this.tableCell60.Multiline = true;
            this.tableCell60.Name = "tableCell60";
            this.tableCell60.StylePriority.UseBackColor = false;
            this.tableCell60.StylePriority.UseBorders = false;
            this.tableCell60.StylePriority.UseForeColor = false;
            this.tableCell60.StylePriority.UseTextAlignment = false;
            this.tableCell60.Text = "ឈ្មោះជាភាសាខ្មែរ";
            this.tableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell60.Weight = 1.0684731001688474D;
            // 
            // tableCell61
            // 
            this.tableCell61.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell61.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell61.ForeColor = System.Drawing.Color.White;
            this.tableCell61.Multiline = true;
            this.tableCell61.Name = "tableCell61";
            this.tableCell61.StylePriority.UseBackColor = false;
            this.tableCell61.StylePriority.UseBorders = false;
            this.tableCell61.StylePriority.UseForeColor = false;
            this.tableCell61.StylePriority.UseTextAlignment = false;
            this.tableCell61.Text = "ឈ្មោះជាភាសាអង់គ្លេស";
            this.tableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell61.Weight = 1.1814249172686373D;
            // 
            // tableCell62
            // 
            this.tableCell62.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell62.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell62.ForeColor = System.Drawing.Color.White;
            this.tableCell62.Multiline = true;
            this.tableCell62.Name = "tableCell62";
            this.tableCell62.StylePriority.UseBackColor = false;
            this.tableCell62.StylePriority.UseBorders = false;
            this.tableCell62.StylePriority.UseForeColor = false;
            this.tableCell62.StylePriority.UseTextAlignment = false;
            this.tableCell62.Text = "ភេទ";
            this.tableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell62.Weight = 0.31950979246489514D;
            // 
            // tableCell65
            // 
            this.tableCell65.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell65.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell65.ForeColor = System.Drawing.Color.White;
            this.tableCell65.Multiline = true;
            this.tableCell65.Name = "tableCell65";
            this.tableCell65.StylePriority.UseBackColor = false;
            this.tableCell65.StylePriority.UseBorders = false;
            this.tableCell65.StylePriority.UseForeColor = false;
            this.tableCell65.StylePriority.UseTextAlignment = false;
            this.tableCell65.Text = "តួនាទី";
            this.tableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell65.Weight = 1.3707551271029403D;
            // 
            // tableCell66
            // 
            this.tableCell66.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell66.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell66.ForeColor = System.Drawing.Color.White;
            this.tableCell66.Multiline = true;
            this.tableCell66.Name = "tableCell66";
            this.tableCell66.StylePriority.UseBackColor = false;
            this.tableCell66.StylePriority.UseBorders = false;
            this.tableCell66.StylePriority.UseForeColor = false;
            this.tableCell66.StylePriority.UseTextAlignment = false;
            this.tableCell66.Text = "ថ្ងៃចូលធ្វើការ";
            this.tableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell66.Weight = 0.71688235084457608D;
            // 
            // tableCell68
            // 
            this.tableCell68.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell68.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell68.ForeColor = System.Drawing.Color.White;
            this.tableCell68.Multiline = true;
            this.tableCell68.Name = "tableCell68";
            this.tableCell68.StylePriority.UseBackColor = false;
            this.tableCell68.StylePriority.UseBorders = false;
            this.tableCell68.StylePriority.UseForeColor = false;
            this.tableCell68.StylePriority.UseTextAlignment = false;
            this.tableCell68.Text = "ថ្ងៃបើកអាវចុង\r\nក្រោយ";
            this.tableCell68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell68.Weight = 0.74410397593418764D;
            // 
            // tableCell69
            // 
            this.tableCell69.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell69.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell69.ForeColor = System.Drawing.Color.White;
            this.tableCell69.Multiline = true;
            this.tableCell69.Name = "tableCell69";
            this.tableCell69.StylePriority.UseBackColor = false;
            this.tableCell69.StylePriority.UseBorders = false;
            this.tableCell69.StylePriority.UseForeColor = false;
            this.tableCell69.StylePriority.UseTextAlignment = false;
            this.tableCell69.Text = "ថ្ងៃបើកអាវ";
            this.tableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell69.Weight = 0.74408932750890111D;
            // 
            // tableCell70
            // 
            this.tableCell70.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.tableCell70.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell70.ForeColor = System.Drawing.Color.White;
            this.tableCell70.Multiline = true;
            this.tableCell70.Name = "tableCell70";
            this.tableCell70.StylePriority.UseBackColor = false;
            this.tableCell70.StylePriority.UseBorders = false;
            this.tableCell70.StylePriority.UseForeColor = false;
            this.tableCell70.StylePriority.UseTextAlignment = false;
            this.tableCell70.Text = "ការិយាល័យ";
            this.tableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell70.Weight = 0.74408932750890111D;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.label1.CanGrow = false;
            this.label1.CanPublish = false;
            this.label1.CanShrink = true;
            this.label1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 14F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 36.41666F);
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(1119F, 23.91666F);
            this.label1.StylePriority.UseBackColor = false;
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseForeColor = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblCompany
            // 
            this.lblCompany.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.lblCompany.CanGrow = false;
            this.lblCompany.CanPublish = false;
            this.lblCompany.CanShrink = true;
            this.lblCompany.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'បញ្ជីឈ្មោះបុគ្គលិកដែលទទួលបានអាវខែ\'+[InMonthKH]")});
            this.lblCompany.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 14F);
            this.lblCompany.ForeColor = System.Drawing.Color.White;
            this.lblCompany.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCompany.SizeF = new System.Drawing.SizeF(1119F, 36.41666F);
            this.lblCompany.StylePriority.UseBackColor = false;
            this.lblCompany.StylePriority.UseFont = false;
            this.lblCompany.StylePriority.UseForeColor = false;
            this.lblCompany.StylePriority.UseTextAlignment = false;
            this.lblCompany.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ReportConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "HR_RPT_EmpShrit";
            queryParameter1.Name = "@Branch";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?Branch", typeof(string));
            queryParameter2.Name = "@Division";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?Division", typeof(string));
            queryParameter3.Name = "@Department";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("?Department", typeof(string));
            queryParameter4.Name = "@Section";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("?Section", typeof(string));
            queryParameter5.Name = "@Position";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?Position", typeof(string));
            queryParameter6.Name = "@Level";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("?Level", typeof(string));
            queryParameter7.Name = "@InMonth";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("?InMonth", typeof(System.DateTime));
            storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4,
            queryParameter5,
            queryParameter6,
            queryParameter7});
            storedProcQuery1.StoredProcName = "HR_RPT_EmpShrit";
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
            this.InMonth.ValueInfo = "1753-01-01";
            // 
            // RptEmpTShirt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "HR_RPT_EmpShrit";
            this.DataSource = this.sqlDataSource1;
            this.DisplayName = "RptEmp_Shirt";
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(25, 25, 25, 25);
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
            this.InMonth});
            this.Version = "21.2";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell58;
        private DevExpress.XtraReports.UI.XRTableCell tableCell59;
        private DevExpress.XtraReports.UI.XRTableCell tableCell60;
        private DevExpress.XtraReports.UI.XRTableCell tableCell61;
        private DevExpress.XtraReports.UI.XRTableCell tableCell62;
        private DevExpress.XtraReports.UI.XRTableCell tableCell65;
        private DevExpress.XtraReports.UI.XRTableCell tableCell66;
        private DevExpress.XtraReports.UI.XRTableCell tableCell68;
        private DevExpress.XtraReports.UI.XRTableCell tableCell69;
        private DevExpress.XtraReports.UI.XRTableCell tableCell70;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRLabel lblCompany;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter Branch;
        private DevExpress.XtraReports.Parameters.Parameter Division;
        private DevExpress.XtraReports.Parameters.Parameter Department;
        private DevExpress.XtraReports.Parameters.Parameter Section;
        private DevExpress.XtraReports.Parameters.Parameter Position;
        private DevExpress.XtraReports.Parameters.Parameter Level;
        private DevExpress.XtraReports.Parameters.Parameter InMonth;
    }
}
