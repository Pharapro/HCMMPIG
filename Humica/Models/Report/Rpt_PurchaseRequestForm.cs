using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for Rpt_PurchaseRequestForm
/// </summary>
public class Rpt_PurchaseRequestForm : DevExpress.XtraReports.UI.XtraReport
{
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private DetailBand Detail;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader1;
    private GroupFooterBand GroupFooter1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRTable xrTable2;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell18;
    private XRLabel xrLabel70;
    private XRLabel xrLabel60;
    private XRLabel xrLabel61;
    private DevExpress.XtraReports.Parameters.Parameter RequestNumber;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRPanel xrPanel4;
    private XRLabel xrLabel56;
    private XRLabel xrLabel51;
    private XRLabel xrLabel52;
    private XRLabel xrLabel53;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLabel xrLabel50;
    private XRLabel xrLabel49;
    private XRLabel xrLabel48;
    private XRLabel xrLabel47;
    private XRLabel xrLabel46;
    private XRLabel xrLabel45;
    private XRLabel xrLabel44;
    private XRLabel xrLabel43;
    private XRLabel xrLabel42;
    private XRLabel xrLabel41;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRPanel xrPanel3;
    private XRLabel xrLabel34;
    private XRLabel xrLabel35;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRPanel xrPanel2;
    private XRLabel xrLabel9;
    private XRLabel xrLabel30;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLabel xrLabel33;
    private XRPanel xrPanel1;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLabel xrLabel27;
    public XRPictureBox xrPictureBox2;
    public XRPictureBox xrPictureBox1;
    public XRPictureBox RequestBySign;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Rpt_PurchaseRequestForm()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rpt_PurchaseRequestForm));
        DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.RequestNumber = new DevExpress.XtraReports.Parameters.Parameter();
        this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
        this.RequestBySign = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 35F;
        this.TopMargin.Name = "TopMargin";
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 26F;
        this.BottomMargin.Name = "BottomMargin";
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail.FillEmptySpace = true;
        this.Detail.HeightF = 25F;
        this.Detail.Name = "Detail";
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTable2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable2.SizeF = new System.Drawing.SizeF(749F, 25F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell16,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell18});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell17.BorderWidth = 1F;
        this.xrTableCell17.CanGrow = false;
        this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
        this.xrTableCell17.Multiline = true;
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseBorderWidth = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell17.Summary = xrSummary1;
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell17.Weight = 0.48778062263599636D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell16.CanGrow = false;
        this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemDescription]")});
        this.xrTableCell16.Multiline = true;
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.Weight = 2.041281464550265D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell13.CanGrow = false;
        this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Unit]")});
        this.xrTableCell13.Multiline = true;
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.StylePriority.UseBorders = false;
        this.xrTableCell13.Weight = 0.5759576522847305D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell14.CanGrow = false;
        this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Brand]")});
        this.xrTableCell14.Multiline = true;
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.Weight = 0.96758321159896954D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell15.CanGrow = false;
        this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Quantity]")});
        this.xrTableCell15.Multiline = true;
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseBorders = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell15.Weight = 0.57237707261522652D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell18.BorderWidth = 1F;
        this.xrTableCell18.CanGrow = false;
        this.xrTableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Remark]")});
        this.xrTableCell18.Multiline = true;
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseBorderWidth = false;
        this.xrTableCell18.Weight = 1.2916117860902412D;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrLabel1});
        this.ReportHeader.HeightF = 256.5834F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel4
        // 
        this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel4.BorderWidth = 2F;
        this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel56,
            this.xrLabel51,
            this.xrLabel52,
            this.xrLabel53,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel50,
            this.xrLabel49,
            this.xrLabel48,
            this.xrLabel47,
            this.xrLabel46,
            this.xrLabel45,
            this.xrLabel44,
            this.xrLabel43,
            this.xrLabel42,
            this.xrLabel41,
            this.xrLabel40,
            this.xrLabel39});
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 68.25002F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(749F, 188.3334F);
        this.xrPanel4.StylePriority.UseBorders = false;
        this.xrPanel4.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel56
        // 
        this.xrLabel56.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel56.CanGrow = false;
        this.xrLabel56.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Description]")});
        this.xrLabel56.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(101.9567F, 150.625F);
        this.xrLabel56.Multiline = true;
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(641.5352F, 27.70834F);
        this.xrLabel56.StylePriority.UseBorderDashStyle = false;
        this.xrLabel56.StylePriority.UseBorders = false;
        this.xrLabel56.StylePriority.UseFont = false;
        // 
        // xrLabel51
        // 
        this.xrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel51.CanGrow = false;
        this.xrLabel51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Requestor]")});
        this.xrLabel51.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(101.9567F, 94.37502F);
        this.xrLabel51.Multiline = true;
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(122.9566F, 28.125F);
        this.xrLabel51.StylePriority.UseBorders = false;
        this.xrLabel51.StylePriority.UseFont = false;
        // 
        // xrLabel52
        // 
        this.xrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel52.CanGrow = false;
        this.xrLabel52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DocumentDate]")});
        this.xrLabel52.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(101.9567F, 38.12502F);
        this.xrLabel52.Multiline = true;
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(122.9566F, 28.125F);
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.TextFormatString = "{0:dd-MM-yyyyy}";
        // 
        // xrLabel53
        // 
        this.xrLabel53.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel53.CanGrow = false;
        this.xrLabel53.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ExtectedDate]")});
        this.xrLabel53.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(325.87F, 38.12502F);
        this.xrLabel53.Multiline = true;
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(162.4568F, 28.125F);
        this.xrLabel53.StylePriority.UseBorders = false;
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.TextFormatString = "{0:dd-MM-yyyyy}";
        // 
        // xrLabel54
        // 
        this.xrLabel54.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel54.CanGrow = false;
        this.xrLabel54.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]")});
        this.xrLabel54.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(325.8701F, 94.37502F);
        this.xrLabel54.Multiline = true;
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(162.4567F, 28.125F);
        this.xrLabel54.StylePriority.UseBorders = false;
        this.xrLabel54.StylePriority.UseFont = false;
        // 
        // xrLabel55
        // 
        this.xrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel55.CanGrow = false;
        this.xrLabel55.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Branch]")});
        this.xrLabel55.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(580.2835F, 94.37502F);
        this.xrLabel55.Multiline = true;
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(163.2083F, 28.125F);
        this.xrLabel55.StylePriority.UseBorders = false;
        this.xrLabel55.StylePriority.UseFont = false;
        // 
        // xrLabel50
        // 
        this.xrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel50.CanGrow = false;
        this.xrLabel50.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(488.3268F, 94.37502F);
        this.xrLabel50.Multiline = true;
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(91.9567F, 28.125F);
        this.xrLabel50.StylePriority.UseBorders = false;
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.Text = "Project Name:";
        // 
        // xrLabel49
        // 
        this.xrLabel49.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel49.CanGrow = false;
        this.xrLabel49.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(224.9133F, 94.37502F);
        this.xrLabel49.Multiline = true;
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(100.9567F, 28.125F);
        this.xrLabel49.StylePriority.UseBorders = false;
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.Text = "Department:";
        // 
        // xrLabel48
        // 
        this.xrLabel48.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel48.CanGrow = false;
        this.xrLabel48.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(224.9133F, 38.12502F);
        this.xrLabel48.Multiline = true;
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(100.9567F, 28.125F);
        this.xrLabel48.StylePriority.UseBorders = false;
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.Text = "Nedded Date:";
        // 
        // xrLabel47
        // 
        this.xrLabel47.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel47.CanGrow = false;
        this.xrLabel47.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(488.3268F, 66.25002F);
        this.xrLabel47.Multiline = true;
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(91.9567F, 28.125F);
        this.xrLabel47.StylePriority.UseBorders = false;
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.Text = "ឈ្មោះគំរោង";
        // 
        // xrLabel46
        // 
        this.xrLabel46.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel46.CanGrow = false;
        this.xrLabel46.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(224.9133F, 66.25002F);
        this.xrLabel46.Multiline = true;
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(100.9567F, 28.125F);
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.Text = " ផ្នែក/ក្រុម";
        // 
        // xrLabel45
        // 
        this.xrLabel45.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel45.CanGrow = false;
        this.xrLabel45.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(224.9133F, 10.00002F);
        this.xrLabel45.Multiline = true;
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(100.9567F, 28.125F);
        this.xrLabel45.StylePriority.UseBorders = false;
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.Text = "កាលបរិច្ឆេទត្រូវការ";
        // 
        // xrLabel44
        // 
        this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel44.CanGrow = false;
        this.xrLabel44.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(6F, 150.625F);
        this.xrLabel44.Multiline = true;
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.Text = "Memo:";
        // 
        // xrLabel43
        // 
        this.xrLabel43.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel43.CanGrow = false;
        this.xrLabel43.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(6F, 122.5F);
        this.xrLabel43.Multiline = true;
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel43.StylePriority.UseBorders = false;
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.Text = "កំណត់សំគាល់:";
        // 
        // xrLabel42
        // 
        this.xrLabel42.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel42.CanGrow = false;
        this.xrLabel42.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(6F, 94.375F);
        this.xrLabel42.Multiline = true;
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel42.StylePriority.UseBorders = false;
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.Text = "Name:";
        // 
        // xrLabel41
        // 
        this.xrLabel41.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel41.CanGrow = false;
        this.xrLabel41.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(6F, 66.25F);
        this.xrLabel41.Multiline = true;
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel41.StylePriority.UseBorders = false;
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.Text = "ឈ្មោះអ្នកស្នើសុំ​​​​​​";
        // 
        // xrLabel40
        // 
        this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel40.CanGrow = false;
        this.xrLabel40.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(6F, 38.125F);
        this.xrLabel40.Multiline = true;
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel40.StylePriority.UseBorders = false;
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.Text = "Request Date:";
        // 
        // xrLabel39
        // 
        this.xrLabel39.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel39.CanGrow = false;
        this.xrLabel39.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(6F, 10F);
        this.xrLabel39.Multiline = true;
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(95.9567F, 28.125F);
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.Text = "កាលបរិច្ឆេទស្នើសុំ    \r\n";
        // 
        // xrLabel1
        // 
        this.xrLabel1.CanGrow = false;
        this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 13F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel1.Multiline = true;
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(747F, 68.25002F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "ទម្រង់ស្នើសុំទិញសម្ភារៈ\r\nPurchase Request Form";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.GroupHeader1.HeightF = 50F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrTable1
        // 
        this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
        this.xrTable1.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2});
        this.xrTable1.SizeF = new System.Drawing.SizeF(749F, 50F);
        this.xrTable1.StylePriority.UseBackColor = false;
        this.xrTable1.StylePriority.UseFont = false;
        this.xrTable1.StylePriority.UseTextAlignment = false;
        this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrTableCell1.BorderWidth = 1F;
        this.xrTableCell1.CanGrow = false;
        this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell1.Multiline = true;
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseBorderWidth = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.Text = "លេខ\n";
        this.xrTableCell1.Weight = 0.48778059240061389D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrTableCell2.CanGrow = false;
        this.xrTableCell2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell2.Multiline = true;
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.Text = "បរិយាយ";
        this.xrTableCell2.Weight = 2.0412814947856459D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrTableCell3.CanGrow = false;
        this.xrTableCell3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell3.Multiline = true;
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseBorders = false;
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.Text = "ឯកតា";
        this.xrTableCell3.Weight = 0.57595742551937179D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrTableCell4.CanGrow = false;
        this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell4.Multiline = true;
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.Text = "ម៉ាក";
        this.xrTableCell4.Weight = 0.96758322671666219D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrTableCell5.CanGrow = false;
        this.xrTableCell5.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell5.Multiline = true;
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.Text = "បរិមាណ";
        this.xrTableCell5.Weight = 0.57237728426289425D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrTableCell6.BorderWidth = 1F;
        this.xrTableCell6.CanGrow = false;
        this.xrTableCell6.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 10F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell6.Multiline = true;
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseBorderWidth = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.Text = "ផ្សេងៗ";
        this.xrTableCell6.Weight = 1.291611786090241D;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell7.BorderWidth = 1F;
        this.xrTableCell7.CanGrow = false;
        this.xrTableCell7.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell7.Multiline = true;
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseBorderWidth = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Nº";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell7.Weight = 0.48778059240061389D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell8.CanGrow = false;
        this.xrTableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell8.Multiline = true;
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "DESCRIPTION";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell8.Weight = 2.0412814947856446D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell9.CanGrow = false;
        this.xrTableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "UoM";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell9.Weight = 0.57595742551937412D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell10.CanGrow = false;
        this.xrTableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell10.Multiline = true;
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "Brand";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell10.Weight = 0.96758322671666019D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell11.CanGrow = false;
        this.xrTableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell11.Multiline = true;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Qty";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell11.Weight = 0.57237728426289525D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell12.BorderWidth = 1F;
        this.xrTableCell12.CanGrow = false;
        this.xrTableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTableCell12.Multiline = true;
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseBorderWidth = false;
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "Remark";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell12.Weight = 1.291611786090241D;
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel3,
            this.xrPanel2,
            this.xrPanel1,
            this.xrLabel70,
            this.xrLabel26,
            this.xrLabel25});
        this.GroupFooter1.GroupUnion = DevExpress.XtraReports.UI.GroupFooterUnion.WithLastDetail;
        this.GroupFooter1.HeightF = 360.6666F;
        this.GroupFooter1.Name = "GroupFooter1";
        this.GroupFooter1.PrintAtBottom = true;
        // 
        // xrPanel3
        // 
        this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel3.CanGrow = false;
        this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrLabel34,
            this.xrLabel35,
            this.xrLabel36,
            this.xrLabel37,
            this.xrLabel38});
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(556.4584F, 185.2084F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(192.5416F, 164.9999F);
        this.xrPanel3.StylePriority.UseBorders = false;
        // 
        // xrLabel34
        // 
        this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel34.CanGrow = false;
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 135.9998F);
        this.xrLabel34.Multiline = true;
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(125.0001F, 24.99994F);
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.Text = "......../............/...........";
        // 
        // xrLabel35
        // 
        this.xrLabel35.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel35.CanGrow = false;
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 110.9998F);
        this.xrLabel35.Multiline = true;
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(125.0001F, 25F);
        this.xrLabel35.StylePriority.UseBorderDashStyle = false;
        this.xrLabel35.StylePriority.UseBorders = false;
        // 
        // xrLabel36
        // 
        this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel36.CanGrow = false;
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(10F, 110.9998F);
        this.xrLabel36.Multiline = true;
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.Text = "Name: ";
        // 
        // xrLabel37
        // 
        this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel37.CanGrow = false;
        this.xrLabel37.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(10F, 3F);
        this.xrLabel37.Multiline = true;
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(176.0417F, 46.16667F);
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.StylePriority.UseTextAlignment = false;
        this.xrLabel37.Text = "អ្នកអនុម័ត\r\nApproved by";
        this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel38
        // 
        this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel38.CanGrow = false;
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(10F, 135.9998F);
        this.xrLabel38.Multiline = true;
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.Text = "Date:";
        // 
        // xrPanel2
        // 
        this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel2.CanGrow = false;
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel9,
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLabel33});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(275.7852F, 185.2084F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(190.5416F, 164.9999F);
        this.xrPanel2.StylePriority.UseBorders = false;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel9.CanGrow = false;
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 135.9998F);
        this.xrLabel9.Multiline = true;
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(125.0001F, 24.99994F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.Text = "......../............/...........";
        // 
        // xrLabel30
        // 
        this.xrLabel30.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel30.CanGrow = false;
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 110.9998F);
        this.xrLabel30.Multiline = true;
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(125.0001F, 25F);
        this.xrLabel30.StylePriority.UseBorderDashStyle = false;
        this.xrLabel30.StylePriority.UseBorders = false;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel31.CanGrow = false;
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(10F, 110.9998F);
        this.xrLabel31.Multiline = true;
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.Text = "Name: ";
        // 
        // xrLabel32
        // 
        this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel32.CanGrow = false;
        this.xrLabel32.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(10F, 3F);
        this.xrLabel32.Multiline = true;
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(176.0417F, 46.16667F);
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.StylePriority.UseTextAlignment = false;
        this.xrLabel32.Text = "អ្នកត្រួតពិនិត្យ\r\nChecked by";
        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel33
        // 
        this.xrLabel33.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel33.CanGrow = false;
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(10F, 135.9998F);
        this.xrLabel33.Multiline = true;
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.Text = "Date:";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.CanGrow = false;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.RequestBySign,
            this.xrLabel61,
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel27,
            this.xrLabel60});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(4F, 185.2084F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(190.5416F, 164.9999F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrLabel61
        // 
        this.xrLabel61.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel61.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel61.CanGrow = false;
        this.xrLabel61.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DocumentDate]")});
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 135.9999F);
        this.xrLabel61.Multiline = true;
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(125.0001F, 24.99994F);
        this.xrLabel61.StylePriority.UseBorderDashStyle = false;
        this.xrLabel61.StylePriority.UseBorders = false;
        this.xrLabel61.Text = "......../............/...........";
        this.xrLabel61.TextFormatString = "{0:dd / MM / yyyy}";
        // 
        // xrLabel29
        // 
        this.xrLabel29.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel29.CanGrow = false;
        this.xrLabel29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Requestor]")});
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(61.0416F, 110.9999F);
        this.xrLabel29.Multiline = true;
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(125.0001F, 25F);
        this.xrLabel29.StylePriority.UseBorderDashStyle = false;
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.Text = "......................................";
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel28.CanGrow = false;
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(10F, 110.9999F);
        this.xrLabel28.Multiline = true;
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.Text = "Name: ";
        // 
        // xrLabel27
        // 
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel27.CanGrow = false;
        this.xrLabel27.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(10F, 3.000015F);
        this.xrLabel27.Multiline = true;
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(176.0417F, 46.16666F);
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "អ្នកស្នើសុំ\r\nRequested by";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel60
        // 
        this.xrLabel60.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel60.CanGrow = false;
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(10F, 135.9999F);
        this.xrLabel60.Multiline = true;
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(51.0416F, 25F);
        this.xrLabel60.StylePriority.UseBorders = false;
        this.xrLabel60.Text = "Date:";
        // 
        // xrLabel70
        // 
        this.xrLabel70.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel70.CanGrow = false;
        this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(176.0416F, 0F);
        this.xrLabel70.Multiline = true;
        this.xrLabel70.Name = "xrLabel70";
        this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel70.SizeF = new System.Drawing.SizeF(572.9584F, 24.125F);
        this.xrLabel70.StylePriority.UseBorders = false;
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel26.CanGrow = false;
        this.xrLabel26.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 7.5F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24.125F);
        this.xrLabel26.Multiline = true;
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(749F, 158.1249F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.Text = resources.GetString("xrLabel26.Text");
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel25.CanGrow = false;
        this.xrLabel25.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel25.Multiline = true;
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(176.0417F, 24.125F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.Text = "ចូរចំណាំ/NOTES:";
        // 
        // RequestNumber
        // 
        this.RequestNumber.Name = "RequestNumber";
        // 
        // sqlDataSource1
        // 
        this.sqlDataSource1.ConnectionName = "ReportConnectionString";
        this.sqlDataSource1.Name = "sqlDataSource1";
        storedProcQuery1.Name = "HR_RPT_PORequest";
        queryParameter1.Name = "@RequestNumber";
        queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
        queryParameter1.Value = new DevExpress.DataAccess.Expression("?RequestNumber", typeof(string));
        storedProcQuery1.Parameters.Add(queryParameter1);
        storedProcQuery1.StoredProcName = "HR_RPT_PORequest";
        this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
        this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
        // 
        // RequestBySign
        // 
        this.RequestBySign.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.RequestBySign.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Signature]")});
        this.RequestBySign.LocationFloat = new DevExpress.Utils.PointFloat(10F, 49.16669F);
        this.RequestBySign.Name = "RequestBySign";
        this.RequestBySign.SizeF = new System.Drawing.SizeF(176.0417F, 61.83322F);
        this.RequestBySign.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.RequestBySign.StylePriority.UseBorders = false;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(10F, 49.16663F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(176.0417F, 61.83322F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(10F, 49.16669F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(176.0417F, 61.83322F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        // 
        // Rpt_PurchaseRequestForm
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter1});
        this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
        this.DataMember = "HR_RPT_PORequest";
        this.DataSource = this.sqlDataSource1;
        this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
        this.Margins = new DevExpress.Drawing.DXMargins(35, 35, 35, 26);
        this.PageHeight = 1169;
        this.PageWidth = 827;
        this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.RequestNumber});
        this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.Version = "19.2";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrLabel5_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
    {

    }
}
