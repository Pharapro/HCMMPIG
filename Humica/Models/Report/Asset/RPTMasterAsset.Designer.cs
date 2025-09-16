namespace HUMICA.Models.Report.Asset
{
	partial class RPTMasterAsset
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPTMasterAsset));
			DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
			DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.table2 = new DevExpress.XtraReports.UI.XRTable();
			this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.tableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
			this.table1 = new DevExpress.XtraReports.UI.XRTable();
			this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
			this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
			this.label15 = new DevExpress.XtraReports.UI.XRLabel();
			this.pictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.label17 = new DevExpress.XtraReports.UI.XRLabel();
			this.label2 = new DevExpress.XtraReports.UI.XRLabel();
			this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
			this.AssetCode = new DevExpress.XtraReports.Parameters.Parameter();
			this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
			this.Status = new DevExpress.XtraReports.Parameters.Parameter();
			this.Type = new DevExpress.XtraReports.Parameters.Parameter();
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
			this.Detail.HeightF = 27.17F;
			this.Detail.Name = "Detail";
			this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("AssetCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 20F;
			this.BottomMargin.Name = "BottomMargin";
			// 
			// PageHeader
			// 
			this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
			this.PageHeader.HeightF = 27.17F;
			this.PageHeader.Name = "PageHeader";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label15,
            this.pictureBox2,
            this.label17,
            this.label2});
			this.ReportHeader.HeightF = 117.0834F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// table2
			// 
			this.table2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.table2.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
			this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table2.Name = "table2";
			this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
			this.table2.SizeF = new System.Drawing.SizeF(1892F, 27.17F);
			this.table2.StylePriority.UseBorders = false;
			this.table2.StylePriority.UseFont = false;
			this.table2.StylePriority.UseTextAlignment = false;
			this.table2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// tableRow2
			// 
			this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell20,
            this.tableCell21,
            this.tableCell24,
            this.tableCell25,
            this.tableCell27,
            this.tableCell28,
            this.tableCell29,
            this.tableCell30,
            this.tableCell31,
            this.tableCell32,
            this.tableCell33,
            this.tableCell34,
            this.tableCell36,
            this.tableCell38,
            this.tableCell3,
            this.tableCell18});
			this.tableRow2.Name = "tableRow2";
			this.tableRow2.Weight = 1D;
			// 
			// tableCell20
			// 
			this.tableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
			this.tableCell20.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell20.Multiline = true;
			this.tableCell20.Name = "tableCell20";
			this.tableCell20.StylePriority.UseBorders = false;
			this.tableCell20.StylePriority.UseFont = false;
			xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
			this.tableCell20.Summary = xrSummary1;
			this.tableCell20.Weight = 0.36666526250976483D;
			// 
			// tableCell21
			// 
			this.tableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AssetCode]")});
			this.tableCell21.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell21.Multiline = true;
			this.tableCell21.Name = "tableCell21";
			this.tableCell21.StylePriority.UseBorders = false;
			this.tableCell21.StylePriority.UseFont = false;
			this.tableCell21.StylePriority.UseTextAlignment = false;
			this.tableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell21.Weight = 1.5249977735987483D;
			// 
			// tableCell24
			// 
			this.tableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[assetClass]")});
			this.tableCell24.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell24.Multiline = true;
			this.tableCell24.Name = "tableCell24";
			this.tableCell24.StylePriority.UseBorders = false;
			this.tableCell24.StylePriority.UseFont = false;
			this.tableCell24.StylePriority.UseTextAlignment = false;
			this.tableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell24.Weight = 1.9333327622960002D;
			// 
			// tableCell25
			// 
			this.tableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[assetDes]")});
			this.tableCell25.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell25.Multiline = true;
			this.tableCell25.Name = "tableCell25";
			this.tableCell25.StylePriority.UseBorders = false;
			this.tableCell25.StylePriority.UseFont = false;
			this.tableCell25.StylePriority.UseTextAlignment = false;
			this.tableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell25.Weight = 2.3333333695835092D;
			// 
			// tableCell27
			// 
			this.tableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Model]")});
			this.tableCell27.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell27.Multiline = true;
			this.tableCell27.Name = "tableCell27";
			this.tableCell27.StylePriority.UseBorders = false;
			this.tableCell27.StylePriority.UseFont = false;
			this.tableCell27.StylePriority.UseTextAlignment = false;
			this.tableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell27.Weight = 2.2166652000464353D;
			// 
			// tableCell28
			// 
			this.tableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SerialNumber]")});
			this.tableCell28.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell28.Multiline = true;
			this.tableCell28.Name = "tableCell28";
			this.tableCell28.StylePriority.UseBorders = false;
			this.tableCell28.StylePriority.UseFont = false;
			this.tableCell28.Weight = 1.6333330913101225D;
			// 
			// tableCell29
			// 
			this.tableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StatusUse]")});
			this.tableCell29.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell29.Multiline = true;
			this.tableCell29.Name = "tableCell29";
			this.tableCell29.StylePriority.UseBorders = false;
			this.tableCell29.StylePriority.UseFont = false;
			this.tableCell29.StylePriority.UseTextAlignment = false;
			this.tableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell29.Weight = 0.68333296827761414D;
			// 
			// tableCell30
			// 
			this.tableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell30.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Condition]")});
			this.tableCell30.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell30.Multiline = true;
			this.tableCell30.Name = "tableCell30";
			this.tableCell30.StylePriority.UseBorders = false;
			this.tableCell30.StylePriority.UseFont = false;
			this.tableCell30.StylePriority.UseTextAlignment = false;
			this.tableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell30.Weight = 0.68333296827761425D;
			// 
			// tableCell31
			// 
			this.tableCell31.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell31.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString(\'{0:0}\', [Qty])\n")});
			this.tableCell31.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell31.Multiline = true;
			this.tableCell31.Name = "tableCell31";
			this.tableCell31.StylePriority.UseBorders = false;
			this.tableCell31.StylePriority.UseFont = false;
			this.tableCell31.StylePriority.UseTextAlignment = false;
			this.tableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell31.Weight = 0.49166736160454189D;
			// 
			// tableCell32
			// 
			this.tableCell32.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell32.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ReceiptDate]")});
			this.tableCell32.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell32.Multiline = true;
			this.tableCell32.Name = "tableCell32";
			this.tableCell32.StylePriority.UseBorders = false;
			this.tableCell32.StylePriority.UseFont = false;
			this.tableCell32.TextFormatString = "{0:dd-MMM-yyyy}";
			this.tableCell32.Weight = 1.0249996096160348D;
			// 
			// tableCell33
			// 
			this.tableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[UsefulLifeYear]")});
			this.tableCell33.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell33.Multiline = true;
			this.tableCell33.Name = "tableCell33";
			this.tableCell33.StylePriority.UseBorders = false;
			this.tableCell33.StylePriority.UseFont = false;
			this.tableCell33.TextFormatString = "{0:N2}";
			this.tableCell33.Weight = 0.8083323300514812D;
			// 
			// tableCell34
			// 
			this.tableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Floor]")});
			this.tableCell34.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell34.Multiline = true;
			this.tableCell34.Name = "tableCell34";
			this.tableCell34.StylePriority.UseBorders = false;
			this.tableCell34.StylePriority.UseFont = false;
			this.tableCell34.StylePriority.UseTextAlignment = false;
			this.tableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.tableCell34.Weight = 0.67500110538264857D;
			// 
			// tableCell36
			// 
			this.tableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WarrantyExpirationDate]")});
			this.tableCell36.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell36.Multiline = true;
			this.tableCell36.Name = "tableCell36";
			this.tableCell36.StylePriority.UseBorders = false;
			this.tableCell36.StylePriority.UseFont = false;
			this.tableCell36.TextFormatString = "{0:dd-MMM-yyyy}";
			this.tableCell36.Weight = 1.49166357767275D;
			// 
			// tableCell38
			// 
			this.tableCell38.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell38.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OPNumber]")});
			this.tableCell38.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell38.Multiline = true;
			this.tableCell38.Name = "tableCell38";
			this.tableCell38.StylePriority.UseBorders = false;
			this.tableCell38.StylePriority.UseFont = false;
			this.tableCell38.Weight = 0.9183396969035228D;
			// 
			// tableCell3
			// 
			this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ReceiptNumber]")});
			this.tableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell3.Multiline = true;
			this.tableCell3.Name = "tableCell3";
			this.tableCell3.StylePriority.UseBorders = false;
			this.tableCell3.StylePriority.UseFont = false;
			this.tableCell3.Text = "tableCell3";
			this.tableCell3.Weight = 0.98166366024930729D;
			// 
			// tableCell18
			// 
			this.tableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BuildingNumber]")});
			this.tableCell18.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
			this.tableCell18.Multiline = true;
			this.tableCell18.Name = "tableCell18";
			this.tableCell18.StylePriority.UseBorders = false;
			this.tableCell18.StylePriority.UseFont = false;
			this.tableCell18.Text = "tableCell18";
			this.tableCell18.Weight = 1.1533358369306144D;
			// 
			// table1
			// 
			this.table1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(73)))), ((int)(((byte)(125)))));
			this.table1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.table1.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
			this.table1.ForeColor = System.Drawing.Color.White;
			this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table1.Name = "table1";
			this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
			this.table1.SizeF = new System.Drawing.SizeF(1892F, 27.17F);
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
            this.tableCell5,
            this.tableCell6,
            this.tableCell8,
            this.tableCell9,
            this.tableCell10,
            this.tableCell11,
            this.tableCell12,
            this.tableCell13,
            this.tableCell14,
            this.tableCell15,
            this.tableCell17,
            this.tableCell19,
            this.tableCell7,
            this.tableCell16});
			this.tableRow1.Name = "tableRow1";
			this.tableRow1.Weight = 1D;
			// 
			// tableCell1
			// 
			this.tableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell1.Multiline = true;
			this.tableCell1.Name = "tableCell1";
			this.tableCell1.StylePriority.UseBorders = false;
			this.tableCell1.StylePriority.UseFont = false;
			this.tableCell1.Text = "NO";
			this.tableCell1.Weight = 0.36666531080353726D;
			// 
			// tableCell2
			// 
			this.tableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell2.Multiline = true;
			this.tableCell2.Name = "tableCell2";
			this.tableCell2.StylePriority.UseBorders = false;
			this.tableCell2.StylePriority.UseFont = false;
			this.tableCell2.Text = "Asset Code";
			this.tableCell2.Weight = 1.5249976914154901D;
			// 
			// tableCell5
			// 
			this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell5.Multiline = true;
			this.tableCell5.Name = "tableCell5";
			this.tableCell5.StylePriority.UseBorders = false;
			this.tableCell5.StylePriority.UseFont = false;
			this.tableCell5.Text = "Asset Class";
			this.tableCell5.Weight = 1.9333324571202803D;
			// 
			// tableCell6
			// 
			this.tableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell6.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell6.Multiline = true;
			this.tableCell6.Name = "tableCell6";
			this.tableCell6.StylePriority.UseBorders = false;
			this.tableCell6.StylePriority.UseFont = false;
			this.tableCell6.Text = "Description";
			this.tableCell6.Weight = 2.3333324540563383D;
			// 
			// tableCell8
			// 
			this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell8.Multiline = true;
			this.tableCell8.Name = "tableCell8";
			this.tableCell8.StylePriority.UseBorders = false;
			this.tableCell8.StylePriority.UseFont = false;
			this.tableCell8.Text = "Model";
			this.tableCell8.Weight = 2.2166657841520476D;
			// 
			// tableCell9
			// 
			this.tableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell9.Multiline = true;
			this.tableCell9.Name = "tableCell9";
			this.tableCell9.StylePriority.UseBorders = false;
			this.tableCell9.StylePriority.UseFont = false;
			this.tableCell9.Text = "Serial Number";
			this.tableCell9.Weight = 1.6333330769172429D;
			// 
			// tableCell10
			// 
			this.tableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell10.Multiline = true;
			this.tableCell10.Name = "tableCell10";
			this.tableCell10.StylePriority.UseBorders = false;
			this.tableCell10.StylePriority.UseFont = false;
			this.tableCell10.Text = "Status";
			this.tableCell10.Weight = 0.68333235792617475D;
			// 
			// tableCell11
			// 
			this.tableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell11.Multiline = true;
			this.tableCell11.Name = "tableCell11";
			this.tableCell11.StylePriority.UseBorders = false;
			this.tableCell11.StylePriority.UseFont = false;
			this.tableCell11.Text = "Condition";
			this.tableCell11.Weight = 0.68333357862905364D;
			// 
			// tableCell12
			// 
			this.tableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell12.Multiline = true;
			this.tableCell12.Name = "tableCell12";
			this.tableCell12.StylePriority.UseBorders = false;
			this.tableCell12.StylePriority.UseFont = false;
			this.tableCell12.Text = "Quality";
			this.tableCell12.Weight = 0.49166736160454177D;
			// 
			// tableCell13
			// 
			this.tableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell13.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell13.Multiline = true;
			this.tableCell13.Name = "tableCell13";
			this.tableCell13.StylePriority.UseBorders = false;
			this.tableCell13.StylePriority.UseFont = false;
			this.tableCell13.Text = "Aquisition Date";
			this.tableCell13.Weight = 1.0249997622038944D;
			// 
			// tableCell14
			// 
			this.tableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell14.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell14.Multiline = true;
			this.tableCell14.Name = "tableCell14";
			this.tableCell14.StylePriority.UseBorders = false;
			this.tableCell14.StylePriority.UseFont = false;
			this.tableCell14.Text = "Useful life";
			this.tableCell14.Weight = 0.80833233005148053D;
			// 
			// tableCell15
			// 
			this.tableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell15.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell15.Multiline = true;
			this.tableCell15.Name = "tableCell15";
			this.tableCell15.StylePriority.UseBorders = false;
			this.tableCell15.StylePriority.UseFont = false;
			this.tableCell15.Text = "Floor";
			this.tableCell15.Weight = 0.67500110270565394D;
			// 
			// tableCell17
			// 
			this.tableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell17.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell17.Multiline = true;
			this.tableCell17.Name = "tableCell17";
			this.tableCell17.StylePriority.UseBorders = false;
			this.tableCell17.StylePriority.UseFont = false;
			this.tableCell17.Text = "WarrantyExpirationDate";
			this.tableCell17.Weight = 1.491663501378838D;
			// 
			// tableCell19
			// 
			this.tableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell19.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell19.Multiline = true;
			this.tableCell19.Name = "tableCell19";
			this.tableCell19.StylePriority.UseBorders = false;
			this.tableCell19.StylePriority.UseFont = false;
			this.tableCell19.Text = "OPNumber";
			this.tableCell19.Weight = 0.91833969607358568D;
			// 
			// tableCell7
			// 
			this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell7.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell7.Multiline = true;
			this.tableCell7.Name = "tableCell7";
			this.tableCell7.StylePriority.UseBorders = false;
			this.tableCell7.StylePriority.UseFont = false;
			this.tableCell7.Text = "ReceiptNumber";
			this.tableCell7.Weight = 0.9816635842499094D;
			// 
			// tableCell16
			// 
			this.tableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.tableCell16.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
			this.tableCell16.Multiline = true;
			this.tableCell16.Name = "tableCell16";
			this.tableCell16.StylePriority.UseBorders = false;
			this.tableCell16.StylePriority.UseFont = false;
			this.tableCell16.Text = "BuildingNumber";
			this.tableCell16.Weight = 1.1533361405804208D;
			// 
			// label15
			// 
			this.label15.BorderWidth = 3F;
			this.label15.Font = new DevExpress.Drawing.DXFont("Khmer OS Moul Light", 11F);
			this.label15.LocationFloat = new DevExpress.Utils.PointFloat(1666.167F, 0F);
			this.label15.Multiline = true;
			this.label15.Name = "label15";
			this.label15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 2, 100F);
			this.label15.SizeF = new System.Drawing.SizeF(225.8334F, 29.25F);
			this.label15.StylePriority.UseBorderWidth = false;
			this.label15.StylePriority.UseFont = false;
			this.label15.StylePriority.UsePadding = false;
			this.label15.StylePriority.UseTextAlignment = false;
			this.label15.Text = "ព្រះរាជាណាចក្រកម្ពុជា";
			this.label15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// pictureBox2
			// 
			this.pictureBox2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("pictureBox2.ImageSource"));
			this.pictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(1666.167F, 58.49999F);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.SizeF = new System.Drawing.SizeF(225.8334F, 30.00001F);
			this.pictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
			// 
			// label17
			// 
			this.label17.BorderWidth = 3F;
			this.label17.Font = new DevExpress.Drawing.DXFont("Khmer OS Moul Light", 11F);
			this.label17.LocationFloat = new DevExpress.Utils.PointFloat(1666.167F, 29.25F);
			this.label17.Multiline = true;
			this.label17.Name = "label17";
			this.label17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 2, 100F);
			this.label17.SizeF = new System.Drawing.SizeF(225.8334F, 29.25F);
			this.label17.StylePriority.UseBorderWidth = false;
			this.label17.StylePriority.UseFont = false;
			this.label17.StylePriority.UsePadding = false;
			this.label17.StylePriority.UseTextAlignment = false;
			this.label17.Text = "ជាតិ សាសនា ព្រះមហាក្សត្រ";
			this.label17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new DevExpress.Drawing.DXFont("Times New Roman", 14F, DevExpress.Drawing.DXFontStyle.Bold);
			this.label2.LocationFloat = new DevExpress.Utils.PointFloat(189.1664F, 83.75005F);
			this.label2.Multiline = true;
			this.label2.Name = "label2";
			this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.label2.SizeF = new System.Drawing.SizeF(1397.5F, 33.33333F);
			this.label2.StylePriority.UseFont = false;
			this.label2.StylePriority.UseTextAlignment = false;
			this.label2.Text = "Masterlist Asset Record";
			this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// sqlDataSource1
			// 
			this.sqlDataSource1.ConnectionName = "ReportConnectionString";
			this.sqlDataSource1.Name = "sqlDataSource1";
			storedProcQuery1.Name = "HR_RPT_AssetMaster";
			queryParameter1.Name = "@AssetCode";
			queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter1.Value = new DevExpress.DataAccess.Expression("?AssetCode", typeof(string));
			queryParameter2.Name = "@Branch";
			queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter2.Value = new DevExpress.DataAccess.Expression("?Branch", typeof(string));
			queryParameter3.Name = "@Status";
			queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter3.Value = new DevExpress.DataAccess.Expression("?Status", typeof(string));
			queryParameter4.Name = "@Type";
			queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
			queryParameter4.Value = new DevExpress.DataAccess.Expression("?Type", typeof(string));
			storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4});
			storedProcQuery1.StoredProcName = "HR_RPT_AssetMaster";
			this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
			this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
			// 
			// AssetCode
			// 
			this.AssetCode.Name = "AssetCode";
			// 
			// Branch
			// 
			this.Branch.Name = "Branch";
			// 
			// Status
			// 
			this.Status.Name = "Status";
			// 
			// Type
			// 
			this.Type.Name = "Type";
			// 
			// RPTMasterAsset
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader});
			this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
			this.DataMember = "HR_RPT_AssetMaster";
			this.DataSource = this.sqlDataSource1;
			this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
			this.Margins = new DevExpress.Drawing.DXMargins(20F, 20F, 20F, 20F);
			this.PageWidth = 1932;
			this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.AssetCode,
            this.Branch,
            this.Status,
            this.Type});
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
		private DevExpress.XtraReports.UI.XRTableCell tableCell20;
		private DevExpress.XtraReports.UI.XRTableCell tableCell21;
		private DevExpress.XtraReports.UI.XRTableCell tableCell24;
		private DevExpress.XtraReports.UI.XRTableCell tableCell25;
		private DevExpress.XtraReports.UI.XRTableCell tableCell27;
		private DevExpress.XtraReports.UI.XRTableCell tableCell28;
		private DevExpress.XtraReports.UI.XRTableCell tableCell29;
		private DevExpress.XtraReports.UI.XRTableCell tableCell30;
		private DevExpress.XtraReports.UI.XRTableCell tableCell31;
		private DevExpress.XtraReports.UI.XRTableCell tableCell32;
		private DevExpress.XtraReports.UI.XRTableCell tableCell33;
		private DevExpress.XtraReports.UI.XRTableCell tableCell34;
		private DevExpress.XtraReports.UI.XRTableCell tableCell36;
		private DevExpress.XtraReports.UI.XRTableCell tableCell38;
		private DevExpress.XtraReports.UI.XRTableCell tableCell3;
		private DevExpress.XtraReports.UI.XRTableCell tableCell18;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
		private DevExpress.XtraReports.UI.XRTable table1;
		private DevExpress.XtraReports.UI.XRTableRow tableRow1;
		private DevExpress.XtraReports.UI.XRTableCell tableCell1;
		private DevExpress.XtraReports.UI.XRTableCell tableCell2;
		private DevExpress.XtraReports.UI.XRTableCell tableCell5;
		private DevExpress.XtraReports.UI.XRTableCell tableCell6;
		private DevExpress.XtraReports.UI.XRTableCell tableCell8;
		private DevExpress.XtraReports.UI.XRTableCell tableCell9;
		private DevExpress.XtraReports.UI.XRTableCell tableCell10;
		private DevExpress.XtraReports.UI.XRTableCell tableCell11;
		private DevExpress.XtraReports.UI.XRTableCell tableCell12;
		private DevExpress.XtraReports.UI.XRTableCell tableCell13;
		private DevExpress.XtraReports.UI.XRTableCell tableCell14;
		private DevExpress.XtraReports.UI.XRTableCell tableCell15;
		private DevExpress.XtraReports.UI.XRTableCell tableCell17;
		private DevExpress.XtraReports.UI.XRTableCell tableCell19;
		private DevExpress.XtraReports.UI.XRTableCell tableCell7;
		private DevExpress.XtraReports.UI.XRTableCell tableCell16;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.UI.XRLabel label15;
		private DevExpress.XtraReports.UI.XRPictureBox pictureBox2;
		private DevExpress.XtraReports.UI.XRLabel label17;
		private DevExpress.XtraReports.UI.XRLabel label2;
		private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
		private DevExpress.XtraReports.Parameters.Parameter AssetCode;
		private DevExpress.XtraReports.Parameters.Parameter Branch;
		private DevExpress.XtraReports.Parameters.Parameter Status;
		private DevExpress.XtraReports.Parameters.Parameter Type;
	}
}
