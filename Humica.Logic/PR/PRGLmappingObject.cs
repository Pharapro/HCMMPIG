using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Management;
using System.Xml.Linq;

namespace Humica.Logic.PR
{
    public class PRGLmappingObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRGLmapping Header { get; set; }
        public string ScreenId { get; set; }
        public string EmpCode { get; set; }
        public string Branch { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string MessageCode { get; set; }
        public string GLMCode { get; set; }
        public FTFilterData Filter { get; set; }
        public PRCostCenter HeaderCost { get; set; }
        public PRInteAccount HeaderAcc { get; set; }
        public PRIntegratePO HeaderPO { get; set; }
        public List<PRInteAccount> ListHeaderAcc { get; set; }
        public List<PRIntegratePO> ListHeaderPO { get; set; }
        public List<PRInteAccountItem> ListAccItem { get; set; }
        public List<PRIntegratePOItem> ListPOItem { get; set; }
        public List<PRGLmapping> ListHeader { get; set; }
        public List<PRCostCenter> ListCostCenter { get; set; }
        public List<HRStaffProfile> ListStaffs { get; set; }
        public List<ClsAccount> ListGLCharge { get; set; }
        public List<PR_InetAccount_view> ListGLReference { get; set; }
        public List<PR_POPending_view> ListPOReference { get; set; }
        public List<PR_TEMP_SYGL_VIEW> ListGLMapping { get; set; }

        public PRGLmappingObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ClsAccount> LoadDataGL(FTFilterData Filter1, List<HRBranch> _ListBranch)
        {
            var _List = new List<ClsAccount>();
            var ListGL = DBV.PR_GLCharge_View.Where(w => w.InMonth == Filter1.PaymentDate.Month
             && w.InYear == Filter1.PaymentDate.Year).ToList();
            if (Filter1.Branch != null)
            {
                string[] Branch = Filter1.Branch.Split(',');
                List<string> LstBranch = new List<string>();
                foreach (var read in Branch)
                {
                    if (read.Trim() != "")
                    {
                        LstBranch.Add(read.Trim());
                    }
                }
                ListGL = ListGL.Where(w => LstBranch.Contains(w.Branch)).ToList();
            }
            var _listGL = (from GL in ListGL
                           group GL by new { GL.CompanyCode, GL.Branch, GL.CostCenter, GL.GLCode }
                                           into myGroup
                           where myGroup.Count() > 0
                           select new
                           {
                               myGroup.Key.CompanyCode,
                               myGroup.Key.Branch,
                               myGroup.Key.CostCenter,
                               myGroup.Key.GLCode,
                               PostingDate = myGroup.FirstOrDefault().PaymentDate,
                               CurrencyCode = myGroup.FirstOrDefault().CurrencyCode,
                               DebitAmount = myGroup.Sum(w => w.DEBITAM),
                               CreditAmount = myGroup.Sum(w => w.CREDITAM)
                           }).ToList();
            var ListAccDes = DB.PRChartofAccounts.ToList();
            foreach (var read in _listGL)
            {
                var obj = new ClsAccount();
                obj.CompanyCode = read.CompanyCode;
                obj.JournalType = Filter1.JournalType;
                obj.Branch = read.Branch;
                obj.CostCenter = read.CostCenter;
                obj.CurrencyCode = read.CurrencyCode;
                obj.PostingDate = Filter1.PostingDate;
                obj.PostingPeriod = Filter1.PostingDate.ToString("MM-yyyy");
                obj.AccountCode = read.GLCode;
                obj.AccountDescription = "";
                if (ListAccDes.Where(w => w.Code == read.GLCode).Count() > 0)
                {
                    obj.AccountDescription = ListAccDes.FirstOrDefault(w => w.Code == read.GLCode).Description;
                }
                obj.DebitAmount = read.DebitAmount;
                obj.CreditAmount = read.CreditAmount;
                _List.Add(obj);
            }

            return _List.OrderBy(w => w.AccountCode).ToList();
        }
        public List<PRGLmapping> LoadDataMapping(int Code)
        {
            var DBI = new HumicaDBContext();
            ListHeader = new List<PRGLmapping>();
            List<PRGLmapping> ListTemp = DBI.PRGLmappings.Where(w => w.ID == Code).ToList();
            List<PR_TEMP_SYGL_VIEW> GLMapping = DBV.PR_TEMP_SYGL_VIEW.OrderBy(w => w.SortKey).ToList();
            foreach (var v in GLMapping)
            {
                var obj = new PRGLmapping();
                obj.BenGrp = v.GroupAcc;
                obj.BenCode = v.AccCode;
                obj.BenefitGroup = v.BenefitGroup;
                var GL = ListTemp.FirstOrDefault(w => w.BenGrp == v.GroupAcc && w.BenCode == v.AccCode);
                if (GL != null)
                {
                    obj.GLCode = GL.GLCode;
                    obj.MaterialCode = GL.MaterialCode;
                }
                ListHeader.Add(obj);
            }
            return ListHeader;
        }
        public List<PRInteAccountItem> LoadDataGLItem(int InYear, int InMonth, string Branch)
        {
            var _List = new List<PRInteAccountItem>();
            var ListGL = DBV.PR_GLCharge_View.Where(w => w.InMonth == InMonth && w.InYear == InYear
            && w.Branch == Branch).ToList();

            var _listGL = (from GL in ListGL
                           group GL by new { GL.Branch, GL.CostCenter, GL.GLCode, GL.MaterialCode }
                                           into myGroup
                           where myGroup.Count() > 0
                           select new
                           {
                               myGroup.Key.Branch,
                               myGroup.Key.CostCenter,
                               myGroup.Key.GLCode,
                               myGroup.Key.MaterialCode,
                               PaymentDate = myGroup.FirstOrDefault().PaymentDate,
                               CurrencyCode = myGroup.FirstOrDefault().CurrencyCode,
                               DebitAmount = myGroup.Sum(w => w.DEBITAM),
                               CreditAmount = myGroup.Sum(w => w.CREDITAM)
                           }).ToList();
            var ListAccDes = DB.PRChartofAccounts.ToList();
            foreach (var read in _listGL)
            {
                var obj = new PRInteAccountItem();
                obj.Branch = read.Branch;
                obj.CostCenter = read.CostCenter;
                obj.AccountCode = read.GLCode;
                obj.PaymentDate = read.PaymentDate;
                if (ListAccDes.Where(w => w.Code == read.GLCode).Count() > 0)
                {
                    obj.Description = ListAccDes.FirstOrDefault(w => w.Code == read.GLCode).Description;
                }
                obj.DebitAmount = read.DebitAmount;
                obj.CreditAmount = read.CreditAmount;
                _List.Add(obj);
            }

            return _List.OrderBy(w => w.AccountCode).ToList();
        }
        public List<PRIntegratePOItem> LoadDataPOItem(int InYear, int InMonth, string Branch,
            string Warehouse, string Project, string DocType)
        {
            var _List = new List<PRIntegratePOItem>();
            var DocumentType = DB.HRJournalTypes.ToList();
            //var ListGL = DBV.PR_GLCharge_View.Where(w => w.InMonth == InMonth && w.InYear == InYear
            //&& w.Branch == Branch && w.IsPO == true).ToList();
            var ListGL = DBV.PR_GLCharge_View.Where(w => w.InMonth == InMonth && w.InYear == InYear
            && w.CostCenter == Branch && w.Warehouse == Warehouse && w.Project == Project).ToList();
            if (string.IsNullOrEmpty(DocType))
            {
                ListGL = ListGL.Where(w => w.IsDebitNote != true).ToList();
                var _DocType = DocumentType.FirstOrDefault(w => w.IsDebitNote != true);
                if (_DocType != null) { HeaderPO.DocumentType = _DocType.JournalType; }
            }
            else
            {
                ListGL = ListGL.Where(w => w.IsDebitNote == true).ToList();
                var _DocType = DocumentType.FirstOrDefault(w => w.IsDebitNote == true);
                if (_DocType != null) { HeaderPO.DocumentType = _DocType.JournalType; }
            }
            var ListAccDes = DB.HRMaterialAccounts.ToList();
            foreach (var read in ListGL)
            {
                int Credit = 1;
                if (read.IsCredit == true) Credit = -1;
                if (read.IsDebitNote == true) Credit = 1;
                if (!_List.Where(w => w.MaterialCode == read.MaterialCode).Any())
                {
                    var obj = new PRIntegratePOItem();
                    obj.Branch = read.Branch;
                    obj.CostCenter = read.CostCenter;
                    obj.Warehouse = read.Warehouse;
                    obj.AccountCode = read.GLCode;
                    obj.Unit = "UNIT";
                    obj.Quantity = 1;
                    if (ListAccDes.Where(w => w.MaterialCode == read.MaterialCode).Count() > 0)
                    {
                        obj.Description = ListAccDes.FirstOrDefault(w => w.MaterialCode == read.MaterialCode).MaterialDescription;
                    }
                    obj.Amount = read.Amount * Credit;
                    obj.MaterialCode = read.MaterialCode;
                    _List.Add(obj);
                }
                else
                {
                    _List.Where(w => w.MaterialCode == read.MaterialCode).ToList().ForEach(w => w.Amount += (read.Amount * Credit));
                }
            }
            return _List.OrderBy(w => w.AccountCode).ToList();
        }
        public string CreateGLMapping()
        {
            try
            {
                var check = DB.PRGLmappings.Where(u => u.Branch == HeaderCost.Branch).ToList();

                //var List = DB.PREmpAllws.Where(u => u.TranNo == id).ToList();

                foreach (var read in check)
                {
                    DB.PRGLmappings.Remove(read);
                    DB.SaveChanges();
                }
                var getlist = DBV.PR_TEMP_SYGL_VIEW.ToList();

                foreach (var read in ListHeader)
                {
                    var objCheck = getlist.Where(w => w.AccCode == read.BenCode).First();

                    read.BenGrp = objCheck.GroupAcc.ToUpper();
                    read.Branch = HeaderCost.Branch;
                    read.Description = HeaderCost.Description;
                    read.CreateBy = User.UserName;
                    read.CreateOn = DateTime.Now;
                    DB.PRGLmappings.Add(read);
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
                log.DocurmentAction = HeaderCost.Project.ToString();
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
                log.DocurmentAction = HeaderCost.Project;
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
                log.DocurmentAction = HeaderCost.Project;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string EditGLMapping(int id)
        {
            try
            {
                var objMatch = DB.PRCostCenters.FirstOrDefault(w=>w.ID== id);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                var List = DB.PRGLmappings.Where(u => u.ID == id).ToList();
                //if (List.Count == 0)
                //{
                //    return "NO_CATCHING";
                //}

                foreach (var read in List)
                {
                    DB.PRGLmappings.Remove(read);
                }

                var getlist = DBV.PR_TEMP_SYGL_VIEW.ToList();

                foreach (var read in ListHeader)
                {
                    var objCheck = getlist.Where(w => w.AccCode == read.BenCode).First();
                    read.ID = id;
                    read.BenGrp = objCheck.GroupAcc.ToUpper();
                    read.Branch = HeaderCost.Branch;
                    read.Warehouse = objMatch.Warehouse;
                    read.Project = objMatch.Project;
                    read.Description = HeaderCost.Description;
                    read.CreateBy = User.UserName;
                    read.CreateOn = DateTime.Now;
                    DB.PRGLmappings.Add(read);
                }
                objMatch.Description = HeaderCost.Description;
                DB.PRCostCenters.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;

                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderCost.Project;
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
                log.DocurmentAction = HeaderCost.Project;
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
                log.DocurmentAction = HeaderCost.Project;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string SetStaffGL(string EmpCode, string GL)
        {
            string Result = "";
            try
            {
                string[] multiArrayBill = EmpCode.Split(';');
                var staff = DB.HRStaffProfiles.ToList();
                foreach (var item in multiArrayBill)
                {
                    if (item.Trim() != "")
                    {
                        string EmpCodeS = item.ToString();
                        var StaffUpdate = staff.Where(w => w.EmpCode == EmpCodeS).First();
                        if (StaffUpdate != null)
                        {
                            StaffUpdate.GrpGLAcc = GL;
                            DB.Entry(StaffUpdate).State = EntityState.Modified;
                        }
                    }
                }
                var row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Result;
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
                log.DocurmentAction = Result;
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
                log.DocurmentAction = Result;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteGLMapping(string id)
        {
            try
            {
                var List = DB.PRGLmappings.Where(u => u.Branch == id).ToList();

                if (List.Count == 0)
                {
                    return "NO_CATCHING";
                }
                foreach (var read in List)
                {
                    DB.PRGLmappings.Remove(read);
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string CreateGLPost()
        {
            try
            {
                var DBR = new HumicaDBViewContext();
                if (HeaderAcc.DocumentType == null)
                {
                    return "REQUEST_TYPE_NE";
                }

                var objNumber = new CFNumberRank(HeaderAcc.DocumentType, DocConfType.GL, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                HeaderAcc.IntegrateNo = objNumber.NextNumberRank.Trim();
                HeaderAcc.Status = SYDocumentStatus.OPEN.ToString();
                int lineitem = 0;
                foreach (var item in ListAccItem)
                {
                    lineitem += 1;
                    item.LineItem = lineitem;
                    item.CompanyCode = HeaderAcc.CompanyCode;
                    item.IntegrateNo = HeaderAcc.IntegrateNo;
                    DB.PRInteAccountItems.Add(item);
                }
                var ListGLCharge = DBR.HISGLBenCharges.Where(w => w.IsGenerate == false).ToList();
                ListGLCharge = ListGLCharge.Where(w => ListAccItem.Where(x => x.Branch == w.Branch && x.CostCenter == w.CostCenter
                  && x.AccountCode == w.GLCode && w.InMonth == x.PaymentDate.Month && w.InYear == x.PaymentDate.Year).Any()).ToList();
                foreach (var item in ListGLCharge)
                {
                    item.IsGenerate = true;
                    DB.HISGLBenCharges.Attach(item);
                    DB.Entry(item).Property(w => w.IsGenerate).IsModified = true;
                }
                var SalaryApp = DBR.HISApproveSalaries.FirstOrDefault(w => w.InYear == HeaderAcc.InYear && w.InMonth == HeaderAcc.InMonth && w.IsPostGL == false);
                if (SalaryApp != null)
                {
                    SalaryApp.IsPostGL = true;
                    DB.HISApproveSalaries.Attach(SalaryApp);
                    DB.Entry(SalaryApp).Property(w => w.IsPostGL).IsModified = true;
                }
                HeaderAcc.Status = SYDocumentStatus.OPEN.ToString();
                HeaderAcc.CreatedOn = DateTime.Now;
                HeaderAcc.CreatedBy = User.UserName;

                DB.PRInteAccounts.Add(HeaderAcc);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAcc.IntegrateNo;
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
                log.DocurmentAction = HeaderAcc.IntegrateNo;
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
                log.DocurmentAction = HeaderAcc.IntegrateNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string CreatePO(string DocType)
        {
            try
            {
                var DBR = new HumicaDBViewContext();
                if (HeaderPO.DocumentType == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var DocumentType = DB.HRJournalTypes.FirstOrDefault(w => w.JournalType == HeaderPO.DocumentType);
                if (DocumentType == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(DocumentType.NumberRank, DocConfType.GL, true);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                HeaderPO.IntegrateNo = objNumber.NextNumberRank.Trim();
                HeaderPO.Status = SYDocumentStatus.OPEN.ToString();
                int lineitem = 0;
                foreach (var item in ListPOItem)
                {
                    lineitem += 1;
                    item.LineItem = lineitem;
                    item.CompanyCode = HeaderPO.CompanyCode;
                    item.IntegrateNo = HeaderPO.IntegrateNo;
                    DB.PRIntegratePOItems.Add(item);
                }
                var ListGLCharge = DBR.HISGLBenCharges.Where(w => w.IsGenerate == false && w.InYear == HeaderPO.InYear && w.InMonth == HeaderPO.InMonth
                && w.Project == HeaderPO.Project && w.CostCenter == HeaderPO.Branch).ToList();
                ListGLCharge = ListGLCharge.Where(w => ListPOItem.Where(x => x.AccountCode == w.GLCode && x.MaterialCode == w.MaterialCode
                  && x.Warehouse == w.Warehouse).Any()).ToList();
                if (string.IsNullOrEmpty(DocType))
                {
                    ListGLCharge = ListGLCharge.Where(w => w.IsDebitNote != true).ToList();
                }
                else
                {
                    ListGLCharge = ListGLCharge.Where(w => w.IsDebitNote == true).ToList();
                }
                foreach (var item in ListGLCharge)
                {
                    item.IsGenerate = true;
                    DB.HISGLBenCharges.Attach(item);
                    DB.Entry(item).Property(w => w.IsGenerate).IsModified = true;
                }
                var SalaryApp = DBR.HISApproveSalaries.FirstOrDefault(w => w.InYear == HeaderPO.InYear && w.InMonth == HeaderPO.InMonth && w.IsPostGL == false);
                if (SalaryApp != null)
                {
                    SalaryApp.IsPostGL = true;
                    DB.HISApproveSalaries.Attach(SalaryApp);
                    DB.Entry(SalaryApp).Property(w => w.IsPostGL).IsModified = true;
                }
                HeaderPO.Status = SYDocumentStatus.OPEN.ToString();
                HeaderPO.CreatedOn = DateTime.Now;
                HeaderPO.CreatedBy = User.UserName;

                DB.PRIntegratePOes.Add(HeaderPO);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderPO.IntegrateNo;
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
                log.DocurmentAction = HeaderPO.IntegrateNo;
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
                log.DocurmentAction = HeaderPO.IntegrateNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
    public class ClsAccount
    {
        public string CompanyCode { get; set; }
        public string JournalType { get; set; }
        public string Branch { get; set; }
        public string CostCenter { get; set; }
        public DateTime PostingDate { get; set; }
        public string PostingPeriod { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}