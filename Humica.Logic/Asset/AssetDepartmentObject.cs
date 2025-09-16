using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Humica.Logic.Asset
{
    public class AssetDepartmentObject
	{
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public HRAssetDepartment Header { get; set; }
        public List<HRAssetDepartment> ListHeader { get; set; }
		public List<HRAssetDepartment> ListHeaderDetail { get; set; }
		public List<HR_AssetDepartment_View> List_AssetDepartment_View { get; set; }
		public List<HRAssetDepartmentDetail> ListAssetDepartmentDetail { get; set; }
		public List<HRAssetRegister> ListAsset { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public AssetDepartmentObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

		public string AssignAsset(out List<int> savedIds)
		{
			savedIds = new List<int>();
			try
			{
				if (string.IsNullOrEmpty(Header.ReferenceNum))
					return "DOC_INV";

				if (DB.HRAssetDepartments.Any(w => w.ReferenceNum == Header.ReferenceNum))
					return "NUM_EXIT";

				var newAssetDepartments = new List<HRAssetDepartment>();

				foreach (var item in ListAssetDepartmentDetail)
				{
					if (string.IsNullOrEmpty(item.AssetCode))
						continue;

					var header = new HRAssetDepartment
					{
						HandlerCode = HeaderStaff.EmpCode,
						HandlerName = HeaderStaff.AllName,
						CreatedBy = User.UserName,
						CreatedOn = DateTime.Now,
						Attachment = Header.Attachment,
						Remark = string.IsNullOrEmpty(item.Remark) ? Header.Remark : item.Remark,
						ReferenceNum = Header.ReferenceNum,
						Department = Header.Department,
						HandlerSection = Header.HandlerSection,
						RequestDate = Header.RequestDate,
						AssetCode = item.AssetCode,
						Condition = item.Condition?.ToUpper(),
						Status = item.Status,
						AssignDate = item.AssignDate,
						AssetDescription = item.AssetDescription
					};

					newAssetDepartments.Add(header);
					DB.HRAssetDepartments.Add(header);

					var checkdup = DB.HRAssetRegisters.Where(w => w.AssetCode == item.AssetCode).ToList();
					foreach (var read in checkdup)
					{
						read.StatusUse = SYDocumentStatus.ASSIGN.ToString();
						read.ReferenceNum = Header.ReferenceNum;
						DB.HRAssetRegisters.Attach(read);
						DB.Entry(read).Property(x => x.StatusUse).IsModified = true;
						DB.Entry(read).Property(x => x.ReferenceNum).IsModified = true;
					}
				}

				DB.SaveChanges();
				savedIds.AddRange(newAssetDepartments.Select(x => x.ID));

				if (savedIds.Count == 0)
					return "NO_VALID_ASSETS";

				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				SYEventLog log = new SYEventLog
				{
					ScreenId = ScreenId,
					UserId = User.UserName,
					DocurmentAction = Header.ID.ToString(),
					Action = SYActionBehavior.ADD.ToString()
				};
				SYEventLogObject.saveEventLog(log, e);
				return "EE001";
			}
			catch (DbUpdateException e)
			{
				SYEventLog log = new SYEventLog
				{
					ScreenId = ScreenId,
					UserId = User.UserName,
					DocurmentAction = Header.ID.ToString(),
					Action = SYActionBehavior.ADD.ToString()
				};
				SYEventLogObject.saveEventLog(log, e, true);
				return "EE001";
			}
			catch (Exception e)
			{
				SYEventLog log = new SYEventLog
				{
					ScreenId = ScreenId,
					UserId = User.UserName,
					DocurmentAction = Header.ID.ToString(),
					Action = SYActionBehavior.ADD.ToString()
				};
				SYEventLogObject.saveEventLog(log, e, true);
				return "EE001";
			}
		}
		public string updAssign(string id)
		{
			try
			{
				int ID;
				if (!int.TryParse(id, out ID))
					return "DOC_INV";

				using (var DB = new HumicaDBContext())
				{
					var ObjMatch = DB.HRAssetDepartments.FirstOrDefault(w => w.ID == ID);
					if (ObjMatch == null)
						return "DOC_INV";
					var getRefNum = DB.HRAssetDepartments.Where(s => s.ReferenceNum == ObjMatch.ReferenceNum).ToList();
					foreach (var refNum in getRefNum)
					{
						refNum.Department = Header.Department;
						refNum.HandlerSection = Header.HandlerSection;
						refNum.RequestDate = Header.RequestDate;
						refNum.ChangedBy = User.UserName;
						refNum.ChangedOn = DateTime.Now;
					}
					ObjMatch.Remark = Header.Remark;
					ObjMatch.Attachment = Header.Attachment;
					DB.SaveChanges();
				}
				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				/*------------------SaveLog----------------------------------*/
				SYEventLog log = new SYEventLog();
				log.ScreenId = ScreenId;
				log.UserId = User.UserName;
				log.DocurmentAction = id;
				log.Action = SYActionBehavior.EDIT.ToString();

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
				log.Action = SYActionBehavior.EDIT.ToString();

				SYEventLogObject.saveEventLog(log, e, true);
				/*----------------------------------------------------------*/
				return "EE001";
			}
		}
		public string deleteAssign(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                var ObjMatch = DB.HRAssetDepartments.FirstOrDefault(w => w.ID == ID);
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                var checkdup = DB.HRAssetRegisters.Where(w => w.AssetCode == ObjMatch.AssetCode).ToList();
                foreach (var read in checkdup.ToList())
                {
                    read.StatusUse = SYDocumentStatus.OPEN.ToString();
                    DB.HRAssetRegisters.Attach(read);
                    DB.Entry(read).Property(x => x.StatusUse).IsModified = true;
                }
                DB.HRAssetDepartments.Remove(ObjMatch);
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = SYActionBehavior.DELETE.ToString();

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
                log.Action = SYActionBehavior.DELETE.ToString();

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
                log.Action = SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Import()
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
                    var _list = new List<HREmpFamily>();
                    List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                    _list = DB.HREmpFamilies.ToList();
                    _listStaff = DB.HRStaffProfiles.ToList();

                    var date = DateTime.Now;
                    var ListAsscode = DB.HRAssetRegisters.Where(w => w.StatusUse == "OPEN").ToList();
                    foreach (var staffs in ListHeader.ToList())
                    {
						if (DB.HRAssetDepartments.Any(w => w.ReferenceNum == staffs.ReferenceNum))
						{
							return "NUM_EXIT";
						}
						Header = new HRAssetDepartment();
						var EmpCode = _listStaff.Where(w => w.EmpCode == staffs.HandlerCode).ToList();
                        Header.HandlerCode = "";
                        if (EmpCode.Count <= 1)
                        {
                            if (EmpCode.Count == 1)
                            {
                                Header.HandlerCode = EmpCode.FirstOrDefault().EmpCode;
								Header.HandlerName = EmpCode.FirstOrDefault().AllName;
							}
                            Header.AssetCode = staffs.AssetCode;
							Header.Status = "ASSIGN";
							Header.AssignDate = staffs.AssignDate;
                            Header.Department = staffs.Department;
							Header.HandlerSection = staffs.HandlerSection;
							Header.Remark = staffs.Remark;
							if (string.IsNullOrEmpty(staffs.ReferenceNum))
							{
								return $"Missing Reference Number for AssetCode [{staffs.AssetCode}]";
							}
							Header.ReferenceNum = staffs.ReferenceNum;
							Header.RequestDate = staffs.RequestDate;
							Header.CreatedBy = User.UserName;
                            Header.CreatedOn = DateTime.Now;

							var Asscode = ListAsscode.Where(w => w.AssetCode == Header.AssetCode).FirstOrDefault();
                            if (Asscode != null)
                            {
								Header.Condition = Asscode.Condition;
								Header.AssetDescription = Asscode.Description;
								Asscode.StatusUse = "ASSIGN";
                                DB.HRAssetRegisters.Attach(Asscode);
                                DB.Entry(Asscode).Property(x => x.StatusUse).IsModified = true;
                            }
                            else
                            {
								return "Asset_Duc";
							}
							DB.HRAssetDepartments.Add(Header);
                        }
                        else
                        {
                            foreach (var item1 in EmpCode)
                            {
                                if (item1 != null)
                                {
                                    Header.HandlerCode = item1.EmpCode;
                                }
								Header.HandlerCode = staffs.HandlerCode;
								Header.HandlerName = staffs.HandlerName;
								Header.AssetCode = staffs.AssetCode;
								Header.Status = "ASSIGN";
								Header.AssignDate = staffs.AssignDate;
								Header.Department = staffs.Department;
								Header.HandlerSection = staffs.HandlerSection;
								Header.Remark = staffs.Remark;
								if (string.IsNullOrEmpty(staffs.ReferenceNum))
								{
									return $"Missing Reference Number for AssetCode [{staffs.AssetCode}]";
								}
								Header.ReferenceNum = staffs.ReferenceNum;
								Header.RequestDate = staffs.RequestDate;
								Header.CreatedBy = User.UserName;
								Header.CreatedOn = DateTime.Now;

								var Asscode = ListAsscode.Where(w => w.AssetCode == Header.AssetCode).FirstOrDefault();
                                if (Asscode != null)
                                {
                                    Header.Condition = Asscode.Condition;
                                    Header.AssetDescription = Asscode.Description;
                                    Asscode.StatusUse = "ASSIGN";
                                    DB.HRAssetRegisters.Attach(Asscode);
                                    DB.Entry(Asscode).Property(x => x.StatusUse).IsModified = true;
                                }
                                else 
                                {
									return "Asset_Duc";
								}
								DB.HRAssetDepartments.Add(Header);
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
                log.DocurmentAction = Header.HandlerCode;
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
                log.DocurmentAction = Header.HandlerCode;
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
                log.DocurmentAction = Header.HandlerCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
	public class HRAssetDepartmentDetail
	{
		public string AssetCode { get; set; }
		public DateTime AssignDate { get; set; }
		public string Status { get; set; }
		public string AssetDescription { get; set; }
		public string Condition { get; set; }
		public string Remark { get; set; }
	}
}
