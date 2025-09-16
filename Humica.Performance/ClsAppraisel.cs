using Humica.Condition;
using Humica.Core;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Performance
{
    public class ClsAppraisel : IClsAppraisel
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string DocType { get; set; }

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
        public List<HRKPIEvalIndicator> ListHRKPIndicator { get; set; }
        public List<HREmpAppProcessItem> ListAppProcessItem { get; set; }
        public string Rating { get; set; }
        public int RateMin { get; set; }
        public int RateMax { get; set; }

        public ClsAppraisel()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        //public string uploadStaff()
        //{
        //    try
        //    {
        //        if (ListScore.Count == 0)
        //        {
        //            return "NO_DATA";
        //        }
        //        if (ListHeader.Count == 0)
        //        {
        //            return "NO_DATA";
        //        }
        //        var date = DateTime.Now;

        //        foreach (var staffs in ListHeader.ToList())
        //        {
        //            var checkEmp = DB.HREmpAppraisals.Where(w => w.EmpCode == staffs.EmpCode).ToList();
        //            var checkEmp2 = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == staffs.EmpCode);
        //            DocType = "APP01";
        //            if (checkEmp.Count == 0 && (staffs.EmpCode != null || staffs.EmpCode != ""))
        //            {
        //                Header = new HREmpAppraisal();
        //                Header.EmpCode = staffs.EmpCode;
        //                if (checkEmp2 != null)
        //                {
        //                    Header.EmpName = checkEmp2.AllName;
        //                }
        //                var objNumber = new CFNumberRank(DocType);
        //                Header.ApprID = objNumber.NextNumberRank;
        //                Header.DateJoin = staffs.DateJoin;
        //                Header.Position = staffs.Position;
        //                Header.AppraisalType = staffs.AppraisalType;
        //                Header.Result = staffs.Result;
        //                Header.Department = staffs.Department;
        //                if (checkEmp2 != null)
        //                {
        //                    Header.DateJoin = checkEmp2.StartDate;
        //                }
        //                DB.HREmpAppraisals.Add(Header);
        //            }

        //        }

        //        foreach (var staffs in ListScore.ToList())
        //        {
        //            var checkEmp = DB.HREmpAppraisalItems.Where(w => w.ApprID == Header.ApprID).ToList();
        //            //var checkEmp2 = DB.HREmpAppraisals.Where(w => w.EmpCode == staffs.EmpCode).ToList();

        //            if (checkEmp.Count == 0)
        //            {
        //                Score = new HREmpAppraisalItem();
        //                Score.ApprID = Header.ApprID;
        //                Score.Code = staffs.Code;
        //                Score.Description = staffs.Description;
        //                Score.SecDescription = staffs.SecDescription;
        //                Score.Score = staffs.Score;
        //                DB.HREmpAppraisalItems.Add(Score);

        //            }
        //        }

        //        DB.SaveChanges();
        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Header.EmpCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Header.EmpCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Header.EmpCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}

        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public void NewAppraisalSummary(HREmpAppraisalSummary D, HRApprRegion S, string AppraisalNo, string AppraisalType)
        {
            D.AppraisalNo = AppraisalNo;
            D.TaskID = S.Code;
            D.AppraisalType = AppraisalType;
            D.EvaluationCriteria = S.Description;
            if (!D.Weight.HasValue) D.Weight = 0;
            if (S.IsKPI != true)
            {
                D.Weight = S.Rating;
                D.Score = 0;
            }
            if (D.Weight > 1) D.Weight = D.Weight / 100.00M;
            D.InOrder = S.InOrder;
            D.IsKPI = S.IsKPI;
        }
        public string ValidateAppraisal(string EmpCode, DateTime FromDate, DateTime ToDate)
        {
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            string Close = SYDocumentStatus.CLOSED.ToString();
            var EmpAppr = unitOfWork.Set<HREmpAppraisal>()
                .FirstOrDefault(w => w.EmpCode == EmpCode &&
                    ((FromDate >= w.PeriodFrom && FromDate <= w.PeriodTo) ||
                    (ToDate >= w.PeriodFrom && ToDate <= w.PeriodTo) ||
                    (FromDate <= w.PeriodFrom && ToDate >= w.PeriodTo))
                    && (w.Status != Cancel && w.Status != Close));

            if (EmpAppr != null)
            {
                ErrorMessage = EmpAppr.ApprID;
                return "DOC_INPROGRESS";
            }

            return SYConstant.OK;
        }

        public List<HREmpAppraisal> OnIndexLoading(bool IsESS = false)
        {
            if (Filter == null)
            {
                Filter = new FTFilterEmployee();
                Filter.INYear = DateTime.Now.Year;
                Filter.Status = SYDocumentStatus.PENDING.ToString();
            }
            if (IsESS)
            {
                string Close = SYDocumentStatus.CLOSED.ToString();
                string userName = User.UserName;
                ListHeader = unitOfWork.Set<HREmpAppraisal>()
                    .Where(x => (x.DirectedByCode == userName ||
                                 x.AppraiserCode == userName ||
                                 x.AppraiserCode2 == userName ||
                                 x.EmpCode == userName)
                                 && x.InYear == Filter.INYear
                                 && x.Status != Close).ToList();
            }
            else
            {
                if(Filter.Status != "" && Filter.Status != null)
                    this.ListHeader = unitOfWork.Set<HREmpAppraisal>().Where(w => w.InYear == Filter.INYear && w.Status == Filter.Status).ToList();
                else
                    this.ListHeader = unitOfWork.Set<HREmpAppraisal>().Where(w => w.InYear == Filter.INYear).ToList();
            }
            ListHeader = ListHeader.OrderByDescending(w => w.AppraiserDate).ToList();
            return this.ListHeader;
        }
        public void OnIndexLoadingPending()
        {
            string pending = SYDocumentStatus.PENDING.ToString();
            string userName = User.UserName;
            DateTime DateNow = DateTime.Now.Date;
            ListAppraisaPending = unitOfWork.Set<HREmpAppraisal>().Where(x => x.AppraiserNext == userName && x.ReStatus == pending).ToList();
            ListAppraisaPending = ListAppraisaPending.Where(w => w.AppraiserStart.Date <= DateNow.Date).ToList();
            ListAppraisaPending = ListAppraisaPending.OrderByDescending(x => x.AppraiserDate).ToList();
        }
        public void OnIndexLoadingTeam()
        {
            DateTime InDate = DateTime.Now;
            Header = new HREmpAppraisal();
            Header.AppraiserDate = InDate;
            Header.AppraiserDeadline = InDate;
            Header.AppraiserDeadline2 = InDate;
            Header.PeriodFrom = new DateTime(InDate.Year, 1, 1);
            Header.PeriodTo = new DateTime(InDate.Year, 12, 31);
            Header.Status = SYDocumentStatus.OPEN.ToString();
            Header.InYear = InDate.Year;
            Header.KPIDeadline = Header.PeriodFrom.Value.AddDays(15);
            Filter = new FTFilterEmployee();
            ListEmpStaff = new List<_ListStaff>();
            //var staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == User.UserName);
            //if (staff != null)
            //{
            //    Header.DirectedByCode = staff.EmpCode;
            //}
            //else
            //    Header.DirectedByCode = User.UserName;
        }
        public void LoadData(FTFilterEmployee Filter1, List<HRBranch> _lstBranch)
        {
            OnLoad();
            DateTime date = new DateTime(1900, 1, 1);
            ListEmpStaff = new List<_ListStaff>();
            var positions = unitOfWork.Set<HRPosition>().ToList();
            var Department = unitOfWork.Set<HRDepartment>().ToList();
            var Section = unitOfWork.Set<HRSection>().ToList();
            var _staff = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A"
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
                           ).ToList();

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
                EmpStaff.Appraisal = item.APPAppraisal;
                EmpStaff.Appraisal2 = item.APPAppraisal2;
                ListEmpStaff.Add(EmpStaff);
            }
        }
        public void OnCreatingLoading(params object[] keys)
        {
            string AppType = (string)keys[0];
            string EmpCode = (string)keys[1];
            string HOD = (string)keys[2];
            string Appraiser = (string)keys[3];
            string Appraiser2 = (string)keys[4];
            DateTime InDate = DateTime.Now;
            Header = new HREmpAppraisal();
            Header.AppraiserDate = InDate;
            Header.AppraiserDeadline = InDate;
            Header.AppraiserDeadline2 = InDate;
            Header.PeriodFrom = new DateTime(InDate.Year, 1, 1);
            Header.PeriodTo = new DateTime(InDate.Year, 12, 31);
            Header.Status = SYDocumentStatus.OPEN.ToString();
            Header.InYear = InDate.Year;
            Header.KPIDeadline = Header.PeriodFrom.Value.AddDays(15);
            ListFactor = new List<HRApprFactor>();
            ListRegion = new List<HRApprRegion>();
            ListApprRating = new List<HRApprRating>();
            ListApprResult = new List<HRAppGrade>();
            Header.AppraisalType = AppType;
            var Staff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Staff != null)
            {
                Header.EmpCode = EmpCode;
                Header.EmpName = Staff.AllName;
                Header.Department = Staff.Department;
                Header.Position = Staff.Position;
                Header.DateJoin = Staff.StartDate;
                var EmpHOD = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HOD);
                if (EmpHOD != null)
                {
                    Header.DirectedByCode = EmpHOD.EmpCode;
                    Header.DirectedByName = EmpHOD.AllName;
                }
                var Line = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Appraiser);
                if (Line != null)
                {
                    Header.AppraiserCode = Line.EmpCode;
                    Header.AppraiserName = Line.AllName;
                }
                var SecondLine = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Appraiser2);
                if (SecondLine != null)
                {
                    Header.AppraiserCode2 = SecondLine.EmpCode;
                    Header.AppraiserName2 = SecondLine.AllName;
                }
            }
            var objAppType = unitOfWork.Set<HRApprType>().FirstOrDefault(w => w.Code == AppType);
            Rating = "";
            RateMin = 0;
            RateMax = 0;
            if (objAppType != null)
            {
                ListApprResult = unitOfWork.Set<HRAppGrade>().ToList();
                LoadDataFactor(objAppType.Code);
                //var quiz = unitOfWork.Set<HRApprFactor>().ToList();
                //ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == objAppType.Code).ToList();
                //ListApprRating = unitOfWork.Set<HRApprRating>().Where(w => w.AppraisalType == objAppType.Code).ToList();
                //ListFactor = quiz.Where(w => ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                Rating = "";
                if (ListApprRating.Count > 0)
                {
                    RateMin = ListApprRating.Min(x => x.Rating);
                    RateMax = ListApprRating.Max(x => x.Rating);
                }
                foreach (var read in ListApprRating.OrderBy(w => w.Rating))
                {
                    Rating += read.Rating + ",";
                }
                if (Rating != "")
                    Rating = Rating.Substring(0, Rating.Length - 1);
            }
            ListScore = new List<HREmpAppraisalItem>();
        }
        public string Create()
        {
            try
            {
                var mss = ValidateAppraisal(Header.EmpCode, Header.PeriodFrom.Value, Header.PeriodTo.Value);
                if (mss != SYConstant.OK)
                {
                    return mss;
                }
                var EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                var ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                var EvalRating = unitOfWork.Set<HRApprRating>().ToList();
                var lstappr = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
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
                ClsPerformance performance = new ClsPerformance();
                if (EmpStaff != null)
                {
                    Header.Position = EmpStaff.Position;
                    Header.EmpName = EmpStaff.AllName;
                    Header.Department = EmpStaff.Department;
                    Header.Branch = EmpStaff.Branch;
                    Header.TotalScore = 0;
                    foreach (var item in ListFactor)
                    {
                        var obj = new HREmpAppraisalItem();
                        performance.NewAppraisalItem(obj, ListRegion, item, Header.ApprID);
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
                        unitOfWork.Add(obj);
                    }
                    foreach (var item in ListRegion)
                    {
                        var AppSum = new HREmpAppraisalSummary();
                        performance.NewAppraisalSummary(AppSum, item, Header.ApprID, Header.AppraisalType);
                        unitOfWork.Add(AppSum);
                    }
                    var EvaRate = unitOfWork.Set<HRAppGrade>().ToList();
                    var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                    //foreach (var item in Rate)
                    //{
                    //    Header.Result = item.Grade;
                    //}
                    Header.Status = SYDocumentStatus.OPEN.ToString();
                    Header.ReStatus = SYDocumentStatus.OPEN.ToString();
                    Header.Rating = RatingScale(Header.AppraisalType, Header.TotalScore.Value);
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    HR_STAFF_VIEW LineManager = new HR_STAFF_VIEW();
                    if (string.IsNullOrEmpty(Header.AppraiserCode))
                    {
                        return "NO_LINE_MN";
                    }
                    else
                    {
                        LineManager = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.AppraiserCode);
                        if (LineManager != null)
                        {
                            Header.AppraiserName = LineManager.AllName;
                            Header.AppraiserPosition = LineManager.Position;
                            Header.AppraiserNext = Header.AppraiserCode;
                        }
                    }
                    if (!string.IsNullOrEmpty(Header.AppraiserCode2))
                    {
                        var EmpStaffSecond = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.AppraiserCode2);
                        if (EmpStaffSecond != null)
                        {
                            Header.AppraiserName2 = EmpStaffSecond.AllName;
                            Header.AppraiserPosition2 = EmpStaffSecond.Position;
                        }
                    }
                    if (!string.IsNullOrEmpty(Header.KPIType))
                    {
                        Header.KPIStatus = SYDocumentStatus.OPEN.ToString();
                    }
                    unitOfWork.Add(Header);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string CreateMulti(string EmpCode, HREmpAppraisal Obj)
        {
            ScreenId = "HRA0000001";
            string ErrorCode = "";
            try
            {
                if (string.IsNullOrEmpty(Obj.AppraisalType))
                {
                    return "EE_DOCTYPE";
                }

                var _ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Obj.AppraisalType).ToList();
                var quiz = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Obj.AppraisalType).ToList();
                var lstappr = quiz.Where(w => _ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
                var ListEmployee = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList();
                if (lstappr.Count() == 0)
                {
                    return "INVALID_FACTOR";
                }


                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                string[] Emp = EmpCode.Split(';');
                ClsFilterStaff FTSTaff = new ClsFilterStaff();
                foreach (var Code in Emp)
                {
                    Header = new HREmpAppraisal();
                    Header.AppraisalType = Obj.AppraisalType;
                    Header.AppraiserDate = Obj.AppraiserDate;
                    Header.InYear = Obj.InYear;
                    Header.PeriodFrom = Obj.PeriodFrom;
                    Header.PeriodTo = Obj.PeriodTo;
                    Header.KPIType = Obj.KPIType;
                    Header.AppraiserDeadline = Obj.AppraiserDeadline;
                    Header.AppraiserDeadline2 = Obj.AppraiserDeadline2;
                    Header.KPIExpectedDate = Obj.KPIExpectedDate;
                    Header.KPIDeadline = Obj.KPIDeadline;
                    Header.Status = SYDocumentStatus.PENDING.ToString();
                    Header.ReStatus = SYDocumentStatus.PENDING.ToString();
                    Header.KPICategory = "DPM";
                    ErrorCode = Code;
                    var mss = ValidateAppraisal(Header.EmpCode, Header.PeriodFrom.Value, Header.PeriodTo.Value);
                    if (mss != SYConstant.OK)
                    {
                        return mss;
                    }
                    if (Code.Trim() == "") continue;
                    Header.EmpCode = Code;
                    var EmpStaff = ListEmployee.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (string.IsNullOrEmpty(EmpStaff.APPAppraisal))
                    {
                        return "NO_APPRAISAL";
                    }
                    else
                    {
                        var Appraisal = ListEmployee.FirstOrDefault(w => w.EmpCode == EmpStaff.APPAppraisal);
                        if (Appraisal != null)
                        {
                            Header.AppraiserCode = Appraisal.EmpCode;
                            Header.AppraiserName = Appraisal.AllName;
                            Header.AppraiserPosition = FTSTaff.Get_Positon(Appraisal.JobCode);
                            Header.AppraiserNext = Header.AppraiserCode;
                        }
                    }
                    var Appraisal2 = ListEmployee.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (!string.IsNullOrEmpty(Appraisal2.APPAppraisal2))
                    {
                        var EmpAppraisal2 = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Appraisal2.APPAppraisal2);
                        if (EmpAppraisal2 != null)
                        {
                            Header.AppraiserCode2 = EmpAppraisal2.EmpCode;
                            Header.AppraiserName2 = EmpAppraisal2.AllName;
                            Header.AppraiserPosition2 = EmpAppraisal2.Position;
                        }
                    }
                    //HOD
                    var EmpHOD = ListEmployee.FirstOrDefault(w => w.EmpCode == EmpStaff.HODCode);
                    if (EmpHOD == null)
                    {
                        return "IVALID_HOD";
                    }
                    Header.DirectedByCode = EmpHOD.EmpCode;
                    Header.DirectedByName = EmpHOD.AllName;

                    var objNumber = new CFNumberRank(objCF.NumberRank, EmpStaff.CompanyCode, Header.AppraiserDate.Value.Year, true);

                    if (objNumber.NextNumberRank == null)
                    {
                        return "NUMBER_RANK_NE";
                    }
                    Header.ApprID = objNumber.NextNumberRank;
                    ClsPerformance performance = new ClsPerformance();
                    if (EmpStaff != null)
                    {
                        Header.Position = FTSTaff.Get_Positon(EmpStaff.JobCode);
                        Header.EmpName = EmpStaff.AllName;
                        Header.Department = FTSTaff.Get_Department(EmpStaff.DEPT);
                        Header.Branch = EmpStaff.Branch;
                        Header.DateJoin = EmpStaff.StartDate;
                        Header.TotalScore = 0;
                        foreach (var item in lstappr)
                        {
                            var obj = new HREmpAppraisalItem();
                            performance.NewAppraisalItem(obj, _ListRegion, item, Header.ApprID);
                            unitOfWork.Add(obj);
                        }
                        if (!string.IsNullOrEmpty(Header.KPIType))
                        {
                            Header.KPIStatus = SYDocumentStatus.OPEN.ToString();
                        }

                        Header.CreatedBy = User.UserName;
                        Header.CreatedOn = DateTime.Now;
                        unitOfWork.Add(Header);
                    }
                    foreach (var item in _ListRegion)
                    {
                        var AppSum = new HREmpAppraisalSummary();
                        performance.NewAppraisalSummary(AppSum, item, Header.ApprID, Header.AppraisalType);
                        unitOfWork.Add(AppSum);
                    }
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ErrorCode, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ErrorCode, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public virtual string OnEditESSLoading(params object[] keys)
        {
            string ApprID = (string)keys[0];
            this.Header = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == ApprID);
            string cancelled = SYDocumentStatus.CANCELLED.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            if (Header == null)
            {
                return "DOC_INV";
            }
            if (Header.AppraiserCode != User.UserName &&
                Header.AppraiserCode2 != User.UserName)
            {
                return "EDIT_CAN_N";
            }
            if (!string.IsNullOrEmpty(Header.KPIType) && Header.KPIStatus != SYDocumentStatus.COMPLETED.ToString())
            {
                return "KPI_HAS_NOT_COMPLETED";
            }
            if (!Header.ApprovedStep.HasValue)
                Header.ApprovedStep = 1;
            if (Header.Status == cancelled || Header.Status == approved)
            {
                return "DOC_INV";
            }
            if (this.Header != null)
            {
                ListApprResult = new List<HRAppGrade>();
                var quiz = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                ListApprRating = unitOfWork.Set<HRApprRating>().Where(w => w.AppraisalType == Header.AppraisalType).ToList();
                ListFactor = quiz.Where(w => ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                Rating = "";
                RateMin = 0;
                RateMax = 0;
                if (ListApprRating.Count > 0)
                {
                    RateMin = ListApprRating.Min(x => x.Rating);
                    RateMax = ListApprRating.Max(x => x.Rating);
                }
                foreach (var read in ListApprRating)
                {
                    Rating += read.Rating + ",";
                }
                if (Rating != "")
                    Rating = Rating.Substring(0, Rating.Length - 1);

                ListScore = unitOfWork.Set<HREmpAppraisalItem>().Where(w => w.ApprID == ApprID).ToList();
            }
            return SYConstant.OK;
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string ApprID = (string)keys[0];
            this.Header = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == ApprID);
            ListHRKPIndicator = new List<HRKPIEvalIndicator>();
            if (this.Header != null)
            {
                ListApprResult = new List<HRAppGrade>();
                var quiz = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Header.AppraisalType).ToList();
                ListApprRating = unitOfWork.Set<HRApprRating>().Where(w => w.AppraisalType == Header.AppraisalType).ToList();
                ListFactor = quiz.Where(w => ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                Rating = "";
                RateMin = 0;
                RateMax = 0;
                if (ListApprRating.Count > 0)
                {
                    RateMin = ListApprRating.Min(x => x.Rating);
                    RateMax = ListApprRating.Max(x => x.Rating);
                }
                foreach (var read in ListApprRating)
                {
                    Rating += read.Rating + ",";
                }
                if (Rating != "")
                    Rating = Rating.Substring(0, Rating.Length - 1);

                ListScore = unitOfWork.Set<HREmpAppraisalItem>().Where(w => w.ApprID == ApprID).ToList();
                var KPIEvaluation = unitOfWork.Set<HRKPIEvaluation>().FirstOrDefault(w => w.DocRef == Header.ApprID);
                if (KPIEvaluation != null)
                {
                    ListHRKPIndicator = unitOfWork.Set<HRKPIEvalIndicator>().Where(w => w.KPIEvaCode == KPIEvaluation.KPIEvaCode).ToList();
                }
            }
        }
        public string Update(string ID, bool IS_ESS = false)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == ID);
                var _listScore = unitOfWork.Set<HREmpAppraisalItem>().Where(w => w.ApprID == ID).ToList();
                var _listAppSummary = unitOfWork.Set<HREmpAppraisalSummary>().Where(w => w.AppraisalNo == ID).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                var EvalRating = unitOfWork.Set<HRApprRating>().Where(w => w.AppraisalType == objMatch.AppraisalType).ToList();
                var ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == objMatch.AppraisalType).ToList();
                objMatch.EmpName = Header.EmpName;
                objMatch.Department = Header.Department;
                objMatch.Position = Header.Position;
                objMatch.DateJoin = Header.DateJoin;
                objMatch.AppraisalType = Header.AppraisalType;
                objMatch.AppraiserDate = Header.AppraiserDate;
                Header.TotalScore = 0;
                int Step = 1;
                if (objMatch.ApprovedStep.HasValue) Step = objMatch.ApprovedStep.Value;
                var lstappr = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == objMatch.AppraisalType).ToList();
                foreach (var item in ListFactor)
                {
                    foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                    {
                        foreach (var read1 in EvalRating.Where(w => w.AppraisalType == objMatch.AppraisalType).ToList())
                        {
                            var obj = _listScore.Where(w => w.Region == item.Region && w.Code == read.Code).FirstOrDefault();
                            if (obj != null)
                            {
                                if (!obj.ScoreAppraiser.HasValue) obj.ScoreAppraiser = 0;
                                if (!obj.ScoreAppraiser2.HasValue) obj.ScoreAppraiser2 = 0;
                                obj.Score = (obj.ScoreAppraiser.Value + obj.ScoreAppraiser2.Value) / 2;
                                obj.RatingID = read.RatingID;
                                if (ListRegion.Where(w => w.Code == obj.Region).Count() > 0)
                                {
                                    var Region = ListRegion.FirstOrDefault(w => w.Code == obj.Region);
                                    if (Region != null)
                                    {
                                        obj.RegionDescription = Region.Description;
                                    }
                                }
                                obj.Remark = item.Remark;
                                obj.Comment = read.Comment;
                                unitOfWork.Update(obj);
                            }
                        }
                    }
                }
                ClsPerformance performance = new ClsPerformance();
                foreach (var item in ListRegion)
                {
                    var AppSum = _listAppSummary.FirstOrDefault(w => w.TaskID == item.Code);
                    bool isAdd = false;
                    if (AppSum == null)
                    {
                        isAdd = true;
                        AppSum = new HREmpAppraisalSummary();
                    }
                    performance.NewAppraisalSummary(AppSum, item, objMatch.ApprID, objMatch.AppraisalType);
                    if (!AppSum.SubTotal.HasValue) AppSum.SubTotal = 0;
                    if (item.IsKPI != true)
                    {
                        AppSum.Score = _listScore.Where(w => w.Region == item.Code && w.Score.HasValue).Sum(x => x.Score.Value);
                        if (EvalRating.Count > 0 && AppSum.Score > 0)
                        {
                            int count = _listScore.Where(w => w.Region == item.Code).Count();
                            decimal TotalMaxScore = EvalRating.Max(w => w.Rating) * count;
                            AppSum.SubTotal = ((AppSum.Score * AppSum.Weight) / TotalMaxScore) * 100;
                        }
                    }
                    if (isAdd)
                    {
                        unitOfWork.Add(AppSum);
                    }
                    else
                    {
                        unitOfWork.Update(AppSum);
                    }
                }
                if (IS_ESS == false)
                {
                    objMatch.KPICategory = Header.KPICategory;
                    objMatch.DirectedByCode = Header.DirectedByCode;
                    objMatch.DirectedByName = Header.DirectedByName;
                    HR_STAFF_VIEW LineManager = new HR_STAFF_VIEW();
                    if (string.IsNullOrEmpty(Header.AppraiserCode))
                    {
                        return "NO_LINE_MN";
                    }
                    else
                    {
                        LineManager = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.AppraiserCode);
                        if (LineManager != null)
                        {
                            objMatch.AppraiserCode = LineManager.EmpCode;
                            objMatch.AppraiserName = LineManager.AllName;
                            objMatch.AppraiserPosition = LineManager.Position;
                            objMatch.AppraiserNext = Header.AppraiserCode;
                        }
                    }
                    if (!string.IsNullOrEmpty(Header.AppraiserCode2))
                    {
                        var EmpStaffSecond = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == Header.AppraiserCode2);
                        if (EmpStaffSecond != null)
                        {
                            objMatch.AppraiserCode2 = EmpStaffSecond.EmpCode;
                            objMatch.AppraiserName2 = EmpStaffSecond.AllName;
                            objMatch.AppraiserPosition2 = EmpStaffSecond.Position;
                        }
                    }
                }
                if (IS_ESS == true) objMatch.AppraiserNext = null;
                objMatch.TotalScore = _listAppSummary.Where(w => w.IsKPI != true).Sum(x => x.SubTotal);
                objMatch.Rating = RatingScale(objMatch.AppraisalType, objMatch.TotalScore.Value);
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                objMatch.ApprovedStep = Step;


                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                Calculate(objMatch);
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLogs(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                var objScore = unitOfWork.Set<HREmpAppraisalItem>().Where(w => w.ApprID == id).ToList();
                var objScoreSummary = unitOfWork.Set<HREmpAppraisalSummary>().Where(w => w.AppraisalNo == id).ToList();

                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    if (objMatch.Status == SYDocumentStatus.PENDING.ToString() && !string.IsNullOrEmpty(objMatch.KPIType) && objMatch.KPIStatus != SYDocumentStatus.OPEN.ToString())
                    {
                        return "DELETE_CAN_N";
                    }
                }
                if (!string.IsNullOrEmpty(objMatch.KPIType) && objMatch.KPIStatus != SYDocumentStatus.OPEN.ToString())
                {
                    return "DELETE_CAN_N";
                }
                unitOfWork.Delete(objMatch);
                foreach (var read in objScore)
                {
                    unitOfWork.Delete(read);
                }
                foreach (var read in objScoreSummary)
                {
                    unitOfWork.Delete(read);
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string RequestToApprove(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;
                if (objMatch.ReStatus != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                objMatch.ReStatus = SYDocumentStatus.PENDING.ToString();
                #region Notifican on Mobile
                var FirebaseID = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.IsLock == true && w.UserName == objMatch.EmpCode);
                if (FirebaseID != null)
                {
                    if (!string.IsNullOrEmpty(FirebaseID.FirebaseID) && FirebaseID.FirebaseID.Length > 10)
                    {
                        string Desc = "Appraisal form has been assigned to you  by HR Department."; //+ Staff.Title + " " + objMatch.EmpName;
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(FirebaseID.FirebaseID);
                        //clientToken.Add("ez3uAH-qDEHRgMGhlf18cr:APA91bHliAC1icIxXFMNoxMCMG18AG_5I9OVCbpvkrtTy7wY2hq4Axa1p5DFYGeUlV9u3vunPb3ANkrNkwwQNJZmIYsDLA-4lEPblIDoT60Zv9F9kFMZvKcddG3Gi5F7yMJbNds963Aw");
                        var dd = Noti.SendNotification(clientToken, " Staff Appraisal", Desc);
                    }
                }
                #endregion
                if (!string.IsNullOrEmpty(objMatch.KPIType))
                {
                    //HRStaffProfile EmpStaff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    //var PlanHeader = new HRKPIAssignHeader();
                    //PlanHeader = GetEmployeeKPI(objMatch, EmpStaff.DEPT);
                    DateTime DateNow = DateTime.Now;
                    if (objMatch.KPIExpectedDate > DateNow)
                    {
                        objMatch.KPIStatus = SYDocumentStatus.OPEN.ToString();
                    }
                    //unitOfWork.Add(PlanHeader);
                }

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string CancelDoc(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.ApprID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.ApprID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, Header.ApprID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string CloseDoc(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (!string.IsNullOrEmpty(objMatch.KPIReference))
                {
                    var objKPI = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == objMatch.KPIReference);
                    if (objKPI != null)
                    {
                        if (objKPI.AssignedBy == SYDocumentASSIGN.Individual.ToString())
                        {
                            objKPI.Status = SYDocumentStatus.CLOSED.ToString();
                            objKPI.ReStatus = SYDocumentStatus.CLOSED.ToString();
                        }
                        else
                        {
                            var listMember = unitOfWork.Set<HRKPIAssignMember>().Where(w => w.AssignCode == objMatch.KPIReference);
                            var objMem = listMember.Where(w => w.DocRef == objMatch.ApprID).ToList();
                            foreach (var item in objMem)
                            {
                                unitOfWork.Delete(item);
                            }
                            objKPI.Member = listMember.Where(w => w.DocRef != objMatch.ApprID).ToList().Count();
                        }
                        unitOfWork.Update(objKPI);
                    }
                }
                Header = objMatch;

                objMatch.Status = SYDocumentStatus.CLOSED.ToString();
                objMatch.ReStatus = SYDocumentStatus.CLOSED.ToString();
                objMatch.KPIStatus = SYDocumentStatus.CLOSED.ToString();

                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLogs(ScreenId, User.UserName, Header.ApprID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string ApproveDoc(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                Header = objMatch;
                if (objMatch.ReStatus != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                if (objMatch.AppraiserCode == User.UserName)
                {
                    if (objMatch.ApprovedStep != 1)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }
                if (!string.IsNullOrEmpty(objMatch.AppraiserCode2) && objMatch.ApprovedStep == 1)
                {
                    objMatch.ApprovedStep = 2;
                    objMatch.AppraiserNext = objMatch.AppraiserCode2;
                    #region Notifican on Mobile
                    var FirebaseID = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.IsLock == true && w.UserName == objMatch.AppraiserCode2);
                    var EMP = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    if (FirebaseID != null)
                    {
                        if (!string.IsNullOrEmpty(FirebaseID.FirebaseID))
                        {
                            string Desc = "Staff appraisal form is waiting you to appraisal.";//"ទម្រង់វាយតម្លៃរបស់បុក្គលិកកំពុងត្រូវបានរងចាំអ្នកវាយតម្លៃ។​"​;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(FirebaseID.FirebaseID);
                            //clientToken.Add("ez3uAH-qDEHRgMGhlf18cr:APA91bHliAC1icIxXFMNoxMCMG18AG_5I9OVCbpvkrtTy7wY2hq4Axa1p5DFYGeUlV9u3vunPb3ANkrNkwwQNJZmIYsDLA-4lEPblIDoT60Zv9F9kFMZvKcddG3Gi5F7yMJbNds963Aw");
                            var dd = Noti.SendNotification(clientToken, " Staff Appraisal", Desc);
                        }
                    }
                    #endregion
                }
                else
                {
                    objMatch.Status = SYDocumentStatus.APPROVED.ToString();
                    objMatch.ReStatus = SYDocumentStatus.APPROVED.ToString();
                    #region Notifican on Mobile
                    var FirebaseID = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.IsLock == true && w.UserName == objMatch.AppraiserCode2);
                    var StaffFirebaseID = unitOfWork.Set<TokenResource>().FirstOrDefault(w => w.IsLock == true && User.UserName == objMatch.EmpCode);
                    //alert to staff
                    if (StaffFirebaseID != null)
                    {
                        if (!string.IsNullOrEmpty(StaffFirebaseID.FirebaseID))
                        {
                            string Desc = "You have been appraisal already.";//  "អ្នកត្រូវបានវាយតម្លៃដោយប្រធានផ្នែករួចរាល់។"​;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(FirebaseID.FirebaseID);
                            //clientToken.Add("dVe2G6yE4Es2tATRjg6EII:APA91bG9-NTKtIMWYI9Mq54jEW2G_VKq0su-ytSc5c2gCRTg8GD0FSlbWJej-emzubSdbrsWSvJnFJNpvQWJUP_yjTSDE5Xwm8EpXBRigwE6QO1QgW2kfenfz-9Ec1t8CYvnE7FZzlHO");
                            var dd = Noti.SendNotification(clientToken, " Staff Appraisal", Desc);
                        }
                    }
                    //alert to appraiser
                    if (FirebaseID != null)
                    {
                        if (!string.IsNullOrEmpty(FirebaseID.FirebaseID))
                        {
                            string Desc = "Apraisal have been approved.";//"ការវាយតម្លៃរបស់អ្នករួចរាល់។"​;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(FirebaseID.FirebaseID);
                            //clientToken.Add("dVe2G6yE4Es2tATRjg6EII:APA91bG9-NTKtIMWYI9Mq54jEW2G_VKq0su-ytSc5c2gCRTg8GD0FSlbWJej-emzubSdbrsWSvJnFJNpvQWJUP_yjTSDE5Xwm8EpXBRigwE6QO1QgW2kfenfz-9Ec1t8CYvnE7FZzlHO");
                            var dd = Noti.SendNotification(clientToken, " Staff Appraisal", Desc);
                        }
                    }
                    #endregion
                }
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public decimal RatingScale(string DocType, decimal TotalScore)
        {
            decimal RatingScale = 0;
            HRApprType DocTYpe = unitOfWork.Set<HRApprType>().FirstOrDefault(w => w.Code == DocType);
            if (DocTYpe != null || DocTYpe.RatingScale.HasValue)
                RatingScale = DocTYpe.RatingScale.Value;
            return RatingScale * TotalScore;
        }
        public void Calculate(HREmpAppraisal empAppraisal)
        {
            OnLoad();
            decimal ScoreKPIPA = 0;
            decimal ScoreKPI = 0;
            var Evaluation = unitOfWork.Set<HRKPIEvaluation>().FirstOrDefault(w => w.DocRef == empAppraisal.KPIReference);
            if (Evaluation != null)
            {
                empAppraisal.KPIScore = Evaluation.FinalResultPA;
                ScoreKPIPA = Evaluation.FinalResultPA.Value;
                ScoreKPI = Evaluation.FinalResult.Value;
            }
            var _listAppSummary = unitOfWork.Set<HREmpAppraisalSummary>().Where(w => w.AppraisalNo == empAppraisal.ApprID).ToList();
            foreach (var item in _listAppSummary.Where(w => w.IsKPI == true).ToList())
            {
                item.Weight = Evaluation.KPIAverage;
                item.Score = ScoreKPI;
                item.SubTotal = ScoreKPIPA;
                unitOfWork.Update(item);
            }
            empAppraisal.FinalScore = ClsRounding.Rounding(_listAppSummary.Sum(x => x.SubTotal).Value, 2, "N");
            if (_listAppSummary.Sum(w => w.SubTotal) > 0)
            {
                var EvaRate = unitOfWork.Set<HRAppGrade>().ToList();
                var Rate = EvaRate.Where(w => w.FromScore <= empAppraisal.FinalScore && w.ToScore > empAppraisal.FinalScore).ToList();
                foreach (var item in Rate)
                {
                    empAppraisal.Grade = item.Grade;
                }
            }
            else
            {
                empAppraisal.Grade = "";
            }
            unitOfWork.Update(empAppraisal);
            unitOfWork.Save();
        }
        public decimal IncreaseSalaryPercentage(string EmpCode, string Level, decimal Salary, int Rate, int InYear)
        {
            List<HRAppPerformanceRate> ListAppraisalRate = unitOfWork.Set<HRAppPerformanceRate>().ToList();
            List<HRAppCompareRatio> ListCompare = unitOfWork.Set<HRAppCompareRatio>().ToList();
            decimal IncreaseSalaryPercentage = 0;
            var AppraisalRate = ListAppraisalRate.FirstOrDefault(w => w.Result == Rate);
            if (AppraisalRate != null)
            {
                var MidPoint = unitOfWork.Set<HRAppLevelMidPoint>().FirstOrDefault(w => w.JobLevel == Level);
                if (MidPoint != null)
                {
                    decimal Compa_ration = Math.Round(Salary / MidPoint.JobLevelMidPoint.Value, 2);
                    var ObjCompare = ListCompare.FirstOrDefault(w => w.FromRatio <= Compa_ration && w.ToRatio >= Compa_ration);
                    var ListMatrix = IncreasePercentage(ListAppraisalRate, ListCompare, InYear);
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
        public decimal IncreaseSalary(string EmpCode, string Level, decimal Salary, int Rate, int InYear)
        {
            List<HRAppPerformanceRate> ListAppraisalRate = unitOfWork.Set<HRAppPerformanceRate>().ToList();
            List<HRAppCompareRatio> ListCompare = unitOfWork.Set<HRAppCompareRatio>().ToList();
            decimal IncreaseSalary = 0;
            var AppraisalRate = ListAppraisalRate.FirstOrDefault(w => w.Result == Rate);
            if (AppraisalRate != null)
            {
                var MidPoint = unitOfWork.Set<HRAppLevelMidPoint>().FirstOrDefault(w => w.JobLevel == Level);
                if (MidPoint != null)
                {
                    decimal Compa_ration = Math.Round(Salary / MidPoint.JobLevelMidPoint.Value, 2);
                    var ObjCompare = ListCompare.FirstOrDefault(w => w.FromRatio <= Compa_ration && w.ToRatio >= Compa_ration);
                    var ListMatrix = IncreasePercentage(ListAppraisalRate, ListCompare, InYear);
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

        public List<ClsMatrix> IncreasePercentage(List<HRAppPerformanceRate> ListAppraisalRate,
           List<HRAppCompareRatio> ListCompare, int InYear)
        {
            List<ClsMatrix> ListMatrix = new List<ClsMatrix>();
            var Budget = unitOfWork.Set<HRAppSalaryBudget>().FirstOrDefault(w => w.InYear == InYear);
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
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("STAFF_SELECT", unitOfWork.Set<HR_STAFF_VIEW>().Where(w => w.StatusCode == SYDocumentStatus.A.ToString()).ToList());
            keyValues.Add("APPRTYPE_SELECT", unitOfWork.Set<HRApprType>().ToList());
            keyValues.Add("LIST_GROUPKPI", unitOfWork.Set<HRKPIType>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("Category_SELECT", unitOfWork.Set<HRKPICategory>().ToList());
            keyValues.Add("STATUS_APPROVAL", new SYDataList("STATUS_LEAVE_APPROVAL").ListData.Where(w => w.SelectValue != "CANCELLED"));

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataSelectorTeam(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("BRANCHES_SELECT", SYConstant.getBranchDataAccess());
            keyValues.Add("SECTION_SELECT", ClsFilter.LoadSection());
            keyValues.Add("POSITION_SELECT", ClsFilter.LoadPosition());
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("LEVEL_SELECT", SYConstant.getLevelDataAccess());
            keyValues.Add("BusinessUnit_SELECT", unitOfWork.Set<HRGroupDepartment>().ToList());
            keyValues.Add("STAFF_SELECT", unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList());
            keyValues.Add("OFFICE_SELECT", unitOfWork.Set<HROffice>().ToList());
            keyValues.Add("GROUP_SELECT", unitOfWork.Set<HRGroup>().ToList());
            keyValues.Add("Category_SELECT", unitOfWork.Set<HRCategory>().ToList());
            keyValues.Add("LIST_GROUPKPI", unitOfWork.Set<HRKPIType>().Where(w => w.IsActive == true).ToList());

            keyValues.Add("APPRTYPE_SELECT", unitOfWork.Set<HRApprType>().ToList());
            keyValues.Add("KPICategory_SELECT", unitOfWork.Repository<HRKPICategory>().Queryable().Where(w => w.IsActive == true).ToList());


            return keyValues;
        }
        public HRKPIAssignHeader GetEmployeeKPI(HREmpAppraisal objAppraisal, string DeptCode)
        {
            var KPIType = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == objAppraisal.KPIType);
            var PlanHeader = new HRKPIAssignHeader();
            PlanHeader.AssignCode = objAppraisal.ApprID;
            PlanHeader.CriteriaType = DeptCode;
            PlanHeader.Department = objAppraisal.Department;
            PlanHeader.HandleCode = objAppraisal.EmpCode;
            PlanHeader.HandleName = objAppraisal.EmpName;
            PlanHeader.Position = objAppraisal.Position;
            PlanHeader.DocumentDate = objAppraisal.AppraiserDate.Value;

            PlanHeader.DirectedByCode = objAppraisal.DirectedByCode;
            PlanHeader.DirectedByName = objAppraisal.DirectedByName;
            PlanHeader.PlanerCode = objAppraisal.AppraiserCode;
            PlanHeader.PlanerName = objAppraisal.AppraiserName;
            PlanHeader.PlanerPosition = objAppraisal.AppraiserPosition;

            PlanHeader.ExpectedDate = objAppraisal.KPIExpectedDate;
            PlanHeader.Deadline = objAppraisal.KPIDeadline;
            PlanHeader.KPIType = objAppraisal.KPIType;
            PlanHeader.PeriodFrom = objAppraisal.PeriodFrom;
            PlanHeader.PeriodTo = objAppraisal.PeriodTo;
            PlanHeader.Status = SYDocumentStatus.OPEN.ToString();
            PlanHeader.ReStatus = SYDocumentStatus.OPEN.ToString();
            PlanHeader.TotalWeight = 0;
            PlanHeader.KPIAverage = 0;
            if (KPIType != null) PlanHeader.KPIAverage = KPIType.KPIAverage;
            PlanHeader.AssignedBy = SYDocumentASSIGN.Individual.ToString();
            PlanHeader.DocRef = objAppraisal.ApprID;
            PlanHeader.CreatedBy = User.UserName;
            PlanHeader.CreatedOn = DateTime.Now;

            return PlanHeader;
        }

        public void LoadDataFactor(string AppraisalType)
        {
            var quiz = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == AppraisalType).ToList();
            ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == AppraisalType).ToList();
            ListApprRating = unitOfWork.Set<HRApprRating>().Where(w => w.AppraisalType == AppraisalType).ToList();
            ListFactor = quiz.Where(w => ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
        }

        #region Appraisl Review
        public void OnReveiwIndexLoading()
        {
            ListAppProcess = unitOfWork.Set<HREmpAppProcess>().ToList();
        }
        public void OnReveiwIndexLoadingPending()
        {
            string approved = SYDocumentStatus.APPROVED.ToString();
            ListAppraisaPending = unitOfWork.Set<HREmpAppraisal>().Where(x => x.Status == approved).ToList();
        }
        public HREmpAppProcess GetDateAppraisal(string AppID)
        {
            HeaderProcess = new HREmpAppProcess();
            ListAppProcessItem = new List<HREmpAppProcessItem>();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Open = SYDocumentStatus.OPEN.ToString();
            HREmpAppraisal Appraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == AppID && w.Status == approved);
            HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Appraisal.EmpCode);
            IEnumerable<HREmpAppraisalSummary> ListAppraisalSummary = unitOfWork.Set<HREmpAppraisalSummary>().Where(w => w.AppraisalNo == AppID).ToList();
            List<HRAppPerformanceRate> ListAppraisalRate = unitOfWork.Set<HRAppPerformanceRate>().ToList();
            List<HRAppGrade> ListGrade = unitOfWork.Set<HRAppGrade>().ToList();
            if (Appraisal != null)
            {
                decimal TotalScore = Appraisal.FinalScore.Value;
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
            foreach (var item in ListAppraisalSummary)
            {
                decimal Score = 0;
                if (item.SubTotal.HasValue)
                    Score = item.SubTotal.Value;
                if (ListAppProcessItem.Where(w => w.Category == Appraisal.AppraisalType && w.Code == item.TaskID).Count() == 0)
                {
                    var Obj = new HREmpAppProcessItem();
                    Obj.Category = Appraisal.AppraisalType;
                    Obj.Code = item.TaskID;
                    Obj.Description = item.EvaluationCriteria;
                    Obj.Result = Score;
                    ListAppProcessItem.Add(Obj);
                }
                else
                {
                    ListAppProcessItem.Where(w => w.Category == Appraisal.AppraisalType && w.Code == item.TaskID).ToList().ForEach(w => w.Result += Score);
                }
            }
            return HeaderProcess;
        }
        public string CreateAppraisalReview(string id)
        {
            int i = 0;
            try
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);

                    if (objMatch == null) return "DOC_INV";

                    HeaderProcess.CreatedBy = User.UserName;
                    HeaderProcess.CreatedOn = DateTime.Now;

                    objMatch.Status = SYDocumentStatus.COMPLETED.ToString();
                    unitOfWork.Update(objMatch);
                    unitOfWork.Add(HeaderProcess);
                    unitOfWork.Save();
                    i = HeaderProcess.TranNo;

                    foreach (var item in ListAppProcessItem)
                    {
                        item.AppraisalProcessNo = i;
                        unitOfWork.Add(item);
                    }
                    unitOfWork.Save();
                    unitOfWork.Commit();

                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public void OnDetailLoadingReview(params object[] keys)
        {
            int id = (int)keys[0];
            HeaderProcess = unitOfWork.Set<HREmpAppProcess>().FirstOrDefault(w => w.TranNo == id);
            if (HeaderProcess != null)
            {
                ListAppProcessItem = unitOfWork.Set<HREmpAppProcessItem>().Where(w => w.AppraisalProcessNo == id).ToList();
            }
        }
        public string ReleaseDocReview(params object[] keys)
        {
            int id = (int)keys[0];
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppProcess>().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null) return "DOC_NE";
                HeaderProcess = objMatch;
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        //public string CancelDocReview(int id)
        //{
        //    try
        //    {
        //        var objMatch = DB.HREmpAppProcesses.FirstOrDefault(w => w.TranNo == id);
        //        if (objMatch == null) return "DOC_NE";
        //        HeaderProcess = objMatch;
        //        if (objMatch.Status != SYDocumentStatus.RELEASED.ToString())
        //        {
        //            return "INV_DOC";
        //        }
        //        objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

        //        DB.Entry(objMatch).Property(w => w.Status).IsModified = true;

        //        //
        //        var EmpAppraisal = DB.HREmpAppraisals.FirstOrDefault(w => w.ApprID == objMatch.DocumentRef);
        //        if (EmpAppraisal == null) return "DOC_INV";
        //        EmpAppraisal.Status = SYDocumentStatus.APPROVED.ToString();
        //        DB.Entry(EmpAppraisal).Property(w => w.Status).IsModified = true;

        //        int row = DB.SaveChanges();

        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.ScreenId = Header.ApprID;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Header.ApprID;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Header.ApprID;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}
        public string ClosedTheDocReview(params object[] keys)
        {
            int id = (int)keys[0];
            try
            {

                var objMatch = unitOfWork.Set<HREmpAppProcess>().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderProcess = objMatch;

                objMatch.Status = SYDocumentStatus.CLOSED.ToString();

                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        #endregion
    }
    public class ClsMatrix
    {
        public string PerformanceRating { get; set; }
        public string CompaRatio { get; set; }
        public decimal Percentage { get; set; }
    }
    public class _ListStaff
    {
        public string EmpCode { get; set; }
        public string AllNameKH { get; set; }
        public string AllNameENG { get; set; }
        public string Sex { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Section { get; set; }
        public string Appraisal { get; set; }
        public string Appraisal2 { get; set; }
    }
}