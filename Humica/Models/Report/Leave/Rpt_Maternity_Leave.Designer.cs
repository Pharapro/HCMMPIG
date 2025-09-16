namespace Humica.Models.Report.Leave
{
	partial class Rpt_Maternity_Leave
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
			DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter10 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter11 = new DevExpress.DataAccess.Sql.QueryParameter();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rpt_Maternity_Leave));
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.table2 = new DevExpress.XtraReports.UI.XRTable();
			this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
			this.label2 = new DevExpress.XtraReports.UI.XRLabel();
			this.label1 = new DevExpress.XtraReports.UI.XRLabel();
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
			this.tableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
			this.label3 = new DevExpress.XtraReports.UI.XRLabel();
			this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
			this.Company = new DevExpress.XtraReports.Parameters.Parameter();
			this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
			this.Division = new DevExpress.XtraReports.Parameters.Parameter();
			this.BusinessUnit = new DevExpress.XtraReports.Parameters.Parameter();
			this.Department = new DevExpress.XtraReports.Parameters.Parameter();
			this.Office = new DevExpress.XtraReports.Parameters.Parameter();
			this.Section = new DevExpress.XtraReports.Parameters.Parameter();
			this.Group = new DevExpress.XtraReports.Parameters.Parameter();
			this.Position = new DevExpress.XtraReports.Parameters.Parameter();
			this.Level = new DevExpress.XtraReports.Parameters.Parameter();
			this.InMonth = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
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
			this.Detail.HeightF = 25F;
			this.Detail.Name = "Detail";
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 20F;
			this.BottomMargin.Name = "BottomMargin";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label2,
            this.label1});
			this.ReportHeader.HeightF = 61.83334F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// PageHeader
			// 
			this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
			this.PageHeader.HeightF = 25F;
			this.PageHeader.Name = "PageHeader";
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label3});
			this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Department", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			this.GroupHeader1.HeightF = 23F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// table2
			// 
			this.table2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.table2.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
			this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table2.Name = "table2";
			this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
			this.table2.SizeF = new System.Drawing.SizeF(1290F, 25F);
			this.table2.StylePriority.UseBorders = false;
			this.table2.StylePriority.UseFont = false;
			this.table2.StylePriority.UseTextAlignment = false;
			this.table2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// tableRow2
			// 
			this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell11,
            this.tableCell12,
            this.tableCell13,
            this.tableCell14,
            this.tableCell15,
            this.tableCell16,
            this.tableCell17,
            this.tableCell18,
            this.tableCell19,
            this.tableCell20,
            this.tableCell22});
			this.tableRow2.Name = "tableRow2";
			this.tableRow2.Weight = 1D;
			// 
			// tableCell11
			// 
			this.tableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
			this.tableCell11.Multiline = true;
			this.tableCell11.Name = "tableCell11";
			this.tableCell11.StylePriority.UseBorders = false;
			xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
			this.tableCell11.Summary = xrSummary1;
			this.tableCell11.Weight = 0.33511048263071497D;
			// 
			// tableCell12
			// 
			this.tableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")});
			this.tableCell12.Multiline = true;
			this.tableCell12.Name = "tableCell12";
			this.tableCell12.StylePriority.UseBorders = false;
			this.tableCell12.Weight = 0.91478217443456233D;
			// 
			// tableCell13
			// 
			this.tableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AllName]")});
			this.tableCell13.Multiline = true;
			this.tableCell13.Name = "tableCell13";
			this.tableCell13.StylePriority.UseBorders = false;
			this.tableCell13.StylePriority.UseTextAlignment = false;
			this.tableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.tableCell13.Weight = 1.1255641295139593D;
			// 
			// tableCell14
			// 
			this.tableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
			this.tableCell14.Multiline = true;
			this.tableCell14.Name = "tableCell14";
			this.tableCell14.StylePriority.UseBorders = false;
			this.tableCell14.StylePriority.UseTextAlignment = false;
			this.tableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.tableCell14.Weight = 1.5716202687479823D;
			// 
			// tableCell15
			// 
			this.tableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
			this.tableCell15.Multiline = true;
			this.tableCell15.Name = "tableCell15";
			this.tableCell15.StylePriority.UseBorders = false;
			this.tableCell15.StylePriority.UseTextAlignment = false;
			this.tableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.tableCell15.Weight = 1.5904982124727671D;
			// 
			// tableCell16
			// 
			this.tableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
			this.tableCell16.Multiline = true;
			this.tableCell16.Name = "tableCell16";
			this.tableCell16.StylePriority.UseBorders = false;
			this.tableCell16.TextFormatString = "{0:dd-MMM-yy}";
			this.tableCell16.Weight = 1.0559334847403903D;
			// 
			// tableCell17
			// 
			this.tableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RequestDate]")});
			this.tableCell17.Multiline = true;
			this.tableCell17.Name = "tableCell17";
			this.tableCell17.StylePriority.UseBorders = false;
			this.tableCell17.TextFormatString = "{0:dd-MMM-yy}";
			this.tableCell17.Weight = 1.0559337576860326D;
			// 
			// tableCell18
			// 
			this.tableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FromDate]")});
			this.tableCell18.Multiline = true;
			this.tableCell18.Name = "tableCell18";
			this.tableCell18.StylePriority.UseBorders = false;
			this.tableCell18.TextFormatString = "{0:dd-MMM-yy}";
			this.tableCell18.Weight = 1.0559329120438374D;
			// 
			// tableCell19
			// 
			this.tableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ToDate]")});
			this.tableCell19.Multiline = true;
			this.tableCell19.Name = "tableCell19";
			this.tableCell19.StylePriority.UseBorders = false;
			this.tableCell19.TextFormatString = "{0:dd-MMM-yy}";
			this.tableCell19.Weight = 1.0559322958129851D;
			// 
			// tableCell20
			// 
			this.tableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DurationInDays]")});
			this.tableCell20.Multiline = true;
			this.tableCell20.Name = "tableCell20";
			this.tableCell20.StylePriority.UseBorders = false;
			this.tableCell20.StylePriority.UseTextAlignment = false;
			this.tableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell20.TextFormatString = "{0:N0}";
			this.tableCell20.Weight = 0.82621536431947806D;
			// 
			// tableCell22
			// 
			this.tableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reason]")});
			this.tableCell22.Multiline = true;
			this.tableCell22.Name = "tableCell22";
			this.tableCell22.StylePriority.UseBorders = false;
			this.tableCell22.StylePriority.UseTextAlignment = false;
			this.tableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.tableCell22.Weight = 1.9669079637653994D;
			// 
			// label2
			// 
			this.label2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'ក្នុង​ \'+[InMonthKH]")});
			this.label2.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 12F, DevExpress.Drawing.DXFontStyle.Bold);
			this.label2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28F);
			this.label2.Multiline = true;
			this.label2.Name = "label2";
			this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.label2.SizeF = new System.Drawing.SizeF(1290F, 28F);
			this.label2.StylePriority.UseFont = false;
			this.label2.StylePriority.UseTextAlignment = false;
			this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 13F, DevExpress.Drawing.DXFontStyle.Bold);
			this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.label1.Multiline = true;
			this.label1.Name = "label1";
			this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.label1.SizeF = new System.Drawing.SizeF(1290F, 28F);
			this.label1.StylePriority.UseFont = false;
			this.label1.StylePriority.UseTextAlignment = false;
			this.label1.Text = "របាយការណ៍បុគ្គលិកសុំច្បាប់ឈប់សម្រាកលំហែមាតុភាព";
			this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// table1
			// 
			this.table1.BackColor = System.Drawing.Color.Gray;
			this.table1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.table1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.table1.ForeColor = System.Drawing.Color.White;
			this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table1.Name = "table1";
			this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
			this.table1.SizeF = new System.Drawing.SizeF(1290F, 25F);
			this.table1.StylePriority.UseBackColor = false;
			this.table1.StylePriority.UseBorders = false;
			this.table1.StylePriority.UseFont = false;
			this.table1.StylePriority.UseForeColor = false;
			this.table1.StylePriority.UseTextAlignment = false;
			this.table1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.tableCell10,
            this.tableCell21});
			this.tableRow1.Name = "tableRow1";
			this.tableRow1.Weight = 1D;
			// 
			// tableCell1
			// 
			this.tableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell1.Multiline = true;
			this.tableCell1.Name = "tableCell1";
			this.tableCell1.StylePriority.UseBorders = false;
			this.tableCell1.Text = "ល.រ";
			this.tableCell1.Weight = 0.4D;
			// 
			// tableCell2
			// 
			this.tableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell2.Multiline = true;
			this.tableCell2.Name = "tableCell2";
			this.tableCell2.StylePriority.UseBorders = false;
			this.tableCell2.Text = "អត្តលេខបុគ្គលិក";
			this.tableCell2.Weight = 1.0919169611307422D;
			// 
			// tableCell3
			// 
			this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell3.Multiline = true;
			this.tableCell3.Name = "tableCell3";
			this.tableCell3.StylePriority.UseBorders = false;
			this.tableCell3.Text = "នាម និងគោត្តនាម";
			this.tableCell3.Weight = 1.3435141923992038D;
			// 
			// tableCell4
			// 
			this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell4.Multiline = true;
			this.tableCell4.Name = "tableCell4";
			this.tableCell4.StylePriority.UseBorders = false;
			this.tableCell4.Text = "តួនាទី";
			this.tableCell4.Weight = 1.8759430531478183D;
			// 
			// tableCell5
			// 
			this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell5.Multiline = true;
			this.tableCell5.Name = "tableCell5";
			this.tableCell5.StylePriority.UseBorders = false;
			this.tableCell5.Text = "ទីតាំងបម្រើការងារ";
			this.tableCell5.Weight = 1.8984752237164937D;
			// 
			// tableCell6
			// 
			this.tableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell6.Multiline = true;
			this.tableCell6.Name = "tableCell6";
			this.tableCell6.StylePriority.UseBorders = false;
			this.tableCell6.Text = "ថ្ងៃចូលបម្រើការងារ";
			this.tableCell6.Weight = 1.2604019865972833D;
			// 
			// tableCell7
			// 
			this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell7.Multiline = true;
			this.tableCell7.Name = "tableCell7";
			this.tableCell7.StylePriority.UseBorders = false;
			this.tableCell7.Text = "កាលបរិច្ឆេទស្នើសុំ";
			this.tableCell7.Weight = 1.2603998595328727D;
			// 
			// tableCell8
			// 
			this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell8.Multiline = true;
			this.tableCell8.Name = "tableCell8";
			this.tableCell8.StylePriority.UseBorders = false;
			this.tableCell8.Text = "កាលបរិច្ឆេទ\r\nឈប់សម្រាក";
			this.tableCell8.Weight = 1.2603996822775032D;
			// 
			// tableCell9
			// 
			this.tableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell9.Multiline = true;
			this.tableCell9.Name = "tableCell9";
			this.tableCell9.StylePriority.UseBorders = false;
			this.tableCell9.Text = "កាលបរិច្ឆេទ\r\nចូលបម្រើការងារ";
			this.tableCell9.Weight = 1.2603996822775048D;
			// 
			// tableCell10
			// 
			this.tableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell10.Multiline = true;
			this.tableCell10.Name = "tableCell10";
			this.tableCell10.StylePriority.UseBorders = false;
			this.tableCell10.Text = "រយៈពេលឈប់\r\nសម្រាក";
			this.tableCell10.Weight = 0.98619925562989963D;
			// 
			// tableCell21
			// 
			this.tableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell21.Multiline = true;
			this.tableCell21.Name = "tableCell21";
			this.tableCell21.StylePriority.UseBorders = false;
			this.tableCell21.Text = "មូលហេតុ";
			this.tableCell21.Weight = 2.3477741315592309D;
			// 
			// label3
			// 
			this.label3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.label3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]")});
			this.label3.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 10F);
			this.label3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.label3.Multiline = true;
			this.label3.Name = "label3";
			this.label3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.label3.SizeF = new System.Drawing.SizeF(1290F, 23F);
			this.label3.StylePriority.UseBorders = false;
			this.label3.StylePriority.UseFont = false;
			this.label3.StylePriority.UseTextAlignment = false;
			this.label3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// sqlDataSource1
			// 
			this.sqlDataSource1.ConnectionName = "ReportConnectionString";
			this.sqlDataSource1.Name = "sqlDataSource1";
			storedProcQuery1.Name = "HR_RPT_MaternityLeave";
			queryParameter1.Name = "@Company";
			queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter1.Value = new DevExpress.DataAccess.Expression("?Company", typeof(string));
			queryParameter2.Name = "@Branch";
			queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter2.Value = new DevExpress.DataAccess.Expression("?Branch", typeof(string));
			queryParameter3.Name = "@Division";
			queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter3.Value = new DevExpress.DataAccess.Expression("?Division", typeof(string));
			queryParameter4.Name = "@BusinessUnit";
			queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter4.Value = new DevExpress.DataAccess.Expression("?BusinessUnit", typeof(string));
			queryParameter5.Name = "@Department";
			queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter5.Value = new DevExpress.DataAccess.Expression("?Department", typeof(string));
			queryParameter6.Name = "@Office";
			queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter6.Value = new DevExpress.DataAccess.Expression("?Office", typeof(string));
			queryParameter7.Name = "@Section";
			queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter7.Value = new DevExpress.DataAccess.Expression("?Section", typeof(string));
			queryParameter8.Name = "@Group";
			queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter8.Value = new DevExpress.DataAccess.Expression("?Group", typeof(string));
			queryParameter9.Name = "@Position";
			queryParameter9.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter9.Value = new DevExpress.DataAccess.Expression("?Position", typeof(string));
			queryParameter10.Name = "@Level";
			queryParameter10.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter10.Value = new DevExpress.DataAccess.Expression("?Level", typeof(string));
			queryParameter11.Name = "@InMonth";
			queryParameter11.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter11.Value = new DevExpress.DataAccess.Expression("?InMonth", typeof(System.DateTime));
			storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4,
            queryParameter5,
            queryParameter6,
            queryParameter7,
            queryParameter8,
            queryParameter9,
            queryParameter10,
            queryParameter11});
			storedProcQuery1.StoredProcName = "HR_RPT_MaternityLeave";
			this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
			this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
			// 
			// Company
			// 
			this.Company.Name = "Company";
			// 
			// Branch
			// 
			this.Branch.Name = "Branch";
			// 
			// Division
			// 
			this.Division.Name = "Division";
			// 
			// BusinessUnit
			// 
			this.BusinessUnit.Name = "BusinessUnit";
			// 
			// Department
			// 
			this.Department.Name = "Department";
			// 
			// Office
			// 
			this.Office.Name = "Office";
			// 
			// Section
			// 
			this.Section.Name = "Section";
			// 
			// Group
			// 
			this.Group.Name = "Group";
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
			this.InMonth.ValueInfo = "2025-07-22";
			// 
			// Rpt_Maternity_Leave
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader,
            this.GroupHeader1});
			this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
			this.DataMember = "HR_RPT_MaternityLeave";
			this.DataSource = this.sqlDataSource1;
			this.DisplayName = "Rpt_Maternity_Leave";
			this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
			this.Margins = new DevExpress.Drawing.DXMargins(20F, 20F, 20F, 20F);
			this.PageWidth = 1330;
			this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Company,
            this.Branch,
            this.Division,
            this.BusinessUnit,
            this.Department,
            this.Office,
            this.Section,
            this.Group,
            this.Position,
            this.Level,
            this.InMonth});
			this.Version = "23.2";
			((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.XRTable table2;
		private DevExpress.XtraReports.UI.XRTableRow tableRow2;
		private DevExpress.XtraReports.UI.XRTableCell tableCell11;
		private DevExpress.XtraReports.UI.XRTableCell tableCell12;
		private DevExpress.XtraReports.UI.XRTableCell tableCell13;
		private DevExpress.XtraReports.UI.XRTableCell tableCell14;
		private DevExpress.XtraReports.UI.XRTableCell tableCell15;
		private DevExpress.XtraReports.UI.XRTableCell tableCell16;
		private DevExpress.XtraReports.UI.XRTableCell tableCell17;
		private DevExpress.XtraReports.UI.XRTableCell tableCell18;
		private DevExpress.XtraReports.UI.XRTableCell tableCell19;
		private DevExpress.XtraReports.UI.XRTableCell tableCell20;
		private DevExpress.XtraReports.UI.XRTableCell tableCell22;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.UI.XRLabel label2;
		private DevExpress.XtraReports.UI.XRLabel label1;
		private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
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
		private DevExpress.XtraReports.UI.XRTableCell tableCell21;
		private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
		private DevExpress.XtraReports.UI.XRLabel label3;
		private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
		private DevExpress.XtraReports.Parameters.Parameter Company;
		private DevExpress.XtraReports.Parameters.Parameter Branch;
		private DevExpress.XtraReports.Parameters.Parameter Division;
		private DevExpress.XtraReports.Parameters.Parameter BusinessUnit;
		private DevExpress.XtraReports.Parameters.Parameter Department;
		private DevExpress.XtraReports.Parameters.Parameter Office;
		private DevExpress.XtraReports.Parameters.Parameter Section;
		private DevExpress.XtraReports.Parameters.Parameter Group;
		private DevExpress.XtraReports.Parameters.Parameter Position;
		private DevExpress.XtraReports.Parameters.Parameter Level;
		private DevExpress.XtraReports.Parameters.Parameter InMonth;
	}
}
