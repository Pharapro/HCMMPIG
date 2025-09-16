using Humica.Core.DB;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Humica.Logic
{
    public class ClsCopyFile
    {
        public static string CopyNewStructurePath(string DocumentType, string EmpCode, string path, bool IsLink = false)
        {
            string ReturnPath = "";
            if (!string.IsNullOrEmpty(path))
            {
                HRStaffProfile Staff = new HumicaDBContext().HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
                string Department = ClsFilter.GetDepartment_Description(Staff.DEPT);
                string StructurePath = "/Content/" + DocumentType + "/" + Department + "/" + EmpCode;
                if (IsLink)
                {
                    ReturnPath = CopyFileFromUrl(path, StructurePath);
                }
                else
                    ReturnPath = CopyFilePath(StructurePath, path, "");
            }
            return ReturnPath;
        }
        public static string CopyFilePath(string DocumentPath, string path, string PathDirectory)
        {
            try
            {
                string FileNameOld = System.Web.HttpContext.Current.Request.MapPath(path);
                string fileName = Path.GetFileName(FileNameOld);
                string destinationPath = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, DocumentPath);
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                var filenameSave = System.Web.HttpContext.Current.Request.MapPath(DocumentPath + "/" + fileName);
                //string filenameSave = Path.Combine(destinationPath, fileName);
                if (!string.Equals(FileNameOld, filenameSave, StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(FileNameOld, filenameSave, overwrite: true);
                }
                //File.Copy(FileNameOld, filenameSave, overwrite: true);
                return $"~{DocumentPath}/{fileName}";
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle as needed
                throw new InvalidOperationException("Error copying file.", ex);
            }
        }

        public static string CopyFileFromUrl(string fileUrl, string destinationDirectory)
        {
            try
            {
                string destinationPath = System.Web.HttpContext.Current.Request.MapPath(destinationDirectory);
                string fileName = Path.GetFileName(fileUrl);

                string destinationFilePath = Path.Combine(destinationPath, fileName);

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                if (!File.Exists(destinationFilePath))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(fileUrl, destinationFilePath);
                    }
                    string relativeUrl = VirtualPathUtility.ToAbsolute(Path.Combine(destinationDirectory, fileName));
                    string savedUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + relativeUrl;
                    return savedUrl;
                }
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error copying file from URL.", ex);
            }
        }

    }
}
