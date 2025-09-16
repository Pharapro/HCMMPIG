using Humica.Core;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic
{
    public class ClsMyteamPlan : IClsMyteamPlan
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HRMissionPlan Header { get; set; }
        public List<HRMissionPlan> ListHeader { get; set; }
        public List<HRMissionPlanMem> ListMember { get; set; }
        public List<HRMissProvince> ListProvince { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<ClsMyteamPlanPending> ListPending { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public string MessageCode { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsMyteamPlan()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void LoadData(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var listobj = unitOfWork.Set<HRMissionPlan>().AsQueryable()
                .Where(w => w.Status == pending)
                .ToList();
            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listobj.AsEnumerable().Where(x => w.DocumentNo == x.MissionCode
                    && w.DocumentType == x.MissionType && w.Approver == userName).Any())
                .ToList();
            foreach (var item in listApp)
            {
                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listobj.Where(w => w.MissionCode == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listobj.Where(w => w.MissionCode == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
            }
            var ListLeaveCreater = unitOfWork.Set<HRMissionPlan>().AsQueryable().AsEnumerable()
                    .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                        .Any(w => x.MissionCode == w.DocumentNo
                                   && x.MissionType == w.DocumentType && w.Approver == userName && x.Status != open && x.Status != Cancel)
                        || x.CreatedBy == userName)
                    .ToList();
            ListHeader = ListLeaveCreater;
        }
        private void AddPendingToList(ExDocApproval item, List<HRMissionPlan> listobj)
        {
            foreach (var read in listobj)
            {
                var empSupense = listobj.FirstOrDefault(w => w.MissionCode == item.DocumentNo);
                if (empSupense != null)
                {
                    var ListPen = new ClsMyteamPlanPending()
                    {
                        MissionCode = empSupense.MissionCode,
                        EmpCode = empSupense.EmpCode,
                        PlannerName = empSupense.PlannerName,
                        Position = empSupense.Position,
                        MissionType = empSupense.MissionType,
                        TravelBy = empSupense.TravelBy,
                        Status = empSupense.Status,
                    };

                    ListPending.Add(ListPen);
                }
            }
        }
        public string CreateMissPlan()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(Header.EmpCode)) return "INV_RERQUESTER";
                if (string.IsNullOrEmpty(Header.TravelBy)) return "INV_TRAVELBY";
                if (Header.Credit == true && Header.TotalAmount <= 0) return "INV_TOTAL_AMOUNT";
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (staff == null) return "INV_STAFF";
                if (!ListMember.Any()) return "INVALIDED_MEMBER";
                if (!ListProvince.Any()) return "INVALIDED_PROVINCE";
                if (!ListApproval.Any()) return "INVALIDED_APPROVER";
                if (string.IsNullOrEmpty(Header.MissionType)) return "INVALIDED_MISTYPE";
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId && w.DocType == Header.MissionType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_N";
                }
                var objNumber = new CFNumberRank(objCF.NumberRank, staff.CompanyCode, Header.PlanDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.FromDate = ListProvince.Min(p => p.FromDay);
                Header.ToDate = ListProvince.Max(p => p.ToDay);
                Header.MissionCode = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                if (ListMember.Any())
                {
                    ListMember.ToList().ForEach(h =>
                    {
                        h.MissionCode = Header.MissionCode;
                    });
                    unitOfWork.BulkInsert(ListMember);

                }
                if (ListProvince.Any())
                {
                    ListProvince.ToList().ForEach(h =>
                    {
                        h.MissionCode = Header.MissionCode;
                    });
                    unitOfWork.BulkInsert(ListProvince);
                }
                ListApproval.ToList().ForEach(h =>
                {
                    h.DocumentNo = Header.MissionCode;
                    h.DocumentType = Header.MissionType;
                    h.Status = SYDocumentStatus.OPEN.ToString();
                    h.WFObject = "MISSION";
                    h.WFObject = "MP01";
                    h.ApprovedBy = "";
                    h.ApprovedName = "";
                    h.LastChangedDate = DateTime.Now;
                });
                unitOfWork.BulkInsert(ListApproval);
                unitOfWork.Add(Header);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string UpdateMPlan(string DocNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (ObjMatch == null) return "DOC_INV";
                if (!ListMember.Any()) return "INVALIDED_MEMBER";
                if (!ListProvince.Any()) return "INVALIDED_PROVINCE";
                //delete
                var checkdProvince = unitOfWork.Set<HRMissProvince>().AsQueryable().Where(w => w.MissionCode == DocNo).ToList();
                if (checkdProvince.Any())
                    unitOfWork.BulkDelete(checkdProvince);

                var checkdMem = unitOfWork.Set<HRMissionPlanMem>().AsQueryable().Where(w => w.MissionCode == DocNo).ToList();
                if (checkdMem.Any())
                    unitOfWork.BulkDelete(checkdMem);

                //ADD & Update
                ListMember.ToList().ForEach(h =>
                {
                    h.MissionCode = Header.MissionCode;
                });
                unitOfWork.BulkInsert(ListMember);

                ListProvince.ToList().ForEach(h =>
                {
                    h.MissionCode = Header.MissionCode;
                });
                unitOfWork.BulkInsert(ListProvince);

                ObjMatch.FromDate = ListProvince.Min(p => p.FromDay);
                ObjMatch.ToDate = ListProvince.Max(p => p.ToDay);
                ObjMatch.Member = Header.Member;
                ObjMatch.PlannerName = Header.PlannerName;
                ObjMatch.Province = Header.Province;
                ObjMatch.TravelBy = Header.TravelBy;
                ObjMatch.Credit = Header.Credit;
                ObjMatch.Distance = Header.Distance;
                ObjMatch.Position = Header.Position;
                ObjMatch.MissionType = Header.MissionType;
                ObjMatch.TotalAmount = Header.TotalAmount;
                ObjMatch.Description = Header.Description;
                ObjMatch.WorkingPlan = Header.WorkingPlan;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string deleteMPlan(string DocNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (ObjMatch == null)
                    return "DOC_INV";

                var checkdProvince = unitOfWork.Set<HRMissProvince>().AsQueryable().Where(w => w.MissionCode == DocNo).ToList();
                if (checkdProvince.Any())
                    unitOfWork.BulkDelete(checkdProvince);

                var checkdMem = unitOfWork.Set<HRMissionPlanMem>().AsQueryable().Where(w => w.MissionCode == DocNo).ToList();
                if (checkdMem.Any())
                    unitOfWork.BulkDelete(checkdMem);

                var approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == DocNo && w.DocumentType == ObjMatch.MissionType).ToList();
                if (approver.Any())
                    unitOfWork.BulkDelete(approver);

                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string requestToApprove(string DocNo)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (objMatch == null)
                    return "REQUEST_NE";
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string approveTheDoc(string DocNo)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (objMatch == null)
                    return "REQUEST_NE";
                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                    return "INV_DOC";
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.MissionType
                                    && w.DocumentNo == objMatch.MissionCode && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == User.UserName).ToList();
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
                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
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
                var status = SYDocumentStatus.APPROVED.ToString();
                if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                {
                    status = SYDocumentStatus.PENDING.ToString();
                }

                objMatch.Status = status;
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Reject(string DocNo)
        {
            OnLoad();
            try
            {
                string[] array = DocNo.Split(';');
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (text == "")
                        continue;
                    string status = SYDocumentStatus.REJECTED.ToString();
                    var objmatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == DocNo);
                    if (objmatch == null)
                        return "INV_EN";
                    var queryable = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                        .Where(x => x.DocumentNo == objmatch.MissionCode
                            && x.DocumentType == objmatch.MissionType && x.Approver == User.UserName);
                    foreach (var item in queryable)
                    {
                        item.Status = status;
                        item.LastChangedDate = DateTime.Now;
                        unitOfWork.Update(item);
                    }
                    objmatch.Status = status;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    unitOfWork.Update(objmatch);
                    unitOfWork.Save();
                }
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string CancelDoc(string ApprovalID)
        {
            OnLoad();
            try
            {
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == ApprovalID);
                if (objmatch == null)
                {
                    return "REQUEST_NE";
                }
                objmatch.Status = cancelled;
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SetAutoApproval(string DocType, string Branch, string Department, string SCREEN_ID)
        {
            ListApproval = new List<ExDocApproval>();
            ClsDocApproval docApproval = new ClsDocApproval();
            ListApproval = docApproval.SetAutoApproval(SCREEN_ID, DocType, Branch, Department);
        }
    }
    public class ClsMyteamPlanPending
    {
        public string MissionCode { get; set; }
        public string EmpCode { get; set; }
        public string PlannerName { get; set; }
        public string Position { get; set; }
        public string MissionType { get; set; }
        public string TravelBy { get; set; }
        public string Status { get; set; }
    }
}
