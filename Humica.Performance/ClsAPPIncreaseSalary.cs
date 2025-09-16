using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Logic.HR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Performance
{
    public class ClsAPPIncreaseSalary : IClsAPPIncreaseSalary
    {
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal SalaryInBgP { get; set; }
        public decimal SalaryIncBgUSD { get; set; }
        public decimal SalaryIncBgUtilised { get; set; }
        public decimal SalaryIncBgBalance { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HRAPPIncSalary HeaderIncSalary { get; set; }
        public List<HRAPPMatrixIncreaseSalary> ListMatrixIncrease { get; set; }
        public List<ClsIncreaseSalary> ListPendindIncrease { get; set; }
        public List<HRAPPIncSalary> ListIncSalary { get; set; }
        public List<HRAPPIncSalaryItem> ListIncSalaryItem { get; set; }
        public ClsAPPIncreaseSalary()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        public async Task<List<HRAPPMatrixIncreaseSalary>> LoadData(FTFilterEmployee filter, List<HRBranch> branches)
        {
            var filteredStaffProfiles = await DB.HRStaffProfiles
                .Where(w => w.Status == "A")
                .Where(w => filter.Branch == null || w.Branch == filter.Branch)
                .Where(w => filter.Division == null || w.Division == filter.Division)
                .Where(w => filter.Department == null || w.DEPT == filter.Department)
                .Where(w => filter.Section == null || w.SECT == filter.Section)
                .Where(w => filter.Position == null || w.JobCode == filter.Position)
                .Where(w => filter.Level == null || w.LevelCode == filter.Level)
                .ToListAsync();

            filteredStaffProfiles = filteredStaffProfiles.Where(x => branches.Where(w => w.Code == x.Branch).Any()).ToList();
            var staffEmpCodes = filteredStaffProfiles.Select(sp => sp.EmpCode).ToList();
            var existingSalaries = await DB.HRAPPMatrixIncreaseSalaries.Where(w => staffEmpCodes.Contains(w.EmpCode) && w.InYear == filter.INYear
            && w.ScreenID == ScreenId).ToListAsync();

            var newSalaries = new List<HRAPPMatrixIncreaseSalary>();
            var staffToProcess = filteredStaffProfiles.Where(sp => !existingSalaries.Any(es => es.EmpCode == sp.EmpCode)).ToList();

            var midPoints = await DB.HRAppLevelMidPoints.ToListAsync();

            var ListRating = await new HumicaDBContext().HRAppPerformanceRates.ToListAsync();
            int rating = 5;
            if (ListRating.Count() > 0)
                rating = ListRating.Max(w => w.Result);

            foreach (var staff in staffToProcess)
            {
                newSalaries.Add(CreateSalaryEntry(staff, filter.INYear, midPoints, rating, ""));
            }

            foreach (var existingSalary in existingSalaries)
            {
                newSalaries.Add(existingSalary);
            }

            Refreshvalue(newSalaries, filter.INYear);

            return newSalaries.OrderBy(s => s.EmpCode).ToList();
        }
        public async Task<List<ClsIncreaseSalary>> LoadDataIncrease(FTFilterEmployee filter, List<HRBranch> branches)
        {
            //var filteredStaffProfiles = await DB.HRStaffProfiles
            //    .Where(w => w.Status == "A")
            //    .Where(w => filter.Branch == null || w.Branch == filter.Branch)
            //    .Where(w => filter.Division == null || w.Division == filter.Division)
            //    .Where(w => filter.Department == null || w.DEPT == filter.Department)
            //    .Where(w => filter.Section == null || w.SECT == filter.Section)
            //    .Where(w => filter.Position == null || w.JobCode == filter.Position)
            //    .Where(w => filter.Level == null || w.LevelCode == filter.Level)
            //    .ToListAsync();

            //string RELEASED = SYDocumentStatus.RELEASED.ToString();
            string Open = SYDocumentStatus.OPEN.ToString();
            //var App_Review = await DB.HREmpAppProcesses.Where(w => w.Status == RELEASED).ToListAsync();
            //filteredStaffProfiles = filteredStaffProfiles.Where(w => App_Review.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

            //filteredStaffProfiles = filteredStaffProfiles.Where(x => branches.Where(w => w.Code == x.Branch).Any()).ToList();
            //var staffEmpCodes = filteredStaffProfiles.Select(sp => sp.EmpCode).ToList();
            var existingSalaries = await DB.HRAPPMatrixIncreaseSalaries.Where(w => w.Status == Open).ToListAsync();

            var newSalaries = new List<ClsIncreaseSalary>();
            //var staffToProcess = filteredStaffProfiles.Where(sp => !existingSalaries.Any(es => es.EmpCode == sp.EmpCode)).ToList();
            //var staffToProcess = filteredStaffProfiles.ToList();


            foreach (var existingSalary in existingSalaries)
            {
                if (existingSalary.Adding.HasValue == false) existingSalary.Adding = 0;
                var Obj = new ClsIncreaseSalary();
                Obj.EmpCode = existingSalary.EmpCode;
                Obj.EmployeeName = existingSalary.EmpName;
                Obj.CurrentSalary = existingSalary.Salary;
                Obj.Increase = existingSalary.SalaryIncAmount + existingSalary.Adding.Value;
                Obj.NewSalary = existingSalary.NewSalary;
                newSalaries.Add(Obj);
            }

            //await Refreshvalue(newSalaries, filter.INYear);

            return newSalaries.OrderBy(s => s.EmpCode).ToList();
        }
        public async Task GetDataIncrease(string ID)
        {
            string[] Emp = ID.Split(';');
            string Open = SYDocumentStatus.OPEN.ToString();
            var existingSalaries = await DB.HRAPPMatrixIncreaseSalaries.Where(w => w.Status == Open).ToListAsync();
            HeaderIncSalary = new HRAPPIncSalary();
            HeaderIncSalary.Status = Open;
            HeaderIncSalary.DocumentDate = DateTime.Now;
            ListIncSalaryItem = new List<HRAPPIncSalaryItem>();
            foreach (var item in Emp)
            {
                HRAPPMatrixIncreaseSalary MatIncSalary = existingSalaries.FirstOrDefault(w => w.EmpCode == item);
                if (MatIncSalary != null)
                {
                    var obj = new HRAPPIncSalaryItem();
                    obj.EmpCode = MatIncSalary.EmpCode;
                    obj.EmployeeName = MatIncSalary.EmpName;
                    obj.Increase = MatIncSalary.SalaryIncAmount + MatIncSalary.Adding.Value;
                    obj.NewSalary = MatIncSalary.NewSalary;
                    obj.DocumentRef = MatIncSalary.ID.ToString();
                    ListIncSalaryItem.Add(obj);
                }
            }
            HeaderIncSalary.TotalEmployee = ListIncSalaryItem.Count();
            HeaderIncSalary.TotalIncrease = ListIncSalaryItem.Sum(w => w.Increase);
        }



        public async Task<string> CreateIncSalary()
        {
            try
            {
                long i = 0;
                using (var context = new HumicaDBContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            HRStaffProfile Staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == HeaderIncSalary.Requestor);
                            if (Staff == null)
                            {
                                return "EEREQ";
                            }
                            HeaderIncSalary.RequestorName = Staff.AllName;
                            HeaderIncSalary.CreatedBy = User.UserName;
                            HeaderIncSalary.CreatedOn = DateTime.Today;
                            context.HRAPPIncSalaries.Add(HeaderIncSalary);

                            int row = await context.SaveChangesAsync();
                            i = HeaderIncSalary.ID;
                            string Open = SYDocumentStatus.OPEN.ToString();
                            var existingSalaries = await context.HRAPPMatrixIncreaseSalaries.Where(w => w.Status == Open).ToListAsync();
                            foreach (var item in ListIncSalaryItem)
                            {
                                item.ID = (int)i;
                                context.HRAPPIncSalaryItems.Add(item);
                                var Matri = existingSalaries.FirstOrDefault(w => w.ID == Convert.ToInt32(item.DocumentRef));
                                if (Matri != null)
                                {
                                    Matri.Status = SYDocumentStatus.COMPLETED.ToString();
                                    context.Entry(Matri).Property(w => w.Status).IsModified = true;
                                }
                            }

                            await context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderIncSalary.ID.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderIncSalary.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderIncSalary.ID.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public async Task<string> EditMatrix(HRAPPMatrixIncreaseSalary MModel)
        {
            MessageError = "";
            var ListRating = await new HumicaDBContext().HRAppPerformanceRates.ToListAsync();
            var rating = ListRating.Max(w => w.Result);
            if (rating < MModel.Rating)
            {
                MessageError = " : " + rating.ToString();
                return "RATING_MAX";
            }

            var objUpdate = ListMatrixIncrease.FirstOrDefault(w => w.EmpCode == MModel.EmpCode);
            if (objUpdate == null)
            {
                return "DOC_INV";
            }
            ClsAppraisel AppObj = new ClsAppraisel();
            objUpdate.Rating = MModel.Rating;
            var ratio = ClsRounding.RoundNormal((objUpdate.Salary / objUpdate.JobLevelMidPoint), 2);
            objUpdate.CompaRatio = ratio;
            objUpdate.SalaryIncAmount = ClsRounding.RoundNormal(AppObj.IncreaseSalary(MModel.EmpCode, MModel.JobLevel, MModel.Salary, MModel.Rating, MModel.InYear), 2);
            objUpdate.SalaryIncPers = ClsRounding.RoundNormal(AppObj.IncreaseSalaryPercentage(MModel.EmpCode, MModel.JobLevel, MModel.Salary, MModel.Rating, MModel.InYear), 2);
            objUpdate.NewSalary = objUpdate.Salary + objUpdate.SalaryIncAmount;
            return SYConstant.OK;
        }


        public async Task<string> requestToApprove(int id)
        {
            try
            {
                var objMatch = await DB.HRAPPIncSalaries.FirstOrDefaultAsync(w => w.ID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderIncSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public async Task<string> approveTheDoc(int id)
        {
            try
            {
                var objMatch = await DB.HRAPPIncSalaries.FirstOrDefaultAsync(w => w.ID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderIncSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.APPROVED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public async Task<string> ReleaseTheDoc(int id, DateTime EffectiveDate, string Career)
        {
            try
            {
                var objMatch = await DB.HRAPPIncSalaries.FirstOrDefaultAsync(w => w.ID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderIncSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.APPROVED.ToString())
                {
                    return "INV_DOC";
                }
                var LstCareerCode = await DB.HRCareerHistories.ToListAsync();
                var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == Career);
                if (_CarCode == null)
                {
                    return "CAREERCODE_EN";
                }

                var objMatchItem = await DB.HRAPPIncSalaryItems.Where(w => w.ID == id).ToListAsync();
                HRStaffProfileObject BSM = new HRStaffProfileObject();
                var mms = await BSM.TransferInscreaseSalary(EffectiveDate, objMatchItem);
                if (mms != SYConstant.OK)
                {
                    return mms;
                }
                objMatch.EffectiveDate = EffectiveDate;
                objMatch.CareerType = _CarCode.Description;
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public async Task<string> CancelTheDoc(int id)
        {
            try
            {
                var objMatch = await DB.HRAPPIncSalaries.FirstOrDefaultAsync(w => w.ID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderIncSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.RELEASED.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, HeaderIncSalary.ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }


        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public void OnIndexLoading(bool IsESS = false)
        {
            Filter = new FTFilterEmployee();
            ListMatrixIncrease = new List<HRAPPMatrixIncreaseSalary>();
            TotalSalary = 0;
            SalaryInBgP = 0;
            SalaryIncBgUSD = 0;
            SalaryIncBgUtilised = 0;
            SalaryIncBgBalance = 0;
            Filter.INYear = DateTime.Now.Year;
        }
        public void OnLoadingFilter(FTFilterEmployee filter, List<HRBranch> branches)
        {
            var filteredStaffProfiles = unitOfWork.Set<HRStaffProfile>()
                .Where(w => w.Status == "A")
                .Where(w => filter.Branch == null || w.Branch == filter.Branch)
                .Where(w => filter.Division == null || w.Division == filter.Division)
                .Where(w => filter.Department == null || w.DEPT == filter.Department)
                .Where(w => filter.Section == null || w.SECT == filter.Section)
                .Where(w => filter.Position == null || w.JobCode == filter.Position)
                .Where(w => filter.Level == null || w.LevelCode == filter.Level)
                .ToList();

            string RELEASED = SYDocumentStatus.RELEASED.ToString();
            var App_Review = unitOfWork.Set<HREmpAppProcess>().Where(w => w.Status == RELEASED && w.InYear == filter.INYear).ToList();
            filteredStaffProfiles = filteredStaffProfiles.Where(w => App_Review.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

            filteredStaffProfiles = filteredStaffProfiles.Where(x => branches.Where(w => w.Code == x.Branch).Any()).ToList();
            var staffEmpCodes = filteredStaffProfiles.Select(sp => sp.EmpCode).ToList();
            var existingSalaries = unitOfWork.Set<HRAPPMatrixIncreaseSalary>().Where(w => staffEmpCodes.Contains(w.EmpCode) && w.InYear == filter.INYear).ToList();

            ListMatrixIncrease = new List<HRAPPMatrixIncreaseSalary>();
            var staffToProcess = filteredStaffProfiles.Where(sp => !existingSalaries.Any(es => es.EmpCode == sp.EmpCode)).ToList();
            var midPoints = unitOfWork.Set<HRAppLevelMidPoint>().ToList();

            int rating = 0;

            foreach (var staff in staffToProcess)
            {
                HREmpAppProcess _review = App_Review.FirstOrDefault(w => w.EmpCode == staff.EmpCode);
                if (_review != null)
                {
                    rating = (int)_review.Rate;
                    ListMatrixIncrease.Add(CreateSalaryEntry(staff, filter.INYear, midPoints, rating, _review.TranNo.ToString()));

                }
            }

            foreach (var existingSalary in existingSalaries)
            {
                ListMatrixIncrease.Add(existingSalary);
            }

            Refreshvalue(ListMatrixIncrease, filter.INYear);

            //return ListMatrixIncrease.OrderBy(s => s.EmpCode).ToList();
        }
        public string CareerMatrix()
        {
            try
            {
                foreach (var read in ListMatrixIncrease)
                {
                    read.ScreenID = ScreenId;
                    var obj = unitOfWork.Set<HRAPPMatrixIncreaseSalary>().FirstOrDefault(w => w.EmpCode == read.EmpCode && w.InYear == read.InYear);
                    if (obj != null)
                    {
                        obj.Rating = read.Rating;
                        obj.Adding = read.Adding;
                        obj.SalaryIncAmount = read.SalaryIncAmount;
                        obj.SalaryIncPers = read.SalaryIncPers;
                        obj.NewSalary = read.NewSalary;
                        obj.ChangedOn = DateTime.Now;
                        obj.ChangedBy = User.UserName;
                        unitOfWork.Update(obj);
                    }
                    else
                    {
                        read.Status = SYDocumentStatus.OPEN.ToString();
                        read.CreatedBy = User.UserName;
                        read.CreatedOn = DateTime.Now;
                        unitOfWork.Add(read);
                    }
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string EditMatrix_Adding(HRAPPMatrixIncreaseSalary MModel)
        {
            var objUpdate = ListMatrixIncrease.FirstOrDefault(w => w.EmpCode == MModel.EmpCode);
            if (objUpdate == null)
            {
                return "DOC_INV";
            }
            ClsAppraisel AppObj = new ClsAppraisel();
            objUpdate.Adding = MModel.Adding;
            objUpdate.NewSalary = objUpdate.Salary + objUpdate.SalaryIncAmount + MModel.Adding.Value;
            return SYConstant.OK;
        }
        public string OnGridModifyMaxSalary(HRAPPMatrixIncreaseSalary MModel, string Action)
        {
            try
            {
                //if (Action == "ADD")
                //{
                //    unitOfWork.Add(MModel);
                //}
                //else if (Action == "EDIT")
                //{
                //    unitOfWork.Update(MModel);
                //}
                //else 
                if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<HRAPPMatrixIncreaseSalary>().FirstOrDefault(w => w.EmpCode == MModel.EmpCode && w.InYear == MModel.InYear);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                        unitOfWork.Save();
                    }
                    else
                    {
                        var obj = ListMatrixIncrease.FirstOrDefault(w => w.EmpCode == MModel.EmpCode);
                        if (obj != null)
                        {
                            ListMatrixIncrease.Remove(obj);
                        }
                    }
                }


                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private HRAPPMatrixIncreaseSalary CreateSalaryEntry(HRStaffProfile staff, int year,
           List<HRAppLevelMidPoint> midPoints, int rating, string DocumentRef)
        {
            var appObj = new ClsAppraisel();
            var midPoint = midPoints.FirstOrDefault(mp => mp.JobLevel == staff.LevelCode);

            var salaryEntry = new HRAPPMatrixIncreaseSalary
            {
                DocumentRef = DocumentRef,
                EmpCode = staff.EmpCode,
                EmpName = staff.AllName,
                InYear = year,
                JobLevel = staff.LevelCode,
                Salary = staff.Salary,
                Rating = rating,
                SalaryIncAmount = ClsRounding.RoundNormal(appObj.IncreaseSalary(staff.EmpCode, staff.LevelCode, staff.Salary, rating, year), 2),
                SalaryIncPers = ClsRounding.RoundNormal(appObj.IncreaseSalaryPercentage(staff.EmpCode, staff.LevelCode, staff.Salary, rating, year), 2),
                NewSalary = ClsRounding.RoundNormal(staff.Salary + (appObj.IncreaseSalary(staff.EmpCode, staff.LevelCode, staff.Salary, rating, year)), 2),
                JobLevelMidPoint = midPoint?.JobLevelMidPoint ?? 0,
                CompaRatio = midPoint != null ? ClsRounding.RoundNormal(staff.Salary / midPoint.JobLevelMidPoint.Value, 2) : 0
            };

            return salaryEntry;
        }
        public void Refreshvalue(List<HRAPPMatrixIncreaseSalary> ListIncrease, int InYear)
        {
            TotalSalary = ListIncrease.Sum(w => w.Salary);
            var Budget = unitOfWork.Set<HRAppSalaryBudget>().FirstOrDefault(w => w.InYear == InYear);
            if (Budget != null)
            {
                SalaryInBgP = Budget.Budget;
                SalaryIncBgUSD = Math.Round(TotalSalary * Budget.Budget, 2);
            }
            SalaryIncBgUtilised = Convert.ToDecimal(Math.Round(ListIncrease.Sum(w => w.SalaryIncAmount), 2));
            SalaryIncBgBalance = SalaryIncBgUSD - SalaryIncBgUtilised;
        }

        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("BRANCHES_SELECT", SYConstant.getBranchDataAccess());
            keyValues.Add("SECTION_SELECT", ClsFilter.LoadSection());
            keyValues.Add("BusinessUnit_SELECT", ClsFilter.LoadBusinessUnit());
            keyValues.Add("POSITION_SELECT", ClsFilter.LoadPosition());
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("LEVEL_SELECT", SYConstant.getLevelDataAccess());

            return keyValues;
        }
    }
    public class ClsIncreaseSalary
    {
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Branch { get; set; }
        public string Office { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Group { get; set; }
        public string Position { get; set; }
        public decimal CurrentSalary { get; set; }
        public decimal Increase { get; set; }
        public decimal NewSalary { get; set; }

    }
}
