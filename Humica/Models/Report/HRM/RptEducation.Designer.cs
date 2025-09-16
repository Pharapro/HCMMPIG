namespace Humica.Models.Report
{
    partial class RptEducation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptEducation));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.label16 = new DevExpress.XtraReports.UI.XRLabel();
            this.label17 = new DevExpress.XtraReports.UI.XRLabel();
            this.label18 = new DevExpress.XtraReports.UI.XRLabel();
            this.label19 = new DevExpress.XtraReports.UI.XRLabel();
            this.label20 = new DevExpress.XtraReports.UI.XRLabel();
            this.label21 = new DevExpress.XtraReports.UI.XRLabel();
            this.label22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo4 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.label2 = new DevExpress.XtraReports.UI.XRLabel();
            this.label4 = new DevExpress.XtraReports.UI.XRLabel();
            this.label13 = new DevExpress.XtraReports.UI.XRLabel();
            this.label14 = new DevExpress.XtraReports.UI.XRLabel();
            this.label15 = new DevExpress.XtraReports.UI.XRLabel();
            this.label5 = new DevExpress.XtraReports.UI.XRLabel();
            this.label6 = new DevExpress.XtraReports.UI.XRLabel();
            this.label7 = new DevExpress.XtraReports.UI.XRLabel();
            this.label9 = new DevExpress.XtraReports.UI.XRLabel();
            this.label10 = new DevExpress.XtraReports.UI.XRLabel();
            this.label8 = new DevExpress.XtraReports.UI.XRLabel();
            this.label12 = new DevExpress.XtraReports.UI.XRLabel();
            this.label11 = new DevExpress.XtraReports.UI.XRLabel();
            this.label3 = new DevExpress.XtraReports.UI.XRLabel();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
            this.Division = new DevExpress.XtraReports.Parameters.Parameter();
            this.Department = new DevExpress.XtraReports.Parameters.Parameter();
            this.Section = new DevExpress.XtraReports.Parameters.Parameter();
            this.Position = new DevExpress.XtraReports.Parameters.Parameter();
            this.Level = new DevExpress.XtraReports.Parameters.Parameter();
            this.Status = new DevExpress.XtraReports.Parameters.Parameter();
            this.Company = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label16,
            this.label17,
            this.label18,
            this.label19,
            this.label20,
            this.label21,
            this.label22});
            this.Detail.HeightF = 36.13085F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 5F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 5F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.PageHeader.HeightF = 43.33335F;
            this.PageHeader.Name = "PageHeader";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo4});
            this.ReportFooter.HeightF = 21.7071F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label2,
            this.label4,
            this.label13,
            this.label14,
            this.label15,
            this.label5,
            this.label6,
            this.label7,
            this.label9,
            this.label10,
            this.label8,
            this.label12,
            this.label11,
            this.label3,
            this.label1});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Empcode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Department", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Level", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AllName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Position", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Section", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 90.43587F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label16.CanGrow = false;
            this.label16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Result]")});
            this.label16.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.LocationFloat = new DevExpress.Utils.PointFloat(749.8912F, 0F);
            this.label16.Name = "label16";
            this.label16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label16.SizeF = new System.Drawing.SizeF(430.1088F, 36.13085F);
            this.label16.StylePriority.UseBackColor = false;
            this.label16.StylePriority.UseBorders = false;
            this.label16.StylePriority.UseFont = false;
            this.label16.StylePriority.UseForeColor = false;
            this.label16.StylePriority.UseTextAlignment = false;
            this.label16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label17.CanGrow = false;
            this.label17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EndDate]")});
            this.label17.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.LocationFloat = new DevExpress.Utils.PointFloat(650.4794F, 0F);
            this.label17.Name = "label17";
            this.label17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label17.SizeF = new System.Drawing.SizeF(99.4118F, 36.13085F);
            this.label17.StylePriority.UseBackColor = false;
            this.label17.StylePriority.UseBorders = false;
            this.label17.StylePriority.UseFont = false;
            this.label17.StylePriority.UseForeColor = false;
            this.label17.StylePriority.UseTextAlignment = false;
            this.label17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.label17.TextFormatString = "{0:yyyy-MM-dd}";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label18.CanGrow = false;
            this.label18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
            this.label18.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.LocationFloat = new DevExpress.Utils.PointFloat(571.6592F, 0F);
            this.label18.Name = "label18";
            this.label18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label18.SizeF = new System.Drawing.SizeF(78.82013F, 36.13085F);
            this.label18.StylePriority.UseBackColor = false;
            this.label18.StylePriority.UseBorders = false;
            this.label18.StylePriority.UseFont = false;
            this.label18.StylePriority.UseForeColor = false;
            this.label18.StylePriority.UseTextAlignment = false;
            this.label18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.label18.TextFormatString = "{0:yyyy-MM-dd}";
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label19.CanGrow = false;
            this.label19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EdcCenter]")});
            this.label19.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.LocationFloat = new DevExpress.Utils.PointFloat(359.4644F, 2.288818E-05F);
            this.label19.Name = "label19";
            this.label19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label19.SizeF = new System.Drawing.SizeF(212.1948F, 36.13083F);
            this.label19.StylePriority.UseBackColor = false;
            this.label19.StylePriority.UseBorders = false;
            this.label19.StylePriority.UseFont = false;
            this.label19.StylePriority.UseForeColor = false;
            this.label19.StylePriority.UseTextAlignment = false;
            this.label19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label20.CanGrow = false;
            this.label20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Major]")});
            this.label20.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.LocationFloat = new DevExpress.Utils.PointFloat(226.756F, 7.450581E-06F);
            this.label20.Name = "label20";
            this.label20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label20.SizeF = new System.Drawing.SizeF(132.7085F, 36.13084F);
            this.label20.StylePriority.UseBackColor = false;
            this.label20.StylePriority.UseBorders = false;
            this.label20.StylePriority.UseFont = false;
            this.label20.StylePriority.UseForeColor = false;
            this.label20.StylePriority.UseTextAlignment = false;
            this.label20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label21.CanGrow = false;
            this.label21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EducationType]")});
            this.label21.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.LocationFloat = new DevExpress.Utils.PointFloat(37.91656F, 2.288818E-05F);
            this.label21.Name = "label21";
            this.label21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label21.SizeF = new System.Drawing.SizeF(188.8394F, 36.13083F);
            this.label21.StylePriority.UseBackColor = false;
            this.label21.StylePriority.UseBorders = false;
            this.label21.StylePriority.UseFont = false;
            this.label21.StylePriority.UseForeColor = false;
            this.label21.StylePriority.UseTextAlignment = false;
            this.label21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label22.CanGrow = false;
            this.label22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
            this.label22.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F);
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.051758E-05F);
            this.label22.Name = "label22";
            this.label22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label22.SizeF = new System.Drawing.SizeF(37.91656F, 36.13082F);
            this.label22.StylePriority.UseBackColor = false;
            this.label22.StylePriority.UseBorders = false;
            this.label22.StylePriority.UseFont = false;
            this.label22.StylePriority.UseForeColor = false;
            this.label22.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.label22.Summary = xrSummary1;
            this.label22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999984F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1180F, 33.33337F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "របាយការណ៏ ប្រវត្តិនៃការសិក្សាបុគ្គលិក";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPageInfo4
            // 
            this.xrPageInfo4.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrPageInfo4.ForeColor = System.Drawing.Color.DimGray;
            this.xrPageInfo4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo4.Name = "xrPageInfo4";
            this.xrPageInfo4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo4.SizeF = new System.Drawing.SizeF(165.5833F, 20.83334F);
            this.xrPageInfo4.StylePriority.UseFont = false;
            this.xrPageInfo4.StylePriority.UseForeColor = false;
            this.xrPageInfo4.StylePriority.UseTextAlignment = false;
            this.xrPageInfo4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrPageInfo4.TextFormatString = "Page {0} of {1}";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label2.CanGrow = false;
            this.label2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'នាម និងគោត្តនាម:\'")});
            this.label2.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.LocationFloat = new DevExpress.Utils.PointFloat(300.8274F, 19.94398F);
            this.label2.Name = "label2";
            this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label2.SizeF = new System.Drawing.SizeF(124.0416F, 34.79163F);
            this.label2.StylePriority.UseBackColor = false;
            this.label2.StylePriority.UseBorders = false;
            this.label2.StylePriority.UseFont = false;
            this.label2.StylePriority.UseForeColor = false;
            this.label2.StylePriority.UseTextAlignment = false;
            this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label4.CanGrow = false;
            this.label4.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.LocationFloat = new DevExpress.Utils.PointFloat(872.4867F, 19.94401F);
            this.label4.Name = "label4";
            this.label4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label4.SizeF = new System.Drawing.SizeF(74.42453F, 34.79162F);
            this.label4.StylePriority.UseBackColor = false;
            this.label4.StylePriority.UseBorders = false;
            this.label4.StylePriority.UseFont = false;
            this.label4.StylePriority.UseForeColor = false;
            this.label4.StylePriority.UseTextAlignment = false;
            this.label4.Text = "តួនាទី:";
            this.label4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AllName]")});
            this.label13.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label13.LocationFloat = new DevExpress.Utils.PointFloat(424.8691F, 19.94398F);
            this.label13.Multiline = true;
            this.label13.Name = "label13";
            this.label13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label13.SizeF = new System.Drawing.SizeF(176.7858F, 34.79162F);
            this.label13.StylePriority.UseBackColor = false;
            this.label13.StylePriority.UseBorders = false;
            this.label13.StylePriority.UseFont = false;
            this.label13.StylePriority.UseTextAlignment = false;
            this.label13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.label14.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label14.LocationFloat = new DevExpress.Utils.PointFloat(946.9113F, 19.94401F);
            this.label14.Multiline = true;
            this.label14.Name = "label14";
            this.label14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label14.SizeF = new System.Drawing.SizeF(233.0887F, 34.79162F);
            this.label14.StylePriority.UseBackColor = false;
            this.label14.StylePriority.UseBorders = false;
            this.label14.StylePriority.UseFont = false;
            this.label14.StylePriority.UseTextAlignment = false;
            this.label14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label15.CanGrow = false;
            this.label15.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 54.73561F);
            this.label15.Name = "label15";
            this.label15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label15.SizeF = new System.Drawing.SizeF(37.91656F, 35.70025F);
            this.label15.StylePriority.UseBackColor = false;
            this.label15.StylePriority.UseBorders = false;
            this.label15.StylePriority.UseFont = false;
            this.label15.StylePriority.UseForeColor = false;
            this.label15.StylePriority.UseTextAlignment = false;
            this.label15.Text = "ល.រ";
            this.label15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label5.CanGrow = false;
            this.label5.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.LocationFloat = new DevExpress.Utils.PointFloat(37.91658F, 54.73561F);
            this.label5.Name = "label5";
            this.label5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label5.SizeF = new System.Drawing.SizeF(188.8394F, 35.70026F);
            this.label5.StylePriority.UseBackColor = false;
            this.label5.StylePriority.UseBorders = false;
            this.label5.StylePriority.UseFont = false;
            this.label5.StylePriority.UseForeColor = false;
            this.label5.StylePriority.UseTextAlignment = false;
            this.label5.Text = "កម្រិតវប្បធម៍";
            this.label5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label6.CanGrow = false;
            this.label6.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.LocationFloat = new DevExpress.Utils.PointFloat(226.756F, 54.73561F);
            this.label6.Name = "label6";
            this.label6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label6.SizeF = new System.Drawing.SizeF(132.7085F, 35.70026F);
            this.label6.StylePriority.UseBackColor = false;
            this.label6.StylePriority.UseBorders = false;
            this.label6.StylePriority.UseFont = false;
            this.label6.StylePriority.UseForeColor = false;
            this.label6.StylePriority.UseTextAlignment = false;
            this.label6.Text = "ឯកទេស";
            this.label6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label7.CanGrow = false;
            this.label7.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.LocationFloat = new DevExpress.Utils.PointFloat(359.4644F, 54.73561F);
            this.label7.Name = "label7";
            this.label7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label7.SizeF = new System.Drawing.SizeF(212.1948F, 35.70026F);
            this.label7.StylePriority.UseBackColor = false;
            this.label7.StylePriority.UseBorders = false;
            this.label7.StylePriority.UseFont = false;
            this.label7.StylePriority.UseForeColor = false;
            this.label7.StylePriority.UseTextAlignment = false;
            this.label7.Text = "គ្រឹះស្ថានសិក្សា";
            this.label7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label9.CanGrow = false;
            this.label9.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.LocationFloat = new DevExpress.Utils.PointFloat(571.6592F, 54.73558F);
            this.label9.Name = "label9";
            this.label9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label9.SizeF = new System.Drawing.SizeF(78.82013F, 35.70028F);
            this.label9.StylePriority.UseBackColor = false;
            this.label9.StylePriority.UseBorders = false;
            this.label9.StylePriority.UseFont = false;
            this.label9.StylePriority.UseForeColor = false;
            this.label9.StylePriority.UseTextAlignment = false;
            this.label9.Text = "ថ្ងៃចាប់ផ្ដើម";
            this.label9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label10.CanGrow = false;
            this.label10.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.LocationFloat = new DevExpress.Utils.PointFloat(650.4794F, 54.73558F);
            this.label10.Name = "label10";
            this.label10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label10.SizeF = new System.Drawing.SizeF(99.4118F, 35.70028F);
            this.label10.StylePriority.UseBackColor = false;
            this.label10.StylePriority.UseBorders = false;
            this.label10.StylePriority.UseFont = false;
            this.label10.StylePriority.UseForeColor = false;
            this.label10.StylePriority.UseTextAlignment = false;
            this.label10.Text = "ថ្ងៃបញ្ចប់";
            this.label10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label8.CanGrow = false;
            this.label8.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.LocationFloat = new DevExpress.Utils.PointFloat(749.8912F, 54.73558F);
            this.label8.Name = "label8";
            this.label8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label8.SizeF = new System.Drawing.SizeF(430.1088F, 35.70028F);
            this.label8.StylePriority.UseBackColor = false;
            this.label8.StylePriority.UseBorders = false;
            this.label8.StylePriority.UseFont = false;
            this.label8.StylePriority.UseForeColor = false;
            this.label8.StylePriority.UseTextAlignment = false;
            this.label8.Text = "ផ្សេងៗ";
            this.label8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]")});
            this.label12.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label12.LocationFloat = new DevExpress.Utils.PointFloat(676.0794F, 19.94398F);
            this.label12.Multiline = true;
            this.label12.Name = "label12";
            this.label12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label12.SizeF = new System.Drawing.SizeF(196.4073F, 34.79161F);
            this.label12.StylePriority.UseBackColor = false;
            this.label12.StylePriority.UseBorders = false;
            this.label12.StylePriority.UseFont = false;
            this.label12.StylePriority.UseTextAlignment = false;
            this.label12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Empcode]")});
            this.label11.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label11.LocationFloat = new DevExpress.Utils.PointFloat(124.0416F, 19.94398F);
            this.label11.Multiline = true;
            this.label11.Name = "label11";
            this.label11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label11.SizeF = new System.Drawing.SizeF(176.7858F, 34.79161F);
            this.label11.StylePriority.UseBackColor = false;
            this.label11.StylePriority.UseBorders = false;
            this.label11.StylePriority.UseFont = false;
            this.label11.StylePriority.UseTextAlignment = false;
            this.label11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label3.CanGrow = false;
            this.label3.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.LocationFloat = new DevExpress.Utils.PointFloat(601.6548F, 19.94398F);
            this.label3.Name = "label3";
            this.label3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label3.SizeF = new System.Drawing.SizeF(74.42453F, 34.79161F);
            this.label3.StylePriority.UseBackColor = false;
            this.label3.StylePriority.UseBorders = false;
            this.label3.StylePriority.UseFont = false;
            this.label3.StylePriority.UseForeColor = false;
            this.label3.StylePriority.UseTextAlignment = false;
            this.label3.Text = "នាយកដ្ឋាន:";
            this.label3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.label1.CanGrow = false;
            this.label1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'អត្តលេខ      :\'")});
            this.label1.Font = new DevExpress.Drawing.DXFont("Khmer OS Siemreap", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 19.94398F);
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(124.0416F, 34.79165F);
            this.label1.StylePriority.UseBackColor = false;
            this.label1.StylePriority.UseBorders = false;
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseForeColor = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ReportConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "HR_RPT_Education";
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
            queryParameter8.Name = "@Status";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("?Status", typeof(string));
            storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2,
            queryParameter3,
            queryParameter4,
            queryParameter5,
            queryParameter6,
            queryParameter7,
            queryParameter8});
            storedProcQuery1.StoredProcName = "HR_RPT_Education";
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
            // Status
            // 
            this.Status.Name = "Status";
            // 
            // Company
            // 
            this.Company.Description = "Company";
            this.Company.Name = "Company";
            // 
            // RptEducation
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportFooter,
            this.GroupHeader1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "HR_RPT_Education";
            this.DataSource = this.sqlDataSource1;
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(10F, 10F, 5F, 5F);
            this.PageHeight = 827;
            this.PageWidth = 1200;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Branch,
            this.Division,
            this.Department,
            this.Section,
            this.Position,
            this.Level,
            this.Status,
            this.Company});
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel label16;
        private DevExpress.XtraReports.UI.XRLabel label17;
        private DevExpress.XtraReports.UI.XRLabel label18;
        private DevExpress.XtraReports.UI.XRLabel label19;
        private DevExpress.XtraReports.UI.XRLabel label20;
        private DevExpress.XtraReports.UI.XRLabel label21;
        private DevExpress.XtraReports.UI.XRLabel label22;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo4;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel label2;
        private DevExpress.XtraReports.UI.XRLabel label4;
        private DevExpress.XtraReports.UI.XRLabel label13;
        private DevExpress.XtraReports.UI.XRLabel label14;
        private DevExpress.XtraReports.UI.XRLabel label15;
        private DevExpress.XtraReports.UI.XRLabel label5;
        private DevExpress.XtraReports.UI.XRLabel label6;
        private DevExpress.XtraReports.UI.XRLabel label7;
        private DevExpress.XtraReports.UI.XRLabel label9;
        private DevExpress.XtraReports.UI.XRLabel label10;
        private DevExpress.XtraReports.UI.XRLabel label8;
        private DevExpress.XtraReports.UI.XRLabel label12;
        private DevExpress.XtraReports.UI.XRLabel label11;
        private DevExpress.XtraReports.UI.XRLabel label3;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter Branch;
        private DevExpress.XtraReports.Parameters.Parameter Division;
        private DevExpress.XtraReports.Parameters.Parameter Department;
        private DevExpress.XtraReports.Parameters.Parameter Section;
        private DevExpress.XtraReports.Parameters.Parameter Position;
        private DevExpress.XtraReports.Parameters.Parameter Level;
        private DevExpress.XtraReports.Parameters.Parameter Status;
        private DevExpress.XtraReports.Parameters.Parameter Company;
    }
}
