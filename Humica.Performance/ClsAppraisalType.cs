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

namespace Humica.Performance
{
    public class ClsAppraisalType : IClsAppraisalType
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public HRApprType Header { get; set; }
        public List<HRApprType> ListAppraisalType { get; set; }
        public List<HRApprRegion> ListApprRegion { get; set; }
        public List<HRApprFactor> ListApprFactor { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsAppraisalType()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public string ValidateFactor()
        {
            var Vfactor = ListApprFactor.Where(w => !ListApprRegion.Where(x => x.Code == w.Region).Any()).ToList();
            if (Vfactor.Count > 0)
            {
                return "INVALID_REGION_WITH_CRITERIA";
            }
            return SYConstant.OK;
        }
        public void OnIndexLoading()
        {
            ListAppraisalType = unitOfWork.Set<HRApprType>().ToList();
        }
        public void OnCreatingLoading(params object[] keys)
        {
            Header = new HRApprType();
            ListApprRegion = new List<HRApprRegion>();
            ListApprFactor = new List<HRApprFactor>();
        }
        public string Create()
        {
            OnLoad();
            try
            {
                if(string.IsNullOrEmpty( Header.Code))
                {
                    return "CODE";
                }
                var objPortal = unitOfWork.Set<HRApprType>().FirstOrDefault(w => w.Code == Header.Code);
                if (objPortal != null)
                {
                    return "IVALID_APPRAISAL";
                }
                var sms = ValidateFactor();
                if (sms != SYConstant.OK)
                {
                    return sms;
                }
                foreach (var item in ListApprRegion)
                {
                    item.AppraiselType = Header.Code;
                    unitOfWork.Add(item);
                    foreach (var Factor in ListApprFactor)
                    {
                        Factor.AppraiselType= Header.Code;
                        unitOfWork.Add(Factor);
                    }
                }

                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string Code = (string)keys[0];
            Header = unitOfWork.Set<HRApprType>().FirstOrDefault(w => w.Code == Code);
            if (Header != null)
            {
                ListApprRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Code).ToList();
                ListApprFactor = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Code).ToList();
            }
        }
        public string Update(string Code)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRApprType>().FirstOrDefault(w =>                          w.Code == Code);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                var sms = ValidateFactor();
                if (sms != SYConstant.OK)
                {
                    return sms;
                }
                var _ListRegion = unitOfWork.Set<HRApprRegion>().Where(w => w.AppraiselType == Code).ToList();
                foreach (var item in _ListRegion)
                {
                    unitOfWork.Delete(item);
                }
                var _ListFactor = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == Code).ToList();
                foreach (var item in _ListFactor)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListApprRegion)
                {
                    item.AppraiselType = Header.Code;
                    unitOfWork.Add(item);
                    foreach (var Factor in ListApprFactor)
                    {
                        Factor.AppraiselType = Header.Code;
                        unitOfWork.Add(Factor);
                    }
                }
                objMatch.Description = Header.Description;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string OnGridModifyRegion(HRApprRegion MModel, string Action)
        {
            try
            {
                if (string.IsNullOrEmpty(MModel.Code))
                {
                    return "CODE";
                }
                if (!MModel.InOrder.HasValue)
                {
                    MModel.InOrder = 0;
                }
                if (Action == "ADD")
                {
                    var existingEntity = ListApprRegion.FirstOrDefault(w => w.Code == MModel.Code);
                    if (existingEntity != null)
                    {
                        return "CODE_EXISTS";
                    }
                    MModel.Code = MModel.Code.Trim();
                    ListApprRegion.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var existingEntity = ListApprRegion.FirstOrDefault(w => w.Code == MModel.Code);
                    if (existingEntity != null)
                    {
                        existingEntity.Description = MModel.Description;
                        existingEntity.SecDescription = MModel.SecDescription;
                        existingEntity.Remark = MModel.Remark;
                        existingEntity.InOrder = MModel.InOrder;
                        existingEntity.IsKPI = MModel.IsKPI;
                        existingEntity.Rating = MModel.Rating;
                    }
                }
                else if (Action == "DELETE")
                {

                    var objCheck = ListApprRegion.FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        ListApprRegion.Remove(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OnGridModifyFactor(HRApprFactor MModel, string Action)
        {
            try
            {
                if (string.IsNullOrEmpty(MModel.Code))
                {
                    return "CODE";
                }
                //if (!MModel.InOrder.HasValue)
                //{
                //    MModel.InOrder = 0;
                //}
                if (Action == "ADD")
                {
                    MModel.Code = MModel.Code.Trim();
                    var existingEntity = ListApprFactor.FirstOrDefault(w => w.Code == MModel.Code);
                    if (existingEntity != null)
                    {
                        return "CODE_EXISTS";
                    }
                    ListApprFactor.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    var existingEntity = ListApprFactor.FirstOrDefault(w => w.Code == MModel.Code);
                    if (existingEntity != null)
                    {
                        existingEntity.Description = MModel.Description;
                        existingEntity.SecDescription = MModel.SecDescription;
                        existingEntity.Remark = MModel.Remark;
                        //existingEntity.InOrder = MModel.InOrder;
                    }
                }
                else if (Action == "DELETE")
                {

                    var objCheck = ListApprFactor.FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        ListApprFactor.Remove(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading()
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("REGION_SELECT", unitOfWork.Set<HRApprRegion>().ToList());
            return keyValues;
        }
    }
}
