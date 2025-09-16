using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.HR
{
    public class HREmpContObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpContract Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public HRContractType EduType { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<HRContractType> ListEduType { get; set; }
        public List<HREmpContract> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<EmpContract> ListContract { get; set; }
        public HREmpContObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<List<EmpContract>> LoadData()
        {
            var _list = new List<EmpContract>();

            var empCon = DB.HREmpContracts.ToList();

            List<HRStaffProfile> _listTemp = new List<HRStaffProfile>();
            List<HRCompany> _Listcomapay = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchids = _ListBranch.Select(s => s.Code).ToList();

            _listTemp = await new HumicaDBContext().HRStaffProfiles.Where(w => branchids.Contains(w.Branch)).ToListAsync();
            _listTemp = _listTemp.Where(w => _Listcomapay.Where(x => x.Company == w.CompanyCode).Any()).ToList();
            _listTemp = _listTemp.Where(w => _ListLevel.Where(x => x.Code == w.LevelCode).Any()).ToList();

            var ConType = DB.HRContractTypes.ToList();
            var resu = (from edu in empCon
                        join emp in _listTemp on edu.EmpCode equals emp.EmpCode
                        join ET in ConType on edu.ConType equals ET.Code
                        select new
                        {
                            edu.TranNo,
                            edu.EmpCode,
                            emp.AllName,
                            ContractType = ET.Description,
                            edu.FromDate,
                            edu.ToDate,
                            Remark = edu.Description
                        }).ToList();
            foreach (var item in resu)
            {
                var _empCon = new EmpContract();
                _empCon.TranNo = item.TranNo;
                _empCon.EmpCode = item.EmpCode;
                _empCon.AllName = item.AllName;
                _empCon.ContractType = item.ContractType;
                if (item.FromDate.HasValue)
                    _empCon.FromDate = item.FromDate.Value;
                if (item.ToDate.HasValue)
                    _empCon.ToDate = item.ToDate.Value;
                _empCon.Remark = item.Remark;
                _list.Add(_empCon);
            }

            return _list;
        }
        public string CreateEmpCon()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.ConType == null) return "CONTYPE_EN";
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreateBy = User.UserName;
                Header.CreateOn = DateTime.Now;

                DB.HREmpContracts.Add(Header);
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
        public string EditEmpCon(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }

                objMatch.ConType = Header.ConType;
                objMatch.Conterm = Header.Conterm;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Description = Header.Description;
                objMatch.ContractPath = Header.ContractPath;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.HREmpContracts.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderStaff.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEmpCon(int id)
        {
            try
            {
                Header = new HREmpContract();
                var objMatch = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "CONTRACT_NE";
                }
                Header.TranNo = id;
                DB.HREmpContracts.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.TranNo.ToString();
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
                log.ScreenId = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string upload()
        {
            try
            {
                if (ListHeader.Count == 0)
                {
                    return "NO_DATA";
                }
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    var _list = new List<HREmpContract>();
                    List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                    var HREmpContract = DB.HREmpContracts;
                    _list = HREmpContract.ToList();
                    var Employee = DB.HRStaffProfiles;
                    _listStaff = Employee.ToList();

                    var date = DateTime.Now;
                    foreach (var staffs in ListHeader.ToList())
                    {
                        Header = new HREmpContract();
                        var EmpCode = _listStaff.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                        Header.EmpCode = "";
                        if (EmpCode.Count <= 1)
                        {
                            if (EmpCode.Count == 1)
                            {
                                Header.EmpCode = EmpCode.FirstOrDefault().EmpCode;
                            }
                            Header.ConType = staffs.ConType;
                            Header.Description = staffs.Description;
                            Header.FromDate = staffs.FromDate;
                            Header.ToDate = staffs.ToDate;
                            Header.CreateBy = User.UserName;
                            Header.CreateOn = DateTime.Now;
                            DB.HREmpContracts.Add(Header);
                        }
                    }

                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                }
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
    }
    public class EmpContract
    {
        public long TranNo { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string ContractType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Remark { get; set; }
    }
}