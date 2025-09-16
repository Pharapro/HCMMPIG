using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Performance
{
    public class ClsKPITracking : IClsKPITracking
    {
        public FTINYear FInYear { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string Options { get; set; }
        public string TotalActual { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public HRKPITracking HeaderKPITracking { get; set; }
        public List<ClsTracking> listKPITracking { get; set; }
        public List<ListAssign> ListKPIEmpPending { get; set; }
        public List<HRKPITimeSheet> ListTimeSheet { get; set; }
        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }

        public ClsKPITracking()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public virtual void OnIndexLoading(int INYear,DateTime FromDate,DateTime ToDate, string Status, bool ESS = false)
        {
            listKPITracking = new List<ClsTracking>();
            var TempList = (from kpi in unitOfWork.Repository<HRKPITracking>().Queryable()
                            join timeSheet in unitOfWork.Repository<HRKPITimeSheet>().Queryable()
                            on kpi.TranNo equals timeSheet.ID
                            into tsGroup
                            from timeSheet in tsGroup.DefaultIfEmpty()
							group timeSheet by kpi into g
                            select new
                            {
                                g.Key.TranNo,
                                g.Key.AssignCode,
                                g.Key.DocumentDate,
                                g.Key.EmpCode,
                                g.Key.EmpName,
                                g.Key.Measure,
                                g.Key.KPIDescription,
                                g.Key.Target,
                                g.Key.Actual,
                                g.Key.Position,
                                g.Key.Department,
                                g.Key.Status,
                                g.Key.DirectedByCode,
                                TimeSheets = g.Select(x => x.Description)
                            });
            if (ESS)
            {
                string UserName = User.UserName;
                TempList = TempList.Where(w => w.EmpCode == UserName || w.DirectedByCode == UserName && w.DocumentDate.Year == INYear);
            }
            else
                TempList = TempList.Where(w => w.DocumentDate >= FromDate && w.DocumentDate <= ToDate);
            if (!string.IsNullOrEmpty(Status))
            {
                TempList = TempList.Where(w => w.Status == Status);
            }

            listKPITracking = TempList.ToList().Select(x => new ClsTracking
                {
                    TranNo = x.TranNo,
                    AssignCode = x.AssignCode,
                    DocumentDate = x.DocumentDate,
                    EmpCode = x.EmpCode,
                    EmpName = x.EmpName,
                    Measure = x.Measure,
                    KPIDescription = x.KPIDescription,
                    Target = x.Target,
                    Actual = x.Actual,
                    Position = x.Position,
                    Department = x.Department,
                    Status = x.Status,
                    TimeSheet = string.Join("\n", x.TimeSheets.Where(desc => desc != null).Select(desc => "- " + desc))
                }).ToList();
            listKPITracking= listKPITracking.OrderByDescending(w => w.DocumentDate).ToList();
        }

        public virtual List<ListAssign> OnIndexLoadingAssign(bool ESS = false)
        {
            List<HRKPIAssignHeader> TempList = new List<HRKPIAssignHeader>();
            List<HRKPIAssignMember> TempListMember = new List<HRKPIAssignMember>();
            string Approved = SYDocumentStatus.APPROVED.ToString();
            DateTime Deadline = DateTime.Now;
            var ListEmpKPI = unitOfWork.Repository<HRKPIAssignHeader>().Queryable().Where(w => w.ReStatus == Approved);
            if (ESS)
            {
                string UserName = User.UserName;
                TempList = ListEmpKPI.Where(w => w.AssignedBy != "BYTEAM" && (w.HandleCode == UserName)).ToList();
                var Staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => 
                (w.DEPT == "PLT" || w.DEPT == "FB" || w.DEPT == "RTD" ||
                w.DEPT == "WHD" || w.DEPT == "MART") && w.EmpCode == UserName);
                if (Staff != null && Staff.Branch != "HQ")
                {
                    var TempListTe = (from KPI in unitOfWork.Repository<HRKPIAssignHeader>().Queryable()
                                      join _Staff in unitOfWork.Repository<HRStaffProfile>().Queryable()
                                      on KPI.HandleCode equals _Staff.EmpCode
                                      where (_Staff.DEPT == "PLT" || _Staff.DEPT == "FB" 
                                      || _Staff.DEPT == "RTD" || _Staff.DEPT == "WHD" 
                                      || _Staff.DEPT == "MART")
                                      && KPI.ReStatus == Approved && KPI.AssignedBy != "BYTEAM"
                           && _Staff.APPTracking == Staff.EmpCode
                                      select KPI
                     );
                    TempList.AddRange(TempListTe.ToList());
                }

            }
            else
            {
                TempList = ListEmpKPI.ToList();
            }
            TempListMember = unitOfWork.Repository<HRKPIAssignMember>().Queryable().ToList();
            // Retrieve assignments
            var assignments = (from assign in TempList
                               select new ListAssign
                               {
                                   AssignCode = assign.AssignCode,
                                   EmpCode = assign.HandleCode,
                                   EmployeeName = assign.HandleName,
                                   Position = assign.Position,
                                   Department = assign.Department,
                                   KPIType = assign.KPIType,
                                   PeriodFrom = assign.PeriodFrom.Value,
                                   PeriodTo = assign.PeriodTo.Value,
                                   Status = assign.AssignedBy,
                                   HandlePerson = assign.HandleCode + " : " + assign.HandleName
                               }).ToList();

            var result = new List<ListAssign>();
            if (ESS == true)// && TempList.Count() == 0)
            {
                string UserName = User.UserName;
                var TempListTe = ListEmpKPI.Where(w => w.ReStatus == Approved
                 && w.AssignedBy == "BYTEAM" && (w.PlanerCode == UserName || w.HandleCode == UserName));
                if (TempListTe.Count() == 0)
                {
                    TempListMember = TempListMember.Where(x => x.EmpCode == UserName).ToList();
                    var departmentList = TempListMember.Select(w => w.AssignCode).ToList();
                    TempListTe = ListEmpKPI.Where(w => departmentList.Contains(w.AssignCode));
                }
                var assignmentTem = (from assign in TempListTe
                                     select new ListAssign
                                     {
                                         AssignCode = assign.AssignCode,
                                         EmpCode = assign.HandleCode,
                                         EmployeeName = assign.HandleName,
                                         Position = assign.Position,
                                         Department = assign.Department,
                                         KPIType = assign.KPIType,
                                         PeriodFrom = assign.PeriodFrom.Value,
                                         PeriodTo = assign.PeriodTo.Value,
                                         Status = assign.AssignedBy,
                                         HandlePerson = assign.HandleCode + " : " + assign.HandleName
                                     }).ToList();
                assignments.AddRange(assignmentTem);
            }
            foreach (var assignment in assignments)
            {
                if (assignment.Status != SYDocumentASSIGN.BYTEAM.ToString())
                {
                    result.Add(assignment);
                }
                var relatedMembers = TempListMember.Where(m => m.AssignCode == assignment.AssignCode).ToList();
                foreach (var member in relatedMembers)
                {
                    var memberAssignment = new ListAssign
                    {
                        AssignCode = assignment.AssignCode,
                        EmpCode = member.EmpCode,
                        EmployeeName = member.EmployeeName,
                        Position = member.Position,
                        Department = member.Department,
                        KPIType = assignment.KPIType,
                        PeriodFrom = assignment.PeriodFrom,
                        PeriodTo = assignment.PeriodTo,
                        HandlePerson = assignment.HandlePerson,
                        Status = assignment.Status
                    };

                    result.Add(memberAssignment);
                }
            }
            return result;
        }

        public virtual void OnCreatingLoading(string ID, string EmpCode)
        {
            this.HeaderKPITracking = new HRKPITracking();
            ListTimeSheet = new List<HRKPITimeSheet>();
            var Plan = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ID);
            if (Plan != null)
            {
                HeaderKPITracking.EmpCode = Plan.HandleCode;
                HeaderKPITracking.EmpName = Plan.HandleName;
                HeaderKPITracking.Department = Plan.Department;
                HeaderKPITracking.Position = Plan.Position;
                HeaderKPITracking.DirectedByCode = Plan.PlanerCode;
                if (Plan.HandleCode != EmpCode)
                {
                    var members = unitOfWork.Set<HRKPIAssignMember>().FirstOrDefault(w => w.EmpCode == EmpCode);
                    if (members != null)
                    {
                        HeaderKPITracking.EmpCode = members.EmpCode;
                        HeaderKPITracking.EmpName = members.EmployeeName;
                        HeaderKPITracking.Department = members.Department;
                        HeaderKPITracking.Position = members.Position;
                    }
                }
                HeaderKPITracking.AssignCode = Plan.AssignCode;
                HeaderKPITracking.KPIType = Plan.KPIType;
                HeaderKPITracking.DocumentDate = DateTime.Now;
                HeaderKPITracking.Actual = 0;
                GetTimer(EmpCode, HeaderKPITracking.DocumentDate);

            }
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            int TranNo = (int)keys[0];
            this.HeaderKPITracking = unitOfWork.Set<HRKPITracking>().FirstOrDefault(w => w.TranNo == TranNo);
            if (this.HeaderKPITracking != null)
            {
                this.ListTimeSheet = unitOfWork.Set<HRKPITimeSheet>().Where(w => w.ID == TranNo).ToList();
                GetTimer(HeaderKPITracking.EmpCode, HeaderKPITracking.DocumentDate);
            }
        }
        public string Create()
        {
            OnLoad();
            try
            {
                unitOfWork.BeginTransaction();
                try
                {
                    var objTask = unitOfWork.Set<HRKPIAssignItem>().FirstOrDefault(
                        w => w.AssignCode == HeaderKPITracking.AssignCode &&
                        w.ItemCode == HeaderKPITracking.KPI);
                    if (objTask == null)
                    {
                        return "KPI_Required";
                    }
                    if (HeaderKPITracking.Measure == "%" && HeaderKPITracking.Actual > 100)
                    {
                        return "INV_ACTUAL";
                    }
                    if (ListTimeSheet.Count == 0)
                    {
                        return "TIME_SHEET";
                    }
                    var result = Before_IsValidTimeSheet(HeaderKPITracking, ListTimeSheet);
                    if (result != SYConstant.OK)
                    {
                        return result;
                    }
                    HeaderKPITracking.Status = SYDocumentStatus.PENDING.ToString();
                    HeaderKPITracking.CreatedBy = User.UserName;
                    HeaderKPITracking.CreatedOn = DateTime.Now;
                    var Assign = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == HeaderKPITracking.AssignCode);
                    if (Assign != null)
                    {
                        if (Assign.AssignedBy == "BYTEAM")
                        {
                            if (Assign.HandleCode == User.UserName)
                            {
                                HeaderKPITracking.DirectedByCode = Assign.DirectedByCode;
                            }
                            else if (Assign.PlanerCode == User.UserName)
                            {
                                HeaderKPITracking.DirectedByCode = Assign.HandleCode;
                            }
                            else
                            {
                                HeaderKPITracking.DirectedByCode = Assign.PlanerCode;
                            }
                            if (Assign.HandleCode == User.UserName || Assign.PlanerCode == User.UserName)
                            {
                                if (HeaderKPITracking.EmpCode != User.UserName)
                                {
                                    HeaderKPITracking.Status = SYDocumentStatus.APPROVED.ToString();
                                }
                            }
                        }
                    }
                    var StaffPro = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderKPITracking.EmpCode);
                    if (!string.IsNullOrEmpty(HeaderKPITracking.EmpCode))
                    {
                        if (StaffPro != null && StaffPro.IsAutoAppKPITraing == true)
                        {
                            HeaderKPITracking.Status = SYDocumentStatus.APPROVED.ToString();
                        }
                    }
                    unitOfWork.Add(HeaderKPITracking);
                    unitOfWork.Save();
                    int i = HeaderKPITracking.TranNo;
                    int lineItem = 0;
                    ClsCopyFile clsCopyFile = new ClsCopyFile();
                    foreach (var item in ListTimeSheet)
                    {
                        if (string.IsNullOrEmpty(item.Description))
                        {
                            return "DESCRIPTION";
                        }
                        string Attachment = item.Attachment;
                        var objTimeSheet = new HRKPITimeSheet();
                        SwapEmpTimeSheet(objTimeSheet, item, HeaderKPITracking);
                        if (!string.IsNullOrEmpty(Attachment))
                        {
                            var strpath = clsCopyFile.CopyStructurePath(StaffPro.CompanyCode, "TimeSheet", StaffPro.EmpCode, StaffPro.DEPT, Attachment);
                            objTimeSheet.Attachment = strpath;
                        }
                        lineItem += 1;
                        objTimeSheet.ID = i;
                        objTimeSheet.LineItem = lineItem;
                        unitOfWork.Add(objTimeSheet);
                    }
                    unitOfWork.Save();
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                }
                //CLsKPIAssign assign = new CLsKPIAssign();
                //assign.Calculate(HeaderKPITracking.AssignCode);
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
        public string Update(int id)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRKPITracking>().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                if (HeaderKPITracking.Measure == "%" && HeaderKPITracking.Actual > 100)
                {
                    return "INV_ACTUAL";
                }
                if (ListTimeSheet.Count == 0)
                {
                    return "TIME_SHEET";
                }
                var result = Before_IsValidTimeSheet(HeaderKPITracking, ListTimeSheet, id);
                if (result != SYConstant.OK)
                {
                    return result;
                }
                var _ListAgenda = unitOfWork.Set<HRKPITimeSheet>().Where(w => w.ID == id).ToList();
                foreach (var item in _ListAgenda)
                {
                    unitOfWork.Delete(item);
                }

                int lineItem = 0;
                foreach (var item in ListTimeSheet)
                {
                    lineItem += 1;
                    item.ID = id;
                    if (item.FromTime.HasValue)
                    {
                        item.FromTime =item.FromTime.Value;
                    }
                    if (item.ToTime.HasValue)
                    {
                        item.ToTime = item.ToTime.Value;
                    }
                    item.LineItem = lineItem;
                    if (item.FromTime.HasValue && item.ToTime.HasValue)
                    {
                        item.Hours = (decimal)item.ToTime.Value.Subtract(item.FromTime.Value).TotalHours;
                        item.TotalHours = "";
                        if (item.Hours > 0)
                        {
                            TimeSpan time = item.ToTime.Value.Subtract(item.FromTime.Value);
                            if (time.Hours > 0)
                                item.TotalHours = time.Hours + "h ";
                            if (time.Minutes > 0)
                                item.TotalHours += time.Minutes + "min";
                        }
                    }
                    unitOfWork.Add(item);
                }
                objMatch.Actual = HeaderKPITracking.Actual;
                objMatch.Remark = HeaderKPITracking.Remark;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                CLsKPIAssign assign = new CLsKPIAssign();
                assign.Calculate(objMatch.AssignCode);
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, this.HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(params object[] keys)
        {
            try
            {
                OnLoad();
                int id = (int)keys[0];
                var objMatch = unitOfWork.Set<HRKPITracking>().FirstOrDefault(w => w.TranNo == id);

                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                var objListTimeSheet = unitOfWork.Set<HRKPITimeSheet>().Where(w => w.ID == id).ToList();
                foreach (var read in objListTimeSheet)
                {
                    unitOfWork.Delete(read);
                }
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                CLsKPIAssign assign = new CLsKPIAssign();
                assign.Calculate(objMatch.AssignCode);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string DeleteAll(string id)
        {
            try
            {
                OnLoad();
                string[] c = id.Split(';');
                List<string> LstKPI = new List<string>();
                foreach (var tranNo in c)
                {
                    if (string.IsNullOrEmpty(tranNo)) continue;
                    int _TranNo = Convert.ToInt32(tranNo);
                    var ObjMatch = unitOfWork.Set<HRKPITracking>().FirstOrDefault(w => w.TranNo == _TranNo);
                    if (ObjMatch != null)
                    {
                        if (!LstKPI.Where(w => w.Contains(ObjMatch.AssignCode)).Any())
                        {
                            LstKPI.Add(ObjMatch.AssignCode);
                        }
                        var objListTimeSheet = unitOfWork.Set<HRKPITimeSheet>().Where(w => w.ID == _TranNo).ToList();
                        foreach (var read in objListTimeSheet)
                        {
                            unitOfWork.Delete(read);
                        }
                        unitOfWork.Delete(ObjMatch);
                        unitOfWork.Save();
                    }
                }
                CLsKPIAssign assign = new CLsKPIAssign();
                foreach (var ass in LstKPI)
                {
                    assign.Calculate(ass);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderKPITracking.TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Approved(string id, bool ESS = false)
        {
            try
            {
				string[] c = id.Split(';');
                foreach(var tranNo in c)
                {
					if (tranNo == "") continue;
					var ObjMatch = unitOfWork.Set<HRKPITracking>().Where(w => w.TranNo.ToString() == tranNo).FirstOrDefault();
					if (ObjMatch != null)
					{
						if (ESS)
						{
							if (ObjMatch.EmpCode == User.UserName)
							{
								return "USER_NOT_APPROVOR";
							}
						}
						string Approved = SYDocumentStatus.APPROVED.ToString();
						ObjMatch.Status = Approved;
						unitOfWork.Update(ObjMatch);
						unitOfWork.Save();
						CLsKPIAssign assign = new CLsKPIAssign();
						assign.Calculate(ObjMatch.AssignCode);
					}
				}
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Reject(string id,string comment, bool ESS = false)
        {
            try
            {
				string[] c = id.Split(';');
                foreach (var tranNo in c)
                {
					if (tranNo == "") continue;
					var ObjMatch = unitOfWork.Set<HRKPITracking>().Where(w => w.TranNo.ToString() == tranNo).FirstOrDefault();
					if (ObjMatch != null)
					{
						if (string.IsNullOrEmpty(comment))
						{
							return "COMMENT_REQUIRED";
						}
						if (ESS)
						{
							if (ObjMatch.EmpCode == User.UserName)
							{
								return "USER_NOT_APPROVOR";
							}
						}
						ObjMatch.Remark = comment;
						string Rejected = SYDocumentStatus.REJECTED.ToString();
						ObjMatch.Status = Rejected;
						ObjMatch.ChangedBy = User.UserName;
						ObjMatch.ChangedOn = DateTime.Now;
						unitOfWork.Update(ObjMatch);
						unitOfWork.Save();
					}
				}
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SwapEmpTimeSheet(HRKPITimeSheet D, HRKPITimeSheet S, HRKPITracking objHeader)
        {
            D.Description = S.Description;
            D.Remark = S.Remark;
            D.FromTime = S.FromTime;
            D.ToTime = S.ToTime;
            if (D.FromTime.HasValue)
            {
                D.FromTime =  D.FromTime.Value;
            }
            if (D.ToTime.HasValue)
            {
                D.ToTime = D.ToTime.Value;
            }
            if (D.FromTime.HasValue && D.ToTime.HasValue)
            {
                D.Hours = (decimal)D.ToTime.Value.Subtract(D.FromTime.Value).TotalHours;
                if (D.Hours > 0)
                {
                    TimeSpan time = D.ToTime.Value.Subtract(D.FromTime.Value);
                    if (time.Hours > 0)
                        D.TotalHours = time.Hours + "h ";
                    if (time.Minutes > 0)
                        D.TotalHours += time.Minutes + "min";
                }
            }
        }
        public virtual string OnGridModify(HRKPITimeSheet MModel, string Action)
        {            
            if (MModel.FromTime.HasValue && MModel.ToTime.HasValue)
            {
                MModel.FromTime = DateTimeHelper.DateInHourMin(MModel.FromTime.Value);
                MModel.ToTime = DateTimeHelper.DateInHourMin(MModel.ToTime.Value);
                MModel.Hours = (decimal)MModel.ToTime.Value.Subtract(MModel.FromTime.Value).TotalHours;
                if(MModel.Hours < 0)
                {
                    return "INVALID_TIME";
                }
                if (MModel.Hours > 24)
                {
                    return "INVALID_TIME";
                }
            }
            if (Action != "DELETE")
            {
                if (string.IsNullOrEmpty(MModel.Description))
                {
                    return "DESCRIPTION";
                }
                string result = IsValidTimeSheet(MModel, ListTimeSheet);
                if (result != SYConstant.OK) return result;
            }
            if (Action == "ADD")
            {
                if (ListTimeSheet.Count == 0)
                {
                    MModel.LineItem = 1;
                }
                else
                {
                    int LineItem = ListTimeSheet.Max(w => w.LineItem);
                    MModel.LineItem = LineItem + 1;
                }
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListTimeSheet.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListTimeSheet.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListTimeSheet.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListTimeSheet.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            var check = ListTimeSheet.Where(w => w.LineItem == MModel.LineItem).ToList();
            if (check.Count == 0)
            {
                ListTimeSheet.Add(MModel);
            }

            return SYConstant.OK;
        }

        public HRKPIAssignItem GetDataTaskItem(string AssignCode, string KPI)
        {
            HRKPIAssignItem TaskItem = unitOfWork.Set<HRKPIAssignItem>().FirstOrDefault(w => w.AssignCode == AssignCode
            && w.ItemCode == KPI);
            if (TaskItem == null) TaskItem = new HRKPIAssignItem();
            else
            {
                TotalActual = TaskItem.Actual.Value.ToString();
                var listMeasure = ClsMeasure.LoadDataOption();
                var Measure = listMeasure.FirstOrDefault(w => w.Code == TaskItem.Options);
                if (Measure != null) Options = Measure.Description;
                if (!TaskItem.Actual.HasValue)
                {
                    if (TaskItem.Actual.Value == 0) TotalActual = "0";
                }
                else if (TaskItem.Measure == "%")
                    TotalActual = (TaskItem.Actual.Value / 100.00M).ToString("P2");
                else if (TaskItem.Measure == "#")
                {
                    string[] Actual = TaskItem.Actual.Value.ToString().Split('.');
                    if (Actual.Length > 1 && Convert.ToInt64(Actual[1]) == 0)
                    {
                        TotalActual = Actual[0].ToString();
                    }
                }
               

            }
            return TaskItem;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            string AssignCode = (string)keys[0];
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("KPITASK_LIST", unitOfWork.Set<HRKPIAssignItem>().Where(w => w.AssignCode == AssignCode).ToList());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataStatusLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("STATUS_APPROVAL", new SYDataList("STATUS_LEAVE_APPROVAL").ListData.Where(w => w.SelectValue != "CANCELLED"));

            return keyValues;
        }
        public string IsValidTimeSheet(HRKPITimeSheet objCheck,List<HRKPITimeSheet> ListCurrent)
        {
            if (ListCurrent.Where(w => w.LineItem != objCheck.LineItem && objCheck.FromTime < w.ToTime && objCheck.ToTime > w.FromTime).Any())
            {
                return "INVALID_TIME";
            }
            if (ListCurrent.Where(w => objCheck.FromTime == w.FromTime && objCheck.ToTime == w.ToTime).Any())
            {
                return "INVALID_TIME";
            }
            return SYConstant.OK;
        }
        public string Before_IsValidTimeSheet(HRKPITracking objCheck, List<HRKPITimeSheet> ListCurrent, int ID = 0)
        {
            string Rejected = SYDocumentStatus.REJECTED.ToString();
            var LstChecking = (from Tracking in unitOfWork.Repository<HRKPITracking>().Queryable()
                               join Times in unitOfWork.Repository<HRKPITimeSheet>().Queryable()
                               on Tracking.TranNo equals Times.ID
                               where DbFunctions.TruncateTime(Tracking.DocumentDate) == objCheck.DocumentDate.Date
                               && Tracking.EmpCode == objCheck.EmpCode
                               && Tracking.Status != Rejected
                               select Times);
            if (ID != 0)
            {
                LstChecking = LstChecking.Where(w => w.ID != ID);
            }
            foreach (var item in LstChecking)
            {
                if (ListCurrent.Where(w => item.FromTime < w.ToTime && item.ToTime > w.FromTime).Any())
                {
                    return "INVALID_TIME";
                }
                if (ListCurrent.Where(w => item.FromTime == w.FromTime && item.ToTime == w.ToTime).Any())
                {
                    return "INVALID_TIME";
                }
            }
            return SYConstant.OK;
        }
        public (decimal Hours, DateTime From_Time) CalculateHour(DateTime Docment_Date, DateTime FromTime, DateTime ToTime)
        {
            decimal Hours = 0;
            if (FromTime.Date != Docment_Date)
            {
                FromTime = Docment_Date.Date + FromTime.TimeOfDay;
            }
            FromTime = DateTimeHelper.DateInHourMin(FromTime);
            ToTime = DateTimeHelper.DateInHourMin(ToTime);
            Hours = (decimal)ToTime.Subtract(FromTime).TotalHours;
            if (Hours < 0) FromTime = ToTime;
            return ( Hours, FromTime);
        }

        public void GetTimer(string EmpCode, DateTime InDate)
        {
            FromTime = DateTime.Now;
            ToTime = DateTime.Now;
            var Roster = unitOfWork.Repository<ATEmpSchedule>().FirstOrDefault(w => w.EmpCode == EmpCode && w.TranDate == InDate.Date);
            if (Roster != null && Roster.SHIFT != "PH" && Roster.SHIFT != "OFF")
            {
                FromTime = Roster.IN1.Value;
                ToTime = Roster.OUT1.Value;
                if (Roster.Flag2 == 1)
                    ToTime = Roster.OUT2.Value;
            }
        }

        //public string ImportKPITracking(List<Temp_Amount> ListTemp_Amount)
        //{
        //    try
        //    {
        //        var emp = DB.HRStaffProfiles.ToList();
        //        //var TranNo = DB.HRKPITrackings.OrderByDescending(u => u.TranNo).FirstOrDefault();

        //        foreach (var read in ListTemp_Amount)
        //        {
        //            var empcheck = emp.Where(w => w.EmpCode == read.EmpCode).ToList();

        //            if (empcheck.Count() == 0) return "Invalid EmpCode : " + read.EmpCode;

        //            if (read.DocumentDate > read.DocumentDate) return read.DocumentDate + " has invalid DocumentDate";
        //            //if (read.ToDate < read.FromDate) return read.EmpCode + " has invalid Todate";
        //            if (empcheck.Count > 0)
        //            {
        //                var obj = new HRKPITracking();
        //                obj.KPI = read.KPI;
        //                obj.KPIDescription = read.KPIDescription;
        //                obj.EmpCode = read.EmpCode;

        //                obj.CreatedOn = DateTime.Now;
        //                obj.CreatedBy = User.UserName;
        //                obj.DocumentDate = read.DocumentDate;
        //                obj.Actual = read.Amount;
        //                DB.HRKPITrackings.Add(obj);

        //            }
        //            else
        //            {
        //                MessageError = "EmpCode :" + read.EmpCode + " INVALID CODE: ";
        //                return "INV_DOC";
        //            }
        //        }

        //        int row = DB.SaveChanges();

        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        //log.DocurmentAction = HeaderKPITracking.TranNo.ToString();
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
        //        //log.DocurmentAction = HeaderKPITracking.TranNo.ToString();
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
        //        //log.DocurmentAction = HeaderKPITracking.TranNo.ToString();
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}
    }
    public class ListAssign
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
    }

    public class ClsTracking : HRKPITracking
    {
        public string TimeSheet { get; set; }
    }

}
