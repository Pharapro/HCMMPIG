using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for RPT_PURCHASE_ORDER
/// </summary>
public class RPT_PURCHASE_ORDER : DevExpress.XtraReports.UI.XtraReport
{
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private DetailBand Detail;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel9;
    private XRLabel xrLabel19;
    private XRLabel xrLabel20;
    private XRLabel xrLabel7;
    private XRLabel xrLabel12;
    private XRLabel xrLabel3;
    private XRLabel xrLabel16;
    private XRLabel xrLabel13;
    private XRLabel xrLabel14;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel47;
    private XRLabel xrLabel46;
    private XRLabel xrLabel45;
    private XRLabel xrLabel44;
    private XRLabel xrLabel43;
    private XRLabel xrLabel42;
    private XRLabel xrLabel41;
    private XRLabel xrLabel40;
    private GroupFooterBand GroupFooter1;
    private ReportFooterBand ReportFooter;
    private GroupHeaderBand GroupHeader2;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRLabel xrLabel39;
    private XRLabel xrLabel36;
    private XRLabel xrLabel34;
    private XRLabel xrLabel35;
    private XRLabel xrLabel15;
    private XRLabel xrLabel2;
    private XRLabel xrLabel4;
    private XRLabel xrLabel8;
    private XRLabel xrLabel24;
    private XRLabel xrLabel25;
    private XRLabel xrLabel27;
    private XRLabel xrLabel26;
    private XRLabel xrLabel33;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel10;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel48;
    private XRLabel xrLabel49;
    private XRLabel xrLabel50;
    private XRLabel xrLabel51;
    private XRLabel xrLabel52;
    private XRLabel xrLabel53;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLabel xrLabel61;
    private XRLabel xrLabel62;
    private XRLabel xrLabel59;
    private XRLabel xrLabel60;
    private XRLabel xrLabel64;
    private XRLabel xrLabel63;
    private XRLabel xrLabel56;
    private XRLabel xrLabel86;
    private XRLabel xrLabel87;
    private XRLabel xrLabel82;
    private XRLabel xrLabel83;
    private XRLabel xrLabel76;
    private XRLabel xrLabel77;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private DevExpress.XtraReports.Parameters.Parameter PONumber;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRLabel xrLabel57;
    private XRLabel xrLabel72;
    private XRLabel xrLabel73;
    private XRLabel xrLabel68;
    private XRLabel xrLabel67;
    private XRLabel xrLabel65;
    private XRLabel xrLabel11;
    private XRTableCell xrTableCell12;
    private XRPictureBox xrPictureBox6;
    private XRPanel xrPanel4;
    private XRLabel xrLabel75;
    private XRLabel xrLabel78;
    public XRPictureBox xrPictureBox5;
    private XRPanel xrPanel3;
    private XRLabel xrLabel71;
    private XRLabel xrLabel74;
    public XRPictureBox xrPictureBox4;
    private XRPanel xrPanel2;
    private XRLabel xrLabel66;
    private XRLabel xrLabel69;
    public XRPictureBox xrPictureBox3;
    private XRPanel xrPanel1;
    private XRLabel xrLabel18;
    private XRLabel xrLabel58;
    public XRPictureBox xrPictureBox2;
    private XRPanel xrPanel5;
    private XRLabel xrLabel17;
    public XRPictureBox RequestBySign;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RPT_PURCHASE_ORDER()
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
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPT_PURCHASE_ORDER));
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrPictureBox6 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel75 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel78 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox5 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel74 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox4 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrPanel5 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
        this.RequestBySign = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel86 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel87 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel82 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel83 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel77 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.PONumber = new DevExpress.XtraReports.Parameters.Parameter();
        this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 35F;
        this.TopMargin.Name = "TopMargin";
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 32F;
        this.BottomMargin.Name = "BottomMargin";
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail.FillEmptySpace = true;
        this.Detail.HeightF = 75F;
        this.Detail.Name = "Detail";
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(755.2967F, 75F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        this.xrTable2.StylePriority.UseTextAlignment = false;
        this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell11,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.CanGrow = false;
        this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell9.Summary = xrSummary1;
        this.xrTableCell9.Weight = 0.3350169340307515D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.CanGrow = false;
        this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemCode]")});
        this.xrTableCell10.Multiline = true;
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell10.Weight = 0.813349969075267D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.CanGrow = false;
        this.xrTableCell12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox6});
        this.xrTableCell12.Multiline = true;
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Weight = 1.0729010772704772D;
        // 
        // xrPictureBox6
        // 
        this.xrPictureBox6.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Attachment]")});
        this.xrPictureBox6.LocationFloat = new DevExpress.Utils.PointFloat(4.000015F, 1F);
        this.xrPictureBox6.Name = "xrPictureBox6";
        this.xrPictureBox6.SizeF = new System.Drawing.SizeF(100F, 72F);
        this.xrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox6.StylePriority.UseBorders = false;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.CanGrow = false;
        this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Descr]")});
        this.xrTableCell11.Multiline = true;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell11.Weight = 1.8920654587136061D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.CanGrow = false;
        this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Qty]")});
        this.xrTableCell13.Multiline = true;
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Weight = 0.637516818783486D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.CanGrow = false;
        this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Unit]")});
        this.xrTableCell14.Multiline = true;
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.Weight = 0.687518043518192D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.CanGrow = false;
        this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Amount]")});
        this.xrTableCell15.Multiline = true;
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell15.TextFormatString = "{0:c2}";
        this.xrTableCell15.Weight = 1.065815505981353D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell16.CanGrow = false;
        this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SubTotal]")});
        this.xrTableCell16.Multiline = true;
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell16.TextFormatString = "{0:c2}";
        this.xrTableCell16.Weight = 1.0487837205505022D;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel9,
            this.xrLabel19,
            this.xrLabel20,
            this.xrLabel7,
            this.xrLabel12,
            this.xrLabel3,
            this.xrLabel16,
            this.xrLabel13,
            this.xrLabel14,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel1});
        this.ReportHeader.HeightF = 190.0001F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Com_Images]")});
        this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(90F, 90F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        // 
        // xrLabel23
        // 
        this.xrLabel23.CanGrow = false;
        this.xrLabel23.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 130.0001F);
        this.xrLabel23.Multiline = true;
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(132.5035F, 19F);
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.StylePriority.UseTextAlignment = false;
        this.xrLabel23.Text = "DATE:";
        this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel22
        // 
        this.xrLabel22.CanGrow = false;
        this.xrLabel22.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 168.0001F);
        this.xrLabel22.Multiline = true;
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(132.5034F, 22F);
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.StylePriority.UseTextAlignment = false;
        this.xrLabel22.Text = "PROJECT NAME:";
        this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel21
        // 
        this.xrLabel21.CanGrow = false;
        this.xrLabel21.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 149.0001F);
        this.xrLabel21.Multiline = true;
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(132.5034F, 19F);
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = "DUE DATE:";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel9
        // 
        this.xrLabel9.CanGrow = false;
        this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 111F);
        this.xrLabel9.Multiline = true;
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(132.5034F, 19.00001F);
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "P.O. NO:";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel19
        // 
        this.xrLabel19.CanGrow = false;
        this.xrLabel19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PromisedDate]")});
        this.xrLabel19.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 149.0001F);
        this.xrLabel19.Multiline = true;
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(211.46F, 19.00002F);
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.StylePriority.UseTextAlignment = false;
        this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrLabel19.TextFormatString = "{0:ddd/dd/MMM/yyyy}";
        // 
        // xrLabel20
        // 
        this.xrLabel20.CanGrow = false;
        this.xrLabel20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Branch]")});
        this.xrLabel20.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 168.0001F);
        this.xrLabel20.Multiline = true;
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(211.46F, 22.00003F);
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel7
        // 
        this.xrLabel7.CanGrow = false;
        this.xrLabel7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DocumentDate]")});
        this.xrLabel7.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 130F);
        this.xrLabel7.Multiline = true;
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(211.46F, 19.00002F);
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrLabel7.TextFormatString = "{0:ddd/dd/MMM/yyyy}";
        // 
        // xrLabel12
        // 
        this.xrLabel12.CanGrow = false;
        this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PONumber]")});
        this.xrLabel12.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 111F);
        this.xrLabel12.Multiline = true;
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(211.4599F, 19.00001F);
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.StylePriority.UseTextAlignment = false;
        this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel3
        // 
        this.xrLabel3.CanGrow = false;
        this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Branch]")});
        this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 90.00004F);
        this.xrLabel3.Multiline = true;
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(211.46F, 25F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "TRONUM HOME";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel16
        // 
        this.xrLabel16.CanGrow = false;
        this.xrLabel16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Com_Email]")});
        this.xrLabel16.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(50.0015F, 168.0001F);
        this.xrLabel16.Multiline = true;
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(361.3319F, 22F);
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.Text = "xavter@vehaa.com";
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel13
        // 
        this.xrLabel13.CanGrow = false;
        this.xrLabel13.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 168.0001F);
        this.xrLabel13.Multiline = true;
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(50.00169F, 22F);
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.StylePriority.UseTextAlignment = false;
        this.xrLabel13.Text = "Email:";
        this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel14
        // 
        this.xrLabel14.CanGrow = false;
        this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Tel: \' +  [Com_Phone]")});
        this.xrLabel14.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 149.0001F);
        this.xrLabel14.Multiline = true;
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(411.3334F, 19.00002F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.StylePriority.UseTextAlignment = false;
        this.xrLabel14.Text = "Tel: +855 12 456 810     Tel: +855 23 216 115";
        this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel6
        // 
        this.xrLabel6.CanGrow = false;
        this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Com_Address]")});
        this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 115F);
        this.xrLabel6.Multiline = true;
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(411.3334F, 34.00002F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "No:59B, Sothearos Blvd, Sangkat Tonle \r\nBassac, Khan Chamkar Mon, Phnom Penh Camb" +
"odia";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel5
        // 
        this.xrLabel5.CanGrow = false;
        this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Arial", 17F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 39.00006F);
        this.xrLabel5.Multiline = true;
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(211.46F, 31.99998F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "Purchase Order";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel1
        // 
        this.xrLabel1.CanGrow = false;
        this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 17F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 0F);
        this.xrLabel1.Multiline = true;
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(211.4599F, 39.00006F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "ការបញ្ជាទិញ";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel48,
            this.xrLabel49,
            this.xrLabel50,
            this.xrLabel51,
            this.xrLabel52,
            this.xrLabel53,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel47,
            this.xrLabel46,
            this.xrLabel45,
            this.xrLabel44,
            this.xrLabel43,
            this.xrLabel42,
            this.xrLabel41,
            this.xrLabel40});
        this.GroupHeader1.HeightF = 50F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel48
        // 
        this.xrLabel48.BackColor = System.Drawing.Color.Silver;
        this.xrLabel48.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel48.CanGrow = false;
        this.xrLabel48.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(33.50169F, 25F);
        this.xrLabel48.Multiline = true;
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(81.33499F, 25F);
        this.xrLabel48.StylePriority.UseBackColor = false;
        this.xrLabel48.StylePriority.UseBorders = false;
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.StylePriority.UseTextAlignment = false;
        this.xrLabel48.Text = "ITEM CODE";
        this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel49
        // 
        this.xrLabel49.AllowMarkupText = true;
        this.xrLabel49.BackColor = System.Drawing.Color.Silver;
        this.xrLabel49.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel49.CanGrow = false;
        this.xrLabel49.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(222.1268F, 25F);
        this.xrLabel49.Multiline = true;
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(189.2065F, 25F);
        this.xrLabel49.StylePriority.UseBackColor = false;
        this.xrLabel49.StylePriority.UseBorders = false;
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.StylePriority.UseTextAlignment = false;
        this.xrLabel49.Text = "DESCRIPTION";
        this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel50
        // 
        this.xrLabel50.BackColor = System.Drawing.Color.Silver;
        this.xrLabel50.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel50.CanGrow = false;
        this.xrLabel50.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 25F);
        this.xrLabel50.Multiline = true;
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(68.75168F, 25F);
        this.xrLabel50.StylePriority.UseBackColor = false;
        this.xrLabel50.StylePriority.UseBorders = false;
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "UOM";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel51
        // 
        this.xrLabel51.BackColor = System.Drawing.Color.Silver;
        this.xrLabel51.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel51.CanGrow = false;
        this.xrLabel51.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(543.8366F, 25F);
        this.xrLabel51.Multiline = true;
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(106.5817F, 25F);
        this.xrLabel51.StylePriority.UseBackColor = false;
        this.xrLabel51.StylePriority.UseBorders = false;
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        this.xrLabel51.Text = "UNIT PRICE";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel52
        // 
        this.xrLabel52.BackColor = System.Drawing.Color.Silver;
        this.xrLabel52.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel52.CanGrow = false;
        this.xrLabel52.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(650.4183F, 25F);
        this.xrLabel52.Multiline = true;
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(104.8784F, 25F);
        this.xrLabel52.StylePriority.UseBackColor = false;
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "Amounts";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel53
        // 
        this.xrLabel53.BackColor = System.Drawing.Color.Silver;
        this.xrLabel53.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel53.CanGrow = false;
        this.xrLabel53.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
        this.xrLabel53.Multiline = true;
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(33.50169F, 25F);
        this.xrLabel53.StylePriority.UseBackColor = false;
        this.xrLabel53.StylePriority.UseBorders = false;
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        this.xrLabel53.Text = "Nº";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel54
        // 
        this.xrLabel54.BackColor = System.Drawing.Color.Silver;
        this.xrLabel54.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel54.CanGrow = false;
        this.xrLabel54.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 25F);
        this.xrLabel54.Multiline = true;
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(63.75162F, 25F);
        this.xrLabel54.StylePriority.UseBackColor = false;
        this.xrLabel54.StylePriority.UseBorders = false;
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.Text = "QTY";
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel55
        // 
        this.xrLabel55.BackColor = System.Drawing.Color.Silver;
        this.xrLabel55.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel55.CanGrow = false;
        this.xrLabel55.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 25F);
        this.xrLabel55.Multiline = true;
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(107.2901F, 25F);
        this.xrLabel55.StylePriority.UseBackColor = false;
        this.xrLabel55.StylePriority.UseBorders = false;
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.StylePriority.UseTextAlignment = false;
        this.xrLabel55.Text = "PHOTO";
        this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel47
        // 
        this.xrLabel47.BackColor = System.Drawing.Color.Silver;
        this.xrLabel47.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel47.CanGrow = false;
        this.xrLabel47.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 0F);
        this.xrLabel47.Multiline = true;
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(107.29F, 25F);
        this.xrLabel47.StylePriority.UseBackColor = false;
        this.xrLabel47.StylePriority.UseBorders = false;
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.StylePriority.UseTextAlignment = false;
        this.xrLabel47.Text = "រូបភាព";
        this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel46
        // 
        this.xrLabel46.BackColor = System.Drawing.Color.Silver;
        this.xrLabel46.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel46.CanGrow = false;
        this.xrLabel46.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(411.3333F, 0F);
        this.xrLabel46.Multiline = true;
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(63.75168F, 25F);
        this.xrLabel46.StylePriority.UseBackColor = false;
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.StylePriority.UseTextAlignment = false;
        this.xrLabel46.Text = "ចំនួន";
        this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel45
        // 
        this.xrLabel45.BackColor = System.Drawing.Color.Silver;
        this.xrLabel45.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel45.CanGrow = false;
        this.xrLabel45.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel45.Multiline = true;
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(33.50169F, 25F);
        this.xrLabel45.StylePriority.UseBackColor = false;
        this.xrLabel45.StylePriority.UseBorders = false;
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.StylePriority.UseTextAlignment = false;
        this.xrLabel45.Text = "លេខ";
        this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel44
        // 
        this.xrLabel44.BackColor = System.Drawing.Color.Silver;
        this.xrLabel44.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel44.CanGrow = false;
        this.xrLabel44.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(650.4183F, 0F);
        this.xrLabel44.Multiline = true;
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(104.8784F, 25F);
        this.xrLabel44.StylePriority.UseBackColor = false;
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.StylePriority.UseTextAlignment = false;
        this.xrLabel44.Text = "តម្លែ";
        this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel43
        // 
        this.xrLabel43.BackColor = System.Drawing.Color.Silver;
        this.xrLabel43.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel43.CanGrow = false;
        this.xrLabel43.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(543.8366F, 0F);
        this.xrLabel43.Multiline = true;
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(106.5817F, 25F);
        this.xrLabel43.StylePriority.UseBackColor = false;
        this.xrLabel43.StylePriority.UseBorders = false;
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.StylePriority.UseTextAlignment = false;
        this.xrLabel43.Text = "តម្លែមួយឯកតា";
        this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel42
        // 
        this.xrLabel42.BackColor = System.Drawing.Color.Silver;
        this.xrLabel42.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel42.CanGrow = false;
        this.xrLabel42.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 0F);
        this.xrLabel42.Multiline = true;
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(68.75168F, 25F);
        this.xrLabel42.StylePriority.UseBackColor = false;
        this.xrLabel42.StylePriority.UseBorders = false;
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.StylePriority.UseTextAlignment = false;
        this.xrLabel42.Text = "ខ្នាត";
        this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel41
        // 
        this.xrLabel41.BackColor = System.Drawing.Color.Silver;
        this.xrLabel41.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel41.CanGrow = false;
        this.xrLabel41.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(222.1268F, 0F);
        this.xrLabel41.Multiline = true;
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(189.2065F, 25F);
        this.xrLabel41.StylePriority.UseBackColor = false;
        this.xrLabel41.StylePriority.UseBorders = false;
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.StylePriority.UseTextAlignment = false;
        this.xrLabel41.Text = "បរិយាយ\t";
        this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel40
        // 
        this.xrLabel40.BackColor = System.Drawing.Color.Silver;
        this.xrLabel40.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel40.CanGrow = false;
        this.xrLabel40.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(33.50169F, 0F);
        this.xrLabel40.Multiline = true;
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(81.33499F, 25F);
        this.xrLabel40.StylePriority.UseBackColor = false;
        this.xrLabel40.StylePriority.UseBorders = false;
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.StylePriority.UseTextAlignment = false;
        this.xrLabel40.Text = "លេខកូដ";
        this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel57,
            this.xrLabel61,
            this.xrLabel62,
            this.xrLabel59,
            this.xrLabel60,
            this.xrLabel64,
            this.xrLabel63,
            this.xrLabel56});
        this.GroupFooter1.HeightF = 77.70818F;
        this.GroupFooter1.Name = "GroupFooter1";
        this.GroupFooter1.PrintAtBottom = true;
        // 
        // xrLabel57
        // 
        this.xrLabel57.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrLabel57.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel57.CanGrow = false;
        this.xrLabel57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Description]")});
        this.xrLabel57.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(60.41836F, 0F);
        this.xrLabel57.Multiline = true;
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(483.4183F, 25.00001F);
        this.xrLabel57.StylePriority.UseBorderDashStyle = false;
        this.xrLabel57.StylePriority.UseBorders = false;
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.StylePriority.UseTextAlignment = false;
        this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel61
        // 
        this.xrLabel61.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel61.CanGrow = false;
        this.xrLabel61.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([SubTotal])")});
        this.xrLabel61.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(650.4183F, 50F);
        this.xrLabel61.Multiline = true;
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(104.8784F, 25.00001F);
        this.xrLabel61.StylePriority.UseBorders = false;
        this.xrLabel61.StylePriority.UseFont = false;
        this.xrLabel61.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrLabel61.Summary = xrSummary2;
        this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel61.TextFormatString = "{0:c2}";
        // 
        // xrLabel62
        // 
        this.xrLabel62.CanGrow = false;
        this.xrLabel62.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 50.00002F);
        this.xrLabel62.Multiline = true;
        this.xrLabel62.Name = "xrLabel62";
        this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel62.SizeF = new System.Drawing.SizeF(106.5816F, 25.00001F);
        this.xrLabel62.StylePriority.UseFont = false;
        this.xrLabel62.StylePriority.UseTextAlignment = false;
        this.xrLabel62.Text = "TOTAL";
        this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel59
        // 
        this.xrLabel59.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel59.CanGrow = false;
        this.xrLabel59.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(650.4183F, 25F);
        this.xrLabel59.Multiline = true;
        this.xrLabel59.Name = "xrLabel59";
        this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel59.SizeF = new System.Drawing.SizeF(104.8784F, 25.00001F);
        this.xrLabel59.StylePriority.UseBorders = false;
        this.xrLabel59.StylePriority.UseFont = false;
        this.xrLabel59.StylePriority.UseTextAlignment = false;
        this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel60
        // 
        this.xrLabel60.CanGrow = false;
        this.xrLabel60.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 25F);
        this.xrLabel60.Multiline = true;
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(106.5816F, 25.00001F);
        this.xrLabel60.StylePriority.UseFont = false;
        this.xrLabel60.StylePriority.UseTextAlignment = false;
        this.xrLabel60.Text = "Tax 10%";
        this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel64
        // 
        this.xrLabel64.CanGrow = false;
        this.xrLabel64.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(543.8367F, 0F);
        this.xrLabel64.Multiline = true;
        this.xrLabel64.Name = "xrLabel64";
        this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel64.SizeF = new System.Drawing.SizeF(106.5816F, 25.00001F);
        this.xrLabel64.StylePriority.UseFont = false;
        this.xrLabel64.StylePriority.UseTextAlignment = false;
        this.xrLabel64.Text = "SUBTOTAL";
        this.xrLabel64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel63
        // 
        this.xrLabel63.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel63.CanGrow = false;
        this.xrLabel63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([SubTotal])")});
        this.xrLabel63.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(650.4183F, 0F);
        this.xrLabel63.Multiline = true;
        this.xrLabel63.Name = "xrLabel63";
        this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel63.SizeF = new System.Drawing.SizeF(104.8784F, 25.00001F);
        this.xrLabel63.StylePriority.UseBorders = false;
        this.xrLabel63.StylePriority.UseFont = false;
        this.xrLabel63.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrLabel63.Summary = xrSummary3;
        this.xrLabel63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel63.TextFormatString = "{0:c2}";
        // 
        // xrLabel56
        // 
        this.xrLabel56.CanGrow = false;
        this.xrLabel56.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel56.Multiline = true;
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(60.41835F, 25F);
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.StylePriority.UseTextAlignment = false;
        this.xrLabel56.Text = "Remark:";
        this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrPanel3,
            this.xrPanel2,
            this.xrPanel1,
            this.xrPanel5,
            this.xrLabel86,
            this.xrLabel87,
            this.xrLabel82,
            this.xrLabel83,
            this.xrLabel76,
            this.xrLabel77,
            this.xrLabel72,
            this.xrLabel73,
            this.xrLabel68,
            this.xrLabel67});
        this.ReportFooter.HeightF = 175.2814F;
        this.ReportFooter.Name = "ReportFooter";
        this.ReportFooter.PrintAtBottom = true;
        // 
        // xrPanel4
        // 
        this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel75,
            this.xrLabel78,
            this.xrPictureBox5});
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(615.3765F, 42.00001F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(139.9201F, 128.0313F);
        this.xrPanel4.StylePriority.UseBorders = false;
        // 
        // xrLabel75
        // 
        this.xrLabel75.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel75.CanGrow = false;
        this.xrLabel75.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel75.LocationFloat = new DevExpress.Utils.PointFloat(3F, 76.0313F);
        this.xrLabel75.Multiline = true;
        this.xrLabel75.Name = "xrLabel75";
        this.xrLabel75.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel75.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel75.StylePriority.UseBorders = false;
        this.xrLabel75.StylePriority.UseFont = false;
        this.xrLabel75.StylePriority.UseTextAlignment = false;
        this.xrLabel75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel78
        // 
        this.xrLabel78.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel78.CanGrow = false;
        this.xrLabel78.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel78.LocationFloat = new DevExpress.Utils.PointFloat(3F, 101.0313F);
        this.xrLabel78.Multiline = true;
        this.xrLabel78.Name = "xrLabel78";
        this.xrLabel78.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel78.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel78.StylePriority.UseBorders = false;
        this.xrLabel78.StylePriority.UseFont = false;
        this.xrLabel78.StylePriority.UseTextAlignment = false;
        this.xrLabel78.Text = "Director/CEO";
        this.xrLabel78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox5
        // 
        this.xrPictureBox5.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox5.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
        this.xrPictureBox5.Name = "xrPictureBox5";
        this.xrPictureBox5.SizeF = new System.Drawing.SizeF(133.0034F, 75.0313F);
        this.xrPictureBox5.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox5.StylePriority.UseBorders = false;
        // 
        // xrPanel3
        // 
        this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel71,
            this.xrLabel74,
            this.xrPictureBox4});
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(461.5324F, 42.00001F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(139.9201F, 128.0313F);
        this.xrPanel3.StylePriority.UseBorders = false;
        // 
        // xrLabel71
        // 
        this.xrLabel71.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel71.CanGrow = false;
        this.xrLabel71.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(3F, 76.0313F);
        this.xrLabel71.Multiline = true;
        this.xrLabel71.Name = "xrLabel71";
        this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel71.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel71.StylePriority.UseBorders = false;
        this.xrLabel71.StylePriority.UseFont = false;
        this.xrLabel71.StylePriority.UseTextAlignment = false;
        this.xrLabel71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel74
        // 
        this.xrLabel74.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel74.CanGrow = false;
        this.xrLabel74.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel74.LocationFloat = new DevExpress.Utils.PointFloat(3F, 101.0313F);
        this.xrLabel74.Multiline = true;
        this.xrLabel74.Name = "xrLabel74";
        this.xrLabel74.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel74.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel74.StylePriority.UseBorders = false;
        this.xrLabel74.StylePriority.UseFont = false;
        this.xrLabel74.StylePriority.UseTextAlignment = false;
        this.xrLabel74.Text = "Account Manager";
        this.xrLabel74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox4
        // 
        this.xrPictureBox4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox4.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
        this.xrPictureBox4.Name = "xrPictureBox4";
        this.xrPictureBox4.SizeF = new System.Drawing.SizeF(133.0034F, 75.0313F);
        this.xrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox4.StylePriority.UseBorders = false;
        // 
        // xrPanel2
        // 
        this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel66,
            this.xrLabel69,
            this.xrPictureBox3});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(307.6883F, 42.00001F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(139.9201F, 128.0313F);
        this.xrPanel2.StylePriority.UseBorders = false;
        // 
        // xrLabel66
        // 
        this.xrLabel66.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel66.CanGrow = false;
        this.xrLabel66.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(3F, 76.0313F);
        this.xrLabel66.Multiline = true;
        this.xrLabel66.Name = "xrLabel66";
        this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel66.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel66.StylePriority.UseBorders = false;
        this.xrLabel66.StylePriority.UseFont = false;
        this.xrLabel66.StylePriority.UseTextAlignment = false;
        this.xrLabel66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel69
        // 
        this.xrLabel69.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel69.CanGrow = false;
        this.xrLabel69.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(3F, 101.0313F);
        this.xrLabel69.Multiline = true;
        this.xrLabel69.Name = "xrLabel69";
        this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel69.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel69.StylePriority.UseBorders = false;
        this.xrLabel69.StylePriority.UseFont = false;
        this.xrLabel69.StylePriority.UseTextAlignment = false;
        this.xrLabel69.Text = "Department Manager";
        this.xrLabel69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox3
        // 
        this.xrPictureBox3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
        this.xrPictureBox3.Name = "xrPictureBox3";
        this.xrPictureBox3.SizeF = new System.Drawing.SizeF(133.0034F, 75.0313F);
        this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox3.StylePriority.UseBorders = false;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel18,
            this.xrLabel58,
            this.xrPictureBox2});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(153.8441F, 42.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(139.9201F, 128.0313F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel18.CanGrow = false;
        this.xrLabel18.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(3F, 76.0313F);
        this.xrLabel18.Multiline = true;
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.StylePriority.UseTextAlignment = false;
        this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel58
        // 
        this.xrLabel58.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel58.CanGrow = false;
        this.xrLabel58.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(3F, 101.0313F);
        this.xrLabel58.Multiline = true;
        this.xrLabel58.Name = "xrLabel58";
        this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel58.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel58.StylePriority.UseBorders = false;
        this.xrLabel58.StylePriority.UseFont = false;
        this.xrLabel58.StylePriority.UseTextAlignment = false;
        this.xrLabel58.Text = "Purchasing Manager";
        this.xrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(133.0034F, 75.0313F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        // 
        // xrPanel5
        // 
        this.xrPanel5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel17,
            this.xrLabel65,
            this.RequestBySign});
        this.xrPanel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 42.00001F);
        this.xrPanel5.Name = "xrPanel5";
        this.xrPanel5.SizeF = new System.Drawing.SizeF(139.9201F, 128.0313F);
        this.xrPanel5.StylePriority.UseBorders = false;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel17.CanGrow = false;
        this.xrLabel17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RequestorBy]")});
        this.xrLabel17.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(3F, 76.0313F);
        this.xrLabel17.Multiline = true;
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.StylePriority.UseTextAlignment = false;
        this.xrLabel17.Text = "Name";
        this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel65
        // 
        this.xrLabel65.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel65.CanGrow = false;
        this.xrLabel65.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(3F, 101.0313F);
        this.xrLabel65.Multiline = true;
        this.xrLabel65.Name = "xrLabel65";
        this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel65.SizeF = new System.Drawing.SizeF(133.9201F, 25.00001F);
        this.xrLabel65.StylePriority.UseBorders = false;
        this.xrLabel65.StylePriority.UseFont = false;
        this.xrLabel65.StylePriority.UseTextAlignment = false;
        this.xrLabel65.Text = "Purchasing Officer";
        this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // RequestBySign
        // 
        this.RequestBySign.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.RequestBySign.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Signature]")});
        this.RequestBySign.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
        this.RequestBySign.Name = "RequestBySign";
        this.RequestBySign.SizeF = new System.Drawing.SizeF(133.0034F, 75.0313F);
        this.RequestBySign.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.RequestBySign.StylePriority.UseBorders = false;
        // 
        // xrLabel86
        // 
        this.xrLabel86.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel86.CanGrow = false;
        this.xrLabel86.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel86.LocationFloat = new DevExpress.Utils.PointFloat(615.3766F, 0F);
        this.xrLabel86.Multiline = true;
        this.xrLabel86.Name = "xrLabel86";
        this.xrLabel86.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel86.SizeF = new System.Drawing.SizeF(139.9201F, 25.00001F);
        this.xrLabel86.StylePriority.UseBorders = false;
        this.xrLabel86.StylePriority.UseFont = false;
        this.xrLabel86.StylePriority.UseTextAlignment = false;
        this.xrLabel86.Text = "អ្នកអនុម័ត";
        this.xrLabel86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel87
        // 
        this.xrLabel87.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel87.CanGrow = false;
        this.xrLabel87.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel87.LocationFloat = new DevExpress.Utils.PointFloat(615.3766F, 25F);
        this.xrLabel87.Multiline = true;
        this.xrLabel87.Name = "xrLabel87";
        this.xrLabel87.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel87.SizeF = new System.Drawing.SizeF(139.9201F, 17.00001F);
        this.xrLabel87.StylePriority.UseBorders = false;
        this.xrLabel87.StylePriority.UseFont = false;
        this.xrLabel87.StylePriority.UseTextAlignment = false;
        this.xrLabel87.Text = "Approved by";
        this.xrLabel87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel82
        // 
        this.xrLabel82.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel82.CanGrow = false;
        this.xrLabel82.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel82.LocationFloat = new DevExpress.Utils.PointFloat(461.5324F, 25F);
        this.xrLabel82.Multiline = true;
        this.xrLabel82.Name = "xrLabel82";
        this.xrLabel82.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel82.SizeF = new System.Drawing.SizeF(139.9201F, 17.00001F);
        this.xrLabel82.StylePriority.UseBorders = false;
        this.xrLabel82.StylePriority.UseFont = false;
        this.xrLabel82.StylePriority.UseTextAlignment = false;
        this.xrLabel82.Text = "Seen and agree by";
        this.xrLabel82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel83
        // 
        this.xrLabel83.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel83.CanGrow = false;
        this.xrLabel83.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel83.LocationFloat = new DevExpress.Utils.PointFloat(461.5324F, 0F);
        this.xrLabel83.Multiline = true;
        this.xrLabel83.Name = "xrLabel83";
        this.xrLabel83.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel83.SizeF = new System.Drawing.SizeF(139.9201F, 25.00001F);
        this.xrLabel83.StylePriority.UseBorders = false;
        this.xrLabel83.StylePriority.UseFont = false;
        this.xrLabel83.StylePriority.UseTextAlignment = false;
        this.xrLabel83.Text = "បានឃើញ និង​​ យល់ព្រម";
        this.xrLabel83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel76
        // 
        this.xrLabel76.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel76.CanGrow = false;
        this.xrLabel76.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel76.LocationFloat = new DevExpress.Utils.PointFloat(307.6883F, 0F);
        this.xrLabel76.Multiline = true;
        this.xrLabel76.Name = "xrLabel76";
        this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel76.SizeF = new System.Drawing.SizeF(139.9201F, 25.00001F);
        this.xrLabel76.StylePriority.UseBorders = false;
        this.xrLabel76.StylePriority.UseFont = false;
        this.xrLabel76.StylePriority.UseTextAlignment = false;
        this.xrLabel76.Text = "បានឃើញ និង​​ យល់ព្រម";
        this.xrLabel76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel77
        // 
        this.xrLabel77.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel77.CanGrow = false;
        this.xrLabel77.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel77.LocationFloat = new DevExpress.Utils.PointFloat(307.6883F, 25F);
        this.xrLabel77.Multiline = true;
        this.xrLabel77.Name = "xrLabel77";
        this.xrLabel77.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel77.SizeF = new System.Drawing.SizeF(139.9201F, 17.00001F);
        this.xrLabel77.StylePriority.UseBorders = false;
        this.xrLabel77.StylePriority.UseFont = false;
        this.xrLabel77.StylePriority.UseTextAlignment = false;
        this.xrLabel77.Text = "Seen and agree by";
        this.xrLabel77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel72
        // 
        this.xrLabel72.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel72.CanGrow = false;
        this.xrLabel72.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(153.8441F, 25F);
        this.xrLabel72.Multiline = true;
        this.xrLabel72.Name = "xrLabel72";
        this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel72.SizeF = new System.Drawing.SizeF(139.9201F, 17.00001F);
        this.xrLabel72.StylePriority.UseBorders = false;
        this.xrLabel72.StylePriority.UseFont = false;
        this.xrLabel72.StylePriority.UseTextAlignment = false;
        this.xrLabel72.Text = "Seen and agree by";
        this.xrLabel72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel73
        // 
        this.xrLabel73.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel73.CanGrow = false;
        this.xrLabel73.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(153.8441F, 0F);
        this.xrLabel73.Multiline = true;
        this.xrLabel73.Name = "xrLabel73";
        this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel73.SizeF = new System.Drawing.SizeF(139.9201F, 25.00001F);
        this.xrLabel73.StylePriority.UseBorders = false;
        this.xrLabel73.StylePriority.UseFont = false;
        this.xrLabel73.StylePriority.UseTextAlignment = false;
        this.xrLabel73.Text = "បានឃើញ និង​​ យល់ព្រម";
        this.xrLabel73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel68
        // 
        this.xrLabel68.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel68.CanGrow = false;
        this.xrLabel68.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F);
        this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel68.Multiline = true;
        this.xrLabel68.Name = "xrLabel68";
        this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel68.SizeF = new System.Drawing.SizeF(139.9201F, 25.00001F);
        this.xrLabel68.StylePriority.UseBorders = false;
        this.xrLabel68.StylePriority.UseFont = false;
        this.xrLabel68.StylePriority.UseTextAlignment = false;
        this.xrLabel68.Text = "អ្នករៀបចំ";
        this.xrLabel68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel67
        // 
        this.xrLabel67.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel67.CanGrow = false;
        this.xrLabel67.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
        this.xrLabel67.Multiline = true;
        this.xrLabel67.Name = "xrLabel67";
        this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel67.SizeF = new System.Drawing.SizeF(139.9201F, 17.00001F);
        this.xrLabel67.StylePriority.UseBorders = false;
        this.xrLabel67.StylePriority.UseFont = false;
        this.xrLabel67.StylePriority.UseTextAlignment = false;
        this.xrLabel67.Text = "Prepared by";
        this.xrLabel67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // GroupHeader2
        // 
        this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel39,
            this.xrLabel36,
            this.xrLabel34,
            this.xrLabel35});
        this.GroupHeader2.HeightF = 50F;
        this.GroupHeader2.Level = 1;
        this.GroupHeader2.Name = "GroupHeader2";
        // 
        // xrLabel37
        // 
        this.xrLabel37.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel37.CanGrow = false;
        this.xrLabel37.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PaymentTerm]")});
        this.xrLabel37.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22F);
        this.xrLabel37.Multiline = true;
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(222.1268F, 22F);
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.StylePriority.UseTextAlignment = false;
        this.xrLabel37.Text = "30 Day";
        this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel38
        // 
        this.xrLabel38.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel38.CanGrow = false;
        this.xrLabel38.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(411.3333F, 22F);
        this.xrLabel38.Multiline = true;
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(343.9634F, 22F);
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.StylePriority.UseTextAlignment = false;
        this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel39
        // 
        this.xrLabel39.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel39.CanGrow = false;
        this.xrLabel39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipTerm]")});
        this.xrLabel39.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(222.1268F, 22F);
        this.xrLabel39.Multiline = true;
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(189.2065F, 22F);
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.StylePriority.UseTextAlignment = false;
        this.xrLabel39.Text = "1 Day";
        this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel36
        // 
        this.xrLabel36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel36.CanGrow = false;
        this.xrLabel36.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(222.1268F, 0F);
        this.xrLabel36.Multiline = true;
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(189.2065F, 22F);
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.StylePriority.UseTextAlignment = false;
        this.xrLabel36.Text = "SHIPPING TERMS";
        this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel34
        // 
        this.xrLabel34.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel34.CanGrow = false;
        this.xrLabel34.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(411.3333F, 0F);
        this.xrLabel34.Multiline = true;
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(343.9634F, 22F);
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.StylePriority.UseTextAlignment = false;
        this.xrLabel34.Text = "DELIVERY DATE";
        this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel35
        // 
        this.xrLabel35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel35.CanGrow = false;
        this.xrLabel35.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel35.Multiline = true;
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(222.1268F, 22F);
        this.xrLabel35.StylePriority.UseBorders = false;
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.StylePriority.UseTextAlignment = false;
        this.xrLabel35.Text = "PAYMENT TERM";
        this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel15
        // 
        this.xrLabel15.CanGrow = false;
        this.xrLabel15.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel15.Multiline = true;
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(114.8367F, 25F);
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "VENDOR:";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel2.CanGrow = false;
        this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipEmail]")});
        this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 84F);
        this.xrLabel2.Multiline = true;
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(280.2117F, 25F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel4
        // 
        this.xrLabel4.CanGrow = false;
        this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipAddress]")});
        this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 25F);
        this.xrLabel4.Multiline = true;
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(280.2117F, 34F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel8
        // 
        this.xrLabel8.CanGrow = false;
        this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipName]")});
        this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 0F);
        this.xrLabel8.Multiline = true;
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(280.2117F, 25F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "SHIP TO";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel24
        // 
        this.xrLabel24.CanGrow = false;
        this.xrLabel24.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 25F);
        this.xrLabel24.Multiline = true;
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(63.75162F, 34F);
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.StylePriority.UseTextAlignment = false;
        this.xrLabel24.Text = "Address:";
        this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel25.CanGrow = false;
        this.xrLabel25.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 84F);
        this.xrLabel25.Multiline = true;
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(63.75162F, 25F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.StylePriority.UseTextAlignment = false;
        this.xrLabel25.Text = "E-mail:";
        this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel27
        // 
        this.xrLabel27.CanGrow = false;
        this.xrLabel27.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 59F);
        this.xrLabel27.Multiline = true;
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(63.75162F, 25F);
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "Phone:";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel26
        // 
        this.xrLabel26.CanGrow = false;
        this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipPhone]")});
        this.xrLabel26.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(475.085F, 59F);
        this.xrLabel26.Multiline = true;
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(280.2117F, 25F);
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel33
        // 
        this.xrLabel33.CanGrow = false;
        this.xrLabel33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VendorPhone]")});
        this.xrLabel33.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 59F);
        this.xrLabel33.Multiline = true;
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(296.4966F, 25F);
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.StylePriority.UseTextAlignment = false;
        this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel32
        // 
        this.xrLabel32.CanGrow = false;
        this.xrLabel32.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(0F, 59F);
        this.xrLabel32.Multiline = true;
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(114.8367F, 25F);
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.StylePriority.UseTextAlignment = false;
        this.xrLabel32.Text = "Phone:";
        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel31.CanGrow = false;
        this.xrLabel31.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(0F, 84F);
        this.xrLabel31.Multiline = true;
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(114.8367F, 25F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.StylePriority.UseTextAlignment = false;
        this.xrLabel31.Text = "E-mail:";
        this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel30
        // 
        this.xrLabel30.CanGrow = false;
        this.xrLabel30.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
        this.xrLabel30.Multiline = true;
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(114.8367F, 34F);
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.StylePriority.UseTextAlignment = false;
        this.xrLabel30.Text = "Address:";
        this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel29
        // 
        this.xrLabel29.CanGrow = false;
        this.xrLabel29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VendorAddress]")});
        this.xrLabel29.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 25F);
        this.xrLabel29.Multiline = true;
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(296.4966F, 34F);
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.StylePriority.UseTextAlignment = false;
        this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel28.CanGrow = false;
        this.xrLabel28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VendorEmail]")});
        this.xrLabel28.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 84F);
        this.xrLabel28.Multiline = true;
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(296.4966F, 25F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.StylePriority.UseTextAlignment = false;
        this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLabel33,
            this.xrLabel26,
            this.xrLabel27,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel8,
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel15});
        this.PageHeader.HeightF = 109F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrLabel11
        // 
        this.xrLabel11.CanGrow = false;
        this.xrLabel11.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(411.3334F, 0F);
        this.xrLabel11.Multiline = true;
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(63.75162F, 25F);
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.Text = "SHIP TO";
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        // 
        // xrLabel10
        // 
        this.xrLabel10.CanGrow = false;
        this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VendorName]")});
        this.xrLabel10.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(114.8367F, 0F);
        this.xrLabel10.Multiline = true;
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(296.4966F, 25F);
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // PONumber
        // 
        this.PONumber.Name = "PONumber";
        // 
        // sqlDataSource1
        // 
        this.sqlDataSource1.ConnectionName = "ReportConnectionString";
        this.sqlDataSource1.Name = "sqlDataSource1";
        storedProcQuery1.Name = "HR_RPT_PO";
        queryParameter1.Name = "@PONumber";
        queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
        queryParameter1.Value = new DevExpress.DataAccess.Expression("?PONumber", typeof(string));
        storedProcQuery1.Parameters.Add(queryParameter1);
        storedProcQuery1.StoredProcName = "HR_RPT_PO";
        this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
        this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
        // 
        // RPT_PURCHASE_ORDER
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.ReportFooter,
            this.GroupHeader2});
        this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
        this.DataMember = "HR_RPT_PO";
        this.DataSource = this.sqlDataSource1;
        this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
        this.Margins = new DevExpress.Drawing.DXMargins(35, 35, 35, 32);
        this.PageHeight = 1169;
        this.PageWidth = 827;
        this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PONumber});
        this.Version = "19.2";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
