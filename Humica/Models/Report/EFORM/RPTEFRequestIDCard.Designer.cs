namespace HUMICA.Models.Report.EFORM
{
    partial class RPTEFRequestIDCard
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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPTEFRequestIDCard));
            DevExpress.XtraReports.UI.XRWatermark xrWatermark1 = new DevExpress.XtraReports.UI.XRWatermark();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.label2 = new DevExpress.XtraReports.UI.XRLabel();
            this.label4 = new DevExpress.XtraReports.UI.XRLabel();
            this.label5 = new DevExpress.XtraReports.UI.XRLabel();
            this.label7 = new DevExpress.XtraReports.UI.XRLabel();
            this.label8 = new DevExpress.XtraReports.UI.XRLabel();
            this.label9 = new DevExpress.XtraReports.UI.XRLabel();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.label19 = new DevExpress.XtraReports.UI.XRLabel();
            this.label20 = new DevExpress.XtraReports.UI.XRLabel();
            this.label21 = new DevExpress.XtraReports.UI.XRLabel();
            this.label22 = new DevExpress.XtraReports.UI.XRLabel();
            this.label23 = new DevExpress.XtraReports.UI.XRLabel();
            this.label24 = new DevExpress.XtraReports.UI.XRLabel();
            this.label25 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.DocumentNo = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
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
            this.table2});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            // 
            // table2
            // 
            this.table2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(34.30554F, 0F);
            this.table2.Name = "table2";
            this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(715.6945F, 25F);
            this.table2.StylePriority.UseBorders = false;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell5,
            this.tableCell6,
            this.tableCell10,
            this.tableCell7,
            this.tableCell8});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1D;
            // 
            // tableCell5
            // 
            this.tableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")});
            this.tableCell5.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell5.Multiline = true;
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell5.Summary = xrSummary1;
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell5.Weight = 0.4690687516837313D;
            // 
            // tableCell6
            // 
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpName]")});
            this.tableCell6.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell6.Multiline = true;
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.StylePriority.UseTextAlignment = false;
            this.tableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell6.Weight = 2.0284513766590551D;
            // 
            // tableCell10
            // 
            this.tableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SecPosition]")});
            this.tableCell10.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell10.Multiline = true;
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.StylePriority.UseTextAlignment = false;
            this.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell10.Weight = 2.0284513766590551D;
            // 
            // tableCell7
            // 
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DOB]")});
            this.tableCell7.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell7.Multiline = true;
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell7.Weight = 1.6883742244983537D;
            // 
            // tableCell8
            // 
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Other]")});
            this.tableCell8.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.tableCell8.Multiline = true;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell8.Weight = 1.6648215983806636D;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 50F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label1,
            this.label2,
            this.label4,
            this.label5,
            this.label7,
            this.label8,
            this.label9,
            this.table1});
            this.PageHeader.HeightF = 290.7778F;
            this.PageHeader.Name = "PageHeader";
            // 
            // label1
            // 
            this.label1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(34.30555F, 0F);
            this.label1.Multiline = true;
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(129.1666F, 23F);
            this.label1.StylePriority.UseFont = false;
            this.label1.Text = "លេខៈ.................ក.ត";
            // 
            // label2
            // 
            this.label2.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.label2.LocationFloat = new DevExpress.Utils.PointFloat(252.3611F, 20.55555F);
            this.label2.Multiline = true;
            this.label2.Name = "label2";
            this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label2.SizeF = new System.Drawing.SizeF(228.4722F, 55.08334F);
            this.label2.StylePriority.UseFont = false;
            this.label2.StylePriority.UseTextAlignment = false;
            this.label2.Text = "សូមគោរពជូន\r\nលោកឧកញ៉ា អគ្គនាយករង\r\n";
            this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.label4.LocationFloat = new DevExpress.Utils.PointFloat(34.30556F, 108.1389F);
            this.label4.Multiline = true;
            this.label4.Name = "label4";
            this.label4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label4.SizeF = new System.Drawing.SizeF(86.11109F, 22.99998F);
            this.label4.StylePriority.UseFont = false;
            this.label4.Text = "កម្មវត្ថុ      ៖";
            // 
            // label5
            // 
            this.label5.Font = new DevExpress.Drawing.DXFont("Kh Muol", 9.75F);
            this.label5.LocationFloat = new DevExpress.Utils.PointFloat(34.30555F, 131.1389F);
            this.label5.Multiline = true;
            this.label5.Name = "label5";
            this.label5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label5.SizeF = new System.Drawing.SizeF(86.1111F, 22.99998F);
            this.label5.StylePriority.UseFont = false;
            this.label5.Text = "យោង        ៖";
            // 
            // label7
            // 
            this.label7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'សំណើធ្វើប័ណ្ណសម្គាល់ខ្លួនចំនួន ​  \'+[Amount]+\'  (....)ប័ណ្ណ។\'")});
            this.label7.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label7.LocationFloat = new DevExpress.Utils.PointFloat(120.4166F, 108.1389F);
            this.label7.Multiline = true;
            this.label7.Name = "label7";
            this.label7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label7.SizeF = new System.Drawing.SizeF(629.5834F, 22.99998F);
            this.label7.StylePriority.UseFont = false;
            this.label7.Text = "សំណើធ្វើប័ណ្ណសម្គាល់ខ្លួនចំនួន........ (....)ប័ណ្ណ។";
            // 
            // label8
            // 
            this.label8.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label8.LocationFloat = new DevExpress.Utils.PointFloat(120.4166F, 131.1389F);
            this.label8.Multiline = true;
            this.label8.Name = "label8";
            this.label8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label8.SizeF = new System.Drawing.SizeF(629.5834F, 22.99998F);
            this.label8.StylePriority.UseFont = false;
            // 
            // label9
            // 
            this.label9.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label9.LocationFloat = new DevExpress.Utils.PointFloat(34.30554F, 189.1389F);
            this.label9.Multiline = true;
            this.label9.Name = "label9";
            this.label9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label9.SizeF = new System.Drawing.SizeF(715.6945F, 55.63892F);
            this.label9.StylePriority.UseFont = false;
            this.label9.StylePriority.UseTextAlignment = false;
            this.label9.Text = "តបតាមកម្មវត្ថុខាងលើ ខ្ញុំសូមជម្រាប លោកនាយក នាយកដ្ឋានព័ត៌មានវិទ្យា ឲ្យបានជ្រាបថា៖ " +
    "ការិយាល័យធនធានមនុស្ស សូមស្នើសុំធ្វើប័ណ្ណសម្គាល់ខ្លួនដល់បុគ្គលិកថ្មី មានរាយនាមដូច" +
    "ខាងក្រោម៖";
            this.label9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // table1
            // 
            this.table1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(34.30554F, 265.7778F);
            this.table1.Name = "table1";
            this.table1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(715.6945F, 25F);
            this.table1.StylePriority.UseBorders = false;
            this.table1.StylePriority.UseTextAlignment = false;
            this.table1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2,
            this.tableCell9,
            this.tableCell3,
            this.tableCell4});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell1.Multiline = true;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.Text = "ល.រ";
            this.tableCell1.Weight = 0.4690687516837313D;
            // 
            // tableCell2
            // 
            this.tableCell2.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell2.Multiline = true;
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "ឈ្មោះ";
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell2.Weight = 2.0284513766590551D;
            // 
            // tableCell9
            // 
            this.tableCell9.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell9.Multiline = true;
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StylePriority.UseFont = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.Text = "តួនាទី";
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell9.Weight = 2.0284513766590551D;
            // 
            // tableCell3
            // 
            this.tableCell3.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell3.Multiline = true;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "ថ្ងៃខែឆ្នាំកំណើត";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 1.6883738885266246D;
            // 
            // tableCell4
            // 
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell4.Multiline = true;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "ផ្សេងៗ";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell4.Weight = 1.6648219343523925D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label19,
            this.label20,
            this.label21,
            this.label22,
            this.label23,
            this.label24,
            this.label25});
            this.ReportFooter.HeightF = 222.9722F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // label19
            // 
            this.label19.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label19.LocationFloat = new DevExpress.Utils.PointFloat(34.30555F, 0F);
            this.label19.Multiline = true;
            this.label19.Name = "label19";
            this.label19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label19.SizeF = new System.Drawing.SizeF(715.6945F, 38.97223F);
            this.label19.StylePriority.UseFont = false;
            this.label19.StylePriority.UseTextAlignment = false;
            this.label19.Text = "​​​              ​        អាស្រ័យដូចបានជម្រាបជូនខាងលើ សូម លោកនាយក មេត្តាពិនិត្យ ន" +
    "ិងសម្រេចដោយក្តីអនុគ្រោះ។";
            this.label19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label20.LocationFloat = new DevExpress.Utils.PointFloat(120.4166F, 38.97223F);
            this.label20.Multiline = true;
            this.label20.Name = "label20";
            this.label20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label20.SizeF = new System.Drawing.SizeF(629.5834F, 23F);
            this.label20.StylePriority.UseFont = false;
            this.label20.StylePriority.UseTextAlignment = false;
            this.label20.Text = "សូម លោកនាយក មេត្តាទទួលនូវសេចក្តីគោរពដ៍ខ្ពង់ខ្ពស់ពីខ្ញុំបាទ។";
            this.label20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // label21
            // 
            this.label21.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label21.LocationFloat = new DevExpress.Utils.PointFloat(350.9722F, 61.97226F);
            this.label21.Multiline = true;
            this.label21.Name = "label21";
            this.label21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label21.SizeF = new System.Drawing.SizeF(351.1111F, 23F);
            this.label21.StylePriority.UseFont = false;
            this.label21.StylePriority.UseTextAlignment = false;
            this.label21.Text = "    ថ្ងៃ.................... ខែ......... ឆ្នាំ ព.ស.២៥";
            this.label21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label22
            // 
            this.label22.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label22.LocationFloat = new DevExpress.Utils.PointFloat(350.9722F, 84.97226F);
            this.label22.Multiline = true;
            this.label22.Name = "label22";
            this.label22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label22.SizeF = new System.Drawing.SizeF(351.1111F, 23F);
            this.label22.StylePriority.UseFont = false;
            this.label22.StylePriority.UseTextAlignment = false;
            this.label22.Text = "រាជធានីភ្នំពេញ, ថ្ងៃទី...... ខែ............ ឆ្នាំ២០២";
            this.label22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label23
            // 
            this.label23.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label23.LocationFloat = new DevExpress.Utils.PointFloat(350.9722F, 107.9723F);
            this.label23.Multiline = true;
            this.label23.Name = "label23";
            this.label23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label23.SizeF = new System.Drawing.SizeF(351.1111F, 22.99997F);
            this.label23.StylePriority.UseFont = false;
            this.label23.StylePriority.UseTextAlignment = false;
            this.label23.Text = "នាយកដ្ឋានរដ្ឋបាល ធនធានមនុស្ស";
            this.label23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label24
            // 
            this.label24.Font = new DevExpress.Drawing.DXFont("Khmer OS Battambang", 9.75F);
            this.label24.LocationFloat = new DevExpress.Utils.PointFloat(350.9722F, 130.9722F);
            this.label24.Multiline = true;
            this.label24.Name = "label24";
            this.label24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label24.SizeF = new System.Drawing.SizeF(351.1111F, 23F);
            this.label24.StylePriority.UseFont = false;
            this.label24.StylePriority.UseTextAlignment = false;
            this.label24.Text = "នាយក";
            this.label24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // label25
            // 
            this.label25.LocationFloat = new DevExpress.Utils.PointFloat(350.9722F, 176.9722F);
            this.label25.Multiline = true;
            this.label25.Name = "label25";
            this.label25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label25.SizeF = new System.Drawing.SizeF(351.1111F, 23F);
            this.label25.StylePriority.UseTextAlignment = false;
            this.label25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ReportConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "Query";
            queryParameter1.Name = "DocumentNo";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?DocumentNo", typeof(string));
            customSqlQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1});
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // DocumentNo
            // 
            this.DocumentNo.Name = "DocumentNo";
            // 
            // RPTEFRequestIDCard
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.Detail,
            this.BottomMargin,
            this.PageHeader,
            this.ReportFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "Query";
            this.DataSource = this.sqlDataSource1;
            this.DisplayName = "RequestIDCard";
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 50F, 50F);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.DocumentNo});
            this.Version = "23.2";
            xrWatermark1.Id = "Watermark1";
            this.Watermarks.Add(xrWatermark1);
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRLabel label2;
        private DevExpress.XtraReports.UI.XRLabel label4;
        private DevExpress.XtraReports.UI.XRLabel label5;
        private DevExpress.XtraReports.UI.XRLabel label7;
        private DevExpress.XtraReports.UI.XRLabel label8;
        private DevExpress.XtraReports.UI.XRLabel label9;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel label19;
        private DevExpress.XtraReports.UI.XRLabel label20;
        private DevExpress.XtraReports.UI.XRLabel label21;
        private DevExpress.XtraReports.UI.XRLabel label22;
        private DevExpress.XtraReports.UI.XRLabel label23;
        private DevExpress.XtraReports.UI.XRLabel label24;
        private DevExpress.XtraReports.UI.XRLabel label25;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter DocumentNo;
    }
}
