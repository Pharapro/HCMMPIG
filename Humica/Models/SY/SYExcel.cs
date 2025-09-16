using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Humica.Models.SY
{
    public class SYExcel
    {
        private OleDbConnection oledbConn;

        public string FileName { get; set; }

        public int WriteExcelWithDataSet(DataTable dt)
        {

            return 0;
        }
        public DataTable GenerateExcelData(int row = 0)
        {
            try
            {
                string path__1 = Path.GetFullPath(FileName);

                if (Path.GetExtension(path__1).ToUpper() == ".xls".ToUpper())
                {
                    oledbConn = new OleDbConnection((Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") + path__1) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                }
                else if (Path.GetExtension(path__1).ToUpper() == ".xlsx".ToUpper())
                {
                    oledbConn = new OleDbConnection((Convert.ToString("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=") + path__1) + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
                }
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataTable dt = new DataTable();

                // passing list to drop-down list

                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[row].Field<string>("TABLE_NAME");
                // selecting distict list of Slno 
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + Sheet1 + "]";
                oleda = new OleDbDataAdapter(cmd);


                oleda.Fill(dt);

                // binding form data with grid view

                // need to catch possible exceptions
                oledbConn.Close();
                return dt;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = "UPLOAD";
                log.UserId = "USR";
                log.ScreenId = "MATERIAL_UPLOAD";
                log.Action = "UPLOAD";
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return null;
            }
            finally
            {
                oledbConn.Close();
            }
        }
        // Add a reference to the DevExpress.Docs.dll assembly. 
        public DataTable GenerateExcelData()
        {
            try
            {
                DataTable data = new DataTable();
                DevExpress.Spreadsheet.Workbook wbook = new DevExpress.Spreadsheet.Workbook();

                if (Path.GetExtension(FileName).ToUpper() == ".xls".ToUpper())
                {
                    wbook.LoadDocument(FileName, DevExpress.Spreadsheet.DocumentFormat.Xls);
                }
                else if (Path.GetExtension(FileName).ToUpper() == ".xlsx".ToUpper())
                {
                    wbook.LoadDocument(FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                }
                var range = wbook.Worksheets[0];
                for (int col = 0; col < range.GetDataRange().ColumnCount; col++)
                {
                    data.Columns.Add(range.Cells[col].DisplayText);
                }
                for (int row = 1; row < range.GetDataRange().RowCount; row++)
                {
                    DataRow dataRow = data.NewRow();
                    for (int col = 0; col < range.GetDataRange().ColumnCount; col++)
                    {
                        dataRow[col] = range.GetCellValue(col, row);
                    }
                    data.Rows.Add(dataRow);
                }

                return data;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = "UPLOAD";
                log.UserId = "USR";
                log.ScreenId = "MATERIAL_UPLOAD";
                log.Action = "UPLOAD";
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return null;
            }
        }
    }
}