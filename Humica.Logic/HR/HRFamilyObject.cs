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
    public class HRFamilyObject
    {
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpFamily Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageError { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string MessageCode { get; set; }
        public List<HR_Family_View> ListHeader { get; set; }
        public List<HREmpFamily> ListImport { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public static IQueryable<HR_STAFF_VIEW> ListStaffs
        {
            get
            {
                var DBV = new HumicaDBViewContext();
                var _Branch = SYConstant.getBranchDataAccess();
                var _Level = SYConstant.getLevelDataAccess();
                var list = DBV.HR_STAFF_VIEW.ToList();
                list = list.Where(w => _Branch.Where(x => x.Code == w.BranchID).Any()).ToList();
                list = list.Where(w => _Level.Where(x => x.Code == w.LevelCode).Any()).ToList();
                return list.AsQueryable<HR_STAFF_VIEW>();
            }
        }
        public HRFamilyObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<List<HR_Family_View>> LoadDataFamily()
        {
            List<HRStaffProfile> _listTemp = new List<HRStaffProfile>();
            List<HRCompany> _Listcomapay = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchids = _ListBranch.Select(s => s.Code).ToList();

            _listTemp =await new HumicaDBContext().HRStaffProfiles.Where(w => branchids.Contains(w.Branch)).ToListAsync();
            _listTemp = _listTemp.Where(w => _Listcomapay.Where(x => x.Company == w.CompanyCode).Any()).ToList();
            _listTemp = _listTemp.Where(w => _ListLevel.Where(x => x.Code == w.LevelCode).Any()).ToList();

            ListHeader = await new HumicaDBViewContext().HR_Family_View.ToListAsync();
            ListHeader = ListHeader.Where(w => _listTemp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            return ListHeader;
        }
        public string CreateEmp()
        {
            try
            {
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreateBy = User.UserName;
                Header.CreateOn = DateTime.Now;
                DB.HREmpFamilies.Add(Header);
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
        public string EditEmp(int id)
        {
            try
            {

                var objMatch = DB.HREmpFamilies.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                Header.EmpCode = objMatch.EmpCode;
                objMatch.TaxDeduc = Header.TaxDeduc;
                objMatch.InSchool = Header.InSchool;
                objMatch.RelName = Header.RelName;
                objMatch.Sex = Header.Sex;
                objMatch.IDCard = Header.IDCard;
                objMatch.DateOfBirth = Header.DateOfBirth;
				objMatch.EffectiveDate = Header.EffectiveDate;
				objMatch.ToDate = Header.ToDate;
				objMatch.PhoneNo = Header.PhoneNo;
                objMatch.Occupation = Header.Occupation;
                objMatch.RelCode = Header.RelCode;
                objMatch.Child = Header.Child;
                objMatch.Spouse = Header.Spouse;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.HREmpFamilies.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
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
        public string DeleteFamily(int id)
        {
            try
            {
                Header = new HREmpFamily();
                var objMatch = DB.HREmpFamilies.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                Header.TranNo = id;
                DB.HREmpFamilies.Attach(objMatch);
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
                if (ListImport.Count == 0)
                {
                    return "NO_DATA";
                }
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    var _list = new List<HREmpFamily>();
                    List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                    _list = DB.HREmpFamilies.ToList();
                    _listStaff = DB.HRStaffProfiles.ToList();
                    MessageError = "";
                    var date = DateTime.Now;
                    foreach (var staffs in ListImport.ToList())
                    {
                        Header = new HREmpFamily();
                        var EmpCode = _listStaff.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                        Header.EmpCode = "";
                        if (staffs.Child == true && staffs.Spouse == true)
                        {
                            MessageError = " : " + staffs.EmpCode;
                            return "CH_SP_DUBLICATE";
                        }
                        if (EmpCode.Count <= 1)
                        {
                            if (EmpCode.Count == 1)
                            {
                                Header.EmpCode = EmpCode.FirstOrDefault().EmpCode;
                            }
                            Header.RelCode = staffs.RelCode;
                            Header.RelName = staffs.RelName;
                            Header.Sex = staffs.Sex;
                            Header.DateOfBirth = staffs.DateOfBirth;
                            Header.PhoneNo = staffs.PhoneNo;
                            Header.Occupation = staffs.Occupation;
                            Header.IDCard = staffs.IDCard;
                            Header.TaxDeduc = staffs.TaxDeduc;
                            Header.InSchool = staffs.InSchool;
                            Header.Child = staffs.Child;
                            Header.Spouse = staffs.Spouse;
                            Header.CreateBy = User.UserName;
                            Header.CreateOn = DateTime.Now;
                            DB.HREmpFamilies.Add(Header);
                        }
                        else
                        {
                            foreach (var item1 in EmpCode)
                            {
                                if (item1 != null)
                                {
                                    Header.EmpCode = item1.EmpCode;
                                }
                                Header.RelCode = staffs.RelCode;
                                Header.RelName = staffs.RelName;
                                Header.Sex = staffs.Sex;
                                Header.DateOfBirth = staffs.DateOfBirth;
                                Header.PhoneNo = staffs.PhoneNo;
                                Header.Occupation = staffs.Occupation;
                                Header.TaxDeduc = staffs.TaxDeduc;
                                Header.InSchool = staffs.InSchool;
                                Header.Child = staffs.Child;
                                Header.Spouse = staffs.Spouse;
                                Header.CreateBy = User.UserName;
                                Header.CreateOn = DateTime.Now;
                                DB.HREmpFamilies.Add(Header);
                            }
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
}
