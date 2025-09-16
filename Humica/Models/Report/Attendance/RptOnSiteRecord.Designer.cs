namespace Humica.Models.Report.Attendance
{
	partial class RptOnSiteRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptOnSiteRecord));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.pictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
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
            this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Company = new DevExpress.XtraReports.Parameters.Parameter();
            this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
            this.Division = new DevExpress.XtraReports.Parameters.Parameter();
            this.Department = new DevExpress.XtraReports.Parameters.Parameter();
            this.Level = new DevExpress.XtraReports.Parameters.Parameter();
            this.Position = new DevExpress.XtraReports.Parameters.Parameter();
            this.EmpCode = new DevExpress.XtraReports.Parameters.Parameter();
            this.FromDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.ToDate = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
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
            this.table3});
            this.Detail.HeightF = 22.99998F;
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
            this.pictureBox1,
            this.label2,
            this.label1});
            this.ReportHeader.HeightF = 67.5F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.PageHeader.HeightF = 33.03571F;
            this.PageHeader.Name = "PageHeader";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Dept", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 25F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // table3
            // 
            this.table3.BackColor = System.Drawing.Color.Transparent;
            this.table3.ForeColor = System.Drawing.Color.White;
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table3.Name = "table3";
            this.table3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table3.SizeF = new System.Drawing.SizeF(1386.5F, 22.99998F);
            this.table3.StylePriority.UseBackColor = false;
            this.table3.StylePriority.UseForeColor = false;
            this.table3.StylePriority.UseTextAlignment = false;
            this.table3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell25,
            this.tableCell26,
            this.tableCell27,
            this.tableCell28,
            this.tableCell29,
            this.tableCell30,
            this.tableCell31,
            this.tableCell32,
            this.tableCell33,
            this.tableCell34,
            this.tableCell35,
            this.tableCell36});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 1D;
            // 
            // tableCell25
            // 
            this.tableCell25.BackColor = System.Drawing.Color.Transparent;
            this.tableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")});
            this.tableCell25.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell25.ForeColor = System.Drawing.Color.Black;
            this.tableCell25.Multiline = true;
            this.tableCell25.Name = "tableCell25";
            this.tableCell25.StylePriority.UseBackColor = false;
            this.tableCell25.StylePriority.UseBorders = false;
            this.tableCell25.StylePriority.UseFont = false;
            this.tableCell25.StylePriority.UseForeColor = false;
            this.tableCell25.StylePriority.UseTextAlignment = false;
            this.tableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell25.Weight = 0.86581291348917522D;
            // 
            // tableCell26
            // 
            this.tableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployeeName]")});
            this.tableCell26.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell26.ForeColor = System.Drawing.Color.Black;
            this.tableCell26.Multiline = true;
            this.tableCell26.Name = "tableCell26";
            this.tableCell26.StylePriority.UseBorders = false;
            this.tableCell26.StylePriority.UseFont = false;
            this.tableCell26.StylePriority.UseForeColor = false;
            this.tableCell26.StylePriority.UseTextAlignment = false;
            this.tableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell26.Weight = 1.0418659848552079D;
            // 
            // tableCell27
            // 
            this.tableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Sex]==\'M\',\'ប្រុស\' , \'ស្រី\')")});
            this.tableCell27.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell27.ForeColor = System.Drawing.Color.Black;
            this.tableCell27.Multiline = true;
            this.tableCell27.Name = "tableCell27";
            this.tableCell27.StylePriority.UseBorders = false;
            this.tableCell27.StylePriority.UseFont = false;
            this.tableCell27.StylePriority.UseForeColor = false;
            this.tableCell27.Weight = 0.41119931425394751D;
            // 
            // tableCell28
            // 
            this.tableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Description]")});
            this.tableCell28.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell28.ForeColor = System.Drawing.Color.Black;
            this.tableCell28.Multiline = true;
            this.tableCell28.Name = "tableCell28";
            this.tableCell28.StylePriority.UseBorders = false;
            this.tableCell28.StylePriority.UseFont = false;
            this.tableCell28.StylePriority.UseForeColor = false;
            this.tableCell28.StylePriority.UseTextAlignment = false;
            this.tableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell28.Weight = 1.6549713568961526D;
            // 
            // tableCell29
            // 
            this.tableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartTime]")});
            this.tableCell29.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell29.ForeColor = System.Drawing.Color.Black;
            this.tableCell29.Multiline = true;
            this.tableCell29.Name = "tableCell29";
            this.tableCell29.StylePriority.UseBorders = false;
            this.tableCell29.StylePriority.UseFont = false;
            this.tableCell29.StylePriority.UseForeColor = false;
            this.tableCell29.TextFormatString = "{0:hh:mm tt}";
            this.tableCell29.Weight = 0.61124846756378459D;
            // 
            // tableCell30
            // 
            this.tableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell30.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EndTime]")});
            this.tableCell30.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell30.ForeColor = System.Drawing.Color.Black;
            this.tableCell30.Multiline = true;
            this.tableCell30.Name = "tableCell30";
            this.tableCell30.StylePriority.UseBorders = false;
            this.tableCell30.StylePriority.UseFont = false;
            this.tableCell30.StylePriority.UseForeColor = false;
            this.tableCell30.TextFormatString = "{0:hh:mm tt}";
            this.tableCell30.Weight = 0.61124755865442937D;
            // 
            // tableCell31
            // 
            this.tableCell31.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell31.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InDate]")});
            this.tableCell31.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell31.ForeColor = System.Drawing.Color.Black;
            this.tableCell31.Multiline = true;
            this.tableCell31.Name = "tableCell31";
            this.tableCell31.StylePriority.UseBorders = false;
            this.tableCell31.StylePriority.UseFont = false;
            this.tableCell31.StylePriority.UseForeColor = false;
            this.tableCell31.TextFormatString = "{0:dd-MMM-yyyy}";
            this.tableCell31.Weight = 0.80231758381159024D;
            // 
            // tableCell32
            // 
            this.tableCell32.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell32.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Longitude]")});
            this.tableCell32.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell32.ForeColor = System.Drawing.Color.Black;
            this.tableCell32.Multiline = true;
            this.tableCell32.Name = "tableCell32";
            this.tableCell32.StylePriority.UseBorders = false;
            this.tableCell32.StylePriority.UseFont = false;
            this.tableCell32.StylePriority.UseForeColor = false;
            this.tableCell32.StylePriority.UseTextAlignment = false;
            this.tableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell32.Weight = 1.2820516287086825D;
            // 
            // tableCell33
            // 
            this.tableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Latitude]")});
            this.tableCell33.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell33.ForeColor = System.Drawing.Color.Black;
            this.tableCell33.Multiline = true;
            this.tableCell33.Name = "tableCell33";
            this.tableCell33.StylePriority.UseBorders = false;
            this.tableCell33.StylePriority.UseFont = false;
            this.tableCell33.StylePriority.UseForeColor = false;
            this.tableCell33.StylePriority.UseTextAlignment = false;
            this.tableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell33.Weight = 1.2820502656407091D;
            // 
            // tableCell34
            // 
            this.tableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.tableCell34.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell34.ForeColor = System.Drawing.Color.Black;
            this.tableCell34.Multiline = true;
            this.tableCell34.Name = "tableCell34";
            this.tableCell34.StylePriority.UseBorders = false;
            this.tableCell34.StylePriority.UseFont = false;
            this.tableCell34.StylePriority.UseForeColor = false;
            this.tableCell34.StylePriority.UseTextAlignment = false;
            this.tableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell34.Weight = 1.9073619389361662D;
            // 
            // tableCell35
            // 
            this.tableCell35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reason]")});
            this.tableCell35.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell35.ForeColor = System.Drawing.Color.Black;
            this.tableCell35.Multiline = true;
            this.tableCell35.Name = "tableCell35";
            this.tableCell35.StylePriority.UseBorders = false;
            this.tableCell35.StylePriority.UseFont = false;
            this.tableCell35.StylePriority.UseForeColor = false;
            this.tableCell35.StylePriority.UseTextAlignment = false;
            this.tableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell35.Weight = 1.90736224184016D;
            // 
            // tableCell36
            // 
            this.tableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell36.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell36.ForeColor = System.Drawing.Color.Black;
            this.tableCell36.Multiline = true;
            this.tableCell36.Name = "tableCell36";
            this.tableCell36.StylePriority.UseBorders = false;
            this.tableCell36.StylePriority.UseFont = false;
            this.tableCell36.StylePriority.UseForeColor = false;
            this.tableCell36.StylePriority.UseTextAlignment = false;
            this.tableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell36.Weight = 1.3842979320515791D;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Images]")});
            this.pictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.SizeF = new System.Drawing.SizeF(192.1986F, 67.5F);
            this.pictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // label2
            // 
            this.label2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'From  \'+ [FromDate]+\'  To  \' +[ToDate]")});
            this.label2.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label2.LocationFloat = new DevExpress.Utils.PointFloat(523.5318F, 23.00001F);
            this.label2.Multiline = true;
            this.label2.Name = "label2";
            this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label2.SizeF = new System.Drawing.SizeF(339.1667F, 23F);
            this.label2.StylePriority.UseFont = false;
            this.label2.StylePriority.UseTextAlignment = false;
            this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.label2.TextFormatString = "{0:dd-MMM-yyyy}";
            // 
            // label1
            // 
            this.label1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(523.5318F, 0F);
            this.label1.Multiline = true;
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(339.1667F, 23F);
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "On-Site Report Detail";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // table1
            // 
            this.table1.BackColor = System.Drawing.Color.ForestGreen;
            this.table1.ForeColor = System.Drawing.Color.White;
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table1.Name = "table1";
            this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(1386.5F, 33.03571F);
            this.table1.StylePriority.UseBackColor = false;
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
            this.tableCell11,
            this.tableCell12});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell1.Multiline = true;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseBorders = false;
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "Emp. ID";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.86581291348917522D;
            // 
            // tableCell2
            // 
            this.tableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell2.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell2.Multiline = true;
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StylePriority.UseBorders = false;
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.Text = "Name";
            this.tableCell2.Weight = 1.0418659848552079D;
            // 
            // tableCell3
            // 
            this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell3.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell3.Multiline = true;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseBorders = false;
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.Text = "Sex";
            this.tableCell3.Weight = 0.41119931425394751D;
            // 
            // tableCell4
            // 
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell4.Multiline = true;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.Text = "Position";
            this.tableCell4.Weight = 1.6549713568961526D;
            // 
            // tableCell5
            // 
            this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell5.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell5.Multiline = true;
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StylePriority.UseBorders = false;
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.Text = "From\r\n(Time)";
            this.tableCell5.Weight = 0.61124846756378459D;
            // 
            // tableCell6
            // 
            this.tableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell6.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell6.Multiline = true;
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.Text = "To\r\n(Time)";
            this.tableCell6.Weight = 0.61124755865442937D;
            // 
            // tableCell7
            // 
            this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell7.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell7.Multiline = true;
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StylePriority.UseBorders = false;
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.Text = "Scan Date";
            this.tableCell7.Weight = 0.80231758381159024D;
            // 
            // tableCell8
            // 
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell8.Multiline = true;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.Text = "Longitute";
            this.tableCell8.Weight = 1.2820514772566853D;
            // 
            // tableCell9
            // 
            this.tableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell9.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell9.Multiline = true;
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StylePriority.UseBorders = false;
            this.tableCell9.StylePriority.UseFont = false;
            this.tableCell9.Text = "Latitude";
            this.tableCell9.Weight = 1.2820502656407091D;
            // 
            // tableCell10
            // 
            this.tableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell10.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell10.Multiline = true;
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StylePriority.UseBorders = false;
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.Text = "Location";
            this.tableCell10.Weight = 1.9073620903881634D;
            // 
            // tableCell11
            // 
            this.tableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell11.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell11.Multiline = true;
            this.tableCell11.Name = "tableCell11";
            this.tableCell11.StylePriority.UseBorders = false;
            this.tableCell11.StylePriority.UseFont = false;
            this.tableCell11.Text = "Remark";
            this.tableCell11.Weight = 1.9073622418401597D;
            // 
            // tableCell12
            // 
            this.tableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell12.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.tableCell12.Multiline = true;
            this.tableCell12.Name = "tableCell12";
            this.tableCell12.StylePriority.UseBorders = false;
            this.tableCell12.StylePriority.UseFont = false;
            this.tableCell12.Text = "File\r\n Attachment";
            this.tableCell12.Weight = 1.3842979320515787D;
            // 
            // table2
            // 
            this.table2.BackColor = System.Drawing.Color.Transparent;
            this.table2.ForeColor = System.Drawing.Color.White;
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table2.Name = "table2";
            this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(1386.5F, 25F);
            this.table2.StylePriority.UseBackColor = false;
            this.table2.StylePriority.UseForeColor = false;
            this.table2.StylePriority.UseTextAlignment = false;
            this.table2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell16});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1D;
            // 
            // tableCell16
            // 
            this.tableCell16.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.tableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Dept]")});
            this.tableCell16.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell16.ForeColor = System.Drawing.Color.Black;
            this.tableCell16.Multiline = true;
            this.tableCell16.Name = "tableCell16";
            this.tableCell16.StylePriority.UseBorders = false;
            this.tableCell16.StylePriority.UseFont = false;
            this.tableCell16.StylePriority.UseForeColor = false;
            this.tableCell16.StylePriority.UseTextAlignment = false;
            this.tableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell16.Weight = 4.0620680271392695D;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ReportConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "HR_RPT_OnsiteRecord";
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
            queryParameter5.Name = "@Level";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?Level", typeof(string));
            queryParameter6.Name = "@Position";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("?Position", typeof(string));
            queryParameter7.Name = "@EmpCode";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("?EmpCode", typeof(string));
            queryParameter8.Name = "@FromDate";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("?FromDate", typeof(System.DateTime));
            queryParameter9.Name = "@ToDate";
            queryParameter9.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter9.Value = new DevExpress.DataAccess.Expression("?ToDate", typeof(System.DateTime));
            storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4,
            queryParameter5,
            queryParameter6,
            queryParameter7,
            queryParameter8,
            queryParameter9});
            storedProcQuery1.StoredProcName = "HR_RPT_OnsiteRecord";
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
            // Department
            // 
            this.Department.Name = "Department";
            // 
            // Level
            // 
            this.Level.Name = "Level";
            // 
            // Position
            // 
            this.Position.Name = "Position";
            // 
            // EmpCode
            // 
            this.EmpCode.Name = "EmpCode";
            // 
            // FromDate
            // 
            this.FromDate.Name = "FromDate";
            this.FromDate.Type = typeof(System.DateTime);
            this.FromDate.ValueInfo = "2025-03-01";
            // 
            // ToDate
            // 
            this.ToDate.Name = "ToDate";
            this.ToDate.Type = typeof(System.DateTime);
            this.ToDate.ValueInfo = "2025-03-31";
            // 
            // RptOnSiteRecord
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
            this.DataMember = "HR_RPT_OnsiteRecord";
            this.DataSource = this.sqlDataSource1;
            this.DisplayName = "RptOnSiteRecord";
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(20F, 20F, 20F, 20F);
            this.PageWidth = 1427;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Company,
            this.Branch,
            this.Division,
            this.Department,
            this.Level,
            this.Position,
            this.EmpCode,
            this.FromDate,
            this.ToDate});
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell25;
        private DevExpress.XtraReports.UI.XRTableCell tableCell26;
        private DevExpress.XtraReports.UI.XRTableCell tableCell27;
        private DevExpress.XtraReports.UI.XRTableCell tableCell28;
        private DevExpress.XtraReports.UI.XRTableCell tableCell29;
        private DevExpress.XtraReports.UI.XRTableCell tableCell30;
        private DevExpress.XtraReports.UI.XRTableCell tableCell31;
        private DevExpress.XtraReports.UI.XRTableCell tableCell32;
        private DevExpress.XtraReports.UI.XRTableCell tableCell33;
        private DevExpress.XtraReports.UI.XRTableCell tableCell34;
        private DevExpress.XtraReports.UI.XRTableCell tableCell35;
        private DevExpress.XtraReports.UI.XRTableCell tableCell36;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRPictureBox pictureBox1;
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
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell16;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter Company;
        private DevExpress.XtraReports.Parameters.Parameter Branch;
        private DevExpress.XtraReports.Parameters.Parameter Division;
        private DevExpress.XtraReports.Parameters.Parameter Department;
        private DevExpress.XtraReports.Parameters.Parameter Level;
        private DevExpress.XtraReports.Parameters.Parameter Position;
        private DevExpress.XtraReports.Parameters.Parameter EmpCode;
        private DevExpress.XtraReports.Parameters.Parameter FromDate;
        private DevExpress.XtraReports.Parameters.Parameter ToDate;
    }
}
