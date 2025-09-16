using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;

namespace Humica.Models.SY
{
    public class ClsConstant
    {
        public const int DEFAULT_FILTER_OF_DATE = -5;

        public static int DEFAULT_PAGE_SIZE = 200;
        public static int PROGRESS_PERCEN = 0;
        public const string FORMATE_POSTING_PERIOD = "MM-yyyy";
        public const string TEXT_COMBOBOX_STRING = "{0}:{1}";
        public const string TEXT_COMBOBOX_STRING_SINGLE = "{0}";
        public const string TEXT_COMBOBOX_FILTER = "{0}:{1}:{2}";
        public const string IS_EDITABLE = "IS_EDITABLE";
        public const string IS_READ_ONLY = "IS_READ_ONLY";
        public const string IS_READ_ONLY1 = "IS_READ_ONLY1";
        public const string IS_SALARY = "IS_SALARY";
        public const string PARAM_INDEX = "PARAM_INDEX";
        public const string PARAM_KEY = "PARAM_KEy";
        public const string TOKEN_VALUE = "123123213asfasdasdss";
        public const string BACK_URL = "adjflkasdf123123123";

        private static List<SYAccessDepartment> getDepartmentData()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[Humica.EF.SYSConstant.PLANT_DEPARTMENT_LIST] != null)
                {
                    var listDataAccess = (List<SYAccessDepartment>)HttpContext.Current.Session[Humica.EF.SYSConstant.PLANT_DEPARTMENT_LIST];
                    return listDataAccess.ToList();
                }

            }
            return new List<SYAccessDepartment>();
        }

        //Department
        public static List<SYAccessDepartment> getDepartmentDataAccess()
        {
            return ClsConstant.getDepartmentData().ToList();
        }
        public static string LEVEL_Condition
        {
            get
            {
                if (HttpContext.Current.Session[Humica.EF.SYSConstant.LEVEL_CONDITION] != null)
                {
                    var listDataAccess = HttpContext.Current.Session[Humica.EF.SYSConstant.LEVEL_CONDITION];
                    return listDataAccess.ToString();
                }
                return "";
            }
        }
        public static void MoveFilesDashboard(string path, string userPath)
        {
            var realUserPath = HttpContext.Current.Server.MapPath(userPath);
            var realPath = HttpContext.Current.Server.MapPath(path);

            IEnumerable<string> files = Directory.EnumerateFiles(realPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".xml"));

            foreach (string read in files.ToList())
            {
                FileReportList objFile = new FileReportList();
                FileInfo fi2 = new FileInfo(read);
                objFile.FileName = fi2.Name;
                objFile.Path = fi2.FullName;
                objFile.Extention = fi2.Extension;
                if (!System.IO.Directory.Exists(realUserPath))
                {
                    System.IO.Directory.CreateDirectory(realUserPath);
                    System.IO.File.Copy(objFile.Path, realUserPath + objFile.FileName);
                    var fileName = realUserPath + objFile.FileName;
                    string[] lines = File.ReadAllLines(fileName);
                    string USER_NAME = "@USER_NAME";
                    string USER_ID = "@USER_ID";
                    string COMPANY = "@COMPANY";
                    string BRANCH = "@BRANCH";
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //File.WriteAllLines(targetFilePath, lines);
                        if (lines[i].Contains(USER_NAME))
                        {
                            lines[i] = lines[i].Replace(USER_NAME, GetSystemValue(USER_NAME));
                        }
                        if (lines[i].Contains(USER_ID))
                        {
                            lines[i] = lines[i].Replace(USER_ID, GetSystemValue(USER_ID));
                        }
                        if (lines[i].Contains(COMPANY))
                        {
                            lines[i] = lines[i].Replace(COMPANY, GetSystemValue(COMPANY));
                        }
                        if (lines[i].Contains(BRANCH))
                        {
                            lines[i] = lines[i].Replace(BRANCH, GetSystemValue(BRANCH));
                        }
                    }
                    File.WriteAllLines(fileName, lines);
                    // Write to the target file
                }


            }
        }

        public static int FileDashboard
        {
            get
            {
                var pathApp = "~/App_Data/Dashboards/" + SYSession.getSessionUser().UserName + "/";
                var path = HttpContext.Current.Server.MapPath(pathApp);
                IEnumerable<string> files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".xml"));
                return files.ToList().Count;

            }
        }
        public static string GetSystemValue(string sysType)
        {
            var userType = Humica.EF.MD.SYSession.getSessionUser();
            var branch = SYConstant.getBranchDataAccess().First().Code;
            if (sysType == "@USER_NAME")
            {
                return userType.UserName;
            }
            if (sysType == "@USER_ID")
            {

                return userType.UserID.ToString();
            }
            if (sysType == "@COMPANY")
            {
                return SYConstant.DEFAULT_PLANT;
            }
            if (sysType == "@BRANCH")
            {
                return branch;
            }

            return null;
        }
        public static List<FileReportList> GetFilesReport()
        {
            var listResult = new List<FileReportList>();

            var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~") + Humica.EF.Models.SY.SYSettings.getSetting("REPORT_PATH").SettinValue, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".repx"));

            foreach (var read in files.ToList())
            {
                var objFile = new FileReportList();
                var fi2 = new FileInfo(read);
                objFile.FileName = fi2.Name;
                objFile.Path = fi2.FullName;
                objFile.Extention = fi2.Extension;
                listResult.Add(objFile);
            }
            listResult.Add(new FileReportList());
            return listResult;
        }

        public static List<FileReportList> GetFilesReport(string screenid, bool folder = false)
        {
            try
            {
                var listResult = new List<FileReportList>();

                var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~") + Humica.EF.Models.SY.SYSettings.getSetting("REPORT_PATH").SettinValue + "/" + screenid, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".repx"));

                foreach (var read in files.ToList())
                {
                    var objFile = new FileReportList();
                    var fi2 = new FileInfo(read);
                    //if (folder == true)
                    // {
                    objFile.FileName = "/" + screenid + "/" + fi2.Name;
                    // }

                    objFile.Path = fi2.FullName;
                    objFile.Extention = fi2.Extension;
                    listResult.Add(objFile);
                }
                listResult.Add(new FileReportList());
                return listResult;
            }
            catch
            {
                return new List<FileReportList>();
            }

        }
        public static string DEFAULT_REPORT_PATH
        {
            get { return HostingEnvironment.ApplicationPhysicalPath + Humica.EF.Models.SY.SYSettings.getSetting("REPORT_PATH").SettinValue; }
        }

        public static string GetDefaultThemeFile()
        {
            var ownfile = @"~/Content/user/custom/" + SYSession.getSessionUser().UserName + ".css";
            var css = HttpContext.Current.Server.MapPath(ownfile);
            if (System.IO.File.Exists(css))
            {
                return ownfile;
            }
            return "~/Content/user/color.css";
        }
        public static GridViewSettings CreateExportGridViewSettings(SYSGridSettingExport GridParameter, string screen, int verion, string name)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = name;

            settings.Width = Unit.Percentage(100);
            settings.Columns.Add(column =>
            {

                column.Width = 150;
                column.FieldName = "Empcode";
                //List<SY_Schema> listCheck = listSchemar.Where(w => w.TableName == read.TableName && w.ColumnName == read.FieldName).ToList();
                //if (listCheck.Count > 0)
                //{
                //    SY_Schema obj = listCheck.First();
                //    if (obj.Length > 0)
                //    {
                //        column.Caption = read.Caption + "\n" + obj.DataType + "(" + obj.Length + ")";
                //    }
                //    else
                //    {
                //        if (obj.DataType == "date")
                //        {
                //            column.Caption = read.Caption + "\n" + obj.DataType + "(yyyy-mm-dd)";
                //        }
                //        else
                //        {
                //            column.Caption = read.Caption + "\n" + obj.DataType;
                //        }

                //    }

                //}
                //else
                //{
                //    column.Caption = read.Caption;
                //}


            });

            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.RowType == GridViewRowType.Data && e.VisibleIndex % 2 == 0)
                {
                    e.BrickStyle.BackColor = System.Drawing.Color.FromArgb(0xEE, 0xEE, 0xEE);
                }
            };

            return settings;
        }
        public static GridViewSettings CreateExportGridViewSettings(SYSGridSettingExport GridParameter, List<ExCFUploadMapping> ListConf, string name)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = name;
            settings.Width = Unit.Percentage(100);
            foreach (ExCFUploadMapping read in ListConf)
            {
                settings.Columns.Add(column =>
                {
                    column.Width = 150;
                    column.FieldName = read.FieldName;
                    //List<SY_Schema> listCheck = listSchemar.Where(w => w.TableName == read.TableName && w.ColumnName == read.FieldName).ToList();
                    //if (listCheck.Count > 0)
                    //{
                    //    SY_Schema obj = listCheck.First();
                    //    if (obj.Length > 0)
                    //    {
                    //        column.Caption = read.Caption + "\n" + obj.DataType + "(" + obj.Length + ")";
                    //    }
                    //    else
                    //    {
                    //        if (obj.DataType == "date")
                    //        {
                    //            column.Caption = read.Caption + "\n" + obj.DataType + "(yyyy-mm-dd)";
                    //        }
                    //        else
                    //        {
                    //            column.Caption = read.Caption + "\n" + obj.DataType;
                    //        }

                    //    }

                    //}
                    //else
                    //{
                    column.Caption = read.Caption;
                    //}


                });
            }
            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.RowType == GridViewRowType.Data && e.VisibleIndex % 2 == 0)
                {
                    e.BrickStyle.BackColor = System.Drawing.Color.FromArgb(0xEE, 0xEE, 0xEE);
                }
            };

            return settings;
        }

        public static Worksheet ExportDataToWorksheet(Worksheet sheet, List<ExCFUploadMapping> data)
        {
            // Assuming MyModel has properties: ID, Name, and Value
            int rowIndex = 0;

            //rowIndex++;
            foreach (var item in data)
            {
                sheet[0, rowIndex].Value = item.FieldName;
                var nameCell = sheet[0, rowIndex];
                nameCell.Alignment.WrapText = true;
                nameCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                nameCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                rowIndex++;
            }
            // Set header row formatting
            var headerRange = sheet.Range.FromLTRB(0, 0, rowIndex - 1, 0); // Range covering first row and three columns
            headerRange.FillColor = System.Drawing.Color.Green; // Set background color
            headerRange.Font.Color = System.Drawing.Color.White; // Set font color
            headerRange.Font.Bold = true; // Set font to bold

            sheet.Columns.AutoFit(0, rowIndex);

            return sheet;
        }
        public static Worksheet ExportDataToWorksheetRow(DevExpress.Spreadsheet.Worksheet sheet, List<ClsUploadMapping> data)
        {
            // Assuming MyModel has properties: ID, Name, and Value
            int rowIndex = 1;
            //rowIndex++;
            foreach (var item in data)
            {
                sheet[rowIndex, 0].Value = item.FieldName;
                sheet[rowIndex, 1].Value = item.FieldName1;
                sheet[rowIndex, 2].Value = item.FieldName2;
                rowIndex++;
            }
            sheet.Columns.AutoFit(0, rowIndex);

            return sheet;
        }

    }
    public class FileReportList
    {
        public string FileName { get; set; }
        public string Extention { get; set; }
        public string Path { get; set; }
    }
    public class ExCFUploadMapping
    {
        public string FieldName { get; set; }
        public string Caption { get; set; }
    }
    public class ClsUploadMapping
    {
        public string FieldName { get; set; }
        public string FieldName1 { get; set; }
        public string FieldName2 { get; set; }
    }
}