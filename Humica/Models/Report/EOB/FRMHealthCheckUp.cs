using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for FRMHealthCheckUp
/// </summary>
public class FRMHealthCheckUp : DevExpress.XtraReports.UI.XtraReport
{
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private DetailBand Detail;
    private ReportHeaderBand ReportHeader;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel1;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel3;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRLabel xrLabel15;
    private XRLabel xrLabel16;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private DevExpress.XtraReports.Parameters.Parameter EmpCode;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRLabel xrLabel26;
    private XRLabel xrLabel27;
    private XRLabel xrLabel24;
    private XRLabel xrLabel25;
    private XRLabel xrLabel30;
    private XRLabel xrLabel31;
    private XRLabel xrLabel28;
    private XRLabel xrLabel29;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel2;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public FRMHealthCheckUp()
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
        DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRMHealthCheckUp));
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.EmpCode = new DevExpress.XtraReports.Parameters.Parameter();
        this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 35F;
        this.TopMargin.Name = "TopMargin";
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 39F;
        this.BottomMargin.Name = "BottomMargin";
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3});
        this.Detail.FillEmptySpace = true;
        this.Detail.HeightF = 26F;
        this.Detail.Name = "Detail";
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.CanGrow = false;
        this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 0F);
        this.xrLabel3.Multiline = true;
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(777F, 26F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
        this.ReportHeader.HeightF = 81.34805F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel1
        // 
        this.xrLabel1.BackColor = System.Drawing.Color.MediumSeaGreen;
        this.xrLabel1.CanGrow = false;
        this.xrLabel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.ForeColor = System.Drawing.Color.White;
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 46.84805F);
        this.xrLabel1.Multiline = true;
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(777F, 34.5F);
        this.xrLabel1.StylePriority.UseBackColor = false;
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseForeColor = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "Health Check Up Form";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel10,
            this.xrLabel11,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel15,
            this.xrLabel16,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel6,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel4});
        this.ReportFooter.HeightF = 124.25F;
        this.ReportFooter.Name = "ReportFooter";
        this.ReportFooter.PrintAtBottom = true;
        // 
        // xrLabel19
        // 
        this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel19.CanGrow = false;
        this.xrLabel19.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(750.625F, 0F);
        this.xrLabel19.Multiline = true;
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(26.375F, 25.99999F);
        this.xrLabel19.StylePriority.UseBorders = false;
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.StylePriority.UseTextAlignment = false;
        this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel18.CanGrow = false;
        this.xrLabel18.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel18.Multiline = true;
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(24.00001F, 25.99999F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.StylePriority.UseTextAlignment = false;
        this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel17.CanGrow = false;
        this.xrLabel17.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(269.75F, 0F);
        this.xrLabel17.Multiline = true;
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(210.125F, 25.99999F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.StylePriority.UseTextAlignment = false;
        this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel10.CanGrow = false;
        this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(597.5417F, 0F);
        this.xrLabel10.Multiline = true;
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel11.CanGrow = false;
        this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(479.875F, 0F);
        this.xrLabel11.Multiline = true;
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(117.6666F, 36.41669F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.Text = "Reviewed by :";
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel12
        // 
        this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel12.CanGrow = false;
        this.xrLabel12.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(597.5417F, 36.41669F);
        this.xrLabel12.Multiline = true;
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel12.StylePriority.UseBorders = false;
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.StylePriority.UseTextAlignment = false;
        this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel13
        // 
        this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel13.CanGrow = false;
        this.xrLabel13.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(479.875F, 36.41669F);
        this.xrLabel13.Multiline = true;
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(117.6666F, 36.41669F);
        this.xrLabel13.StylePriority.UseBorders = false;
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.StylePriority.UseTextAlignment = false;
        this.xrLabel13.Text = "Acknowledge by :";
        this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel15.CanGrow = false;
        this.xrLabel15.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(479.875F, 72.83338F);
        this.xrLabel15.Multiline = true;
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(117.6666F, 36.41669F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "Approved by :";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel16.CanGrow = false;
        this.xrLabel16.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(597.5417F, 72.83338F);
        this.xrLabel16.Multiline = true;
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel8.CanGrow = false;
        this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 72.83338F);
        this.xrLabel8.Multiline = true;
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel9.CanGrow = false;
        this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(24.00001F, 72.83338F);
        this.xrLabel9.Multiline = true;
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(92.66666F, 36.41669F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "Approved by :";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel6
        // 
        this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel6.CanGrow = false;
        this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(24.00001F, 36.41669F);
        this.xrLabel6.Multiline = true;
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(92.66666F, 36.41669F);
        this.xrLabel6.StylePriority.UseBorders = false;
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "Verified by :";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel7
        // 
        this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel7.CanGrow = false;
        this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 36.41669F);
        this.xrLabel7.Multiline = true;
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel7.StylePriority.UseBorders = false;
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel5.CanGrow = false;
        this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 0F);
        this.xrLabel5.Multiline = true;
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(153.0833F, 26F);
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel4.CanGrow = false;
        this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(24.00001F, 0F);
        this.xrLabel4.Multiline = true;
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(92.66666F, 36.41669F);
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "Prepared by :";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel26,
            this.xrLabel27,
            this.xrLabel24,
            this.xrLabel25,
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20});
        this.PageHeader.HeightF = 178.75F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrLabel2
        // 
        this.xrLabel2.BackColor = System.Drawing.Color.LightGray;
        this.xrLabel2.BorderColor = System.Drawing.Color.Gray;
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.CanGrow = false;
        this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1.86995E-05F, 130F);
        this.xrLabel2.Multiline = true;
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(777F, 28F);
        this.xrLabel2.StylePriority.UseBackColor = false;
        this.xrLabel2.StylePriority.UseBorderColor = false;
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "Description";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel26.CanGrow = false;
        this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HospitalAddr]")});
        this.xrLabel26.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 104F);
        this.xrLabel26.Multiline = true;
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(282.2917F, 26F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel27
        // 
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel27.CanGrow = false;
        this.xrLabel27.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 104F);
        this.xrLabel27.Multiline = true;
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "Hospital Address :";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel24
        // 
        this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel24.CanGrow = false;
        this.xrLabel24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HPName]")});
        this.xrLabel24.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 77.99999F);
        this.xrLabel24.Multiline = true;
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(282.2917F, 26F);
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.StylePriority.UseTextAlignment = false;
        this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel25.CanGrow = false;
        this.xrLabel25.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(0F, 77.99999F);
        this.xrLabel25.Multiline = true;
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.StylePriority.UseTextAlignment = false;
        this.xrLabel25.Text = "Hospital Name :";
        this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel30
        // 
        this.xrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel30.CanGrow = false;
        this.xrLabel30.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(0F, 51.99999F);
        this.xrLabel30.Multiline = true;
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel30.StylePriority.UseBorders = false;
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.StylePriority.UseTextAlignment = false;
        this.xrLabel30.Text = "Department :";
        this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel31.CanGrow = false;
        this.xrLabel31.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Deptartment]")});
        this.xrLabel31.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 51.99999F);
        this.xrLabel31.Multiline = true;
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(282.2917F, 26F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.StylePriority.UseTextAlignment = false;
        this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel28.CanGrow = false;
        this.xrLabel28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
        this.xrLabel28.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 25.99999F);
        this.xrLabel28.Multiline = true;
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(282.2917F, 26F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.StylePriority.UseTextAlignment = false;
        this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel29
        // 
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel29.CanGrow = false;
        this.xrLabel29.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25.99999F);
        this.xrLabel29.Multiline = true;
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.StylePriority.UseTextAlignment = false;
        this.xrLabel29.Text = "Position :";
        this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel23
        // 
        this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel23.CanGrow = false;
        this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Request date]")});
        this.xrLabel23.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(643.75F, 0F);
        this.xrLabel23.Multiline = true;
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(133.25F, 26F);
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.StylePriority.UseTextAlignment = false;
        this.xrLabel23.Text = "Employee Name";
        this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrLabel23.TextFormatString = "{0:dd.MM.yyyy}";
        // 
        // xrLabel22
        // 
        this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel22.CanGrow = false;
        this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AllName]")});
        this.xrLabel22.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 0F);
        this.xrLabel22.Multiline = true;
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(282.2917F, 26F);
        this.xrLabel22.StylePriority.UseBorders = false;
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.StylePriority.UseTextAlignment = false;
        this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel21
        // 
        this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel21.CanGrow = false;
        this.xrLabel21.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(527.0834F, 0F);
        this.xrLabel21.Multiline = true;
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = "Requested Date :";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel20
        // 
        this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel20.CanGrow = false;
        this.xrLabel20.Font = new System.Drawing.Font("Arial", 9.75F);
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 0F);
        this.xrLabel20.Multiline = true;
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(116.6666F, 26F);
        this.xrLabel20.StylePriority.UseBorders = false;
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.Text = "Employee Name :";
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // EmpCode
        // 
        this.EmpCode.Name = "EmpCode";
        // 
        // sqlDataSource1
        // 
        this.sqlDataSource1.ConnectionName = "ReportConnectionString";
        this.sqlDataSource1.Name = "sqlDataSource1";
        customSqlQuery1.Name = "Query";
        queryParameter1.Name = "EmpCode";
        queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
        queryParameter1.Value = new DevExpress.DataAccess.Expression("?EmpCode", typeof(string));
        customSqlQuery1.Parameters.Add(queryParameter1);
        customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
        this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
        this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
        // 
        // FRMHealthCheckUp
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.ReportFooter,
            this.PageHeader});
        this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
        this.DataMember = "Query";
        this.DataSource = this.sqlDataSource1;
        this.Font = new System.Drawing.Font("Arial", 9.75F);
        this.Margins = new System.Drawing.Printing.Margins(25, 25, 35, 39);
        this.PageHeight = 1169;
        this.PageWidth = 827;
        this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.EmpCode});
        this.Version = "19.2";
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
