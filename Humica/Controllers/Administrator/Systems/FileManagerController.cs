using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Models.FL;
using System.IO;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class FileManagerController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYF00001";
        private const string URL_SCREEN = "/Administrator/Systems/FileManager/";

        private HumicaDBContext DB = new HumicaDBContext();

        private string RootFolder;

        public FileManagerController()
           : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            var path = DB.CFUploadPaths.Find("ROOT_FOLDER");
            RootFolder = path.PathDirectory;
        }

        public ActionResult Index()
        {

            FileManagerDemoHelper.RootFolder = RootFolder;
            return View(FileManagerDemoHelper.RootFolder);
            //EnsureDemoDataPrepared();
            //return View("VirtualScrolling", FileManagerDemoHelper.VirtualScrollingRootFolder);
            //return View(RootFolder);
        }

        public ActionResult CustomToolbarPartial(string strAction, string strFolder, string strFiles)
        {
            FileManagerDemoHelper.RootFolder = RootFolder;
            return PartialView("CustomToolbarPartial", FileManagerDemoHelper.RootFolder);
            //return PartialView("CustomToolbarPartial", RootFolder);
        }
        public ActionResult CustomToolbarAction(string viewType)
        {
            FileManagerDemoHelper.RootFolder = RootFolder;
            HttpContext.Session["aspxCustomToolbarDemoView"] = viewType == "Thumbnails" ? FileListView.Thumbnails : FileListView.Details;
            return PartialView("CustomToolbarPartial", FileManagerDemoHelper.RootFolder);
            //return PartialView("CustomToolbarPartial", RootFolder);
        }
        public ActionResult CustomToolbarPopupPartial(string relativePath)
        {
            FileManagerItemInfo itemInfo = CreateFileManagerItemInfo(relativePath);
            return PartialView("CustomToolbarPopupPartial", itemInfo);
        }
        FileManagerItemInfo CreateFileManagerItemInfo(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return new FileManagerItemInfo();
            string path = Path.Combine(FileManagerDemoHelper.FileManagerFolder.ToString(), relativePath).Replace("\\", "/");
            return FileManagerItemInfo.Create(relativePath.Replace("\\", "/"), path);
        }

        [HttpPost]
        public ActionResult VirtualScrolling(FileListView fileListViewMode, int fileListPageSize)
        {
            EnsureDemoDataPrepared();
            FileManagerDemoHelper.VirtualScrollingViewMode = fileListViewMode;
            FileManagerDemoHelper.VirtualScrollingPageSize = fileListPageSize;
            return View("VirtualScrolling", FileManagerDemoHelper.VirtualScrollingRootFolder);
        }
        [ValidateInput(false)]
        public ActionResult VirtualScrollingPartial()
        {
            EnsureDemoDataPrepared();
            return PartialView("VirtualScrollingPartial", FileManagerDemoHelper.VirtualScrollingRootFolder);
        }
        public FileStreamResult VirtualScrollingDownloadFiles()
        {
            EnsureDemoDataPrepared();
            return FileManagerExtension.DownloadFiles(FileManagerDemoHelper.CreateVirtualScrollingDownloadSettings(), (string)FileManagerDemoHelper.VirtualScrollingRootFolder);
        }

        public FileStreamResult DownloadFiles()
        {
            FileManagerDemoHelper.RootFolder = RootFolder;
            return FileManagerExtension.DownloadFiles(FileManagerDemoHelper.CreateMultipleFilesSelectionDownloadSettings(), (string)FileManagerDemoHelper.RootFolder);
            //return FileManagerExtension.DownloadFiles(FileManagerDemoHelper.CreateMultipleFilesSelectionDownloadSettings(), 
            //    RootFolder);
        }

        void EnsureDemoDataPrepared()
        {
            FileManagmentUtils.EnsureDemoFilesCreated();
        }

    }
}
