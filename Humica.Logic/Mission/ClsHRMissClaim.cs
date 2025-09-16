using Humica.Core;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.HR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Mission
{
    public class ClsHRMissClaim : IClsHRMissClaim
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HRMissionClaim Header { get; set; }
        public HRMissMemo HeaderMemo { get; set; }
        public HRMissionPlan HeaderPlan { get; set; }
        public List<HRMissionClaim> ListHeader { get; set; }
        public List<HRMissionClaimItem> ListItem { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        //public List<ClsListClaimPending> ListPlans { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public string MessageCode { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsHRMissClaim()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void SetAutoApproval(string DocType, string Branch, string Department, string SCREEN_ID)
        {
            ListApproval = new List<ExDocApproval>();
            ClsDocApproval docApproval = new ClsDocApproval();
            ListApproval = docApproval.SetAutoApproval(SCREEN_ID, DocType, Branch, Department);
        }

        public string CreateMissClaim()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {


                if (string.IsNullOrEmpty(Header.EmpCode)) return "INV_RERQUESTER";
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (staff == null) return "INV_STAFF";
                if (!ListItem.Any()) return "INVALIDED_Item";
                //if (!ListApproval.Any()) return "INVALIDED_APPROVER";
                if (string.IsNullOrEmpty(Header.ClaimType)) return "INVALIDED_MISTYPE";
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId && w.DocType == Header.ClaimType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_N";
                }
                var objNumber = new CFNumberRank(objCF.NumberRank, staff.CompanyCode, Header.ClaimDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.ClaimCode = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.FromDate = Header.FromDate;
                Header.ToDate = Header.ToDate;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                if (ListItem.Any())
                {
                    ListItem.ToList().ForEach(h =>
                    {
                        h.ClaimCode = Header.ClaimCode;
                    });
                    unitOfWork.BulkInsert(ListItem);

                }
                ListApproval.ToList().ForEach(h =>
                {
                    h.DocumentNo = Header.MissionCode;
                    h.DocumentType = Header.ClaimType;
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
        public string UpdateMClaim(string DocNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                if (ObjMatch == null) return "DOC_INV";
                if (!ListItem.Any()) return "INVALIDED_MEMBER";

                var checkdItem = unitOfWork.Set<HRMissionClaimItem>().AsQueryable().Where(w => w.ClaimCode == DocNo).ToList();
                if (checkdItem.Any())
                    unitOfWork.BulkDelete(checkdItem);

                //ADD & Update
                ListItem.ToList().ForEach(h =>
                {
                    h.ClaimCode = Header.ClaimCode;
                });
                unitOfWork.BulkInsert(ListItem);
                ObjMatch.FromDate = Header.FromDate;
                ObjMatch.ToDate = Header.ToDate;
                ObjMatch.ClaimType = Header.ClaimType;
                ObjMatch.Branch = Header.Branch;
                ObjMatch.EmpCode = Header.EmpCode;
                ObjMatch.EmployeeName = Header.EmployeeName;
                ObjMatch.Position = Header.Position;
                ObjMatch.TotalAmount = Header.TotalAmount;
                ObjMatch.Position = Header.Position;
                ObjMatch.WorkingPlan = Header.WorkingPlan;
                ObjMatch.Remark = Header.Remark;
                ObjMatch.MissionCode = Header.MissionCode;
                ObjMatch.AssignFinance = Header.AssignFinance;
                ObjMatch.SummitTo = Header.SummitTo;
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
        public string deleteMClaim(string DocNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                if (ObjMatch == null)
                    return "DOC_INV";

                var checkdMem = unitOfWork.Set<HRMissionClaimItem>().AsQueryable().Where(w => w.ClaimCode == DocNo).ToList();
                if (checkdMem.Any())
                    unitOfWork.BulkDelete(checkdMem);

                var approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == DocNo && w.DocumentType == ObjMatch.ClaimType).ToList();
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
                var listApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.TravelBy
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
                    var objmatch = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                    if (objmatch == null)
                        return "INV_EN";
                    var queryable = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                        .Where(x => x.DocumentNo == objmatch.MissionCode
                            && x.DocumentType == objmatch.ClaimType && x.Approver == User.UserName);
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
                var objmatch = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == ApprovalID);
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


    }
    public class ClsListclaimPending
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