using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HREmpDisciplinaryObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpDisciplinary Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<HREmpDisciplinary> Lisgrd { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<EmpDisciplinary> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public HREmpDisciplinaryObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<EmpDisciplinary> LoadData()
        {
            var AccLevel = SMS.SYUserLevels.ToList();
            AccLevel = AccLevel.Where(w => w.UserName == User.UserName).ToList();
            var _list = new List<EmpDisciplinary>();
            var empCon = DB.HREmpDisciplinaries.ToList();
            var DisciplinType = DB.HRDisciplinTypes.ToList();
            var Staff = DB.HRStaffProfiles.ToList();
            Staff = Staff.Where(w => AccLevel.Where(x => x.LevelCode == w.LevelCode).Any()).ToList();
            var DiscpAction = DB.HRDisciplinActions.ToList();
            var resu = (from edu in empCon
                        join emp in Staff on edu.EmpCode equals emp.EmpCode
                        join ET in DiscpAction on edu.DiscAction equals ET.Code
                        join disciplinDesc in DisciplinType on edu.DiscType equals disciplinDesc.Code
                        select new
                        {
                            edu.TranNo,
                            edu.EmpCode,
                            emp.AllName,
                            DisciplinayType = ET.Description,
                            edu.TranDate,
                            DisciplinaryAction = disciplinDesc.Description,
                            edu.Remark,
                            edu.Reference,
                            edu.Consequence
                        }).ToList();
            foreach (var item in resu)
            {
                var _empCon = new EmpDisciplinary();
                _empCon.TranNo = item.TranNo;
                _empCon.EmpCode = item.EmpCode;
                _empCon.AllName = item.AllName;
                _empCon.DisciplinayType = item.DisciplinayType;
                _empCon.PlanForImprovement = item.Reference;
                _empCon.DisciplinaryAction = item.DisciplinaryAction;
                _empCon.Date = item.TranDate;
                _empCon.DescriptionofInfraction = item.Remark;
                _empCon.ConsequencesofFurtherInfractions = item.Consequence;
                _list.Add(_empCon);
            }

            return _list;
        }
        public string CreateEmpDiscp()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.DiscType == null) return "DISC_EN";
                if (Header.DiscAction == null) return "DISC_ACT_EN";
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.AllName = HeaderStaff.AllName;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.HREmpDisciplinaries.Add(Header);
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
        public string EditEmpDiscp(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }

                objMatch.DiscType = Header.DiscType;
                objMatch.TranDate = Header.TranDate;
                objMatch.Remark = Header.Remark;
                objMatch.Reference = Header.Reference;
                objMatch.DiscAction = Header.DiscAction;
                objMatch.Consequence = Header.Consequence;
                objMatch.AttachPath = Header.AttachPath;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.HREmpDisciplinaries.Attach(objMatch);
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
        public string DeleteEmpDiscp(int id)
        {
            try
            {
                Header = new HREmpDisciplinary();
                var objMatch = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }
                Header.TranNo = id;
                DB.HREmpDisciplinaries.Attach(objMatch);
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
    }
    public class EmpDisciplinary
    {
        public long TranNo { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string DisciplinayType { get; set; }
        public string DisciplinaryAction { get; set; }
        public DateTime Date { get; set; }
        public string DescriptionofInfraction { get; set; }
        public string PlanForImprovement { get; set; }
        public string ConsequencesofFurtherInfractions { get; set; }
    }
}