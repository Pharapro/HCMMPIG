using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Humica.Models.FL
{
    public class FileManagerItemInfo
    {
        const string FolderTypeName = "Folder";
        public FileManagerItemInfo()
        {
        }

        public string Name { get; set; }
        public string Type { get; set; }
        [DisplayFormat(DataFormatString = "#,#")]
        public long Size { get; set; }
        [DisplayFormat(DataFormatString = "U")]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "U")]
        public DateTime Modified { get; set; }
        [DisplayFormat(DataFormatString = "U")]
        public DateTime Accessed { get; set; }
        public string Location { get; set; }
        public string RelativeLocation { get; set; }
        public string Details { get; set; }
        public bool IsFile { get { return !string.Equals(Type, FolderTypeName); } }

        static Dictionary<string, string> extensionsDisplayName;
        protected static Dictionary<string, string> ExtensionsDisplayName
        {
            get
            {
                if (extensionsDisplayName == null)
                    extensionsDisplayName = XDocument
                        .Load(HttpContext.Current.Server.MapPath("~/Content/FileManager/ExtensionsDisplayName.xml"))
                        .Descendants("Extension")
                        .ToDictionary(n => n.Attribute("Extension").Value, n => n.Attribute("DisplayName").Value);
                return extensionsDisplayName;
            }
        }

        public static FileManagerItemInfo Create(string relativeLocaton, string location)
        {
            FileManagerItemInfo itemInfo = new FileManagerItemInfo
            {
                Location = location,
                RelativeLocation = relativeLocaton
            };

            string fullPath = HttpContext.Current.Server.MapPath(location);
            bool isDirectory = ((File.GetAttributes(fullPath) & FileAttributes.Directory) == FileAttributes.Directory);
            if (isDirectory)
                itemInfo.PrepareDirectoryInfoItem(fullPath);
            else
                itemInfo.PrepareFileInfoItem(fullPath);
            return itemInfo;
        }
        void PrepareFileInfoItem(string path)
        {
            var file = new FileInfo(path);
            Name = file.Name;
            Type = ExtensionsDisplayName[file.Extension];
            Size = file.Length;
            Created = file.CreationTime;
            Modified = file.LastWriteTime;
            Accessed = file.LastAccessTime;
            Details = string.Empty;
        }
        void PrepareDirectoryInfoItem(string path)
        {
            var directory = new DirectoryInfo(path);

            Name = directory.Name;
            Type = FolderTypeName;
            Size = GetDirectorySize(directory);
            Created = directory.CreationTime;

            int filesCount = directory.GetFiles("*", SearchOption.AllDirectories).Length;
            int subDirectoriesCount = directory.GetDirectories("*", SearchOption.AllDirectories).Length;
            Details = filesCount + " Files, " + subDirectoriesCount + " Folders";
        }
        static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long directorySize = 0;
            foreach (var fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                directorySize += fileInfo.Length;
            return directorySize;
        }
    }
}