using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training
{
    public class ClsEFSPortal : IClsEFSPortal
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRApprSelfAssessment Header { get; set; }
        public List<HRApprSelfAssessment> ListSelfAssessment { get; set; }
        public List<HRApprSelfAssQCM> ListSelfAssQCM { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsEFSPortal()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListSelfAssessment = unitOfWork.Set<HRApprSelfAssessment>().ToList();
        }
        public void OnCreatingLoading(params object[] keys)
        {
            Header = new HRApprSelfAssessment();
            Header.InOrder = 1;
            Header.IsQCM = false;
            ListSelfAssQCM = new List<HRApprSelfAssQCM>();
        }
        public string Create()
        {
            OnLoad();
            try
            {
                //var objPortal=unitOfWork.Set<EFPortalType>().FirstOrDefault(w=>w.Code==Header.PortalCode);
                //if (objPortal == null)
                //{
                //    return "IVALID_PORTAL";
                //}
                //Header.PortalName = objPortal.Description;
                if (Header.IsQCM == true && ListSelfAssQCM.Count() == 0)
                {
                    return "QCM_REQUIRED";
                }
                int lineItem = 0;
                if (Header.IsQCM == true)
                {
                    foreach (var item in ListSelfAssQCM)
                    {
                        lineItem += 1;
                        item.LineItem = lineItem;
                        item.QuestionCode = Header.QuestionCode;
                        item.PortalCode = Header.PortalCode;
                        unitOfWork.Add(item);
                    }
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Update(string AppraiselType, string QuestionCode)
        {
            OnLoad();
            try
            {
                //var objPortal = unitOfWork.Set<EFPortalType>().FirstOrDefault(w => w.Code == Header.PortalCode);
                //if (objPortal == null)
                //{
                //    return "IVALID_PORTAL";
                //}
                //Header.PortalName = objPortal.Description;
                var objMatch = unitOfWork.Set<HRApprSelfAssessment>().FirstOrDefault(w =>
                          w.QuestionCode == QuestionCode && w.PortalCode == AppraiselType);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                if (Header.IsQCM == true && ListSelfAssQCM.Count() == 0)
                {
                    return "QCM_REQUIRED";
                }
                var _ListQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.QuestionCode == QuestionCode && w.PortalCode == AppraiselType).ToList();
                foreach (var item in _ListQCM)
                {
                    unitOfWork.Delete(item);
                }

                int lineItem = 0;
                if (Header.IsQCM == true)
                {
                    foreach (var item in ListSelfAssQCM)
                    {
                        lineItem += 1;
                        item.LineItem = lineItem;
                        item.QuestionCode = Header.QuestionCode;
                        item.PortalCode = Header.PortalCode;
                        unitOfWork.Add(item);
                    }
                }
                objMatch.PortalName = Header.PortalName;
                objMatch.Description1 = Header.Description1;
                objMatch.Description2 = Header.Description2;
                objMatch.IsQCM = Header.IsQCM;
                objMatch.IsRequired = Header.IsRequired;
                objMatch.InOrder = Header.InOrder;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string PortalCode = (string)keys[0];
            string QuestionCode = (string)keys[1];
            Header = unitOfWork.Set<HRApprSelfAssessment>().FirstOrDefault(w =>
            w.QuestionCode == QuestionCode && w.PortalCode == PortalCode);
            if (Header != null)
            {
                ListSelfAssQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w =>
                              w.QuestionCode == QuestionCode && w.PortalCode == PortalCode).ToList();
            }
        }
        public string Delete(params object[] keys)
        {
            OnLoad();
            string AppraiselType = (string)keys[0];
            string QuestionCode = (string)keys[1];
            try
            {
                var objHeader = unitOfWork.Set<HRApprSelfAssessment>().FirstOrDefault(w => w.QuestionCode == QuestionCode
                && w.PortalCode == AppraiselType);
                if (objHeader == null)
                {
                    return "INV_DOC";
                }
                var objSelfAssQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w =>
                              w.QuestionCode == QuestionCode && w.PortalCode == AppraiselType).ToList();

                foreach (var read in objSelfAssQCM)
                {
                    unitOfWork.Delete(read);
                }
                unitOfWork.Delete(objHeader);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, QuestionCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public virtual string OnGridModify(HRApprSelfAssQCM MModel, string Action)
        {
            if (Action == "ADD")
            {
                if (ListSelfAssQCM.Count == 0)
                {
                    MModel.LineItem = 1;
                }
                else
                {
                    int LineItem = ListSelfAssQCM.Max(w => w.LineItem);
                    MModel.LineItem = LineItem + 1;
                }
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListSelfAssQCM.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListSelfAssQCM.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListSelfAssQCM.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListSelfAssQCM.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            var check = ListSelfAssQCM.Where(w => w.LineItem == MModel.LineItem).ToList();
            if (check.Count == 0)
            {
                ListSelfAssQCM.Add(MModel);
            }

            return SYConstant.OK;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading()
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            //keyValues.Add("PortalType_SELECT", unitOfWork.Set<EFPortalType>().ToList());
            return keyValues;
        }
    }
}