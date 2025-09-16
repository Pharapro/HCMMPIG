using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Humica.Logic.Asset
{
    public class AssetNoneStaffObject
	{
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public HRAssetNoneStaff Header { get; set; }
		public HRAssetNoneStaffRecord HeaderRecord { get; set; }
		public List<HRAssetNoneStaff> ListHeader { get; set; }
		public List<HRAssetNoneStaffRecord> ListHeaderRecord { get; set; }
		public List<HRAssetNoneStaff> ListHeaderDetail { get; set; }
		public List<HR_AssetNoneStaff_View> List_AssetNoneStaff_View { get; set; }
		public List<HRAssetRegister> ListAsset { get; set; }
		public List<HRAssetNoneStaffDetail> ListAssetNoneStaffDetail { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public AssetNoneStaffObject()
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

				if (DB.HRAssetNoneStaffs.Any(w => w.ReferenceNum == Header.ReferenceNum))
					return "NUM_EXIT";

				var newNoneStaffs = new List<HRAssetNoneStaff>();

				foreach (var item in ListAssetNoneStaffDetail)
				{
					if (string.IsNullOrEmpty(item.AssetCode))
						continue;

					var header = new HRAssetNoneStaff
					{
						HandlerCode = HeaderRecord.HandlerCode,
						HandlerName = HeaderRecord.HandlerName,
						Position = HeaderRecord.Position,
						CreatedBy = User.UserName,
						CreatedOn = DateTime.Now,
						Attachment = Header.Attachment,
						Remark = string.IsNullOrEmpty(item.Remark) ? Header.Remark : item.Remark,
						ReferenceNum = Header.ReferenceNum,
						RemarkDate = Header.RemarkDate,
						RemarkDateDes = Header.RemarkDateDes,
						AssetCode = item.AssetCode,
						Condition = item.Condition?.ToUpper(),
						Status = item.Status,
						AssignDate = item.AssignDate,
						AssetDescription = item.AssetDescription
					};

					newNoneStaffs.Add(header);
					DB.HRAssetNoneStaffs.Add(header);

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
				savedIds.AddRange(newNoneStaffs.Select(x => x.ID));

				if (savedIds.Count == 0)
					return "NO_VALID_ASSETS";

				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				var log = new SYEventLog
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
				var log = new SYEventLog
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
				var log = new SYEventLog
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
					var ObjMatch = DB.HRAssetNoneStaffs.FirstOrDefault(w => w.ID == ID);
					if (ObjMatch == null)
						return "DOC_INV";

					var getRefNum = DB.HRAssetNoneStaffs.Where(s => s.ReferenceNum == ObjMatch.ReferenceNum).ToList();
					foreach (var refNum in getRefNum)
					{
						refNum.RemarkDate = Header.RemarkDate;
						refNum.RemarkDateDes = Header.RemarkDateDes;
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
                var ObjMatch = DB.HRAssetNoneStaffs.FirstOrDefault(w => w.ID == ID); 
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
                DB.HRAssetNoneStaffs.Remove(ObjMatch);
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
        public string Transfer(string id)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                HRAssetStaff objmatch = DB.HRAssetStaffs.First(w => w.ID == ID);
                string assign = SYDocumentStatus.ASSIGN.ToString();

                if (objmatch == null || objmatch.Status != assign)
                {
                    return "DOC_INV";
                }
                else
                {
                    var checkdup = DB.HRAssetRegisters.Where(w => w.AssetCode == objmatch.AssetCode).ToList();
                    foreach (var read in checkdup.ToList())
                    {
                        read.StatusUse = SYDocumentStatus.OPEN.ToString();
                        DB.HRAssetRegisters.Attach(read);
                        DB.Entry(read).Property(x => x.StatusUse).IsModified = true;
                    }
                    objmatch.ReturnDate = DateTime.Now;
                    objmatch.Status = SYDocumentStatus.RETURN.ToString();
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.HRAssetStaffs.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.ReturnDate).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                }

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
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                    var date = DateTime.Now;
                    var ListAsscode = DB.HRAssetRegisters.Where(w => w.StatusUse == "OPEN").ToList();
                    foreach (var staffs in ListHeader.ToList())
                    {
						if (DB.HRAssetNoneStaffs.Any(w => w.ReferenceNum == staffs.ReferenceNum))
						{
							return "NUM_EXIT";
						}
						Header = new HRAssetNoneStaff();
                        var getRecordNone=DB.HRAssetNoneStaffRecords.FirstOrDefault(w => w.HandlerCode==staffs.HandlerCode);
                        if (getRecordNone == null) {
							return "NO_DATA";
						}
					    Header.HandlerCode = staffs.HandlerCode;
					    Header.HandlerName = getRecordNone.HandlerName;
                        Header.Position= getRecordNone.Position;
						Header.AssetCode = staffs.AssetCode;
						Header.AssignDate = staffs.AssignDate;
						Header.Status = "ASSIGN";
						Header.Remark = staffs.Remark;
						if (string.IsNullOrEmpty(staffs.ReferenceNum))
						{
							return $"Missing Reference Number for AssetCode [{staffs.AssetCode}]";
						}
						Header.ReferenceNum = staffs.ReferenceNum;
						Header.RemarkDate = staffs.RemarkDate;
						Header.RemarkDateDes = staffs.RemarkDateDes;
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
						DB.HRAssetNoneStaffs.Add(Header);
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
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
	public class HRAssetNoneStaffDetail
	{
		public string AssetCode { get; set; }
		public DateTime AssignDate { get; set; }
		public string Status { get; set; }
		public string AssetDescription { get; set; }
		public string Condition { get; set; }
		public string Remark { get; set; }
	}
}
