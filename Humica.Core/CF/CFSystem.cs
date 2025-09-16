using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Humica.Core.CF
{
    public class CFSystem
    {
        public string CopyFrom { get; set; }
        public string ObjectCopying { get; set; }
        public string ScreenID { get; set; }
        public string ScreenIDReference { get; set; }
        public List<CFUploadPath> ListUplaodPath { get; set; }
        public List<TPEmailTemplate> ListEmailTemplate { get; set; }
        public List<CFEmailAccount> ListEmailAccount { get; set; }
        public List<SYEmailParameter> ListEmailParameter { get; set; }
        public List<CFWorkFlow> ListWorkFlow { get; set; }
        public List<CFWorkFlowItem> ListWorkFlowItem { get; set; }
        public List<SYProcessAlert> ListProcessAlert { get; set; }
        public List<SYData> ListWorkFlowType { get; set; }

        //report and dash board
        public List<CFReport> ListReport { get; set; }
        public List<CFReportItem> ListReportItem { get; set; }
        public List<CFReportObject> ListReportObject { get; set; }
        public List<SYDashboard> ListDashboard { get; set; }
        public List<SYDashboardMaster> ListDashboardMaster { get; set; }

        //data selection
        public List<SYData> ListDataSelection { get; set; }
        public List<SYSetting> ListSYSetting { get; set; }

        //Openning Balance
        public string CompanyCode { get; set; }
        public string Storage { get; set; }
        public string Floor { get; set; }
        //User Template
        //  public List<SystemAdmin.CF.CFUserTemplate> ListUserTemplate { get; set; }
        public List<FileReportListInfo> GetFilesReport()
        {
            List<FileReportListInfo> fileReportListInfoList = new List<FileReportListInfo>();
            foreach (string fileName in Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~") + SYSettings.getSetting("REPORT_PATH").SettinValue, "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>)(s => s.EndsWith(".repx"))).ToList<string>())
            {
                FileReportListInfo fileReportListInfo = new FileReportListInfo();
                FileInfo fileInfo = new FileInfo(fileName);
                fileReportListInfo.FileName = fileInfo.Name;
                fileReportListInfo.Path = fileInfo.FullName;
                fileReportListInfo.Extention = fileInfo.Extension;
                fileReportListInfoList.Add(fileReportListInfo);
            }
            return fileReportListInfoList;
        }
        public List<FileReportListInfo> GetFilesReport(string ScreenId)
        {
            List<FileReportListInfo> fileReportListInfoList = new List<FileReportListInfo>();
            string[] strArray = new string[5]
            {
        HttpContext.Current.Server.MapPath("~"),
        SYSettings.getSetting("REPORT_PATH").SettinValue,
        "/",
        ScreenId,
        "/"
            };
            foreach (string fileName in Directory.EnumerateFiles(string.Concat(strArray), "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>)(s => s.EndsWith(".repx"))).ToList<string>())
            {
                FileReportListInfo fileReportListInfo = new FileReportListInfo();
                FileInfo fileInfo = new FileInfo(fileName);
                fileReportListInfo.FileName = fileInfo.Name;
                fileReportListInfo.Path = fileInfo.FullName;
                fileReportListInfo.Extention = fileInfo.Extension;
                fileReportListInfoList.Add(fileReportListInfo);
            }
            return fileReportListInfoList;
        }
    }
}