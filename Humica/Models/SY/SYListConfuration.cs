using System;
using System.Diagnostics;

namespace Humica.Models.SY
{
    public class SYListConfuration
    {
        public static string ListDefaultView
        {
            get { return "~/Views/ListDefault/PartialList.cshtml"; }
        }

        public static string ListDefaultViewProcess
        {
            get { return "~/Views/ListDefault/PartialListProcess.cshtml"; }
        }

        public static string ListDefaultViewSelect
        {
            get { return "~/Views/ListDefault/PartialListSelect.cshtml"; }
        }

        public static string ListPivotSelectView
        {
            get { return "~/Views/ListDefault/PartialPivot.cshtml"; }
        }

        public static string ListRowSelectView
        {
            get { return "~/Views/ListDefault/PartialRow.cshtml"; }
        }

        public static string ListDefaultUpload
        {
            get { return "~/Views/ListDefault/UploadList.cshtml"; }
        }

        public static string FilterGeneralAccounting
        {
            get { return "~/Views/FILTER/FTGernalAccounting.cshtml"; }
        }

        public static string ReportingPathPartial
        {
            get { return "~/Views/ListReportingView/"; }
        }
        public static string ReportingPathView
        {
            get { return "~/Views/ListReportingView/ReportView.cshtml"; }
        }
        public static string ReportingList
        {
            get { return "~/Views/ListDefault/ReportList.cshtml"; }
        }

        public static string FilterGeneralPeriodReport
        {
            get { return "~/Views/FILTER/FTGeneralPeriodReport.cshtml"; }
        }

        public static string FilterDealerSelector
        {
            get { return "~/Views/ListReportingView/PartialDealerSearch.cshtml"; }
        }

        public static string FilterDPostalCode
        {
            get { return "~/Views/ListFilter/PartialPostalSearch.cshtml"; }
        }

        public static void OpenFileAuotoDes()
        {
            //SYListConfuration.CloseSFileAuoto();
            //var DB = new Humica.Core.DB.HumicaDBContext();
            //var path = DB.HLIntegreteTypes.Find("AUTOOPEN");
            ////System.Diagnostics.Process.Start(path.PathFileStore);

            //ProcessStartInfo procInfo = new ProcessStartInfo();
            //procInfo.UseShellExecute = true;
            //procInfo.FileName = path.PathFileStore;  //The file in that DIR.
            //procInfo.WorkingDirectory = @""; //The working DIR.
            //procInfo.Verb = "runas";
            //Process.Start(procInfo);

            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            //process.StartInfo = startInfo;
            //process.Start();

            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.FileName = @path.PathFileStore;
            //startInfo.Arguments = @" -u Sunfix -x @dmin123 -a Auto";
            //startInfo.Verb = "runas";
            //process.StartInfo = startInfo;
            //process.Start();

            //var psi = new ProcessStartInfo(@path.PathFileStore)
            //{
            //    Arguments = @" -u Sunfix -x @dmin123 -a Auto",
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};
            //Process.Start(psi);

        }

        public static void OpenFileAuto(string Part)
        {
            SYListConfuration.CloseSFileAuoto();
            string PartFill = "";
            if (Part == "TINV")
            {
                PartFill = "D:\\LYP\\AutoDesk\\CTL-TINV.bat";  //The file in that DIR.
            }
            else if (Part == "SINV")
            {
                PartFill = "D:\\LYP\\AutoDesk\\CTL-SINV.bat";
            }
            else if (Part == "DepositCancel")
            {
                PartFill = "D:\\LYP\\AutoDesk\\CTL-DepositCancel.bat";
            }
            else if (Part == "Deposit")
            {
                PartFill = "D:\\LYP\\AutoDesk\\CTL-Deposit.bat";
            }
            else if (Part == "Receipt")
            {
                PartFill = "D:\\LYP\\AutoDesk\\CTL-Receipt.bat";
            }
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = PartFill;  //The file in that DIR.
            procInfo.WorkingDirectory = @""; //The working DIR.
            procInfo.Verb = "runas";
            Process.Start(procInfo);
        }


        public static void OpenBathFile(string OpenPath, string ClosePath)
        {
            SYListConfuration.CloseTransactionFile(ClosePath);
            //string PartFill = "";

            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = OpenPath;  //The file in that DIR.
            procInfo.WorkingDirectory = @""; //The working DIR.
            procInfo.Verb = "runas";
            Process.Start(procInfo);
        }
        public static void CloseTransactionFile(string path)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = path;  //The file in that DIR.
            procInfo.WorkingDirectory = @""; //The working DIR.
            procInfo.Verb = "runas";
            Process.Start(procInfo);
        }

        public static void CloseSFileAuoto()
        {
            string PartFill = "D:\\LYP\\Test\\CloseAutoDesk.bat";
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = PartFill;  //The file in that DIR.
            procInfo.WorkingDirectory = @""; //The working DIR.
            procInfo.Verb = "runas";
            Process.Start(procInfo);
        }



        public static void CloseSFileAuotoDes()
        {
            //var DB = new Humica.Core.DB.HumicaDBContext();
            //var path = DB.HLIntegreteTypes.Find("AUTOCLOSE");
            //System.Diagnostics.Process.Start(path.PathFileStore);
        }


        public static int ExecuteBatchFile(string batchFilePath, int timeout, bool killOnTimeout = false)
        {
            try
            {
                using (var p = Process.Start(batchFilePath))
                {
                    p.WaitForExit(timeout);

                    if (p.HasExited)
                        return p.ExitCode;

                    if (killOnTimeout)
                    {
                        p.Kill();
                    }
                    else
                    {
                        p.CloseMainWindow();
                    }

                    throw new TimeoutException(string.Format("Time allotted for executing `{0}` has expired ({1} ms).", batchFilePath, timeout));
                }
            }
            catch
            {
                return 0;
            }

        }

    }
}