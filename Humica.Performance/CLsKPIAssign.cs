using Humica.Calculate;
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Humica.Performance
{
    public class CLsKPIAssign : IClsKPIAssign
    {
        public string EmpID { get; set; }
        HumicaDBContext DB = new HumicaDBContext();
        public string ScreenId { get; set; }
        public static string KPI_Department { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ErrorMessage { get; set; }
        //public FTDGeneralAccount Filter { get; set; }
        public FTFilerIndex Filter { get; set; }
        public HRKPIAssignHeader AssignHeader { get; set; }
        public HRKPIAssignItem AssignItem { get; set; }
        public List<HRKPIAssignHeader> listAssignHeader { get; set; }
        public List<HRKPIAssignItem> listAssignInsertItem { get; set; }
        public List<HRKPIAssignHeader> ListPlanPending { get; set; }
        public List<HREmpAppraisal> ListPending { get; set; }
        public List<HRKPIAssignMember> ListKPIMember { get; set; }
        public List<ListAssignItem> ListTask { get; set; }
        public List<ListAssignItem> ListTaskPending { get; set; }
        public List<ListAssignItem> ListTaskCompleted { get; set; }
        public List<HRKPITracking> ListKPITracking { get; set; }
        public List<ClsTaskSummary> ListtaskSummary { get; set; }

        public CLsKPIAssign()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        public async Task<List<ListAssignItem>> LoadDataTask()
        {
            try
            {
                using (var context = new HumicaDBContext())
                {
                    // Retrieve all members
                    string approveStatus = SYDocumentStatus.APPROVED.ToString();
                    string RELEASED = SYDocumentStatus.RELEASED.ToString();
                    string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
                    // Retrieve assignments
                    var assignments = await (from assign in context.HRKPIAssignHeaders
                                             join item in context.HRKPIAssignItems on assign.AssignCode equals item.AssignCode
                                             where (assign.Status == approveStatus || assign.Status == RELEASED || assign.Status== COMPLETED)
                                             select new ListAssignItem
                                             {
                                                 AssignCode = assign.AssignCode,
                                                 EmpCode = assign.HandleCode,
                                                 EmployeeName = assign.HandleName,
                                                 Position = assign.Position,
                                                 Department = assign.Department,
                                                 KPIType = assign.KPIType,
                                                 PeriodFrom = assign.PeriodFrom.HasValue ? assign.PeriodFrom.Value : DateTime.MinValue,
                                                 PeriodTo = assign.PeriodTo.HasValue ? assign.PeriodTo.Value : DateTime.MinValue,
                                                 HandlePerson = assign.HandleCode + " : " + assign.HandleName,
                                                 ItemCode = item.ItemCode,
                                                 KPI = item.KPI,
                                                 Measure = item.Measure,
                                                 Weight = item.Weight,
                                                 Traget = item.Target.HasValue ? item.Target.Value : 0,
                                                 Actual = item.Actual.HasValue ? item.Actual.Value : 0,
                                                 Achievement = item.Acheivement.HasValue ? item.Acheivement.Value : 0,
                                                 StartDate = item.StartDate.HasValue ? item.StartDate.Value : DateTime.MinValue,
                                                 EndDate = item.EndDate.HasValue ? item.EndDate.Value : DateTime.MinValue,
                                                 Status = item.Status
                                             }).ToListAsync();

                    return assignments;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use your logging framework here)
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Consider how you want to handle errors, for now, we'll return an empty list
                return new List<ListAssignItem>();
            }
        }
        public async Task<List<ClsTaskSummary>> LoadDataTaskSummary(string Id, string ItemCode, decimal Target)
        {
            try
            {
                using (var context = new HumicaDBContext())
                {
                    string APPROVED = SYDocumentStatus.APPROVED.ToString();
                    var _ListKPITracking = await DB.HRKPITrackings.Where(w => w.AssignCode == Id && w.KPI == ItemCode
                    && w.Status == APPROVED).ToListAsync();
                    List<ClsTaskSummary> assignments = new List<ClsTaskSummary>();
                    foreach (var item in _ListKPITracking)
                    {
                        if (assignments.Where(w => w.EmpCode == item.EmpCode).Count() > 0)
                        {
                            assignments.Where(w => w.EmpCode == item.EmpCode).ToList().ForEach(w => w.Actual += item.Actual);
                            assignments.Where(w => w.EmpCode == item.EmpCode).ToList().ForEach(w => w.Achievement = w.Actual / Target);
                        }
                        else
                        {
                            var obj = new ClsTaskSummary();
                            obj.EmpCode = item.EmpCode;
                            obj.EmployeeName = item.EmpName;
                            obj.Position = item.Position;
                            obj.Actual = item.Actual;
                            obj.Achievement = item.Actual / Target;
                            assignments.Add(obj);
                        }
                    }
                    return assignments;
                }
            }
            catch (Exception ex)
            {
                return new List<ClsTaskSummary>();
            }
        }
        public async Task<HRKPIAssignHeader> getSetupHeader(string id)
        {
            listAssignInsertItem = new List<HRKPIAssignItem>();
            ListKPIMember = new List<HRKPIAssignMember>();
            AssignHeader = await DB.HRKPIAssignHeaders.FirstOrDefaultAsync(w => w.AssignCode == id);
            if (AssignHeader != null)
            {
                listAssignInsertItem = await DB.HRKPIAssignItems.Where(w => w.AssignCode == id).ToListAsync();
                ListKPIMember = await DB.HRKPIAssignMembers.Where(w => w.AssignCode == id).ToListAsync();
                AssignHeader.TotalScore = 0;
                AssignHeader.EmpCode = "";
                AssignHeader.HandleCode = "";
                AssignHeader.EmpName = "";
                AssignHeader.PICPosition = "";
                return AssignHeader;
            }
            return null;
        }
        public async Task<string> CreateAssign(string Screen_Id)
        {
            DB = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var ObjPlan = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == AssignHeader.KPICode);
                var objCF = await DB.ExCfWorkFlowItems.FirstOrDefaultAsync(w => w.ScreenID == Screen_Id);
                var Staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == AssignHeader.HandleCode);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (AssignHeader.HandleCode == null)
                {
                    return "DOC_INV";
                }
                if (string.IsNullOrEmpty(AssignHeader.KPIType))
                {
                    return "DOC_INV";
                }
                if (listAssignInsertItem.Count() == 0)
                {
                    return "DOC_INV";
                }
                if (string.IsNullOrEmpty(AssignHeader.EmpCode) && AssignHeader.AssignedBy != SYDocumentASSIGN.Individual.ToString())
                {
                    return "EMPCODE_EN";
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                AssignHeader.AssignCode = objNumber.NextNumberRank;
                foreach (var read in listAssignInsertItem.ToList())
                {
                    read.AssignCode = AssignHeader.AssignCode;
                    read.EmpCode = AssignHeader.EmpCode;
                    read.Status = AssignHeader.Status;
                    DB.HRKPIAssignItems.Add(read);
                }
                AssignHeader.ScreenID = ScreenId;
                AssignHeader.DirectedByCode = ObjPlan.PlanerCode;
                AssignHeader.DirectedByName = ObjPlan.PlanerName;
                AssignHeader.CreatedBy = User.UserName;
                AssignHeader.CreatedOn = DateTime.Now;

                if (ListKPIMember != null)
                {
                    foreach (var read in ListKPIMember.ToList())
                    {
                        read.AssignCode = AssignHeader.AssignCode;
                        DB.HRKPIAssignMembers.Add(read);
                    }
                }
                ObjPlan.Status = SYDocumentStatus.COMPLETED.ToString();
                DB.Entry(ObjPlan).Property(w => w.Status).IsModified = true;

                if (Staff != null)
                {
                    AssignHeader.HandleName = Staff.AllName;
                }
                DB.HRKPIAssignHeaders.Add(AssignHeader);
                int row = await DB.SaveChangesAsync();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.EmpID;
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
                //log.DocurmentAction = Header.EmpID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public async Task<string> EditAssignKPI(string id)
        {
            DB = new HumicaDBContext();
            using (var DBM = new HumicaDBContext())
            {
                using (var transaction = DBM.Database.BeginTransaction())
                {
                    try
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = false;
                        var objMatch = await DB.HRKPIAssignHeaders.FirstOrDefaultAsync(w => w.AssignCode == id);
                        var listAssignItem = await DB.HRKPIAssignItems.Where(w => w.AssignCode == id).ToListAsync();
                        var listAssignMember = await DB.HRKPIAssignMembers.Where(w => w.AssignCode == id).ToListAsync();

                        if (objMatch == null)
                        {
                            return "INV_DOC";
                        }

                        foreach (var read in listAssignItem)
                        {
                            DBM.HRKPIAssignItems.Attach(read);
                            DBM.HRKPIAssignItems.Remove(read);
                        }
                        foreach (var read in listAssignMember)
                        {
                            DBM.HRKPIAssignMembers.Attach(read);
                            DBM.HRKPIAssignMembers.Remove(read);
                        }
                        foreach (var read in listAssignInsertItem.ToList())
                        {
                            read.Status = AssignHeader.Status;
                            read.AssignCode = AssignHeader.AssignCode;
                            DBM.HRKPIAssignItems.Add(read);
                        }
                        if (ListKPIMember != null)
                        {
                            foreach (var read in ListKPIMember.ToList())
                            {
                                read.AssignCode = AssignHeader.AssignCode;
                                DBM.HRKPIAssignMembers.Add(read);
                            }
                        }
                        var Staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == AssignHeader.HandleCode);
                        if (Staff != null)
                        {
                            AssignHeader.HandleName = Staff.AllName;
                        }
                        AssignHeader.ScreenID = objMatch.ScreenID;
                        AssignHeader.DirectedByCode = objMatch.DirectedByCode;
                        AssignHeader.DirectedByName = objMatch.DirectedByName;
                        AssignHeader.CriteriaType = objMatch.CriteriaType;
                        AssignHeader.CreatedOn = objMatch.CreatedOn;
                        AssignHeader.CreatedBy = objMatch.CreatedBy;
                        AssignHeader.ChangedBy = User.UserName;
                        AssignHeader.ChangedOn = DateTime.Now;
                        DBM.Entry(AssignHeader).State = System.Data.Entity.EntityState.Modified;
                        int row = await DBM.SaveChangesAsync();
                        transaction.Commit();
                        return SYConstant.OK;
                    }
                    catch (DbEntityValidationException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = ScreenId;
                        log.UserId = User.UserName;
                        log.DocurmentAction = AssignHeader.AssignCode;
                        log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        return "EE001";
                    }
                    finally
                    {
                        DBM.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }

        public async Task<string> ReleaseTheDoc(string id)
        {
            string ID = id;
            try
            {
                var objMatch = await DB.HRKPIAssignHeaders.FirstOrDefaultAsync(w => w.AssignCode == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                AssignHeader = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public async Task<string> CompeledDoc(string id, string KPI)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();

                var objmatch = await DB.HRKPIAssignItems.FirstOrDefaultAsync(w => w.AssignCode == id && w.ItemCode == KPI);

                if (objmatch == null) return "DOC_INV";
                if (objmatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                var Status = SYDocumentStatus.COMPLETED.ToString();

                objmatch.Status = Status;
                DB.HRKPIAssignItems.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = id;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = id;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public List<HRKPIAssignHeader> OnIndexLoading(string AssignedBy, bool ESS = false)
        {
            OnLoad();
            if (Filter == null)
            {
                Filter = new FTFilerIndex();
                Filter.InYear = DateTime.Now.Year;
            }
            listAssignHeader = unitOfWork.Set<HRKPIAssignHeader>().Where(w => w.InYear == Filter.InYear && w.AssignedBy == AssignedBy).ToList();
            if (ESS == true)
            {
                string UserName = User.UserName;
                //var Staff = unitOfWork.Set<HRStaffProfile>().Where(w => w.FirstLine == UserName || w.SecondLine == UserName || w.HODCode == UserName || w.EmpCode == UserName || w.Manager == UserName).ToList();
                var Staff = unitOfWork.Set<HRStaffProfile>().Where(w => w.HODCode == UserName ||
                w.EmpCode == UserName || w.APPTracking == UserName || w.APPEvaluator == UserName).ToList();
                //listAssignHeader = listAssignHeader.Where(w => w.EmpCode == UserName || w.HandleCode == UserName
                //|| w.DirectedByCode == UserName).ToList();
                if (AssignedBy == "BYTEAM")
                {
                    listAssignHeader = listAssignHeader
                    .Where(w => (w.EmpCode != null && w.EmpCode == UserName) ||
                                (w.HandleCode != null && w.HandleCode == UserName) ||
                                (w.DirectedByCode != null && w.DirectedByCode == UserName) ||
                                (w.PlanerCode == UserName) ||
                                Staff.Any(e => e.EmpCode != null && e.EmpCode == w.HandleCode))
                    .ToList();
                }
                else
                {
                    var ListStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.APPTracking == UserName || w.APPEvaluator == UserName || w.HODCode == UserName​ || w.EmpCode == UserName).ToList();

                    listAssignHeader = listAssignHeader
                    .Where(w => ListStaff.Any(e => e.EmpCode == w.HandleCode))
                    .ToList();
                }
            }

            return listAssignHeader;
        }
        public List<HRKPIAssignHeader> OnIndexLoadingPending(bool ESS = false)
        {
            string OPEN = SYDocumentStatus.OPEN.ToString();
            ListPlanPending = unitOfWork.Set<HRKPIAssignHeader>().Where(w => w.ReStatus == OPEN).ToList();
            if (ESS == true)
            {
                string UserName = User.UserName;
                ListPlanPending = ListPlanPending.Where(w => w.PlanerCode == UserName || w.HandleCode == UserName).ToList();
            }
            return ListPlanPending;
        }
        public void OnIndexPending(bool ESS = false)
        {
            string OPEN = SYDocumentStatus.OPEN.ToString();
            string RELEASED = SYDocumentStatus.RELEASED.ToString();
            string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            string closed = SYDocumentStatus.CLOSED.ToString();
            //ListPending = unitOfWork.Set<HREmpAppraisal>().Where(w => w.KPIStatus != RELEASED && w.KPIStatus != COMPLETED && w.KPIStatus != PENDING && !string.IsNullOrEmpty(w.KPIType)
            //&& w.KPIStatus != closed && w.Status == PENDING).ToList();
            ListPending = unitOfWork.Set<HREmpAppraisal>().Where(w => !string.IsNullOrEmpty(w.KPIType)
            && w.KPIStatus == OPEN && w.Status == PENDING).ToList();
            if (ESS == true)
            {
                string UserName = User.UserName;
                var ListStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.APPTracking == UserName).ToList();
                ListPending = ListPending.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }
        }
        public virtual void OnCreatingLoading(string ID)
        {
            AssignHeader = new HRKPIAssignHeader();
            listAssignInsertItem = new List<HRKPIAssignItem>();
            var objAppraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == ID);
            if (objAppraisal != null)
            {
                var KPIType = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == objAppraisal.KPIType);
                var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objAppraisal.EmpCode);
                AssignHeader.AssignCode = objAppraisal.ApprID;
                if (Staff != null)
                {
                    if (objAppraisal.KPICategory == "BSN")
                    {
                        AssignHeader.CriteriaType = Staff.GroupDept;
                    }
                    else
                    {
                        AssignHeader.CriteriaType = Staff.DEPT;
                    }
                }
                AssignHeader.Department = objAppraisal.Department;
                AssignHeader.HandleCode = objAppraisal.EmpCode;
                AssignHeader.HandleName = objAppraisal.EmpName;
                AssignHeader.Position = objAppraisal.Position;
                AssignHeader.DocumentDate = objAppraisal.AppraiserDate.Value;

                AssignHeader.DirectedByCode = objAppraisal.DirectedByCode;
                AssignHeader.DirectedByName = objAppraisal.DirectedByName;
                AssignHeader.PlanerCode = objAppraisal.AppraiserCode;
                AssignHeader.PlanerName = objAppraisal.AppraiserName;
                AssignHeader.PlanerPosition = objAppraisal.AppraiserPosition;

                AssignHeader.ExpectedDate = objAppraisal.KPIExpectedDate;
                AssignHeader.Deadline = objAppraisal.KPIDeadline;
                AssignHeader.KPIType = objAppraisal.KPIType;
                AssignHeader.PeriodFrom = objAppraisal.PeriodFrom;
                AssignHeader.PeriodTo = objAppraisal.PeriodTo;
                AssignHeader.KPICategory = objAppraisal.KPICategory;
                AssignHeader.Status = SYDocumentStatus.OPEN.ToString();
                AssignHeader.ReStatus = SYDocumentStatus.OPEN.ToString();
                AssignHeader.TotalWeight = 0;
                AssignHeader.KPIAverage = 0;
                if (KPIType != null) AssignHeader.KPIAverage = KPIType.KPIAverage;
                AssignHeader.AssignedBy = SYDocumentASSIGN.Individual.ToString();
                AssignHeader.DocRef = objAppraisal.ApprID;
            }
        }
        public virtual string OnCreateMultiLoading(string ID)
        {
            string[] ApproArr = ID.Split(';');
            string AssignCode = "";
            string Department = "";
            string EmpCode = "";
            listAssignInsertItem = new List<HRKPIAssignItem>();
            ListKPIMember = new List<HRKPIAssignMember>();
            AssignHeader = new HRKPIAssignHeader();
            foreach (var item in ApproArr)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    AssignCode = item;
                    var objApp = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == AssignCode);
                    if (objApp != null)
                    {
                        if (!string.IsNullOrEmpty(Department) && Department != objApp.Department)
                        {
                            return "INVALID_DEPARTMENT";
                        }
                        ListKPIMember.Add(new HRKPIAssignMember
                        {
                            EmpCode = objApp.EmpCode,
                            EmployeeName = objApp.EmpName,
                            Department = objApp.Department,
                            Position = objApp.Position,
                            DocRef = objApp.ApprID,
                           Status  = SYDocumentStatus.OPEN.ToString()
                        });
                        AssignHeader.PeriodFrom = objApp.PeriodFrom;
                        AssignHeader.PeriodTo = objApp.PeriodTo;
                        AssignHeader.KPIType = objApp.KPIType;
                        AssignHeader.Department = objApp.Department;
                        AssignHeader.Position = objApp.Position;
                        AssignHeader.ExpectedDate = objApp.KPIExpectedDate;
                        AssignHeader.Deadline = objApp.KPIDeadline;
                        AssignHeader.PlanerCode = objApp.AppraiserCode;
                        AssignHeader.PlanerName = objApp.AppraiserName;
                        AssignHeader.PlanerPosition = objApp.AppraiserPosition;
                        if (string.IsNullOrEmpty(Department))
                        {
                            EmpCode = objApp.DirectedByCode;
                            Department = objApp.Department;
                        }
                    }
                }
            }
            var KPIType = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == AssignHeader.KPIType);
            //var Staff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            var StaffTeam = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == AssignHeader.PlanerCode);
            if (StaffTeam != null)
            {
                AssignHeader.CriteriaType = StaffTeam.DEPT;
                var Staff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == StaffTeam.FirstLine);
                if (Staff != null)
                {
                    AssignHeader.HandleCode = Staff.EmpCode;
                    AssignHeader.HandleName = Staff.AllName;
                    AssignHeader.Position = Staff.Position;
                }
            }
            if (KPIType != null) AssignHeader.KPIAverage = KPIType.KPIAverage;
            AssignHeader.DocumentDate = DateTime.Now;
            AssignHeader.Member = ListKPIMember.Count();
            AssignHeader.Status = SYDocumentStatus.OPEN.ToString();

            return SYSConstant.OK;
        }
        public string CreateMulti(string Screen_Id)
        {
            OnLoad();
            try
            {
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == Screen_Id);
                var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == AssignHeader.HandleCode);
                var ListIndicator = unitOfWork.Set<HRKPIIndicator>().ToList();
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (AssignHeader.HandleCode == null)
                {
                    return "DOC_INV";
                }
                if (string.IsNullOrEmpty(AssignHeader.KPIType))
                {
                    return "DOC_INV";
                }
                if (listAssignInsertItem.Count() == 0)
                {
                    return "DOC_INV";
                }
                if (string.IsNullOrEmpty(AssignHeader.PlanerCode) && AssignHeader.AssignedBy != SYDocumentASSIGN.Individual.ToString())
                {
                    return "EMPCODE_EN";
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                AssignHeader.AssignCode = objNumber.NextNumberRank;
                var tempIndicator = new List<HRKPIAssignIndicator>();
                foreach (var read in listAssignInsertItem.ToList())
                {
                    read.AssignCode = AssignHeader.AssignCode;
                    read.EmpCode = AssignHeader.EmpCode;
                    read.Status = AssignHeader.Status;
                    if (tempIndicator.Where(w => w.Indicator == read.Indicator).Any())
                    {
                        tempIndicator.Where(w => w.Indicator == read.Indicator).ToList().ForEach(w => w.Weight += read.Weight);
                    }
                    else
                    {
                        var obj = new HRKPIAssignIndicator();
                        NewIndicator(obj, read, ListIndicator);
                        tempIndicator.Add(obj);
                    }
                    unitOfWork.Add(read);
                }
                AssignHeader.ScreenID = ScreenId;
                //AssignHeader.DirectedByCode = ObjPlan.PlanerCode;
                //AssignHeader.DirectedByName = ObjPlan.PlanerName;
                AssignHeader.CreatedBy = User.UserName;
                AssignHeader.CreatedOn = DateTime.Now;

                foreach (var read in tempIndicator)
                {
                    unitOfWork.Add(read);
                }
                if (ListKPIMember != null)
                {
                    foreach (var read in ListKPIMember.ToList())
                    {
                        read.AssignCode = AssignHeader.AssignCode;
                        unitOfWork.Add(read);
                        var ObjPlan = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == read.DocRef);
                        if (ObjPlan != null)
                        {
                            ObjPlan.KPIStatus = SYDocumentStatus.PENDING.ToString();
                            ObjPlan.KPIReference = AssignHeader.AssignCode;
                            AssignHeader.InYear = ObjPlan.InYear;
                            AssignHeader.KPICategory = ObjPlan.KPICategory;
                            unitOfWork.Update(ObjPlan);
                            if (ObjPlan.EmpCode == AssignHeader.HandleCode)
                            {
                                AssignHeader.DirectedByCode = ObjPlan.AppraiserCode;
                                AssignHeader.DirectedByName = ObjPlan.AppraiserName;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(AssignHeader.DirectedByCode))
                    {
                        var StaffManager = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.APPEvaluator);
                        if (StaffManager != null)
                        {
                            AssignHeader.DirectedByCode = StaffManager.EmpCode;
                            AssignHeader.DirectedByName = StaffManager.AllName;
                        }
                    }
                }
                unitOfWork.Add(AssignHeader);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Create()
        {
            OnLoad();
            try
            {
                //var ObjPlan = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == AssignHeader.AssignCode);
                var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == AssignHeader.HandleCode);
                var ListIndicator = unitOfWork.Set<HRKPIIndicator>().ToList();
                if (string.IsNullOrEmpty(AssignHeader.KPIType))
                {
                    return "DOC_INV";
                }
                if (listAssignInsertItem.Count() == 0)
                {
                    return "DOC_INV";
                }
                var tempIndicator = new List<HRKPIAssignIndicator>();
                foreach (var read in listAssignInsertItem.ToList())
                {
                    read.AssignCode = AssignHeader.AssignCode;
                    read.EmpCode = AssignHeader.EmpCode;
                    read.Status = AssignHeader.Status;
                    if (tempIndicator.Where(w => w.Indicator == read.Indicator).Any())
                    {
                        tempIndicator.Where(w => w.Indicator == read.Indicator).ToList().ForEach(w => w.Weight += read.Weight);
                    }
                    else
                    {
                        var obj = new HRKPIAssignIndicator();
                        NewIndicator(obj, read, ListIndicator);
                        tempIndicator.Add(obj);
                    }

                    unitOfWork.Add(read);
                }
                foreach (var read in tempIndicator)
                {
                    unitOfWork.Add(read);
                }
                AssignHeader.ScreenID = ScreenId;

                if (ListKPIMember != null)
                {
                    foreach (var read in ListKPIMember.ToList())
                    {
                        read.AssignCode = AssignHeader.AssignCode;
                        unitOfWork.Add(read);
                    }
                }
                var ObjPlan = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == AssignHeader.AssignCode);
                if (ObjPlan != null)
                {
                    AssignHeader.DocRef = AssignHeader.AssignCode;
                    AssignHeader.HandleName = ObjPlan.EmpName;
                    AssignHeader.DirectedByCode = ObjPlan.DirectedByCode;
                    AssignHeader.DirectedByName = ObjPlan.DirectedByName;
                    AssignHeader.KPICategory = ObjPlan.KPICategory;
                    AssignHeader.InYear = ObjPlan.InYear;
                    ObjPlan.KPIStatus = SYDocumentStatus.PENDING.ToString();
                    ObjPlan.KPIReference = AssignHeader.AssignCode;
                    unitOfWork.Update(ObjPlan);
                }
                AssignHeader.TotalWeight = listAssignInsertItem.Sum(w => w.Weight);
                AssignHeader.ReStatus = SYDocumentStatus.OPEN.ToString();
                AssignHeader.Description = AssignHeader.Description;
                AssignHeader.ExpectedDate = AssignHeader.ExpectedDate;
                AssignHeader.CreatedBy = User.UserName;
                AssignHeader.CreatedOn = DateTime.Now;
                unitOfWork.Add(AssignHeader);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string CopyData(string id)
        {
            listAssignInsertItem = new List<HRKPIAssignItem>();
            AssignHeader = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
            if (AssignHeader == null)
            {
                return "DOC_ED";
            }
            listAssignInsertItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == id).ToList();
            AssignHeader.TotalScore = 0;
            AssignHeader.EmpCode = "";
            AssignHeader.HandleCode = "";
            AssignHeader.EmpName = "";
            AssignHeader.PICPosition = "";
            AssignHeader.Status = SYDocumentStatus.OPEN.ToString();
            if (listAssignInsertItem.Count() > 0)
            {
                listAssignInsertItem.ForEach(w => w.Acheivement = 0);
                listAssignInsertItem.ForEach(w => w.Actual = 0);
                listAssignInsertItem.ForEach(w => w.Score = 0);
            }
            return SYConstant.OK;
        }
        public virtual string OnEditLoading(params object[] keys)
        {
            string AssignCode = (string)keys[0];
            string RELEASED = SYDocumentStatus.RELEASED.ToString();
            this.AssignHeader = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == AssignCode &&
            w.PlanerCode == User.UserName && w.Status != RELEASED);
            if (this.AssignHeader != null)
            {
                this.listAssignInsertItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == AssignCode).ToList();
            }
            else
            {
                return "DOC_ED";
            }
            return SYConstant.OK;
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string AssignCode = (string)keys[0];
            this.AssignHeader = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == AssignCode);
            if (this.AssignHeader != null)
            {
                this.listAssignInsertItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == AssignCode).ToList();
            }
        }
        public virtual void OnDetailMultiLoading(params object[] keys)
        {
            string AssignCode = (string)keys[0];
            this.AssignHeader = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == AssignCode);
            if (this.AssignHeader != null)
            {
                this.listAssignInsertItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == AssignCode).ToList();
                this.ListKPIMember = unitOfWork.Set<HRKPIAssignMember>().Where(w => w.AssignCode == AssignCode).ToList();
            }
        }
        public string Delete(string id)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
                var listAssignItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == id).ToList();
                var listAssignIndicator = unitOfWork.Set<HRKPIAssignIndicator>().Where(w => w.AssignCode == id).ToList();
                var listAssignMember = unitOfWork.Set<HRKPIAssignMember>().Where(w => w.AssignCode == id).ToList();

                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                foreach (var read in listAssignIndicator)
                {
                    unitOfWork.Delete(read);
                }
                foreach (var read in listAssignItem)
                {
                    unitOfWork.Delete(read);
                }
                foreach (var read in listAssignMember)
                {
                    unitOfWork.Delete(read);
                }
                if (objMatch.AssignedBy == "Individual")
                {
                    var ObjAppraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == objMatch.AssignCode);
                    if (ObjAppraisal != null)
                    {
                        ObjAppraisal.KPIStatus = SYDocumentStatus.OPEN.ToString();
                        ObjAppraisal.KPIReference = "";
                        unitOfWork.Update(ObjAppraisal);
                    }
                }
                else if (objMatch.AssignedBy == "BYTEAM")
                {
                    foreach (var read in listAssignMember.ToList())
                    {
                        var ObjAppraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == read.DocRef);
                        if (ObjAppraisal != null)
                        {
                            ObjAppraisal.KPIStatus = SYDocumentStatus.OPEN.ToString();
                            ObjAppraisal.KPIReference = "";
                            unitOfWork.Update(ObjAppraisal);
                        }
                    }
                }
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Update(string id)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
                var listAssignItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == id).ToList();
                var listAssignIndicator = unitOfWork.Set<HRKPIAssignIndicator>().Where(w => w.AssignCode == id).ToList();
                var listAssignMember = unitOfWork.Set<HRKPIAssignMember>().Where(w => w.AssignCode == id).ToList();
                var ListIndicator = unitOfWork.Set<HRKPIIndicator>().ToList();

                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                foreach (var read in listAssignIndicator)
                {
                    unitOfWork.Delete(read);
                }
                foreach (var read in listAssignItem)
                {
                    unitOfWork.Delete(read);
                }
                foreach (var read in listAssignMember)
                {
                    unitOfWork.Delete(read);
                }
                var tempIndicator = new List<HRKPIAssignIndicator>();
                foreach (var read in listAssignInsertItem.ToList())
                {
                    read.AssignCode = AssignHeader.AssignCode;

                    if (tempIndicator.Where(w => w.Indicator == read.Indicator).Any())
                    {
                        tempIndicator.Where(w => w.Indicator == read.Indicator).ToList().ForEach(w => w.Weight += read.Weight);
                    }
                    else
                    {
                        var obj = new HRKPIAssignIndicator();
                        NewIndicator(obj, read, ListIndicator);
                        tempIndicator.Add(obj);
                    }
                    unitOfWork.Add(read);
                }
                foreach (var read in tempIndicator)
                {
                    unitOfWork.Add(read);
                }
                if (objMatch.AssignedBy== "BYTEAM" && ListKPIMember != null)
                {
                    foreach (var read in ListKPIMember.ToList())
                    {
                        read.AssignCode = AssignHeader.AssignCode;
                        unitOfWork.Add(read);
                    }
                    var ListDeleteMem = listAssignMember.Where(w => !ListKPIMember.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                    foreach (var read in ListDeleteMem)
                    {
                        var ObjAppraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == read.DocRef);
                        if (ObjAppraisal != null)
                        {
                            ObjAppraisal.KPIStatus = SYDocumentStatus.OPEN.ToString();
                            ObjAppraisal.KPIReference = "";
                            unitOfWork.Update(ObjAppraisal);
                        }
                    }
                    objMatch.Member = ListKPIMember.Count();
                }
                objMatch.Description = AssignHeader.Description;
                objMatch.TotalWeight = listAssignInsertItem.Sum(w => w.Weight);
                objMatch.ExpectedDate = AssignHeader.ExpectedDate;
                objMatch.TeamName = AssignHeader.TeamName;
                //var Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == AssignHeader.HandleCode);
                //if (Staff != null)
                //{
                //    AssignHeader.HandleName = Staff.AllName;
                //}
                //AssignHeader.ScreenID = objMatch.ScreenID;
                //AssignHeader.DirectedByCode = objMatch.DirectedByCode;
                //AssignHeader.DirectedByName = objMatch.DirectedByName;
                //AssignHeader.CriteriaType = objMatch.CriteriaType;
                //AssignHeader.CreatedOn = objMatch.CreatedOn;
                //AssignHeader.CreatedBy = objMatch.CreatedBy;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, AssignHeader.AssignCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RequestRelease(string id)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (objMatch.TotalWeight < 1)
                {
                    return "WEIGHT<100%";
                }
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                objMatch.AutoAccept = DateTime.Now.AddDays(5);
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string ReleaseDoc(string id)
        {
            OnLoad();
            string ID = id;
            try
            {
                var objMatch = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
                var objMatchItem = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == id).ToList();
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                AssignHeader = objMatch;
                objMatch.TotalWeight = objMatchItem.Sum(w => w.Weight);
                if (objMatch.TotalWeight < 1)
                {
                    return "WEIGHT<100%";
                }
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                objMatch.AutoAccept = DateTime.Now.AddDays(5);

                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Accept(string id, bool ESS = false)
        {
            OnLoad();
            string ID = id;
            try
            {
                var objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
                var objMatchkpi = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
                if (objMatchkpi == null)
                {
                    return "REQUEST_NE";
                }

                if (objMatchkpi.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                if (ESS == true && objMatchkpi.HandleCode != User.UserName)
                {
                    return "USER_NOT_APPROVOR";
                }
                if (objMatch == null)
                {
                    var AssignMember = unitOfWork.Set<HRKPIAssignMember>().Where(w => w.AssignCode == id);
                    foreach (var member in AssignMember)
                    {
                        objMatch = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == member.DocRef);
                        objMatch.KPIStatus = SYDocumentStatus.COMPLETED.ToString();
                        unitOfWork.Update(objMatch);
                    }
                }
                else
                {
                    objMatch.KPIStatus = SYDocumentStatus.COMPLETED.ToString();
                    unitOfWork.Update(objMatch);
                }
                objMatchkpi.Status = SYDocumentStatus.RELEASED.ToString();
                objMatchkpi.ReStatus = SYDocumentStatus.APPROVED.ToString();
                unitOfWork.Update(objMatchkpi);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Approved(string ID)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ID);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string Open = SYDocumentStatus.OPEN.ToString();
                var listApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == objMatch.CriteriaType
                                    && w.DocumentNo == objMatch.AssignCode && w.Status == Open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                foreach (var read in listApproval)
                {
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            {
                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            }
                            //StaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            unitOfWork.Update(read);
                            b = true;
                            break;
                        }
                    }
                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }

                var Status = SYDocumentStatus.APPROVED.ToString();

                if (listApproval.Where(w => w.Status == Open).ToList().Count > 0)
                {
                    Status = SYDocumentStatus.PENDING.ToString();
                }

                objMatch.Status = Status;
                objMatch.ApprovedBy = User.UserName;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public virtual string OnGridModify(HRKPIAssignItem MModel, string Action)
        {
            decimal? TotalWeight = 0;

            if (Action == "ADD")
            {
                TotalWeight = listAssignInsertItem.Sum(w => w.Weight) + MModel.Weight;
            }
            else if (Action == "EDIT")
            {
                var objCheck = listAssignInsertItem.Where(w => w.ItemCode == MModel.ItemCode).FirstOrDefault();
                if (objCheck != null)
                {
                    listAssignInsertItem.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
                TotalWeight = listAssignInsertItem.Sum(w => w.Weight) + MModel.Weight;
            }
            else if (Action == "DELETE")
            {
                var objCheck = listAssignInsertItem.Where(w => w.ItemCode == MModel.ItemCode).FirstOrDefault();
                if (objCheck != null)
                {
                    listAssignInsertItem.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            if (MModel.Weight == 0)
            {
                return "WEIGHT_NOT_0";
            }
            else if (MModel.Weight <= 0 || MModel.Weight > 1)
            {
                return "INV_WEIGHT";
            }
            else if (TotalWeight > 1)
            {
                return "KPI_Percent";
            }
            var check = listAssignInsertItem.Where(w => w.ItemCode == MModel.ItemCode).ToList();
            if (check.Count == 0)
            {
                listAssignInsertItem.Add(MModel);
            }
            else
            {
                return "ISDUPLICATED";
            }
            listAssignInsertItem = listAssignInsertItem.OrderBy(w => w.ItemCode).ToList();
            return SYConstant.OK;
        }
        public string OnGridModify(HRKPIAssignMember MModel, string Action)
        {
            if (Action == "EDIT")
            {
                var objCheck = ListKPIMember.Where(w => w.EmpCode == MModel.EmpCode).FirstOrDefault();
                if (objCheck != null)
                {
                    ListKPIMember.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListKPIMember.Where(w => w.EmpCode == MModel.EmpCode).FirstOrDefault();
                if (objCheck != null)
                {
                    ListKPIMember.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }

            var check = ListKPIMember.Where(w => w.EmpCode == MModel.EmpCode).ToList();
            if (check.Count == 0)
            {
                ListKPIMember.Add(MModel);
            }
            else
            {
                return "ISDUPLICATED";
            }
            return SYConstant.OK;
        }
        public string Calculate(string id)
        {
            OnLoad();
            var ASHeader = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
            List<HRKPITracking> ListKPITracking = new List<HRKPITracking>();
            string Status = SYDocumentStatus.APPROVED.ToString();
            var KPIType = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == ASHeader.KPIType);
            if (KPIType != null)
            {
                DateTime StatDate = DateTimeHelper.DateInHourMin(KPIType.StartDate.Value);
                DateTime EndDate = DateTimeHelper.DateInHourMin(KPIType.EndDate.Value);
                ListKPITracking = unitOfWork.Set<HRKPITracking>().Where(w => w.AssignCode == id && w.Status == Status
                && w.DocumentDate >= StatDate && w.DocumentDate <= EndDate).ToList();
            }
            var ListKPITask = unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == id).ToList();
            var ListAssignIndicator = unitOfWork.Set<HRKPIAssignIndicator>().Where(w => w.AssignCode == id).ToList();
            ClsKPICalculate KPI_Cal = new ClsKPICalculate();
            decimal Score = 0;
            int Indicator = 0;
            int CountTask = 0;
            decimal TotalAcheivement = 0;
            foreach (var group in ListAssignIndicator)
            {
                Indicator++;
                CountTask = 0;
                Score = 0;
                group.Weight = 0;
                foreach (var item in ListKPITask.Where(w => w.Indicator == group.Indicator).ToList())
                {
                    CountTask++;
                    item.Acheivement = 0;
                    item.Actual = 0;
                    item.Score = 0;
                    group.Weight += item.Weight;
                    var ListTrack = ListKPITracking.Where(w => w.KPI == item.ItemCode).ToList();
                    if (item.Target > 0)
                    {
                        if (ListTrack.Sum(w => w.Actual) > 0)
                        {
                            decimal Amount = 0;
                            if (item.Options == "AVERAGE")
                            {
                                Amount = ClsRounding.Rounding(ListTrack.Where(w => w.Actual > 0).Average(w => w.Actual), 4, "N");
                            }
                            else if (item.Options == StatusMeasure.Latest.ToString())
                            {
                                Amount = ListTrack.OrderByDescending(w => w.DocumentDate).FirstOrDefault().Actual;
                            }
                            else
                            {
                                Amount = ListTrack.Sum(w => w.Actual);
                            }

                            item.Actual = Amount;
                        }
                        ClsAppraiselCalculate KPI = new ClsAppraiselCalculate();
                        KPI.Target = item.Target.Value;
                        KPI.Weight = item.Weight;
                        KPI.Actual = item.Actual.Value;
                        var Sccore = KPI.CalculateScore(item.Symbol);
                        item.Score = Sccore;
                        item.Acheivement = KPI.Achievement;
                    }
                    Score += item.Acheivement.Value;

                    unitOfWork.Update(item);
                }
                decimal temScore = Score / CountTask;
                decimal tempTotal = ClsRounding.RoundNormal(temScore, 4);
                group.Acheivement = tempTotal;
                group.Score = ListKPITask.Where(w => w.Indicator == group.Indicator).Sum(x => x.Score);
                unitOfWork.Update(group);
                TotalAcheivement += tempTotal;
            }

            if (ASHeader != null)
            {
                if (ListAssignIndicator.Count > 0)
                {
                    if (KPIType != null)
                    {
                        ASHeader.KPIAverage = KPIType.KPIAverage;
                    }
                    ASHeader.TotalAchievement = ClsRounding.Rounding(TotalAcheivement / ListAssignIndicator.Count(), 4, "N");
                    ASHeader.TotalScore = ListAssignIndicator.Sum(w => w.Score);
                    ASHeader.FinalResult = ASHeader.TotalScore * ASHeader.KPIAverage;
                    ASHeader.Grade = "";
                    if (ASHeader.TotalScore > 0)
                    {
                        ClsKPICalculate KPIGrade = new ClsKPICalculate();
                        ASHeader.Grade = KPIGrade.Get_GradeKPI(ASHeader.TotalScore.Value);
                    }
                }
                unitOfWork.Update(ASHeader);
            }
            unitOfWork.Save();
            //ClsAppraisel appraisel = new ClsAppraisel();
            //appraisel.Calculate(id);
            return SYConstant.OK;
        }
        public Dictionary<string, dynamic> OnDataSelectorKPI(params object[] keys)
        {
            string Department = (string)keys[0];
            int InYear = (int)keys[1];
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            string Status = SYDocumentStatus.RELEASED.ToString();
            var LIST_Indicator = (from Plan in unitOfWork.Set<HRKPIPlanHeader>()
                                  join Item in unitOfWork.Set<HRKPIPlanItem>()
                                  on Plan.Code equals Item.KPIPlanCode
                                  join Indicator in unitOfWork.Set<HRKPIIndicator>()
                                  on Item.Indicator equals Indicator.Code
                                  where Plan.InYear == InYear && Plan.Status == Status
                                  && ((string.IsNullOrEmpty(Indicator.Department) || Indicator.Department == Department) && Indicator.IsActive == true)
                                  select Indicator
                            ).ToList();

            keyValues.Add("KPI_LIST", unitOfWork.Set<HRKPIList>().Where(w => w.IsActive == true && string.IsNullOrEmpty(w.Department) || w.Department == Department).ToList());
            keyValues.Add("LIST_Indicator", LIST_Indicator);
            //keyValues.Add("LIST_Indicator", unitOfWork.Set<HRKPIIndicator>().Where(w => (string.IsNullOrEmpty(w.Department) || w.Department == Department) && w.IsActive == true).ToList());
            //KPI_Department = Department;
            if (Department == null)
            {
                Department = "";
            }
            HttpContext.Current.Session[SYConstant.KPI_DEPARTMENT] = Department;
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("KPI_TYPEINPUT", ClsKPI_InputType.LoadDataKPIInput());
            keyValues.Add("LIST_KPICATEGORY", unitOfWork.Set<HRKPICategory>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("KPI_Measure", ClsMeasure.LoadDataMeasure());
            keyValues.Add("KPI_Symbol", ClsMeasure.LoadDataSymbol());
            keyValues.Add("KPI_OPTION", ClsMeasure.LoadDataOption());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataListLoading(string Department, int InYear, bool ESS = false)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            string OPEN = SYDocumentStatus.OPEN.ToString();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            var ListKPIPending = (from EmpAPP in unitOfWork.Set<HREmpAppraisal>()
                                  join Staff in unitOfWork.Set<HRStaffProfile>()
                                  on EmpAPP.EmpCode equals Staff.EmpCode
                                  where !string.IsNullOrEmpty(EmpAPP.KPIType) && Staff.DEPT == Department
                                        && EmpAPP.KPIStatus == OPEN && EmpAPP.Status == PENDING
                                        && EmpAPP.KPICategory != null && EmpAPP.InYear == InYear
                                  select EmpAPP).ToList();

            if (ESS == true)
            {
                string UserName = User.UserName;
                var ListStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.APPTracking == UserName).ToList();
                ListKPIPending = ListKPIPending.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }

            keyValues.Add("LIST_Appraisal", ListKPIPending.ToList());
            return keyValues;
        }
        public static IEnumerable<HRKPIList> GetKPIList()
        {
            string Department = HttpContext.Current.Session[SYConstant.KPI_DEPARTMENT].ToString();
            string INDICATOR = "";
            if (HttpContext.Current.Session[SYConstant.KPI_INDICATOR] != null)
            {
                INDICATOR = HttpContext.Current.Session[SYConstant.KPI_INDICATOR].ToString();
            }
            var DBR = new HumicaDBContext();
            var dataRead = DBR.HRKPILists.Where(w => w.IsActive == true && w.Indicator == INDICATOR && (string.IsNullOrEmpty(w.Department) || w.Department == Department)).ToList();
            //var dataRead = DBR.HRKPILists.ToList();
            return dataRead;
        }
        public ClsStaff GetEmpAppraisal(string AppraisalNo)
        {
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            ClsStaff objStaff = new ClsStaff();
            var EmpAppraisal = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == AppraisalNo);
            if (EmpAppraisal == null)
            {
                EmpAppraisal = new HREmpAppraisal();
            }

            var Staffpro = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpAppraisal.EmpCode);
            if (Staffpro != null)
            {
                var Staff_pro = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staffpro.APPTracking);
                if (Staff_pro != null)
                {
                    objStaff.EmpCode = Staff_pro.EmpCode;
                    objStaff.EmployeeName = Staff_pro.AllName;
                    objStaff.Position = filterStaff.Get_Positon(Staff_pro.JobCode);
                }
            }
            return objStaff;
        }
        public Dictionary<string, dynamic> OnDataListByDept(params object[] keys)
        {
            string Department = (string)keys[0];
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            string RELEASED = SYDocumentStatus.RELEASED.ToString();
            string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
            string PENDING = SYDocumentStatus.PENDING.ToString();

            keyValues.Add("Staff_KPI", unitOfWork.Set<HR_STAFF_VIEW>().Where(w => w.DEPT == Department && w.StatusCode == "A").ToList());
            keyValues.Add("STAFF_LIST", unitOfWork.Set<HREmpAppraisal>().Where(w => w.KPIStatus != RELEASED && w.KPIStatus != COMPLETED
            && w.KPIStatus != PENDING && !string.IsNullOrEmpty(w.KPIType)).ToList());

            return keyValues;
        }
        public void NewIndicator(HRKPIAssignIndicator D, HRKPIAssignItem S, List<HRKPIIndicator> ListInd)
        {
            D.AssignCode = S.AssignCode;
            D.Indicator = S.Indicator;
            D.Weight = S.Weight;
            D.Acheivement = 0;
            if (ListInd.Where(w => w.Code == S.Indicator).Any())
            {
                D.IndicatorType = ListInd.FirstOrDefault(w => w.Code == S.Indicator).Description;
            }
        }

    }
    public class ListAssignItem
    {
        public string AssignCode { get; set; }
        public string KPIType { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public string HandlePerson { get; set; }
        public string Status { get; set; }
        public string ItemCode { get; set; }
        public string KPI { get; set; }
        public string Measure { get; set; }
        public decimal Weight { get; set; }
        public decimal Traget { get; set; }
        public decimal Actual { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Achievement { get; set; }
    }
    public class ClsTaskSummary
    {
        public string AssignCode { get; set; }
        public string KPIType { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public string HandlePerson { get; set; }
        public string Status { get; set; }
        public string ItemCode { get; set; }
        public string KPI { get; set; }
        public string Measure { get; set; }
        public decimal Weight { get; set; }
        public decimal Traget { get; set; }
        public decimal Actual { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Achievement { get; set; }
    }
    public enum SYDocumentASSIGN
    {
        Individual, BYTEAM
    }
}
