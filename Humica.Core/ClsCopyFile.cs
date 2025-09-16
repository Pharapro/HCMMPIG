using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Humica.Core
{
    public class ClsCopyFile : IClsApplication
    {
        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsCopyFile()
        {
            OnLoad();
        }
        public string CopyStructurePath(string CompanyCode, string DocumentType, string EmpCode,string Dept ,string path, bool IsLink = false)
        {
            ClsFilterStaff clsFilterStaff = new ClsFilterStaff();
            HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            string Department = clsFilterStaff.Get_Department(Dept);
            var strpath = unitOfWork.Set<CFUploadPath>().Find("Attachment");
            if (strpath == null)
            {
				return "No_Path";
			}
            string ReturnPath = path;
            string ReturnPathTem = "";
            string[] _path = path.Split(';');
            foreach (var P in _path)
            {
                if (!string.IsNullOrEmpty(P))
                {
                    Department = Department.Trim();
                    string StructurePath = "/Content/UPLOAD/" + CompanyCode + "/" + DocumentType + "/" + Department + "/" + EmpCode;
                    StructurePath = StructurePath.Replace("&", " and ");
                    if (IsLink)
                    {
                        ReturnPathTem = CopyFileFromUrl(P, StructurePath, strpath.PathDirectory);
                    }
                    else
                    {
                        ReturnPathTem = CopyFilePath(StructurePath, P, strpath.PathDirectory);
                    }
                    if (ReturnPathTem != "Error")
                    {
                        ReturnPath = ReturnPath.Replace(P, ReturnPathTem);

					}
				}
            }
            return ReturnPath;
        }
        public string CopyFilePath(string DocumentPath, string path, string PathDirectory)
        {
            try
            {
                string FileNameOld = System.Web.HttpContext.Current.Request.MapPath(path);
                string fileName = Path.GetFileName(FileNameOld);
                string destinationPath = Path.Combine(PathDirectory, DocumentPath.Trim('/'));
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                string filenameSave = Path.Combine(destinationPath, fileName);
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
                return "Error";
                // Log the exception and rethrow or handle as needed
                //throw new InvalidOperationException("Error copying file.", ex);
            }
        }
        public string CopyFileFromUrl(string fileUrl, string destinationDirectory, string PathDirectory)
        {
            try
            {
                //string destinationPath = System.Web.HttpContext.Current.Request.MapPath(destinationDirectory.Trim('/'));
                string destinationPath = Path.Combine(PathDirectory, destinationDirectory.Trim('/'));
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
        public string DeleteFileStore(string FilePath, string NewFilePath = ";")
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return SYConstant.OK;
            }
            string root = HostingEnvironment.ApplicationPhysicalPath;
            string[] path = FilePath.Split(';');
            string[] newPaths = NewFilePath.Split(';');
            foreach (var pathItem in path)
            {
                if (!newPaths.Contains(pathItem))
                {
                    string fileNameDelete = root + pathItem;
                    fileNameDelete = fileNameDelete.Replace("~", "").Replace("/", "\\");
                    if (System.IO.File.Exists(fileNameDelete))
                    {
                        System.IO.File.Delete(fileNameDelete);
                    }
                }
            }
            return SYConstant.OK;
        }
        public string DeleteFileStore_URL(string fileUrl)
        {
            try
            {
                Uri uri = new Uri(fileUrl);
                string filePath = uri.LocalPath;
                DeleteFileStore(filePath);
                return SYConstant.OK;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
