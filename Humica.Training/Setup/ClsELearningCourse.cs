using Humica.Core.BS;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training.Setup
{
    public class ClsELearningCourse
    {
        public string ScreenId { get; set; }
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public TrainingCourse Header { get; set; }
        public List<TrainingCourse> ListHeader { get; set; }
        public List<TrainingCourse> ListWaitHeader { get; set; }
        public List<TRTrainingCalender> ListTrainingCalender { get; set; }
        // public List<TrainingCourseRequirement> ListRequirement { get; set; }
        public List<TrainingCourseItem> ListItem { get; set; }
        public List<Humica.Core.DB.HRStaffProfile> ListStaff { get; set; }

        public List<int> ListDelete { get; set; }
        public string GroupDesc { get; set; }
        public int GroupValue { get; set; }
        public string DocumentNo { get; set; }
        public string ShipmentType { get; set; }
        public string WCType = "SPC01";
        public string GIDocumentType = "NGI01";
        public string DeliveryType = "NDO01";
        public string TRNINV = "TRINV01";
        public string CompanyCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ShipDate { get; set; }
        public BSDocConfg DocConfigure { get; set; }
        public FTDGeneralPeriod Filter { get; set; }
        // public SYSplitItem SelectListItem { get; set; }
        //public BSQuotaConf Configure { get; set; }

        private const string REMAINING = "REMAINING";
        public string DefaultPlant { get; set; }
        public string DefaultStorage { get; set; }
        public string Course { get; set; }
        public string ActionType { get; set; }
        public string Program { get; set; }
        public ClsELearningCourse()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string Release(string Course)
        {
            try
            {
                if (Course == null)
                {
                    return "COURSE_EMPTY";
                }
                var trc = DB.TrainingCourse.ToList();
                var trp = DB.TrainingReleaseProgram.ToList();
                string[] c = Course.Split(';');

                var lstdlr = new List<TrainingReleaseProgram>();
                var lstdlrcourse = new List<TrainingReleaseProgram>();

                foreach (var r in c)
                {
                    if (r != "")
                    {
                        int id = Convert.ToInt32(r);
                        string approve = SYDocumentStatus.APPROVED.ToString();
                        //var h = trc.FirstOrDefault(w => w.ID == id && w.Status == approve);
                        //if (h != null)
                        //{
                        //    var getrelease = DB.TrainingReleaseProgram.Where(w =>  w.Program== h.ProgramCode && w.Course==h.Coursecode && w.InvCode==h.InvCode).ToList();
                        //    foreach (var r2 in getrelease)
                        //    {
                        //        DB.TrainingReleaseProgram.Remove(r2);
                        //    }

                        //    h.Status = SYDocumentStatus.RELEASED.ToString();
                        //    h.ReleaseDate = DateTime.Now;
                        //    DB.TrainingCourse.Attach(h);
                        //    DB.Entry(h).Property(w => w.Status).IsModified = true;
                        //    DB.Entry(h).Property(w => w.ReleaseDate).IsModified = true;

                        //    string REJECTED = SYDocumentStatus.REJECTED.ToString();
                        //    var citem = DB.TrainingCourseItem.Where(w =>  w.Program == h.ProgramCode && w.Course == h.Coursecode && w.InvCode == h.InvCode && w.Status != REJECTED).ToList();
                        //    var lst = new List<TrainingReleaseProgram>();
                        //    //var lstalldlr = new List<MDDealer>();
                        //    foreach (var r2 in citem)
                        //    {
                        //        var cci = DB.TrainingCourseItem.Where(w => w.ID == r2.ID).ToList();
                        //        if (cci.Count > 0)
                        //        {
                        //            r2.Status = SYDocumentStatus.RELEASED.ToString();
                        //            DB.TrainingCourseItem.Attach(r2);
                        //            DB.Entry(r2).Property(w => w.Status).IsModified = true;

                        //        }

                        //        var obj = new TrainingReleaseProgram();
                        //        obj.Program = h.ProgramCode;
                        //        obj.Course = h.Coursecode;
                        //        obj.InvCode = h.InvCode;
                        //        obj.Type = h.Type;
                        //        obj.Status = SYDocumentStatus.RELEASED.ToString();
                        //        var ctrp = lst.Where(w => w.Program == obj.Program && w.Course == obj.Course && w.InvCode == obj.InvCode).ToList();

                        //        if (ctrp.Count == 0)
                        //        {
                        //            lst.Add(obj);
                        //            DB.TrainingReleaseProgram.Add(obj);

                        //            //var obdlr = new MDDealer();
                        //            //obdlr.DealerCode = obdlr.DealerCode;
                        //           // if(lstalldlr.Where(w=>w.DealerCode==obdlr.DealerCode).ToList().Count ==0)
                        //            //{
                        //            //    lstalldlr.Add(obdlr);
                        //            //    //Add Number Range Training Exam
                        //            //    var ph = DB.BSNumberRankNies.Where(w => w.DealerCode == obj.DealerCode && w.NbrObject == "TREX01").ToList();
                        //            //    if (ph.Count == 0)
                        //            //    {
                        //            //        var objNbr = new BSNumberRankNY();
                        //            //        objNbr.NbrObject = "TREX01";
                        //            //        objNbr.ObjectSequent = "10";
                        //            //        objNbr.Prefix = "EX";
                        //            //        objNbr.Module = "TRN";
                        //            //        objNbr.Separated = "-";
                        //            //        objNbr.FromNumber = 1;
                        //            //        objNbr.ToNumber = 999999999;
                        //            //        objNbr.Status = 0;
                        //            //        DB.BSNumberRankNies.Add(objNbr);
                        //            //    }

                        //            //    //Add Number Range Training Request
                        //            //    var re = DB.BSNumberRankNies.Where(w => w.DealerCode == obj.DealerCode && w.NbrObject == "TRRQ01").ToList();
                        //            //    if (re.Count == 0)
                        //            //    {
                        //            //        var objNbr = new BSNumberRankNY();
                        //            //        objNbr.NbrObject = "TRRQ01";
                        //            //        objNbr.ObjectSequent = "10";
                        //            //        objNbr.Prefix = "RQ";
                        //            //        objNbr.Module = "TRN";
                        //            //        objNbr.Separated = "-";
                        //            //        objNbr.FromNumber = 1;
                        //            //        objNbr.ToNumber = 999999999;
                        //            //        objNbr.Status = 0;
                        //            //        DB.BSNumberRankNies.Add(objNbr);
                        //            //    }
                        //            //}

                        //        }

                        //    }

                        //}
                    }


                }

                int row = DB.SaveChanges();
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        //public string RemoveModule(string Course)
        //{
        //    try
        //    {
        //        if (Course == null)
        //        {
        //            return "COURSE_EMPTY";
        //        }
        //        var trc = DB.TrainigModule.ToList();
        //        string[] c = Course.Split(';');

        //        foreach (var r in c)
        //        {
        //            int id = Convert.ToInt32(r);
        //            string OPEN = SYDocumentStatus.OPEN.ToString();
        //            string RELEASED = SYDocumentStatus.RELEASED.ToString();
        //            var h = trc.Single(w => w.ID == id);
        //            if (h != null)
        //            {

        //                DB.TrainigModule.Remove(h);

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
        //        log.DocurmentAction = "";
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
        //        log.DocurmentAction = "";
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
        //        log.DocurmentAction = "";
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}
        public string Remove(string Course)
        {
            try
            {
                if (Course == null)
                {
                    return "COURSE_EMPTY";
                }
                var trc = DB.TrainingCourse.ToList();
                var trp = DB.TrainingReleaseProgram.ToList();
                string[] c = Course.Split(';');

                var lstdlr = new List<TrainingReleaseProgram>();
                var lstdlrcourse = new List<TrainingReleaseProgram>();

                foreach (var r in c)
                {
                    int id = Convert.ToInt32(r);
                    string training = SYDocumentStatus.TRAINING.ToString();
                    string finished = SYDocumentStatus.FINISHED.ToString();
                    string released = SYDocumentStatus.RELEASED.ToString();

                    //var h = trc.Single(w => w.ID == id);
                    //if (h != null)
                    //{

                    //    if(!(h.Status == training || h.Status == finished || h.Status==released))
                    //    {
                    //        DB.TrainingCourse.Remove(h);

                    //        var citem = DB.TrainingCourseItem.Where(w =>  w.Program == h.ProgramCode && w.Course == h.Coursecode && w.Group == h.TrainingGroup && h.InvCode==h.InvCode).ToList();
                    //        var lst = new List<TrainingReleaseProgram>();
                    //        foreach (var r2 in citem)
                    //        {
                    //            var cci = DB.TrainingCourseItem.Where(w => w.ID == r2.ID).ToList();
                    //            if (cci.Count > 0)
                    //            {
                    //                DB.TrainingCourseItem.Remove(r2);

                    //            }
                    //        }
                    //        foreach (var r2 in trp.Where(w=>w.Program== h.ProgramCode && w.Course== h.Coursecode && w.InvCode== h.InvCode).ToList())
                    //        {
                    //            DB.TrainingReleaseProgram.Remove(r2);

                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    return "DOC_INV "+ h.InvCode + ":" + h.Coursecode+":"+h.Coursename;
                    //}

                }

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string SAVE()
        {
            try
            {
                DB = new HumicaDBContext();

                var pg = DB.TrainingProgram.ToList();
                //var pc = DB.TrainingCourse.ToList();
                var CourseName = DB.TrainingCourseMaster.FirstOrDefault(w => w.Code == Header.Coursecode);
                if (CourseName == null)
                {
                    return "INVALID_EN";
                }
                if (ListItem.Count == 0)
                {
                    return "STAFF_EMPTY";
                }
                //var check = pc.Where(w => w.ProgramCode == Header.ProgramCode && w.Coursecode == Header.Coursecode && w.TrainingGroup == Header.TrainingGroup && w.Startdate == Header.Startdate && w.Enddate == Header.Enddate).ToList();
                //if (check.Count > 0)
                //{
                //    return "ISDUPLICATED";
                //}


                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                Header.ChangedBy = User.UserName;
                Header.ChangedOn = DateTime.Now;
                Header.Coursename = CourseName.Description;
                Header.ProgramName = pg.Where(w => w.ProgramCode == Header.ProgramCode).First().ProgramName;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.IsActive = true;
                var Configure = new CFNumberRank(TRNINV);
                Header.InvCode = Configure.NextNumberRank;
                if (Header.InvCode == null)
                {
                    return "NUMBERANGE_EMPTY";
                }
                foreach (var r in ListItem)
                {
                    r.Course = Header.Coursecode;
                    r.Program = Header.ProgramCode;
                    r.Status = Header.Status;
                    r.InvCode = Header.InvCode;
                    r.Type = Header.Type;
                    r.Group = Header.TrainingGroup;
                    r.StaffLevel = Header.StaffLevel;
                    r.Startdate = Header.Startdate;
                    r.Enddate = Header.Enddate;
                    DB.TrainingCourseItem.Add(r);
                }

                DB.TrainingCourse.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Coursecode;
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
                log.DocurmentAction = Header.Coursecode;
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
                log.DocurmentAction = Header.Coursecode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string update(string id)
        {
            try
            {
                DB = new HumicaDBContext();

                var ObjMatch = DB.TrainingCourse.FirstOrDefault(w => w.InvCode == id);
                if (ObjMatch == null)
                {
                    return "INV_DOC";
                }
                var citem = DB.TrainingCourseItem.Where(w => w.InvCode == ObjMatch.InvCode).ToList();
                foreach (var r in citem)
                {
                    var ct = citem.Where(w => w.StaffID == r.StaffID).ToList();
                    if (ct.Count > 0)
                    {
                        DB.TrainingCourseItem.Remove(ct.First());
                    }
                }
                foreach (var r in ListItem)
                {
                    r.Course = Header.Coursecode;
                    r.Program = Header.ProgramCode;
                    r.Status = Header.Status;
                    r.InvCode = Header.InvCode;
                    r.Type = Header.Type;
                    r.Group = Header.TrainingGroup;
                    r.StaffLevel = Header.StaffLevel;
                    r.Startdate = Header.Startdate;
                    r.Enddate = Header.Enddate;
                    DB.TrainingCourseItem.Add(r);
                }
                ObjMatch.Score = Header.Score;
                ObjMatch.Target = Header.Target;
                ObjMatch.Capacity = Header.Capacity;
                ObjMatch.Attatchment = Header.Attatchment;
                ObjMatch.Venue = Header.Venue;
                ObjMatch.Startdate = Header.Startdate;
                ObjMatch.Enddate = Header.Enddate;
                ObjMatch.AnnounceCode = Header.AnnounceCode;
                ObjMatch.Coursename = Header.Coursename;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                DB.TrainingCourse.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(w => w.Coursename).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Score).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Target).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Startdate).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Enddate).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Capacity).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Venue).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Attatchment).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Coursecode).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ProgramCode).IsModified = true;

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Coursecode;
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
                log.DocurmentAction = Header.Coursecode;
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
                log.DocurmentAction = Header.Coursecode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        //public static IEnumerable<HR_STAFF_VIEW> GetMaterialAll()
        //{
        //    var DB = new DBBusinessProcess();

        //    var st = new List<HRStaffProfile>();

        //    var check = DB.HR_STAFF_VIEW.Where(w => w.Status == "A").ToList();

        //    string lv = "";
        //    string poss = "";
        //    string prog = "";
        //    string cos = "";
        //    if(!(HttpContext.Current.Session["pg"] == null || HttpContext.Current.Session["pg"]=="") && !(HttpContext.Current.Session["cs"]==null || HttpContext.Current.Session["cs"]==""))
        //    {
        //        prog = HttpContext.Current.Session["pg"].ToString();
        //        cos = HttpContext.Current.Session["cs"].ToString();

        //        if (!(HttpContext.Current.Session["pos"] == null || HttpContext.Current.Session["pos"] == ""))
        //        {
        //            poss = HttpContext.Current.Session["pos"].ToString();
        //            check = check.Where(w => w.Position == poss).ToList();
        //        }
        //        if (!(HttpContext.Current.Session["Level"] == null || HttpContext.Current.Session["Level"] == ""))
        //        {
        //            lv = HttpContext.Current.Session["Level"].ToString();

        //        }
        //        check = check.Where(w => w.Level == lv).ToList();

        //        //DateTime StartDate = Convert.ToDateTime(HttpContext.Current.Session["StartDate"]);
        //        //DateTime EndDate = Convert.ToDateTime(HttpContext.Current.Session["EndDate"]);

        //        string released = SYDocumentStatus.RELEASED.ToString();
        //        string open = SYDocumentStatus.OPEN.ToString();
        //        var citem = DB.TrainingCourseItem.Where(w =>  (w.Status == released || w.Status == open)).ToList();

        //        foreach (var r in check)
        //        {
        //            var obj = new Staff();
        //            obj.ID = r.ID;
        //            obj.StaffID = r.StaffID;
        //            obj.NameInENG = r.NameInENG;
        //            obj.NameInKH = r.NameInKH;
        //            obj.Gender = r.Gender;
        //            obj.Position = r.Position;
        //            obj.PhoneNo = r.PhoneNo;
        //            obj.NatID = r.NatID;
        //            obj.CompanyCode = r.CompanyCode;

        //            string stid = r.ID.ToString();
        //            //var check1 = citem.Where(w =>  w.Program == prog && w.Course == cos && w.StaffID== stid).ToList();
        //            //if(check1.Count==0)
        //            //{
        //            //    st.Add(obj);
        //            //}
        //            st.Add(obj);

        //        }
        //    }
        //    else
        //    {
        //        st = check;
        //    }

        //    //st = check;

        //    return st.ToList();
        //}
    }

}