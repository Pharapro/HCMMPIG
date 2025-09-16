using System.IO;
using System.Web;

namespace Humica.Models.FL
{
    public static class FileManagmentUtils
    {
        const string RootFolderVirtPath = "~/Content/FileManager/VirtualScrolling/Files";

        static object filesCreationLocker = new object();
        static bool FilesCreated { get; set; }

        public static void EnsureDemoFilesCreated()
        {
            if (FilesCreated) return;

            lock (FileManagmentUtils.filesCreationLocker)
            {
                if (!FilesCreated)
                {
                    CreateFiles();
                    FilesCreated = true;
                }
            }
        }

        static void CreateFiles()
        {
            CreateFolderWithFiles("Projects", 1000, "Project {0}.txt");
            CreateFolderWithFiles("Reports", 100, "Report {0}.xls");
            CreateFolderWithFiles("Articles", 500, "Article {0}.rtf");
        }
        static void CreateFolderWithFiles(string keyFolderName, int fileCount, string fileNameTemplate)
        {
            string rootFolderPath = HttpContext.Current.Server.MapPath(RootFolderVirtPath);
            string displayFolderName = string.Format("{0} ({1})", keyFolderName, fileCount);
            string folderPath = Path.Combine(rootFolderPath, displayFolderName);
            Directory.CreateDirectory(folderPath);
            for (int i = 1; i <= fileCount; i++)
            {
                string fileName = string.Format(fileNameTemplate, i);
                string filePath = Path.Combine(folderPath, fileName);
                using (File.Create(filePath)) { }
            }
        }
    }
}