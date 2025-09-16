using Hangfire;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Integration.API;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.Att
{
    public class ATDownloadObject : ReuqestBaseService<ClsBioTime, ClsBioTime, FilterRequest>
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTFilterInOut Filter { get; set; }
        public ATDownloadObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();

        }
        public async Task<string> DownloadData(DateTime FromDate, DateTime ToDate)
        {
           
            //DateTime Now = DateTime.Now.AddDays(-1);
            //DateTime FromDate = new DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0);
            //DateTime ToDate = new DateTime(Now.Year, Now.Month, Now.Day, 23, 59, 59);
            BackgroundJob.Enqueue(() => GetData(FromDate,ToDate));
            return SYConstant.OK;
        }
        public async Task GetData(DateTime FromDate, DateTime ToDate)
        {
            DB = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                LoginRequest request = new LoginRequest
                {
                    username = "admin",
                    password = "biotime@tela23",
                };
                var Token = await Login(request);

                //await Login(request);
                var objectResult = await Get("/iclock/api/transactions/?start_time="+FromDate.ToString("yyyy-MM-dd HH:mm:ss")+"&end_time=" + ToDate.ToString("yyyy-MM-dd HH:mm:ss") + "&page=1&page_size=100");
                var _listINOut = await Task.Run(() =>
                {
                    return (from s in DB.ATInOuts
                            where EntityFunctions.TruncateTime(s.FullDate) >= FromDate.Date
                            && EntityFunctions.TruncateTime(s.FullDate) <= ToDate.Date
                            && s.EmpCode.Trim() != ""
                            select s).ToList();
                });
                List<ClsAttInOut> ListAtt = objectResult.Result.Data.ToList();
                List<ATInOut> tmpInout = new List<ATInOut>();
                ListAtt = ListAtt.Where(w => !_listINOut.Where(at => Convert.ToInt32(at.CardNo) == Convert.ToInt32(w.emp) && at.FullDate == w.upload_time).Any()).ToList();

                foreach (var item in ListAtt)
                {
                    DateTime FullDate = item.upload_time;
                    if (!_listINOut.Where(w => w.EmpCode == item.emp_code
                                   && w.FullDate.Year == FullDate.Year
                                   && w.FullDate.Month == FullDate.Month
                                   && w.FullDate.Day == FullDate.Day
                                   && w.FullDate.Hour == FullDate.Hour
                                   && w.FullDate.Minute == FullDate.Minute).Any())
                    {
                        if (tmpInout.Where(w => w.EmpCode == item.emp_code
                      && w.FullDate.Year == FullDate.Year && w.FullDate.Month == FullDate.Month
                      && w.FullDate.Day == FullDate.Day && w.FullDate.Hour == FullDate.Hour
                      && w.FullDate.Minute == FullDate.Minute).Any())
                            continue;

                        var InOut = new ATInOut();

                        InOut.EmpCode = item.emp_code;
                        InOut.STATUS = 3;
                        InOut.CardNo = item.emp.ToString();
                        InOut.FullDate = item.upload_time;
                        InOut.CreateBy = "System";
                        InOut.CreateOn = DateTime.Now;
                        tmpInout.Add(InOut);
                        DB.ATInOuts.Add(InOut);
                    }
                }
                int row = DB.SaveChanges();
            }

            catch (Exception ex)
            {
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }
}
