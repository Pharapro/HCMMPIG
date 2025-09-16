namespace HUMICA.Models.Report.EFORM
{
    partial class RPTRequestNewIDCard
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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPTRequestNewIDCard));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.label3 = new DevExpress.XtraReports.UI.XRLabel();
            this.label2 = new DevExpress.XtraReports.UI.XRLabel();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.label18 = new DevExpress.XtraReports.UI.XRLabel();
            this.label17 = new DevExpress.XtraReports.UI.XRLabel();
            this.label16 = new DevExpress.XtraReports.UI.XRLabel();
            this.label15 = new DevExpress.XtraReports.UI.XRLabel();
            this.label14 = new DevExpress.XtraReports.UI.XRLabel();
            this.label13 = new DevExpress.XtraReports.UI.XRLabel();
            this.label12 = new DevExpress.XtraReports.UI.XRLabel();
            this.label11 = new DevExpress.XtraReports.UI.XRLabel();
            this.label10 = new DevExpress.XtraReports.UI.XRLabel();
            this.label9 = new DevExpress.XtraReports.UI.XRLabel();
            this.label8 = new DevExpress.XtraReports.UI.XRLabel();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource2 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.ID = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
            this.TopMargin.Name = "TopMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label3,
            this.label2,
            this.table1,
            this.label18,
            this.label17,
            this.label16,
            this.label15,
            this.label14,
            this.label13,
            this.label12,
            this.label11,
            this.label10,
            this.label9,
            this.label8,
            this.label1});
            this.Detail.HeightF = 575F;
            this.Detail.Name = "Detail";
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // label3
            // 
            this.label3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OthAllName]")});
            this.label3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label3.LocationFloat = new DevExpress.Utils.PointFloat(154.5833F, 447.5001F);
            this.label3.Multiline = true;
            this.label3.Name = "label3";
            this.label3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label3.SizeF = new System.Drawing.SizeF(197.5F, 23.00003F);
            this.label3.StylePriority.UseFont = false;
            this.label3.StylePriority.UseTextAlignment = false;
            this.label3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // label2
            // 
            this.label2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 167.9444F);
            this.label2.Multiline = true;
            this.label2.Name = "label2";
            this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label2.SizeF = new System.Drawing.SizeF(677.0001F, 23.00002F);
            this.label2.StylePriority.UseFont = false;
            this.label2.Text = "             ​​          អាស្រ័យដូចបានជម្រាបជូនខាងលើ សូម លោកនាយក មេត្តាពិនិត្យ និ" +
    "ងសម្រេចដោយក្តីអនុគ្រោះ។";
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 74.61111F);
            this.table1.Name = "table1";
            this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1,
            this.tableRow2,
            this.tableRow3});
            this.table1.SizeF = new System.Drawing.SizeF(677.0001F, 75.00001F);
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.tableCell1.Multiline = true;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "តាមរយៈ   ៖ ";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.tableCell1.Weight = 0.3852597115176366D;
            // 
            // tableCell2
            // 
            this.tableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'នាយក​ នាយកដ្ឋាន \'+[HODNameKH]+\' ។\'")});
            this.tableCell2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell2.Multiline = true;
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.Text = "នាយក​ នាយកដ្ឋាន";
            this.tableCell2.Weight = 2.3993294574012225D;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell3,
            this.tableCell4});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1D;
            // 
            // tableCell3
            // 
            this.tableCell3.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.tableCell3.Multiline = true;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "កម្មវត្ថុ      ៖";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.tableCell3.Weight = 0.38525970198497905D;
            // 
            // tableCell4
            // 
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell4.Multiline = true;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.Text = "សំណើសុំកាតការងាររបស់បុគ្គលិក ។";
            this.tableCell4.Weight = 2.3993294669338803D;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell6});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 1D;
            // 
            // tableCell6
            // 
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'\t                        តបតាមកម្មវត្ថុខាងលើ ខ្ញុំបាទសូមគោរពជូន លោកនាយក មេត្តាជ្" +
                    "រាបថា៖  \'+[Remark] +\' ។\'")});
            this.tableCell6.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell6.Multiline = true;
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.Text = "\tតបតាមកម្មវត្ថុខាងលើ ខ្ញុំបាទសូមគោរពជូន លោកនាយក មេត្តាជ្រាបថា៖  ";
            this.tableCell6.Weight = 2.7845891689188593D;
            // 
            // label18
            // 
            this.label18.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label18.LocationFloat = new DevExpress.Utils.PointFloat(48.61111F, 539.5001F);
            this.label18.Multiline = true;
            this.label18.Name = "label18";
            this.label18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label18.SizeF = new System.Drawing.SizeF(303.4722F, 23F);
            this.label18.StylePriority.UseFont = false;
            this.label18.StylePriority.UseTextAlignment = false;
            this.label18.Text = "នាយក";
            this.label18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label17
            // 
            this.label17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'នាយកដ្ឋាន  \'+[DepartmentKH]")});
            this.label17.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label17.LocationFloat = new DevExpress.Utils.PointFloat(48.61111F, 516.5001F);
            this.label17.Multiline = true;
            this.label17.Name = "label17";
            this.label17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label17.SizeF = new System.Drawing.SizeF(303.4722F, 23F);
            this.label17.StylePriority.UseFont = false;
            this.label17.StylePriority.UseTextAlignment = false;
            this.label17.Text = "នាយកដ្ឋាន";
            this.label17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label16
            // 
            this.label16.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label16.LocationFloat = new DevExpress.Utils.PointFloat(48.61111F, 493.5001F);
            this.label16.Multiline = true;
            this.label16.Name = "label16";
            this.label16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label16.SizeF = new System.Drawing.SizeF(303.4722F, 23F);
            this.label16.StylePriority.UseFont = false;
            this.label16.StylePriority.UseTextAlignment = false;
            this.label16.Text = "រាជធានីភ្នំពេញ, ថ្ងៃទី..........ខែ...........ឆ្នាំ២០២...";
            this.label16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label15
            // 
            this.label15.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label15.LocationFloat = new DevExpress.Utils.PointFloat(48.61111F, 470.5001F);
            this.label15.Multiline = true;
            this.label15.Name = "label15";
            this.label15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label15.SizeF = new System.Drawing.SizeF(303.4722F, 23F);
            this.label15.StylePriority.UseFont = false;
            this.label15.StylePriority.UseTextAlignment = false;
            this.label15.Text = "ពិតជាបាន.................................ពិតប្រាកដមែន។";
            this.label15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label14
            // 
            this.label14.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label14.LocationFloat = new DevExpress.Utils.PointFloat(48.61111F, 447.5001F);
            this.label14.Multiline = true;
            this.label14.Name = "label14";
            this.label14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label14.SizeF = new System.Drawing.SizeF(105.9722F, 23.00003F);
            this.label14.StylePriority.UseFont = false;
            this.label14.StylePriority.UseTextAlignment = false;
            this.label14.Text = "ឈ្មោះ";
            this.label14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label13
            // 
            this.label13.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label13.LocationFloat = new DevExpress.Utils.PointFloat(48.61112F, 424.5001F);
            this.label13.Multiline = true;
            this.label13.Name = "label13";
            this.label13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label13.SizeF = new System.Drawing.SizeF(303.4722F, 23F);
            this.label13.StylePriority.UseFont = false;
            this.label13.StylePriority.UseTextAlignment = false;
            this.label13.Text = "បានឃើញ និងបញ្ជាក់ថា៖";
            this.label13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'ឈ្មោះ   \'+[OthAllName]")});
            this.label12.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label12.LocationFloat = new DevExpress.Utils.PointFloat(412.5F, 539.5001F);
            this.label12.Multiline = true;
            this.label12.Name = "label12";
            this.label12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label12.SizeF = new System.Drawing.SizeF(262.5F, 23F);
            this.label12.StylePriority.UseFont = false;
            this.label12.StylePriority.UseTextAlignment = false;
            this.label12.Text = "ឈ្មោះ............................";
            this.label12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label11.LocationFloat = new DevExpress.Utils.PointFloat(373.2222F, 403.5001F);
            this.label11.Multiline = true;
            this.label11.Name = "label11";
            this.label11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label11.SizeF = new System.Drawing.SizeF(302.7778F, 23F);
            this.label11.StylePriority.UseFont = false;
            this.label11.StylePriority.UseTextAlignment = false;
            this.label11.Text = "ហត្ថលេខាសាមីខ្លួន";
            this.label11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label10
            // 
            this.label10.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label10.LocationFloat = new DevExpress.Utils.PointFloat(374.2222F, 380.5001F);
            this.label10.Multiline = true;
            this.label10.Name = "label10";
            this.label10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label10.SizeF = new System.Drawing.SizeF(302.7778F, 23F);
            this.label10.StylePriority.UseFont = false;
            this.label10.Text = "រាជធានីភ្នំពេញ, ថ្ងៃទី.......... ខែ......... ឆ្នាំ២០២...";
            // 
            // label9
            // 
            this.label9.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label9.LocationFloat = new DevExpress.Utils.PointFloat(338.5F, 357.5001F);
            this.label9.Multiline = true;
            this.label9.Name = "label9";
            this.label9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label9.SizeF = new System.Drawing.SizeF(337.5F, 23F);
            this.label9.StylePriority.UseFont = false;
            this.label9.Text = "ថ្ងៃ................... ខែ........... ឆ្នាំ..........ស័ក ព.ស.២៥៦...";
            // 
            // label8
            // 
            this.label8.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 190.9445F);
            this.label8.Multiline = true;
            this.label8.Name = "label8";
            this.label8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label8.SizeF = new System.Drawing.SizeF(677.0001F, 23F);
            this.label8.StylePriority.UseFont = false;
            this.label8.Text = "                       សូម លោកនាយក មេត្តាទទួលនូវសេចក្តីគោរពដ៏ខ្ពង់ខ្ពស់ពីខ្ញុំបាទ" +
    "។";
            // 
            // label1
            // 
            this.label1.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.label1.Multiline = true;
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(677F, 55.63888F);
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "សូមគោរពជូន\r\nលោកនាយក នាយកដ្ឋានព័ត៌មានវិទ្យា\r\n";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 50F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // sqlDataSource2
            // 
            this.sqlDataSource2.ConnectionName = "ReportConnectionString";
            this.sqlDataSource2.Name = "sqlDataSource2";
            customSqlQuery1.Name = "Query";
            queryParameter1.Name = "ID";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?ID", typeof(int));
            customSqlQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1});
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource2.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource2.ResultSchemaSerializable = resources.GetString("sqlDataSource2.ResultSchemaSerializable");
            // 
            // ID
            // 
            this.ID.Description = "ID";
            this.ID.Name = "ID";
            this.ID.Type = typeof(int);
            this.ID.ValueInfo = "0";
            // 
            // RPTRequestNewIDCard
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource2});
            this.DataMember = "Query";
            this.DataSource = this.sqlDataSource2;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(75F, 75F, 50F, 50F);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ID});
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel label3;
        private DevExpress.XtraReports.UI.XRLabel label2;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRLabel label18;
        private DevExpress.XtraReports.UI.XRLabel label17;
        private DevExpress.XtraReports.UI.XRLabel label16;
        private DevExpress.XtraReports.UI.XRLabel label15;
        private DevExpress.XtraReports.UI.XRLabel label14;
        private DevExpress.XtraReports.UI.XRLabel label13;
        private DevExpress.XtraReports.UI.XRLabel label12;
        private DevExpress.XtraReports.UI.XRLabel label11;
        private DevExpress.XtraReports.UI.XRLabel label10;
        private DevExpress.XtraReports.UI.XRLabel label9;
        private DevExpress.XtraReports.UI.XRLabel label8;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource2;
        private DevExpress.XtraReports.Parameters.Parameter ID;
    }
}
