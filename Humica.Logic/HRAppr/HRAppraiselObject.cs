using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.HR
{
    public class HRAppraiselObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string DocType { get; set; }

        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public HREmpAppraisal Header { get; set; }
        public HREmpAppProcess HeaderProcess { get; set; }
        public HREmpAppraisalItem Score { get; set; }
        public string ErrorMessage { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public List<HRApprFactor> ListFactor { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }

        public List<HREmpAppraisalItem> ListScore { get; set; }
        public List<HREmpAppraisal> ListHeader { get; set; }
        public List<HREmpAppraisal> ListAppraisaPending { get; set; }
        public List<HRApprRating> ListApprRating { get; set; }
        public List<HRAppGrade> ListApprResult { get; set; }
        public List<HRApprRegion> ListRegion { get; set; }
        public List<_ListStaff> ListEmpStaff { get; set; }
        public List<HREmpAppProcess> ListAppProcess { get; set; }
        public List<HREmpAppProcessItem> ListAppProcessItem { get; set; }
        public string Rating { get; set; }
        public int RateMin { get; set; }
        public int RateMax { get; set; }

        public HRAppraiselObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<List<_ListStaff>> LoadData(FTFilterEmployee Filter1, List<HRBranch> _lstBranch)
        {
            DateTime date = new DateTime(1900, 1, 1);
            var _List = new List<_ListStaff>();
            var positions = await DB.HRPositions.ToListAsync();
            var Department = await DB.HRDepartments.ToListAsync();
            var Section = await DB.HRSections.ToListAsync();
            var _staff = await DB.HRStaffProfiles.Where(w => w.Status == "A"
                           && (string.IsNullOrEmpty(Filter1.Branch) || w.Branch == Filter1.Branch)
                           && (string.IsNullOrEmpty(Filter1.Division) || w.Division == Filter1.Division)
                           && (string.IsNullOrEmpty(Filter1.BusinessUnit) || w.GroupDept == Filter1.BusinessUnit)
                           && (string.IsNullOrEmpty(Filter1.Department) || w.DEPT == Filter1.Department)
                           && (string.IsNullOrEmpty(Filter1.Office) || w.Office == Filter1.Office)
                           && (string.IsNullOrEmpty(Filter1.Section) || w.SECT == Filter1.Section)
                           && (string.IsNullOrEmpty(Filter1.Group) || w.Groups == Filter1.Group)
                           && (string.IsNullOrEmpty(Filter1.Position) || w.JobCode == Filter1.Position)
                           && (string.IsNullOrEmpty(Filter1.Level) || w.LevelCode == Filter1.Level)
                           && (string.IsNullOrEmpty(Filter1.Category) || w.CATE == Filter1.Category)
                           ).ToListAsync();

            foreach (var item in _staff)
            {
                var s = _staff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                var EmpStaff = new _ListStaff();
                EmpStaff.EmpCode = item.EmpCode;
                EmpStaff.AllNameKH = item.OthAllName;
                EmpStaff.AllNameENG = item.AllName;
                if (_lstBranch.Where(w => w.Code == item.Branch).Any())
                    EmpStaff.Branch = _lstBranch.FirstOrDefault(w => w.Code == item.Branch).Description;
                if (Department.Where(w => w.Code == item.DEPT).Any())
                    EmpStaff.Department = Department.FirstOrDefault(w => w.Code == item.DEPT).Description;
                if (positions.Where(w => w.Code == item.JobCode).Any())
                    EmpStaff.Position = positions.FirstOrDefault(w => w.Code == item.JobCode).Description;
                if (Section.Where(w => w.Code == item.SECT).Any())
                    EmpStaff.Section = Section.FirstOrDefault(w => w.Code == item.SECT).Description;
                EmpStaff.Sex = item.Sex;
                EmpStaff.FirstLine = item.FirstLine;
                EmpStaff.SecondLine = item.SecondLine;
                _List.Add(EmpStaff);
            }
            return _List.OrderBy(w => w.EmpCode).ToList();
        }
        public async Task<HREmpAppProcess> GetDateAppraisel(string AppID)
        {
            HeaderProcess = new HREmpAppProcess();
            ListAppProcessItem = new List<HREmpAppProcessItem>();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Open = SYDocumentStatus.OPEN.ToString();
            HREmpAppraisal Appraisal = await DB.HREmpAppraisals.FirstOrDefaultAsync(w => w.ApprID == AppID && w.Status == approved);
            HRStaffProfile Staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == Appraisal.EmpCode);
            IEnumerable<HREmpAppraisalItem> ListAppraisalItem = await DB.HREmpAppraisalItems.Where(w => w.ApprID == AppID).ToListAsync();
            List<HRAppPerformanceRate> ListAppraisalRate = await DB.HRAppPerformanceRates.ToListAsync();
            List<HRAppGrade> ListGrade = await DB.HRAppGrades.ToListAsync();
            if (Appraisal != null)
            {
                decimal TotalScore = Appraisal.TotalScore.Value;
                HeaderProcess.EmpCode = Appraisal.EmpCode;
                HeaderProcess.EmployeeName = Appraisal.EmpName;
                HeaderProcess.Department = Appraisal.Department;
                HeaderProcess.Position = Appraisal.Position;
                HeaderProcess.DocumentDate = DateTime.Now;
                HeaderProcess.AppraisalType = Appraisal.AppraisalType;
                HeaderProcess.InYear = Appraisal.InYear;
                HeaderProcess.PeriodFrom = Appraisal.PeriodFrom.Value;
                HeaderProcess.PeriodTo = Appraisal.PeriodTo.Value;
                HeaderProcess.TotalScore = TotalScore;
                HeaderProcess.Rate = 0;
                if (ListAppraisalRate.Count > 0)
                {
                    var Rate = ListAppraisalRate.FirstOrDefault(w => w.FromScore <= TotalScore && w.ToScore >= TotalScore);
                    if (Rate != null)
                        HeaderProcess.Rate = Rate.Result;
                }
                if (ListGrade.Count > 0)
                {
                    var Rate = ListGrade.FirstOrDefault(w => w.FromScore <= TotalScore && w.ToScore >= TotalScore);
                    if (Rate != null)
                        HeaderProcess.Grade = Rate.Grade;
                }
                //HeaderProcess.Increase = await IncreaseSalary(Appraisal.EmpCode, Staff.LevelCode, Staff.Salary, (int) HeaderProcess.Rate, Appraisal.InYear);
                HeaderProcess.DocumentRef = AppID;
                HeaderProcess.Status = Open;
            }
            foreach (var item in ListAppraisalItem)
            {
                decimal Score = 0;
                if (item.Score.HasValue == true)
                    Score = item.Score.Value;
                if (ListAppProcessItem.Where(w => w.Category == Appraisal.AppraisalType && w.Code == item.Region).Count() == 0)
                {
                    var Obj = new HREmpAppProcessItem();
                    Obj.Category = Appraisal.AppraisalType;
                    Obj.Code = item.Region;
                    Obj.Description = item.Description;
                    Obj.Result = Score;
                    ListAppProcessItem.Add(Obj);
                }
                else
                {
                    ListAppProcessItem.Where(w => w.Category == Appraisal.AppraisalType && w.Code == item.Region).ToList().ForEach(w => w.Result += Score);
                }
            }
            return HeaderProcess;
        }
        
        public string CreateAppr()
        {
            try
            {
                DB = new HumicaDBContext();
                HumicaDBContext DBM = new HumicaDBContext();
                var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                var ListRegion = DB.HRApprRegions.Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                var EvalRating = DB.HRApprRatings.ToList();
                var lstappr = DB.HRApprFactors.ToList();
                var objCF = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.NumberRank, EmpStaff.CompanyCode, Header.AppraiserDate.Value.Year, true);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (Header.AppraiserCode == null)
                {
                    return "APP_INVALID";
                }
                Header.ApprID = objNumber.NextNumberRank;
               
                if (EmpStaff != null)
                {
                    Header.Position = EmpStaff.Position;
                    Header.EmpName = EmpStaff.AllName;
                    Header.Department = EmpStaff.Department;

                    Header.TotalScore = 0;
                    foreach (var item in ListFactor)
                    {
                        var obj = new HREmpAppraisalItem();
                        obj.ApprID = Header.ApprID;
                        obj.Region = item.Region;
                        obj.Remark = item.Remark;
                        var check = lstappr.Where(w => w.Code == item.Code).ToList();
                        if (check.Count > 0)
                        {
                            obj.Description = check.First().Description;
                            obj.SecDescription = check.First().SecDescription;
                            //obj.Weighting = check.First().Weighting;
                        }
                        obj.Code = item.Code;
                        foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                        {
                            obj.RatingID = read.RatingID;
                            obj.Comment = read.Comment;
                            foreach (var read1 in EvalRating.Where(w => w.AppraisalType == Header.AppraisalType && w.ID == read.RatingID).ToList())
                            {
                                obj.Score = read.RatingID;
                                Header.TotalScore += (decimal)obj.Score;
                            }
                        }
                        foreach (var read in ListRegion.Where(w => w.Code == item.Region).ToList())
                        {
                            obj.RegionDescription = read.Description;
                        }
                        DB.HREmpAppraisalItems.Add(obj);
                    }
                    var EvaRate = DB.HRAppGrades.ToList();
                    var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                    foreach (var item in Rate)
                    {
                        Header.Result = item.Grade;
                    }
                    Header.Status = SYDocumentStatus.OPEN.ToString();
                    Header.Rating = RatingScale(Header.AppraisalType, Header.TotalScore.Value);
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    DB.HREmpAppraisals.Add(Header);
                }

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public async Task<string> CreateMulti(string EmpCode, HREmpAppraisal Obj)
        {
            ScreenId = "HRA0000001";
            string ErrorCode = "";
            try
            {
                if (string.IsNullOrEmpty(Obj.AppraisalType))
                {
                    return "EE_DOCTYPE";
                }

                DB = new HumicaDBContext();
                HumicaDBContext DBM = new HumicaDBContext();
                var _ListRegion = await DB.HRApprRegions.Where(w => w.AppraiselType == Obj.AppraisalType).ToListAsync();
                var quiz = DB.HRApprFactors.ToList();
                var lstappr = quiz.Where(w => _ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                var objCF = await DB.ExCfWorkFlowItems.FirstOrDefaultAsync(w => w.ScreenID == ScreenId);
                var EmpApp = await DBV.HR_STAFF_VIEW.FirstOrDefaultAsync(w => w.EmpCode == Obj.AppraiserCode);
                if (EmpApp == null)
                {
                    return "EMPLOYEE_NE";
                }
                if (lstappr.Count() == 0)
                {
                    return "INVALID_FACTOR";
                }


                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                string[] Emp = EmpCode.Split(';');
                foreach (var Code in Emp)
                {
                    Header = new HREmpAppraisal();
                    Header.AppraisalType = Obj.AppraisalType;
                    Header.AppraiserCode = Obj.AppraiserCode;
                    Header.AppraiserDate = Obj.AppraiserDate;
                    Header.InYear = Obj.InYear;
                    Header.PeriodFrom = Obj.PeriodFrom;
                    Header.PeriodTo = Obj.PeriodTo;
                    Header.KPIType = Obj.KPIType;
                    Header.AppraiserDeadline = Obj.AppraiserDeadline;
                    Header.AppraiserDeadline2 = Obj.AppraiserDeadline2;
                    Header.KPIExpectedDate = Obj.KPIExpectedDate;
                    Header.KPIDeadline = Obj.KPIDeadline;
                    Header.AppraiserName = EmpApp.AllName;
                    Header.AppraiserPosition = EmpApp.Position;
                    Header.Status = SYDocumentStatus.PENDING.ToString();
                    ErrorCode = Code;
                    if (Code.Trim() == "") continue;
                    Header.EmpCode = Code;
                    var EmpStaff = await DBV.HR_STAFF_VIEW.FirstOrDefaultAsync(w => w.EmpCode == Header.EmpCode);
                    
                    if(!string.IsNullOrEmpty(Header.KPIType))
                    {
                        var PlanHeader = new HRKPIPlanHeader();
                        PlanHeader.CriteriaType = EmpStaff.DEPT;
                        PlanHeader.CriteriaName = EmpStaff.Department;
                        PlanHeader.PlanerCode = EmpStaff.EmpCode;
                        PlanHeader.PlanerName = EmpStaff.AllName;
                        PlanHeader.PlanerPosition = EmpStaff.Position;
                        PlanHeader.ExpectedDate = Header.KPIExpectedDate;
                        PlanHeader.Dateline = Header.KPIDeadline;
                        PlanHeader.KPIType = Header.KPIType;
                    }
                    if(!string.IsNullOrEmpty(EmpStaff.SecondLine))
                    {
                        var EmpStaffSecond = await DBV.HR_STAFF_VIEW.FirstOrDefaultAsync(w => w.EmpCode == EmpStaff.SecondLine);
                        if (EmpStaffSecond != null)
                        {
                            Header.AppraiserCode2 = EmpStaffSecond.EmpCode;
                            Header.AppraiserName2 = EmpStaffSecond.AllName;
                            Header.AppraiserPosition2 = EmpStaffSecond.Position;
                        }
                    }
                    var objNumber = new CFNumberRank(objCF.NumberRank, EmpStaff.CompanyCode, Header.AppraiserDate.Value.Year, true);

                    if (objNumber.NextNumberRank == null)
                    {
                        return "NUMBER_RANK_NE";
                    }
                    Header.ApprID = objNumber.NextNumberRank;
                    if (EmpStaff != null)
                    {
                        Header.Position = EmpStaff.Position;
                        Header.EmpName = EmpStaff.AllName;
                        Header.Department = EmpStaff.Department;
                        Header.DateJoin = EmpStaff.StartDate;
                        Header.TotalScore = 0;
                        foreach (var item in lstappr)
                        {
                            var obj = new HREmpAppraisalItem();
                            obj.ApprID = Header.ApprID;
                            obj.Region = item.Region;
                            obj.Remark = item.Remark;
                            obj.Description = item.Description;
                            obj.SecDescription = item.SecDescription;
                            obj.Code = item.Code;
                            obj.RatingID = 0;
                            foreach (var read in _ListRegion.Where(w => w.Code == item.Region).ToList())
                            {
                                obj.RegionDescription = read.Description;
                            }
                            DB.HREmpAppraisalItems.Add(obj);
                        }
                        Header.CreatedBy = User.UserName;
                        Header.CreatedOn = DateTime.Now;
                        DB.HREmpAppraisals.Add(Header);
                    }

                }

                int row = await DB.SaveChangesAsync();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ErrorCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = ErrorCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditAppr(string ApprID, bool IS_ESS = false)
        {
            try
            {
                DB = new HumicaDBContext();
                // var DBD = new HumicaDBContext();
                //Header.EmpCode = HeaderStaff.EmpCode;
                var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == ApprID);
                var _listScore = DB.HREmpAppraisalItems.Where(w => w.ApprID == ApprID).ToList();
                var EvalRating = DB.HRApprRatings.ToList();
                var ListRegion = DB.HRApprRegions.Where(w => w.AppraiselType == objMatch.AppraisalType).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                objMatch.EmpName = Header.EmpName;
                objMatch.Department = Header.Department;
                objMatch.Position = Header.Position;
                objMatch.DateJoin = Header.DateJoin;
                objMatch.AppraisalType = Header.AppraisalType;
                objMatch.AppraiserDate = Header.AppraiserDate;
                foreach (var read in _listScore)
                {

                    DB.HREmpAppraisalItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                //   HumicaDBContext DBM = new HumicaDBContext();
                Header.TotalScore = 0;
                var lstappr = DB.HRApprFactors.ToList();
                foreach (var item in ListFactor)
                {
                    var obj = new HREmpAppraisalItem();
                    obj.ApprID = objMatch.ApprID;
                    obj.Region = item.Region;
                    var check = lstappr.Where(w => w.Code == item.Code).ToList();
                    obj.Remark = item.Remark;
                    if (check.Count > 0)
                    {
                        obj.Description = check.First().Description;
                        obj.SecDescription = check.First().SecDescription;
                        //obj.Weighting = check.First().Weighting;
                    }
                    obj.Code = item.Code;
                    foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                    {
                        obj.RatingID = read.RatingID;
                        obj.Comment = read.Comment;
                        foreach (var read1 in EvalRating.Where(w => w.AppraisalType == objMatch.AppraisalType && w.ID == read.RatingID).ToList())
                        {
                            //obj.Score = (item.Weighting * read.RatingID) / 100.00M;
                            obj.Score = read.RatingID;
                            Header.TotalScore += (decimal)obj.Score;
                        }
                    }
                    foreach (var read in ListRegion.Where(w => w.Code == item.Region).ToList())
                    {
                        obj.RegionDescription = read.Description;
                    }
                    DB.HREmpAppraisalItems.Add(obj);
                }
                //foreach (var item in _listScore)
                //{
                //    foreach (var read in EvalRating.Where(w => w.Region == item.Region && w.ID == item.EvaluateID).ToList())
                //    {
                //        item.Score = read.Score;
                //    }
                //    if (item.Score.HasValue)
                //    {
                //        Header.TotalScore += (int)item.Score;
                //    }
                //    item.EvaluateID = 0;
                //    item.MaxScore = 0;
                //    item.Remark = "";
                //    foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                //    {
                //        item.EvaluateID = read.EvaluateID;
                //        item.Remark = read.Remark;
                //        if (read.Score != null)
                //        {
                //            var check = lstappr.Where(w => w.Code == read.Code).ToList();
                //            if (check.Count > 0)
                //            {
                //                read.Description = check.First().Description;
                //                read.SecDescription = check.First().SecDescription;
                //                read.MaxScore = check.First().MaxScore;
                //            }
                //        }
                //    }

                //    DB.HREmpAppraisalItems.Attach(item);
                //    DB.Entry(item).Property(x => x.Score).IsModified = true;
                //    DB.Entry(item).Property(x => x.EvaluateID).IsModified = true;
                //    DB.Entry(item).Property(x => x.MaxScore).IsModified = true;
                //    DB.Entry(item).Property(x => x.Description).IsModified = true;
                //    DB.Entry(item).Property(x => x.SecDescription).IsModified = true;
                //    DB.Entry(item).Property(x => x.Remark).IsModified = true;
                //}
                var EvaRate = DB.HRAppGrades.ToList();
                var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                foreach (var item in Rate)
                {
                    Header.Result = item.Grade;
                    objMatch.Result = item.Grade;

                }
                if (IS_ESS == true)
                    objMatch.Status = SYDocumentStatus.PROCESSING.ToString();
                objMatch.Rating = RatingScale(objMatch.AppraisalType, objMatch.TotalScore.Value);
                objMatch.TotalScore = Header.TotalScore;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HREmpAppraisals.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.TotalScore).IsModified = true;
                DB.Entry(objMatch).Property(x => x.DateJoin).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Position).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Department).IsModified = true;
                DB.Entry(objMatch).Property(x => x.EmpName).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Result).IsModified = true;
                DB.Entry(objMatch).Property(x => x.AppraiserDate).IsModified = true;
                DB.Entry(objMatch).Property(x => x.AppraisalType).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Rating).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Status).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteAppr(string id)
        {
            try
            {
                //Header = new HREmpEduc();
                var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == id);
                var objScore = DB.HREmpAppraisalItems.Where(w => w.ApprID == id).ToList();

                if (objMatch == null)
                {
                    return "EDCUATION _NE";
                }
                //Header.TranNo = id;
                DB.HREmpAppraisals.Remove(objMatch);
                foreach (var read in objScore)
                {
                    DB.HREmpAppraisalItems.Remove(read);
                }

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.EmpCode.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.EmpCode.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string requestToApprove(string id)
        {
            try
            {
                var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                //#region *****Send to Telegram
                //var Objmatch = DB.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == Header.PONumber);
                //var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocumentType);
                //Humica.Core.SY.SYSendTelegramObject wfo =
                //    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                //wfo.User = User;
                //wfo.ListObjectDictionary = new List<object>();
                //wfo.ListObjectDictionary.Add(Objmatch);
                //HRStaffProfile Staff = getNextApprover(Header.PONumber, "");
                //wfo.ListObjectDictionary.Add(Staff);
                //wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                //#endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.AppraiserCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.AppraiserCode;
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
                log.DocurmentAction = Header.AppraiserCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ApproveDoc(string id)
        {
            try
            {
                var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null) return "DOC_NE";
                Header = objMatch;
                if (objMatch.Status != SYDocumentStatus.PROCESSING.ToString()) return "INV_DOC";

                objMatch.Status = SYDocumentStatus.APPROVED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.ApprID;
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
                log.DocurmentAction = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
       
        public string CancelDoc(string id)
        {
            try
            {
                var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null) return "DOC_NE";
                Header = objMatch;
                //if (objMatch.Status != SYDocumentStatus.PROCESSING.ToString()) return "INV_DOC";

                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.ApprID;
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
                log.DocurmentAction = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string uploadStaff()
        {
            try
            {
                if (ListScore.Count == 0)
                {
                    return "NO_DATA";
                }
                if (ListHeader.Count == 0)
                {
                    return "NO_DATA";
                }
                var date = DateTime.Now;

                foreach (var staffs in ListHeader.ToList())
                {
                    var checkEmp = DB.HREmpAppraisals.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                    var checkEmp2 = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == staffs.EmpCode);
                    DocType = "APP01";
                    if (checkEmp.Count == 0 && (staffs.EmpCode != null || staffs.EmpCode != ""))
                    {
                        Header = new HREmpAppraisal();
                        Header.EmpCode = staffs.EmpCode;
                        if (checkEmp2 != null)
                        {
                            Header.EmpName = checkEmp2.AllName;
                        }
                        var objNumber = new CFNumberRank(DocType);
                        Header.ApprID = objNumber.NextNumberRank;
                        Header.DateJoin = staffs.DateJoin;
                        Header.Position = staffs.Position;
                        Header.AppraisalType = staffs.AppraisalType;
                        Header.Result = staffs.Result;
                        Header.Department = staffs.Department;
                        if (checkEmp2 != null)
                        {
                            Header.DateJoin = checkEmp2.StartDate;
                        }
                        DB.HREmpAppraisals.Add(Header);
                    }

                }

                foreach (var staffs in ListScore.ToList())
                {
                    var checkEmp = DB.HREmpAppraisalItems.Where(w => w.ApprID == Header.ApprID).ToList();
                    //var checkEmp2 = DB.HREmpAppraisals.Where(w => w.EmpCode == staffs.EmpCode).ToList();

                    if (checkEmp.Count == 0)
                    {
                        Score = new HREmpAppraisalItem();
                        Score.ApprID = Header.ApprID;
                        Score.Code = staffs.Code;
                        Score.Description = staffs.Description;
                        Score.SecDescription = staffs.SecDescription;
                        Score.Score = staffs.Score;
                        DB.HREmpAppraisalItems.Add(Score);

                    }
                }

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        #region Appraisl Review
        public string CreateAppraisalReview(string APPID)
        {
            string ID = APPID;
            int i = 0;
            using (var context = new HumicaDBContext())
            {
                try
                {

                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;

                        var objMatch = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == APPID);

                        if (objMatch == null) return "DOC_INV";

                        HeaderProcess.CreatedBy = User.UserName;
                        HeaderProcess.CreatedOn = DateTime.Now;

                        objMatch.Status = SYDocumentStatus.COMPLETED.ToString();
                        context.HREmpAppraisals.Attach(objMatch);
                        context.Entry(objMatch).Property(x => x.Status).IsModified = true;

                        context.HREmpAppProcesses.Add(HeaderProcess);

                        int row = context.SaveChanges();
                        i = HeaderProcess.TranNo;

                        foreach (var item in ListAppProcessItem)
                        {
                            item.AppraisalProcessNo = i;
                            context.HREmpAppProcessItems.Add(item);
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return SYConstant.OK;
                    }
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = ID;
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
                    log.DocurmentAction = ID;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }
        public string ReleaseDocReview(int id)
        {
            try
            {
                var objMatch = DB.HREmpAppProcesses.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null) return "DOC_NE";
                HeaderProcess = objMatch;
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.ApprID;
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
                log.DocurmentAction = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelDocReview(int id)
        {
            try
            {
                var objMatch = DB.HREmpAppProcesses.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null) return "DOC_NE";
                HeaderProcess = objMatch;
                if (objMatch.Status != SYDocumentStatus.RELEASED.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;

                //
                var EmpAppraisal = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == objMatch.DocumentRef);
                if (EmpAppraisal == null) return "DOC_INV";
                EmpAppraisal.Status = SYDocumentStatus.APPROVED.ToString();
                DB.Entry(EmpAppraisal).Property(w => w.Status).IsModified = true;

                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.ApprID;
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
                log.DocurmentAction = Header.ApprID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ClosedTheDoc(int id)
        {
            HumicaDBContext DBX = new HumicaDBContext();
            try
            {

                var objMatch = DB.HREmpAppProcesses.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderProcess = objMatch;

                objMatch.Status = SYDocumentStatus.CLOSED.ToString();

                DBX.HREmpAppProcesses.Attach(objMatch);
                DBX.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderProcess.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = HeaderProcess.TranNo.ToString();
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
                log.DocurmentAction = HeaderProcess.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion

        public decimal RatingScale(string DocType, decimal TotalScore)
        {
            decimal RatingScale = 0;
            HRApprType DocTYpe = new HumicaDBContext().HRApprTypes.FirstOrDefault(w => w.Code == DocType);
            if (DocTYpe != null)
                RatingScale = DocTYpe.RatingScale.Value;
            return RatingScale * TotalScore;
        }
        public async Task<decimal> IncreaseSalaryPercentage(string EmpCode, string Level, decimal Salary, int Rate, int InYear)
        {
            List<HRAppPerformanceRate> ListAppraisalRate = await new HumicaDBContext().HRAppPerformanceRates.ToListAsync();
            List<HRAppCompareRatio> ListCompare = await new HumicaDBContext().HRAppCompareRatios.ToListAsync();
            decimal IncreaseSalaryPercentage = 0;
            var AppraisalRate = ListAppraisalRate.FirstOrDefault(w => w.Result == Rate);
            if(AppraisalRate!= null)
            {
                var MidPoint = await new HumicaDBContext().HRAppLevelMidPoints.FirstOrDefaultAsync(w => w.JobLevel == Level);
                if (MidPoint != null)
                {
                    decimal Compa_ration = Math.Round(Salary / MidPoint.JobLevelMidPoint.Value, 2);
                    var ObjCompare = ListCompare.FirstOrDefault(w => w.FromRatio <= Compa_ration && w.ToRatio >= Compa_ration);
                    var ListMatrix = await IncreasePercentage(ListAppraisalRate, ListCompare, InYear);
                    if (ObjCompare != null)
                    {
                        if (ListMatrix.Count() > 0)
                        {
                            var Matrix = ListMatrix.FirstOrDefault(w => w.PerformanceRating == AppraisalRate.ID.ToString() && w.CompaRatio == ObjCompare.ID.ToString());
                            if (Matrix != null)
                            {
                                IncreaseSalaryPercentage = Matrix.Percentage;
                            }
                        }
                    }
                }
            }
            return IncreaseSalaryPercentage;
        }
        public async Task<decimal> IncreaseSalary(string EmpCode, string Level, decimal Salary, int Rate, int InYear)
        {
            List<HRAppPerformanceRate> ListAppraisalRate = await new HumicaDBContext().HRAppPerformanceRates.ToListAsync();
            List< HRAppCompareRatio > ListCompare = await new HumicaDBContext().HRAppCompareRatios.ToListAsync();
            decimal IncreaseSalary = 0;
            var AppraisalRate = ListAppraisalRate.FirstOrDefault(w => w.Result == Rate);
            if (AppraisalRate != null)
            {
                var MidPoint = await new HumicaDBContext().HRAppLevelMidPoints.FirstOrDefaultAsync(w => w.JobLevel == Level);
                if (MidPoint != null)
                {
                    decimal Compa_ration = Math.Round(Salary / MidPoint.JobLevelMidPoint.Value, 2);
                    var ObjCompare = ListCompare.FirstOrDefault(w => w.FromRatio <= Compa_ration && w.ToRatio >= Compa_ration);
                    var ListMatrix = await IncreasePercentage(ListAppraisalRate, ListCompare, InYear);
                    if (ObjCompare != null)
                    {
                        if (ListMatrix.Count() > 0)
                        {
                            var Matrix = ListMatrix.FirstOrDefault(w => w.PerformanceRating == AppraisalRate.ID.ToString() && w.CompaRatio == ObjCompare.ID.ToString());
                            if (Matrix != null)
                            {
                                IncreaseSalary = Salary * Matrix.Percentage;
                            }
                        }
                    }
                }
            }
            return IncreaseSalary;
        }

        public async Task<List<ClsMatrix>> IncreasePercentage(List<HRAppPerformanceRate> ListAppraisalRate,
           List<HRAppCompareRatio> ListCompare, int InYear)
        {
            List<ClsMatrix> ListMatrix = new List<ClsMatrix>();
            var Budget = await new HumicaDBContext().HRAppSalaryBudgets.FirstOrDefaultAsync(w => w.InYear == InYear);
            foreach (var Rate in ListAppraisalRate)
            {
                foreach (var Ratio in ListCompare)
                {
                    decimal Percentage = 0;
                    if (Budget != null)
                        Percentage = Budget.Budget;
                    decimal _Percentage = Rate.Rate * Ratio.Factor * Percentage;
                    ClsMatrix obj = new ClsMatrix();
                    obj.PerformanceRating = Rate.ID.ToString();
                    obj.CompaRatio = Ratio.ID.ToString();
                    obj.Percentage = _Percentage;
                    ListMatrix.Add(obj);
                }
            }
            return ListMatrix;
        }
    }
    public class ClsMatrix
    {
        public string PerformanceRating { get; set; }
        public string CompaRatio { get; set; }
        public decimal Percentage { get; set; }
    }
}