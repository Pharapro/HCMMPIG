using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for RptMonthlyReport
/// </summary>
public class RptMonthlyReport : DevExpress.XtraReports.UI.XtraReport
{
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private DetailBand Detail;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel3;
    private XRLabel xrLabel6;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel5;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel66;
    private XRLabel xrLabel2;
    private XRLabel xrLabel65;
    private XRLabel xrLabel64;
    private XRLabel xrLabel63;
    private XRLabel xrLabel62;
    private XRLabel xrLabel7;
    private XRLabel xrLabel4;
    private XRLabel xrLabel27;
    private XRLabel xrLabel28;
    private XRLabel xrLabel29;
    private XRLabel xrLabel30;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLabel xrLabel33;
    private XRLabel xrLabel34;
    private XRLabel xrLabel35;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRLabel xrLabel39;
    private XRLabel xrLabel40;
    private XRLabel xrLabel41;
    private XRLabel xrLabel42;
    private XRLabel xrLabel43;
    private XRLabel xrLabel44;
    private XRLabel xrLabel45;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private ReportFooterBand ReportFooter;
    private GroupFooterBand GroupFooter1;
    private XRLabel xrLabel58;
    private XRLabel xrLabel59;
    private XRLabel xrLabel60;
    private XRLabel xrLabel61;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLabel xrLabel56;
    private XRLabel xrLabel57;
    private XRLabel xrLabel53;
    private XRLabel xrLabel52;
    private XRLabel xrLabel51;
    private XRLabel xrLabel50;
    private XRLabel xrLabel49;
    private XRLabel xrLabel48;
    private XRLabel xrLabel47;
    private XRLabel xrLabel46;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private DevExpress.XtraReports.Parameters.Parameter Branch;
    private DevExpress.XtraReports.Parameters.Parameter Division;
    private DevExpress.XtraReports.Parameters.Parameter Department;
    private DevExpress.XtraReports.Parameters.Parameter Section;
    private DevExpress.XtraReports.Parameters.Parameter Position;
    private DevExpress.XtraReports.Parameters.Parameter Level;
    private DevExpress.XtraReports.Parameters.Parameter InMonth;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RptMonthlyReport()
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
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary9 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary10 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary11 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary12 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary13 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary14 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary15 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary16 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary17 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary19 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
        DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptMonthlyReport));
        DevExpress.XtraReports.UI.XRSummary xrSummary18 = new DevExpress.XtraReports.UI.XRSummary();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
        this.Branch = new DevExpress.XtraReports.Parameters.Parameter();
        this.Division = new DevExpress.XtraReports.Parameters.Parameter();
        this.Department = new DevExpress.XtraReports.Parameters.Parameter();
        this.Section = new DevExpress.XtraReports.Parameters.Parameter();
        this.Position = new DevExpress.XtraReports.Parameters.Parameter();
        this.Level = new DevExpress.XtraReports.Parameters.Parameter();
        this.InMonth = new DevExpress.XtraReports.Parameters.Parameter();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
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
        this.BottomMargin.HeightF = 38F;
        this.BottomMargin.Name = "BottomMargin";
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.Detail.HeightF = 25F;
        this.Detail.Name = "Detail";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel6,
            this.xrPictureBox1,
            this.xrLabel5,
            this.xrLabel1});
        this.ReportHeader.HeightF = 115.9999F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel66,
            this.xrLabel2,
            this.xrLabel65,
            this.xrLabel64,
            this.xrLabel63,
            this.xrLabel62,
            this.xrLabel7,
            this.xrLabel4,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLabel33,
            this.xrLabel34,
            this.xrLabel35,
            this.xrLabel36,
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel39,
            this.xrLabel40,
            this.xrLabel41,
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel44,
            this.xrLabel45,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8});
        this.GroupHeader1.HeightF = 86.00003F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // ReportFooter
        // 
        this.ReportFooter.HeightF = 0F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel58,
            this.xrLabel59,
            this.xrLabel60,
            this.xrLabel61,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel56,
            this.xrLabel57,
            this.xrLabel53,
            this.xrLabel52,
            this.xrLabel51,
            this.xrLabel50,
            this.xrLabel49,
            this.xrLabel48,
            this.xrLabel47,
            this.xrLabel46,
            this.xrTable1});
        this.GroupFooter1.HeightF = 180.0813F;
        this.GroupFooter1.Name = "GroupFooter1";
        // 
        // xrTable3
        // 
        this.xrTable3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] { new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0))) });
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(1156.269F, 25F);
        this.xrTable3.StylePriority.UseFont = false;
        this.xrTable3.StylePriority.UseTextAlignment = false;
        this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell5,
            this.xrTableCell21,
            this.xrTableCell20,
            this.xrTableCell4,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell2,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell26,
            this.xrTableCell56,
            this.xrTableCell57});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
        this.xrTableCell39.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell39.Multiline = true;
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.StylePriority.UseBorders = false;
        this.xrTableCell39.StylePriority.UseFont = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell39.Summary = xrSummary1;
        this.xrTableCell39.Weight = 0.791932082293632D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")});
        this.xrTableCell40.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell40.Multiline = true;
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.StylePriority.UseBorders = false;
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.Text = "VAN-10003";
        this.xrTableCell40.Weight = 1.8632473452012202D;
        // 
        // xrTableCell41
        // 
        this.xrTableCell41.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell41.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployeeKH]")});
        this.xrTableCell41.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrTableCell41.Multiline = true;
        this.xrTableCell41.Name = "xrTableCell41";
        this.xrTableCell41.StylePriority.UseBorders = false;
        this.xrTableCell41.StylePriority.UseFont = false;
        this.xrTableCell41.StylePriority.UseTextAlignment = false;
        this.xrTableCell41.Text = "បេអ៊រ ហ្សាវីយ៉េហ្យែហេននី";
        this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell41.Weight = 3.157454141411697D;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell42.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Employee]")});
        this.xrTableCell42.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell42.Multiline = true;
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.StylePriority.UseBorders = false;
        this.xrTableCell42.StylePriority.UseFont = false;
        this.xrTableCell42.StylePriority.UseTextAlignment = false;
        this.xrTableCell42.Text = "BEAUR Xavier Pierre Henri";
        this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell42.Weight = 3.5048337219203578D;
        // 
        // xrTableCell43
        // 
        this.xrTableCell43.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell43.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Sex]")});
        this.xrTableCell43.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell43.Multiline = true;
        this.xrTableCell43.Name = "xrTableCell43";
        this.xrTableCell43.StylePriority.UseBorders = false;
        this.xrTableCell43.StylePriority.UseFont = false;
        this.xrTableCell43.Text = "Female";
        this.xrTableCell43.Weight = 1.2202521098740002D;
        // 
        // xrTableCell44
        // 
        this.xrTableCell44.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell44.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[POSITION]")});
        this.xrTableCell44.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell44.Multiline = true;
        this.xrTableCell44.Name = "xrTableCell44";
        this.xrTableCell44.StylePriority.UseBorders = false;
        this.xrTableCell44.StylePriority.UseFont = false;
        this.xrTableCell44.StylePriority.UseTextAlignment = false;
        this.xrTableCell44.Text = "Gardener Carpenter Manager";
        this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell44.Weight = 3.5929098536337061D;
        // 
        // xrTableCell45
        // 
        this.xrTableCell45.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell45.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BaseSalary]")});
        this.xrTableCell45.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell45.Multiline = true;
        this.xrTableCell45.Name = "xrTableCell45";
        this.xrTableCell45.StylePriority.UseBorders = false;
        this.xrTableCell45.StylePriority.UseFont = false;
        this.xrTableCell45.StylePriority.UseTextAlignment = false;
        this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell45.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell45.Weight = 1.554637024662306D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NormalOT]")});
        this.xrTableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell5.Multiline = true;
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell5.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell5.Weight = 1.2957922507463195D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OFFOT]")});
        this.xrTableCell21.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell21.Multiline = true;
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseBorders = false;
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "xrTableCell21";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell21.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell21.Weight = 1.1848613781529476D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PHOT]")});
        this.xrTableCell20.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell20.Multiline = true;
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseBorders = false;
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "xrTableCell20";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell20.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell20.Weight = 1.2488562833780796D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ONOT]")});
        this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell4.Multiline = true;
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "xrTableCell4";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell4.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell4.Weight = 1.4103409760459678D;
        // 
        // xrTableCell46
        // 
        this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell46.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OTAM]")});
        this.xrTableCell46.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell46.Multiline = true;
        this.xrTableCell46.Name = "xrTableCell46";
        this.xrTableCell46.StylePriority.UseBorders = false;
        this.xrTableCell46.StylePriority.UseFont = false;
        this.xrTableCell46.StylePriority.UseTextAlignment = false;
        this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell46.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell46.Weight = 1.1478814400947286D;
        // 
        // xrTableCell47
        // 
        this.xrTableCell47.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell47.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Meals]")});
        this.xrTableCell47.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell47.Multiline = true;
        this.xrTableCell47.Name = "xrTableCell47";
        this.xrTableCell47.StylePriority.UseBorders = false;
        this.xrTableCell47.StylePriority.UseFont = false;
        this.xrTableCell47.StylePriority.UseTextAlignment = false;
        this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell47.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell47.Weight = 1.6639816337929623D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parking]")});
        this.xrTableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell2.Multiline = true;
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell2.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell2.Weight = 1.6518954545196747D;
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell48.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AllwPhone]")});
        this.xrTableCell48.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell48.Multiline = true;
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseBorders = false;
        this.xrTableCell48.StylePriority.UseFont = false;
        this.xrTableCell48.StylePriority.UseTextAlignment = false;
        this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell48.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell48.Weight = 1.5779408585014259D;
        // 
        // xrTableCell49
        // 
        this.xrTableCell49.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell49.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Transport]")});
        this.xrTableCell49.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell49.Multiline = true;
        this.xrTableCell49.Name = "xrTableCell49";
        this.xrTableCell49.StylePriority.UseBorders = false;
        this.xrTableCell49.StylePriority.UseFont = false;
        this.xrTableCell49.StylePriority.UseTextAlignment = false;
        this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell49.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell49.Weight = 2.0067370995036482D;
        // 
        // xrTableCell50
        // 
        this.xrTableCell50.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell50.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Incentive]")});
        this.xrTableCell50.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell50.Multiline = true;
        this.xrTableCell50.Name = "xrTableCell50";
        this.xrTableCell50.StylePriority.UseBorders = false;
        this.xrTableCell50.StylePriority.UseFont = false;
        this.xrTableCell50.StylePriority.UseTextAlignment = false;
        this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell50.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell50.Weight = 1.6369636162812205D;
        // 
        // xrTableCell51
        // 
        this.xrTableCell51.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Seniority]")});
        this.xrTableCell51.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell51.Multiline = true;
        this.xrTableCell51.Name = "xrTableCell51";
        this.xrTableCell51.StylePriority.UseBorders = false;
        this.xrTableCell51.StylePriority.UseFont = false;
        this.xrTableCell51.StylePriority.UseTextAlignment = false;
        this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell51.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell51.Weight = 1.5260306738467091D;
        // 
        // xrTableCell52
        // 
        this.xrTableCell52.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Increased]")});
        this.xrTableCell52.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell52.Multiline = true;
        this.xrTableCell52.Name = "xrTableCell52";
        this.xrTableCell52.StylePriority.UseBorders = false;
        this.xrTableCell52.StylePriority.UseFont = false;
        this.xrTableCell52.StylePriority.UseTextAlignment = false;
        this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell52.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell52.Weight = 1.8588332839040804D;
        // 
        // xrTableCell53
        // 
        this.xrTableCell53.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell53.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Adjusted]")});
        this.xrTableCell53.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell53.Multiline = true;
        this.xrTableCell53.Name = "xrTableCell53";
        this.xrTableCell53.StylePriority.UseBorders = false;
        this.xrTableCell53.StylePriority.UseFont = false;
        this.xrTableCell53.StylePriority.UseTextAlignment = false;
        this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell53.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell53.Weight = 1.5999875560103609D;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell54.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ServiceCharge]")});
        this.xrTableCell54.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell54.Multiline = true;
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.StylePriority.UseBorders = false;
        this.xrTableCell54.StylePriority.UseFont = false;
        this.xrTableCell54.StylePriority.UseTextAlignment = false;
        this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell54.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell54.Weight = 1.3411461474264186D;
        // 
        // xrTableCell55
        // 
        this.xrTableCell55.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell55.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Deductions]")});
        this.xrTableCell55.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell55.Multiline = true;
        this.xrTableCell55.Name = "xrTableCell55";
        this.xrTableCell55.StylePriority.UseBorders = false;
        this.xrTableCell55.StylePriority.UseFont = false;
        this.xrTableCell55.StylePriority.UseTextAlignment = false;
        this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell55.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell55.Weight = 1.4890508478518463D;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell56.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NetWage]")});
        this.xrTableCell56.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell56.Multiline = true;
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseBorders = false;
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.StylePriority.UseTextAlignment = false;
        this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell56.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell56.Weight = 1.7478968207037049D;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell57.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell57.Multiline = true;
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.StylePriority.UseBorders = false;
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.StylePriority.UseTextAlignment = false;
        this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell57.Weight = 1.3041723903186591D;
        // 
        // xrLabel3
        // 
        this.xrLabel3.CanGrow = false;
        this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'ក្រុម/Team: \' +[BranchDesc]")});
        this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 11F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(157.1945F, 57.99996F);
        this.xrLabel3.Multiline = true;
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(437.0343F, 28.99998F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Salary for Employee";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel6
        // 
        this.xrLabel6.CanGrow = false;
        this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'តារាងប្រាក់ខែនិយោជិតសំរាប់ខែ\' + [MonthKH] +\' \' + [YearKH]")});
        this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 11F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(157.1945F, 0F);
        this.xrLabel6.Multiline = true;
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(437.0344F, 28.99998F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "តារាងប្រាក់ខែនិយោជិតសំរាប់ខែធ្នូ ២០២០";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "[Com_Image]")});
        this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(157.1945F, 115.9999F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        // 
        // xrLabel5
        // 
        this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'ទីតាំង/Location:  \'  + [Com_Address]")});
        this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 11F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(157.1946F, 86.99994F);
        this.xrLabel5.Multiline = true;
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(437.0343F, 28.99998F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel1
        // 
        this.xrLabel1.CanGrow = false;
        this.xrLabel1.CanPublish = false;
        this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Salary In \' + FormatString(\'{0:MMMM yyyy}\',?InMonth)")});
        this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 11F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(157.1945F, 28.99998F);
        this.xrLabel1.Multiline = true;
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(437.0344F, 28.99998F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "Salary In ";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrLabel1.TextFormatString = "{0:MMM yyyy}";
        // 
        // xrLabel65
        // 
        this.xrLabel65.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel65.CanGrow = false;
        this.xrLabel65.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(525.0453F, 42.99999F);
        this.xrLabel65.Multiline = true;
        this.xrLabel65.Name = "xrLabel65";
        this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel65.SizeF = new System.Drawing.SizeF(38.14069F, 43F);
        this.xrLabel65.StylePriority.UseBorders = false;
        this.xrLabel65.StylePriority.UseFont = false;
        this.xrLabel65.StylePriority.UseTextAlignment = false;
        this.xrLabel65.Text = "ថែមម៉ោងយប់/ON";
        this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel64
        // 
        this.xrLabel64.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel64.CanGrow = false;
        this.xrLabel64.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(491.2718F, 43.00002F);
        this.xrLabel64.Multiline = true;
        this.xrLabel64.Name = "xrLabel64";
        this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel64.SizeF = new System.Drawing.SizeF(33.77353F, 43F);
        this.xrLabel64.StylePriority.UseBorders = false;
        this.xrLabel64.StylePriority.UseFont = false;
        this.xrLabel64.StylePriority.UseTextAlignment = false;
        this.xrLabel64.Text = "ថ្ងៃបុណ្យ\r\nPH";
        this.xrLabel64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel63
        // 
        this.xrLabel63.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel63.CanGrow = false;
        this.xrLabel63.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(459.2289F, 43.00002F);
        this.xrLabel63.Multiline = true;
        this.xrLabel63.Name = "xrLabel63";
        this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel63.SizeF = new System.Drawing.SizeF(32.04291F, 43F);
        this.xrLabel63.StylePriority.UseBorders = false;
        this.xrLabel63.StylePriority.UseFont = false;
        this.xrLabel63.StylePriority.UseTextAlignment = false;
        this.xrLabel63.Text = "ថ្ងៃឈប់\r\nOFF";
        this.xrLabel63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel62
        // 
        this.xrLabel62.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel62.CanGrow = false;
        this.xrLabel62.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(424.1859F, 43.00001F);
        this.xrLabel62.Multiline = true;
        this.xrLabel62.Name = "xrLabel62";
        this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel62.SizeF = new System.Drawing.SizeF(35.04291F, 43F);
        this.xrLabel62.StylePriority.UseBorders = false;
        this.xrLabel62.StylePriority.UseFont = false;
        this.xrLabel62.StylePriority.UseTextAlignment = false;
        this.xrLabel62.Text = "ថ្ងៃធម្មតា\r\nNormal";
        this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel7
        // 
        this.xrLabel7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel7.CanGrow = false;
        this.xrLabel7.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(639.2289F, 42.99998F);
        this.xrLabel7.Multiline = true;
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(44.67322F, 43.00002F);
        this.xrLabel7.StylePriority.UseBorders = false;
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        this.xrLabel7.Text = "Parking Allowance";
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel4.CanGrow = false;
        this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(594.2289F, 0F);
        this.xrLabel4.Multiline = true;
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(45F, 43F);
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "ប្រាក់បាយ";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel27
        // 
        this.xrLabel27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel27.CanGrow = false;
        this.xrLabel27.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(0F, 43.00001F);
        this.xrLabel27.Multiline = true;
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(21.41669F, 43F);
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "Nº";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel28.CanGrow = false;
        this.xrLabel28.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(21.41669F, 43.00001F);
        this.xrLabel28.Multiline = true;
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(50.3889F, 43F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.StylePriority.UseTextAlignment = false;
        this.xrLabel28.Text = "staff ID";
        this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel29
        // 
        this.xrLabel29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel29.CanGrow = false;
        this.xrLabel29.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(71.80559F, 43.00001F);
        this.xrLabel29.Multiline = true;
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(85.3889F, 43F);
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.StylePriority.UseTextAlignment = false;
        this.xrLabel29.Text = "Name in Khmer";
        this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel30
        // 
        this.xrLabel30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel30.CanGrow = false;
        this.xrLabel30.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(157.1945F, 43.00001F);
        this.xrLabel30.Multiline = true;
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(94.78329F, 43F);
        this.xrLabel30.StylePriority.UseBorders = false;
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.StylePriority.UseTextAlignment = false;
        this.xrLabel30.Text = "Name in English";
        this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel31.CanGrow = false;
        this.xrLabel31.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(251.9778F, 43.00001F);
        this.xrLabel31.Multiline = true;
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(33F, 43F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.StylePriority.UseTextAlignment = false;
        this.xrLabel31.Text = "Sex";
        this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel32
        // 
        this.xrLabel32.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel32.CanGrow = false;
        this.xrLabel32.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(284.9778F, 43.00001F);
        this.xrLabel32.Multiline = true;
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(97.16519F, 43F);
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.StylePriority.UseTextAlignment = false;
        this.xrLabel32.Text = "Position";
        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel33
        // 
        this.xrLabel33.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel33.CanGrow = false;
        this.xrLabel33.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(382.143F, 43.00001F);
        this.xrLabel33.Multiline = true;
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(42.04297F, 43F);
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.StylePriority.UseTextAlignment = false;
        this.xrLabel33.Text = "Basic salary";
        this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel34
        // 
        this.xrLabel34.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel34.CanGrow = false;
        this.xrLabel34.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(563.186F, 43.00001F);
        this.xrLabel34.Multiline = true;
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(31.04291F, 43F);
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.StylePriority.UseTextAlignment = false;
        this.xrLabel34.Text = "Total";
        this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel35
        // 
        this.xrLabel35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel35.CanGrow = false;
        this.xrLabel35.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(594.2289F, 43.00001F);
        this.xrLabel35.Multiline = true;
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(45F, 43F);
        this.xrLabel35.StylePriority.UseBorders = false;
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.StylePriority.UseTextAlignment = false;
        this.xrLabel35.Text = "Meal\r\nAllowance";
        this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel36
        // 
        this.xrLabel36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel36.CanGrow = false;
        this.xrLabel36.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(683.9021F, 43.00001F);
        this.xrLabel36.Multiline = true;
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(42.67322F, 43F);
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.StylePriority.UseTextAlignment = false;
        this.xrLabel36.Text = "Phone card allowance";
        this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel37
        // 
        this.xrLabel37.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel37.CanGrow = false;
        this.xrLabel37.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(726.5753F, 43.00001F);
        this.xrLabel37.Multiline = true;
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(54.26941F, 43F);
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.StylePriority.UseTextAlignment = false;
        this.xrLabel37.Text = "Transport allowance";
        this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel38
        // 
        this.xrLabel38.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel38.CanGrow = false;
        this.xrLabel38.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(780.8447F, 43.00001F);
        this.xrLabel38.Multiline = true;
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(44.26941F, 43F);
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.StylePriority.UseTextAlignment = false;
        this.xrLabel38.Text = "Incentive/Bonus";
        this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel39
        // 
        this.xrLabel39.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel39.CanGrow = false;
        this.xrLabel39.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(825.1141F, 43.00001F);
        this.xrLabel39.Multiline = true;
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(41.26941F, 43F);
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.StylePriority.UseTextAlignment = false;
        this.xrLabel39.Text = "Seniority Payment";
        this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel40
        // 
        this.xrLabel40.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel40.CanGrow = false;
        this.xrLabel40.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(866.3835F, 43.00001F);
        this.xrLabel40.Multiline = true;
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(50.26941F, 43F);
        this.xrLabel40.StylePriority.UseBorders = false;
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.StylePriority.UseTextAlignment = false;
        this.xrLabel40.Text = "Salary Increased";
        this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel41
        // 
        this.xrLabel41.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel41.CanGrow = false;
        this.xrLabel41.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(916.653F, 43.00001F);
        this.xrLabel41.Multiline = true;
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(43.26941F, 43F);
        this.xrLabel41.StylePriority.UseBorders = false;
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.StylePriority.UseTextAlignment = false;
        this.xrLabel41.Text = "Salary adjment";
        this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel42
        // 
        this.xrLabel42.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel42.CanGrow = false;
        this.xrLabel42.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(959.9224F, 43.00001F);
        this.xrLabel42.Multiline = true;
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(36.26941F, 43F);
        this.xrLabel42.StylePriority.UseBorders = false;
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.StylePriority.UseTextAlignment = false;
        this.xrLabel42.Text = "Service charge 5%";
        this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel43
        // 
        this.xrLabel43.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel43.CanGrow = false;
        this.xrLabel43.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(996.1918F, 43.00001F);
        this.xrLabel43.Multiline = true;
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(40.26941F, 43F);
        this.xrLabel43.StylePriority.UseBorders = false;
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.StylePriority.UseTextAlignment = false;
        this.xrLabel43.Text = "Salary deducted";
        this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel44
        // 
        this.xrLabel44.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel44.CanGrow = false;
        this.xrLabel44.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(1073.73F, 43.00003F);
        this.xrLabel44.Multiline = true;
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(47.26941F, 43F);
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.StylePriority.UseTextAlignment = false;
        this.xrLabel44.Text = "NET Salary";
        this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel45
        // 
        this.xrLabel45.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel45.CanGrow = false;
        this.xrLabel45.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(1121F, 43.00003F);
        this.xrLabel45.Multiline = true;
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(35.26941F, 43F);
        this.xrLabel45.StylePriority.UseBorders = false;
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.StylePriority.UseTextAlignment = false;
        this.xrLabel45.Text = "Other";
        this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel26.CanGrow = false;
        this.xrLabel26.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(1121F, 0F);
        this.xrLabel26.Multiline = true;
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(35.26941F, 43F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.Text = "ផ្សេងៗ";
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel25.CanGrow = false;
        this.xrLabel25.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(1073.73F, 7.629395E-06F);
        this.xrLabel25.Multiline = true;
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(47.26941F, 43F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.StylePriority.UseTextAlignment = false;
        this.xrLabel25.Text = "ប្រាក់ទទួលបាន";
        this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel24
        // 
        this.xrLabel24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel24.CanGrow = false;
        this.xrLabel24.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(996.1918F, 7.629395E-06F);
        this.xrLabel24.Multiline = true;
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(40.26941F, 43F);
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.StylePriority.UseTextAlignment = false;
        this.xrLabel24.Text = "ប្រាក់ដែលបានកាត់";
        this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel23
        // 
        this.xrLabel23.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel23.CanGrow = false;
        this.xrLabel23.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(959.9224F, 7.629395E-06F);
        this.xrLabel23.Multiline = true;
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(36.26941F, 43F);
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.StylePriority.UseTextAlignment = false;
        this.xrLabel23.Text = "ប្រាក់ថ្លៃសេវ៉ាកម្ម";
        this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel22
        // 
        this.xrLabel22.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel22.CanGrow = false;
        this.xrLabel22.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(916.653F, 7.629395E-06F);
        this.xrLabel22.Multiline = true;
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(43.26941F, 43F);
        this.xrLabel22.StylePriority.UseBorders = false;
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.StylePriority.UseTextAlignment = false;
        this.xrLabel22.Text = "ប្រាក់ខែកែរតម្រូវខែមុន";
        this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel21
        // 
        this.xrLabel21.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel21.CanGrow = false;
        this.xrLabel21.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(866.3835F, 7.629395E-06F);
        this.xrLabel21.Multiline = true;
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(50.26941F, 43F);
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = "ប្រាក់ខែដែលបានតម្លើង";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel20
        // 
        this.xrLabel20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel20.CanGrow = false;
        this.xrLabel20.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(825.1141F, 7.629395E-06F);
        this.xrLabel20.Multiline = true;
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(41.26941F, 43F);
        this.xrLabel20.StylePriority.UseBorders = false;
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.Text = "ប្រាក់រំលឹកអតីតភាព";
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel19
        // 
        this.xrLabel19.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel19.CanGrow = false;
        this.xrLabel19.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(780.8447F, 7.629395E-06F);
        this.xrLabel19.Multiline = true;
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(44.26941F, 43F);
        this.xrLabel19.StylePriority.UseBorders = false;
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.StylePriority.UseTextAlignment = false;
        this.xrLabel19.Text = "ប្រាក់រង្វាន់";
        this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel18.CanGrow = false;
        this.xrLabel18.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(726.5753F, 7.629395E-06F);
        this.xrLabel18.Multiline = true;
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(54.26941F, 43F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.StylePriority.UseTextAlignment = false;
        this.xrLabel18.Text = "ប្រាក់សោហ៊ុយធ្វើដំណើ";
        this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel17.CanGrow = false;
        this.xrLabel17.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(683.9021F, 7.629395E-06F);
        this.xrLabel17.Multiline = true;
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(42.67322F, 43F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.StylePriority.UseTextAlignment = false;
        this.xrLabel17.Text = "ប្រាក់កាត\r\nទូរស័ព្ទ";
        this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel16.CanGrow = false;
        this.xrLabel16.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(639.2289F, 7.629395E-06F);
        this.xrLabel16.Multiline = true;
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(44.67322F, 43F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.Text = "ចតម៉ូតូ";
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel15.CanGrow = false;
        this.xrLabel15.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(424.1859F, 7.629395E-06F);
        this.xrLabel15.Multiline = true;
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(170.0429F, 43F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "ប្រាក់ថែមម៉ោង";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel14.CanGrow = false;
        this.xrLabel14.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(382.143F, 7.629395E-06F);
        this.xrLabel14.Multiline = true;
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(42.04297F, 43F);
        this.xrLabel14.StylePriority.UseBorders = false;
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.StylePriority.UseTextAlignment = false;
        this.xrLabel14.Text = "ប្រាក់ខែ\r\nគោល";
        this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel13
        // 
        this.xrLabel13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel13.CanGrow = false;
        this.xrLabel13.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(284.9778F, 7.629395E-06F);
        this.xrLabel13.Multiline = true;
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(97.16519F, 43F);
        this.xrLabel13.StylePriority.UseBorders = false;
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.StylePriority.UseTextAlignment = false;
        this.xrLabel13.Text = "តួនាទី";
        this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel12
        // 
        this.xrLabel12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel12.CanGrow = false;
        this.xrLabel12.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(251.9778F, 7.629395E-06F);
        this.xrLabel12.Multiline = true;
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(33F, 43F);
        this.xrLabel12.StylePriority.UseBorders = false;
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.StylePriority.UseTextAlignment = false;
        this.xrLabel12.Text = "ភេទ";
        this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel11.CanGrow = false;
        this.xrLabel11.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(157.1945F, 7.629395E-06F);
        this.xrLabel11.Multiline = true;
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(94.78329F, 43F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.Text = "ឈ្មោះអង់គ្លេស";
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel10.CanGrow = false;
        this.xrLabel10.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(71.80559F, 7.629395E-06F);
        this.xrLabel10.Multiline = true;
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(85.3889F, 43F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.Text = "ឈ្មោះភាសារខ្មែរ";
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel9.CanGrow = false;
        this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(21.41669F, 7.629395E-06F);
        this.xrLabel9.Multiline = true;
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(50.3889F, 43F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "លេខកាតសំគាល់";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel8.CanGrow = false;
        this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 7.629395E-06F);
        this.xrLabel8.Multiline = true;
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(21.41669F, 43F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "ល.រ";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel58
        // 
        this.xrLabel58.CanGrow = false;
        this.xrLabel58.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(0F, 149.0961F);
        this.xrLabel58.Multiline = true;
        this.xrLabel58.Name = "xrLabel58";
        this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel58.SizeF = new System.Drawing.SizeF(282.9778F, 23.99998F);
        this.xrLabel58.StylePriority.UseFont = false;
        this.xrLabel58.StylePriority.UseTextAlignment = false;
        this.xrLabel58.Text = "Signature & Name (Human Resource Manager)";
        this.xrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel59
        // 
        this.xrLabel59.CanGrow = false;
        this.xrLabel59.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(320.9777F, 149.0961F);
        this.xrLabel59.Multiline = true;
        this.xrLabel59.Name = "xrLabel59";
        this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel59.SizeF = new System.Drawing.SizeF(242.2512F, 23.99998F);
        this.xrLabel59.StylePriority.UseFont = false;
        this.xrLabel59.StylePriority.UseTextAlignment = false;
        this.xrLabel59.Text = "Signature & Name (Accounting Manager)";
        this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel60
        // 
        this.xrLabel60.CanGrow = false;
        this.xrLabel60.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(577.2288F, 149.0961F);
        this.xrLabel60.Multiline = true;
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(251.1548F, 23.99998F);
        this.xrLabel60.StylePriority.UseFont = false;
        this.xrLabel60.StylePriority.UseTextAlignment = false;
        this.xrLabel60.Text = "Signature & Name (Head Department)";
        this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel61
        // 
        this.xrLabel61.CanGrow = false;
        this.xrLabel61.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(870.6529F, 149.0961F);
        this.xrLabel61.Multiline = true;
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(248.3471F, 23.99998F);
        this.xrLabel61.StylePriority.UseFont = false;
        this.xrLabel61.StylePriority.UseTextAlignment = false;
        this.xrLabel61.Text = "Signature & Name / Mr. Xavier Beaur ( CEO)";
        this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel54
        // 
        this.xrLabel54.CanGrow = false;
        this.xrLabel54.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(0F, 125.0961F);
        this.xrLabel54.Multiline = true;
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(282.9778F, 23.99998F);
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.Text = "ហត្ថលេខា & ឈ្មោះ (ប្រធានគ្រប់គ្រងធនធានមនុស្ស)";
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel55
        // 
        this.xrLabel55.CanGrow = false;
        this.xrLabel55.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(320.9777F, 125.0961F);
        this.xrLabel55.Multiline = true;
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(242.2512F, 23.99998F);
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.StylePriority.UseTextAlignment = false;
        this.xrLabel55.Text = "ហត្ថលេខា & ឈ្មោះ​ (ប្រធានគណនេយ្យ)";
        this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel56
        // 
        this.xrLabel56.CanGrow = false;
        this.xrLabel56.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(577.2289F, 125.0961F);
        this.xrLabel56.Multiline = true;
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(251.1547F, 23.99998F);
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.StylePriority.UseTextAlignment = false;
        this.xrLabel56.Text = "ហត្ថលេខា & ឈ្មោះ​ (ប្រធានផ្នែក)";
        this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel57
        // 
        this.xrLabel57.CanGrow = false;
        this.xrLabel57.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(870.6529F, 125.0962F);
        this.xrLabel57.Multiline = true;
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(248.3471F, 23.99998F);
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.StylePriority.UseTextAlignment = false;
        this.xrLabel57.Text = "ហត្ថលេខា & ឈ្មោះ (លោក ហ្សាវីយ៉េ)";
        this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel53
        // 
        this.xrLabel53.CanGrow = false;
        this.xrLabel53.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(870.6529F, 101.0961F);
        this.xrLabel53.Multiline = true;
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(248.3471F, 23.99998F);
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        this.xrLabel53.Text = "Approved by:………………………………";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel52
        // 
        this.xrLabel52.CanGrow = false;
        this.xrLabel52.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(577.2289F, 101.0961F);
        this.xrLabel52.Multiline = true;
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(251.1547F, 23.99998F);
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "Confirmed by:……………………………";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel51
        // 
        this.xrLabel51.CanGrow = false;
        this.xrLabel51.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(320.9777F, 101.0961F);
        this.xrLabel51.Multiline = true;
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(242.2512F, 23.99998F);
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        this.xrLabel51.Text = "Verified by:………………………….............";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel50
        // 
        this.xrLabel50.CanGrow = false;
        this.xrLabel50.Font = new DevExpress.Drawing.DXFont("Arial", 8.5F);
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(0F, 101.0961F);
        this.xrLabel50.Multiline = true;
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(282.9778F, 23.99998F);
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "Prepared & Check by:............................................";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel49
        // 
        this.xrLabel49.CanGrow = false;
        this.xrLabel49.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(870.6529F, 31.00001F);
        this.xrLabel49.Multiline = true;
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(134.8083F, 28.99998F);
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.StylePriority.UseTextAlignment = false;
        this.xrLabel49.Text = "អនុម័តដោយៈ";
        this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel48
        // 
        this.xrLabel48.CanGrow = false;
        this.xrLabel48.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(577.2289F, 30.99999F);
        this.xrLabel48.Multiline = true;
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(155.6158F, 28.99998F);
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.StylePriority.UseTextAlignment = false;
        this.xrLabel48.Text = "បានបញ្ចាក់ដោយៈ";
        this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel47
        // 
        this.xrLabel47.CanGrow = false;
        this.xrLabel47.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(320.9777F, 31F);
        this.xrLabel47.Multiline = true;
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(164.2083F, 28.99998F);
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.StylePriority.UseTextAlignment = false;
        this.xrLabel47.Text = "ត្រួតពិនិត្យដោយៈ";
        this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel46
        // 
        this.xrLabel46.CanGrow = false;
        this.xrLabel46.Font = new DevExpress.Drawing.DXFont("Khmer OS Bokor", 8.5F, DevExpress.Drawing.DXFontStyle.Bold);
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31F);
        this.xrLabel46.Multiline = true;
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(282.9778F, 28.99998F);
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.StylePriority.UseTextAlignment = false;
        this.xrLabel46.Text = "រៀបចំ និង ត្រួតពិនិត្យដោយៈ";
        this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1156.269F, 25F);
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
        this.xrTable1.StylePriority.UseTextAlignment = false;
        this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell3,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell27,
            this.xrTableCell18,
            this.xrTableCell19});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(230)))), ((int)(((byte)(153)))));
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 7F);
        this.xrTableCell1.Multiline = true;
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBackColor = false;
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell1.Weight = 0.59737290481759531D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(230)))), ((int)(((byte)(153)))));
        this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell6.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell6.Multiline = true;
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBackColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "SUB TOTAL: ";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell6.Weight = 10.061692031091868D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([BaseSalary])")});
        this.xrTableCell7.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell7.Multiline = true;
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBackColor = false;
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell7.Summary = xrSummary2;
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell7.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell7.Weight = 1.1726981429718801D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([NormalOT])")});
        this.xrTableCell22.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell22.Multiline = true;
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseBackColor = false;
        this.xrTableCell22.StylePriority.UseBorders = false;
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell22.Summary = xrSummary3;
        this.xrTableCell22.Text = "xrTableCell22";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell22.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell22.Weight = 0.977445995219866D;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell23.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([OFFOT])")});
        this.xrTableCell23.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell23.Multiline = true;
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseBackColor = false;
        this.xrTableCell23.StylePriority.UseBorders = false;
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.StylePriority.UseTextAlignment = false;
        xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell23.Summary = xrSummary4;
        this.xrTableCell23.Text = "xrTableCell23";
        this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell23.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell23.Weight = 0.89376829615357711D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([PHOT])")});
        this.xrTableCell24.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell24.Multiline = true;
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseBackColor = false;
        this.xrTableCell24.StylePriority.UseBorders = false;
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell24.Summary = xrSummary5;
        this.xrTableCell24.Text = "xrTableCell24";
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell24.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell24.Weight = 0.94204111982539263D;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([ONOT])")});
        this.xrTableCell25.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell25.Multiline = true;
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.StylePriority.UseBackColor = false;
        this.xrTableCell25.StylePriority.UseBorders = false;
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.StylePriority.UseTextAlignment = false;
        xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell25.Summary = xrSummary6;
        this.xrTableCell25.Text = "xrTableCell25";
        this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell25.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell25.Weight = 1.063853328045101D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([OTAM])")});
        this.xrTableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell8.Multiline = true;
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseBackColor = false;
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell8.Summary = xrSummary7;
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell8.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell8.Weight = 0.86587459887165841D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Meals])")});
        this.xrTableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        xrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell9.Summary = xrSummary8;
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell9.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell9.Weight = 1.2551811872710155D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Parking])")});
        this.xrTableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell3.Multiline = true;
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseBackColor = false;
        this.xrTableCell3.StylePriority.UseBorders = false;
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.StylePriority.UseTextAlignment = false;
        xrSummary9.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell3.Summary = xrSummary9;
        this.xrTableCell3.Text = "xrTableCell3";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell3.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell3.Weight = 1.2460640016917455D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([AllwPhone])")});
        this.xrTableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell10.Multiline = true;
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseBackColor = false;
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        xrSummary10.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell10.Summary = xrSummary10;
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell10.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell10.Weight = 1.1902784858058184D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Transport])")});
        this.xrTableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell11.Multiline = true;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBackColor = false;
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        xrSummary11.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell11.Summary = xrSummary11;
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell11.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell11.Weight = 1.5137288855660716D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Incentive])")});
        this.xrTableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell12.Multiline = true;
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.StylePriority.UseBackColor = false;
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        xrSummary12.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell12.Summary = xrSummary12;
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell12.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell12.Weight = 1.2347985695248527D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Seniority])")});
        this.xrTableCell13.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell13.Multiline = true;
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.StylePriority.UseBackColor = false;
        this.xrTableCell13.StylePriority.UseBorders = false;
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        xrSummary13.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell13.Summary = xrSummary13;
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell13.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell13.Weight = 1.1511202885629608D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Increased])")});
        this.xrTableCell14.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell14.Multiline = true;
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBackColor = false;
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        xrSummary14.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell14.Summary = xrSummary14;
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell14.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell14.Weight = 1.4021593537849353D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Adjusted])")});
        this.xrTableCell15.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell15.Multiline = true;
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseBackColor = false;
        this.xrTableCell15.StylePriority.UseBorders = false;
        this.xrTableCell15.StylePriority.UseFont = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        xrSummary15.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell15.Summary = xrSummary15;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell15.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell15.Weight = 1.2069096528670922D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([ServiceCharge])")});
        this.xrTableCell16.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell16.Multiline = true;
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBackColor = false;
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        xrSummary16.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell16.Summary = xrSummary16;
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell16.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell16.Weight = 1.0116598475029144D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([Deductions])")});
        this.xrTableCell17.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell17.Multiline = true;
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBackColor = false;
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        xrSummary17.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell17.Summary = xrSummary17;
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell17.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell17.Weight = 1.1232249069797224D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([NetWage])")});
        this.xrTableCell18.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell18.Multiline = true;
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBackColor = false;
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        xrSummary19.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell18.Summary = xrSummary19;
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell18.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell18.Weight = 1.3184792626241619D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell19.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell19.Multiline = true;
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseBackColor = false;
        this.xrTableCell19.StylePriority.UseBorders = false;
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.StylePriority.UseTextAlignment = false;
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell19.Weight = 0.98376559622315607D;
        // 
        // sqlDataSource1
        // 
        this.sqlDataSource1.ConnectionName = "ReportConnectionString";
        this.sqlDataSource1.Name = "sqlDataSource1";
        storedProcQuery1.Name = "HR_RPT_MonthlyPay";
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
        storedProcQuery1.StoredProcName = "HR_RPT_MonthlyPay";
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
        // xrLabel2
        // 
        this.xrLabel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel2.CanGrow = false;
        this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 6F);
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1036.461F, 2.119276E-05F);
        this.xrLabel2.Multiline = true;
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(37.26941F, 43F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "ប្រាក់ សោធន";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel66
        // 
        this.xrLabel66.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel66.CanGrow = false;
        this.xrLabel66.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(1036.461F, 43.00003F);
        this.xrLabel66.Multiline = true;
        this.xrLabel66.Name = "xrLabel66";
        this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel66.SizeF = new System.Drawing.SizeF(37.26941F, 43F);
        this.xrLabel66.StylePriority.UseBorders = false;
        this.xrLabel66.StylePriority.UseFont = false;
        this.xrLabel66.StylePriority.UseTextAlignment = false;
        this.xrLabel66.Text = "Pension Fund";
        this.xrLabel66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StaffPensionFundAmount]")});
        this.xrTableCell26.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell26.Multiline = true;
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseBorders = false;
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell26.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell26.Weight = 1.3781233526314307D;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
        this.xrTableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([StaffPensionFundAmount])")});
        this.xrTableCell27.Font = new DevExpress.Drawing.DXFont("Arial", 6F);
        this.xrTableCell27.Multiline = true;
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.StylePriority.UseBackColor = false;
        this.xrTableCell27.StylePriority.UseBorders = false;
        this.xrTableCell27.StylePriority.UseFont = false;
        this.xrTableCell27.StylePriority.UseTextAlignment = false;
        xrSummary18.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell27.Summary = xrSummary18;
        this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell27.TextFormatString = "{0:#,0.00;(#,0.00);\'-\';}";
        this.xrTableCell27.Weight = 1.0395507898305074D;
        // 
        // RptMonthlyReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.GroupHeader1,
            this.ReportFooter,
            this.GroupFooter1});
        this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
        this.DataMember = "HR_RPT_MonthlyPay";
        this.DataSource = this.sqlDataSource1;
        this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
        this.Landscape = true;
        this.Margins = new DevExpress.Drawing.DXMargins(5, 5, 35, 38);
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
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
