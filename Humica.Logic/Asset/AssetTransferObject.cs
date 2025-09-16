using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Asset
{
    public class AssetTransferObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public HRAssetTransfer Header { get; set; }
		public AssetTransferDetail HeaderDetail { get; set; }
		public PREmpDeduc HeaderDed { get; set; }
        public List<HRAssetTransfer> ListHeader { get; set; }
		public List<AssetTransferDetail> ListHeaderDetail { get; set; }
		public List<HRAssetTransfer> ListHeaderPending { get; set; }
		public List<PREmpDeduc> ListDed { get; set; }
        public List<HRAssetStaff> ListStaffAsset { get; set; }
		public List<HRAssetDepartment> ListAssetDepartment { get; set; }
		public List<HRAssetNoneStaff> ListAssetNoneStaff { get; set; }
		public List<HRAssetRegister> ListAsset { get; set; }
		public List<AssetTransferPending> ListAssetTransferPending { get; set; }
		public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
		public int Filter { get; set; }
		public AssetTransferObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
		public string GetDataCreateMuti(string ID)
		{
			Header = new HRAssetTransfer();
		    ListHeaderDetail = new List<AssetTransferDetail>();
			HashSet<string> empCodes = new HashSet<string>();
			string[] Emp = ID.Split(';');
			foreach (var item in Emp)
			{
				if (!string.IsNullOrEmpty(item))
				{
					string[] Train = item.Split(',');
					string EmpCode = Train.Length > 0 ? Train[0] : "";
					string AssetCode = Train.Length > 1 ? Train[1] : "";

					if (string.IsNullOrEmpty(EmpCode) || string.IsNullOrEmpty(AssetCode))
						continue;

					empCodes.Add(EmpCode);
					if (empCodes.Count > 1)
					{
						return "ASSET_DUP";
					}

					if (Filter == 1)
					{
						var AssetStaff = DB.HRAssetStaffs.FirstOrDefault(w => w.EmpCode == EmpCode && w.AssetCode == AssetCode);
						if (AssetStaff == null) continue;

						Header.ID = AssetStaff.ID;
						Header.EmpCode = AssetStaff.EmpCode;
						Header.EmployeName = AssetStaff.EmployeName;

						Header.FromDate = DateTime.Now;
						Header.ToDate = DateTime.Now;
						Header.Amount = 0;
						Header.Period = 1;
						Header.DedAmount = 0;

						ListHeaderDetail.Add(new AssetTransferDetail
						{
							AssetCode = AssetStaff.AssetCode,
							AssetDescription = AssetStaff.AssetDescription,
							AssignDate = AssetStaff.AssignDate,
							ReferenceNum = AssetStaff.ReferenceNum,
							Type= "IsStaff"
						});
					}	
					else if (Filter == 2)
					{
						var AssetDepartment = DB.HRAssetDepartments.FirstOrDefault(w => w.HandlerCode == EmpCode && w.AssetCode == AssetCode);
						if (AssetDepartment == null) continue;

						Header.ID = AssetDepartment.ID;
						Header.EmpCode = AssetDepartment.HandlerCode;
						Header.EmployeName = AssetDepartment.HandlerName;

						Header.FromDate = DateTime.Now;
						Header.ToDate = DateTime.Now;
						Header.Amount = 0;
						Header.Period = 1;
						Header.DedAmount = 0;

						ListHeaderDetail.Add(new AssetTransferDetail
						{
							AssetCode = AssetDepartment.AssetCode,
							AssetDescription = AssetDepartment.AssetDescription,
							AssignDate = AssetDepartment.AssignDate,
							ReferenceNum=AssetDepartment.ReferenceNum,
							Type = "IsDept"
						});
					}
					else if (Filter == 3)
					{
						var AssetNoneStaff = DB.HRAssetNoneStaffs.FirstOrDefault(w => w.HandlerCode == EmpCode && w.AssetCode == AssetCode);
						if (AssetNoneStaff == null) continue;

						Header.ID = AssetNoneStaff.ID;
						Header.EmpCode = AssetNoneStaff.HandlerCode;
						Header.EmployeName = AssetNoneStaff.HandlerName;

						Header.FromDate = DateTime.Now;
						Header.ToDate = DateTime.Now;
					    Header.Amount = 0;
					    Header.Period = 1;
						Header.DedAmount = 0;

						ListHeaderDetail.Add(new AssetTransferDetail
						{
							AssetCode = AssetNoneStaff.AssetCode,
							AssetDescription = AssetNoneStaff.AssetDescription,
							AssignDate = AssetNoneStaff.AssignDate,
							ReferenceNum = AssetNoneStaff.ReferenceNum,
							Type = "IsNone"
						});
					}
					else
					{
						var assetItem = ListAssetTransferPending
							.FirstOrDefault(w => w.EmpCode == EmpCode && w.AssetCode == AssetCode);

						if (assetItem == null) continue;

						Header.ID = assetItem.ID;
						Header.EmpCode = assetItem.EmpCode;
						Header.EmployeName = assetItem.EmployeName;

						Header.FromDate = DateTime.Now;
						Header.ToDate = DateTime.Now;
						Header.Amount = 0;
						Header.Period = 1;
						Header.DedAmount = 0;

						ListHeaderDetail.Add(new AssetTransferDetail
						{
							AssetCode = assetItem.AssetCode,
							AssetDescription = assetItem.AssetDescription,
							AssignDate = assetItem.AssignDate,
							ReferenceNum = assetItem.ReferenceNum,
							Type = assetItem.Type,
						});
					}

				}
			}
			return SYConstant.OK;
		}
		public string CreateMuti(out List<int> savedIds)
		{
			savedIds = new List<int>();
			if (ListHeaderDetail.Count == 0)
				return "NO_DATA";

			if (string.IsNullOrEmpty(Header.ReferenceNum))
				return "DOC_INV";

			if (DB.HRAssetTransfers.Any(w => w.ReferenceNum == Header.ReferenceNum))
				return "NUM_EXIT";

			try
			{
				var transferList = new List<HRAssetTransfer>();

				foreach (var detail in ListHeaderDetail)
				{
					if(detail.IsDedSalary == false && Header.IsDedSalary == false)
					{
						if (string.IsNullOrEmpty(Header.Receiver) && string.IsNullOrEmpty(detail.Receiver)) return "DOC_INV";
						if (string.IsNullOrEmpty(Header.Condition) && string.IsNullOrEmpty(detail.Condition)) return "DOC_INV";
						if (string.IsNullOrEmpty(Header.Location) && string.IsNullOrEmpty(detail.Location)) return "DOC_INV";
						if (!Header.ReturnDate.HasValue && !detail.ReturnDate.HasValue) return "DOC_INV";
					}
					var newHeader = new HRAssetTransfer
					{
						EmpCode = Header.EmpCode,
						EmployeName = Header.EmployeName,
						RemarkDate = Header.RemarkDate,
						RemarkDateDes = Header.RemarkDateDes,
						AssetCode = detail.AssetCode,
						AssetDescription = detail.AssetDescription,
						AssignDate = detail.AssignDate,
						ReturnDate = detail.ReturnDate ?? Header.ReturnDate,
						Receiver = detail.Receiver ?? Header.Receiver,
						IsDedSalary = detail.IsDedSalary || Header.IsDedSalary,
						Condition = detail.Condition ?? Header.Condition,
						Status = SYDocumentStatus.RETURN.ToString(),
						CreatedBy = User.UserName,
						CreatedOn = DateTime.Now,
						TransferType = detail.Type,
						Remark = detail.Remark ?? Header.Remark,
						ReferenceNum = Header.ReferenceNum,
						Location = detail.Location ?? Header.Location,
						Attachment = detail.AttachFile ?? Header.Attachment
					};

					transferList.Add(newHeader);
					DB.HRAssetTransfers.Add(newHeader);

					var assetRegs = DB.HRAssetRegisters.Where(w => w.AssetCode == detail.AssetCode).ToList();
					foreach (var reg in assetRegs)
					{
						reg.ReferenceNum = Header.ReferenceNum;
						if ((detail.Condition ?? Header.Condition) == "DAMAGE")
						{
							reg.StatusUse = "DAMAGE";
							reg.IsActive = false;
							reg.Status = "InActive";
						}
						else
						{
							reg.StatusUse = SYDocumentStatus.OPEN.ToString();
						}
						DB.HRAssetRegisters.Attach(reg);
						DB.Entry(reg).Property(x => x.StatusUse).IsModified = true;
						DB.Entry(reg).Property(x => x.IsActive).IsModified = true;
						DB.Entry(reg).Property(x => x.Status).IsModified = true;
						DB.Entry(reg).Property(x => x.ReferenceNum).IsModified = true;
					}

					string condition = detail.Condition ?? Header.Condition;
					DateTime? returnDate = detail.ReturnDate ?? Header.ReturnDate;

					if (Filter == 1)
					{
						var assets = DB.HRAssetStaffs.Where(w => w.AssetCode == detail.AssetCode && w.EmpCode == Header.EmpCode).ToList();
						foreach (var item in assets)
						{
							item.Status = SYDocumentStatus.RETURN.ToString();
							item.Condition = condition;
							item.ReturnDate = returnDate;

							DB.HRAssetStaffs.Attach(item);
							DB.Entry(item).Property(x => x.Status).IsModified = true;
							DB.Entry(item).Property(x => x.Condition).IsModified = true;
							DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
						}
					}
				    else if (Filter == 2) 
					{
						var assets = DB.HRAssetDepartments.Where(w => w.AssetCode == detail.AssetCode && w.HandlerCode == Header.EmpCode).ToList();
						foreach (var item in assets)
						{
							item.Status = SYDocumentStatus.RETURN.ToString();
							item.Condition = condition;
							item.ReturnDate = returnDate;

							DB.HRAssetDepartments.Attach(item);
							DB.Entry(item).Property(x => x.Status).IsModified = true;
							DB.Entry(item).Property(x => x.Condition).IsModified = true;
							DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
						}
					}
					else if (Filter == 3) 
					{
						var assets = DB.HRAssetNoneStaffs.Where(w => w.AssetCode == detail.AssetCode && w.HandlerCode == Header.EmpCode).ToList();
						foreach (var item in assets)
						{
							item.Status = SYDocumentStatus.RETURN.ToString();
							item.Condition = condition;
							item.ReturnDate = returnDate;

							DB.HRAssetNoneStaffs.Attach(item);
							DB.Entry(item).Property(x => x.Status).IsModified = true;
							DB.Entry(item).Property(x => x.Condition).IsModified = true;
							DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
						}
					}
					else 
					{
						if (detail.Type == "IsStaff")
						{
							var assets = DB.HRAssetStaffs.Where(w => w.AssetCode == detail.AssetCode && w.EmpCode == Header.EmpCode).ToList();
							foreach (var item in assets)
							{
								item.Status = SYDocumentStatus.RETURN.ToString();
								item.Condition = condition;
								item.ReturnDate = returnDate;

								DB.HRAssetStaffs.Attach(item);
								DB.Entry(item).Property(x => x.Status).IsModified = true;
								DB.Entry(item).Property(x => x.Condition).IsModified = true;
								DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
							}
						}
						else if(detail.Type == "IsDept")
						{
							var assets = DB.HRAssetDepartments.Where(w => w.AssetCode == detail.AssetCode && w.HandlerCode == Header.EmpCode).ToList();
							foreach (var item in assets)
							{
								item.Status = SYDocumentStatus.RETURN.ToString();
								item.Condition = condition;
								item.ReturnDate = returnDate;

								DB.HRAssetDepartments.Attach(item);
								DB.Entry(item).Property(x => x.Status).IsModified = true;
								DB.Entry(item).Property(x => x.Condition).IsModified = true;
								DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
							}
						}
						else if( detail.Type == "IsNone")
						{
							var assets = DB.HRAssetNoneStaffs.Where(w => w.AssetCode == detail.AssetCode && w.HandlerCode == Header.EmpCode).ToList();
							foreach (var item in assets)
							{
								item.Status = SYDocumentStatus.RETURN.ToString();
								item.Condition = condition;
								item.ReturnDate = returnDate;

								DB.HRAssetNoneStaffs.Attach(item);
								DB.Entry(item).Property(x => x.Status).IsModified = true;
								DB.Entry(item).Property(x => x.Condition).IsModified = true;
								DB.Entry(item).Property(x => x.ReturnDate).IsModified = true;
							}
						}
					}
				}

				DB.SaveChanges();
				savedIds.AddRange(transferList.Select(x => x.ID));

				if (savedIds.Count == 0)
					return "NO_VALID_ASSETS";

				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				SYEventLog log = new SYEventLog();
				log.ScreenId = ScreenId;
				log.UserId = User.UserName;
				log.DocurmentAction = Header.ID.ToString();
				log.Action = SYActionBehavior.ADD.ToString();
				SYEventLogObject.saveEventLog(log, e);
				return "EE001";
			}
			catch (DbUpdateException e)
			{
				SYEventLog log = new SYEventLog();
				log.ScreenId = ScreenId;
				log.UserId = User.UserName;
				log.DocurmentAction = Header.ID.ToString();
				log.Action = SYActionBehavior.ADD.ToString();
				SYEventLogObject.saveEventLog(log, e, true);
				return "EE001";
			}
			catch (Exception e)
			{
				SYEventLog log = new SYEventLog();
				log.ScreenId = ScreenId;
				log.UserId = User.UserName;
				log.DocurmentAction = Header.ID.ToString();
				log.Action = SYActionBehavior.ADD.ToString();
				SYEventLogObject.saveEventLog(log, e, true);
				return "EE001";
			}
		}
		public string Update(string id,int checkEdit)
        {
            try
            {
                DB = new HumicaDBContext();
                int ID = Convert.ToInt32(id);
                var ObjMatch = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
				if (ObjMatch == null)
                {
                    return "DOC_INV";
                }

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.ReturnDate = Header.ReturnDate;
                ObjMatch.Remark = Header.Remark;
                ObjMatch.Condition = Header.Condition;
                ObjMatch.Attachment = Header.Attachment;
                ObjMatch.IsDedSalary = Header.IsDedSalary;
                ObjMatch.DedType = Header.DedType;
                ObjMatch.FromDate = Header.FromDate;
                ObjMatch.ToDate = Header.ToDate;
                ObjMatch.Period = Header.Period;
                ObjMatch.DedAmount = Header.DedAmount;
                ObjMatch.Amount = Header.Amount;
                ObjMatch.Receiver = Header.Receiver;
				ObjMatch.Location = Header.Location;
				ObjMatch.RemarkDate = Header.RemarkDate;
				ObjMatch.RemarkDateDes = Header.RemarkDateDes;
				var checkdAssetstaff = DB.HRAssetStaffs.Where(w => w.AssetCode == Header.AssetCode && w.EmpCode == Header.EmpCode).ToList();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "Ded").ToList();
                foreach (var read in checkdAssetstaff.ToList())
                {
                    
                    read.Status = SYDocumentStatus.RETURN.ToString();
                    read.Condition = Header.Condition;
                    read.ReturnDate = Header.ReturnDate;
                    DB.HRAssetStaffs.Attach(read);
                    DB.Entry(read).Property(x => x.Status).IsModified = true;
                    DB.Entry(read).Property(x => x.Condition).IsModified = true;
                    DB.Entry(read).Property(x => x.ReturnDate).IsModified = true;
                }

                var checkdup = DB.HRAssetRegisters.Where(w => w.AssetCode == Header.AssetCode).ToList();
                foreach (var read in checkdup.ToList())
                {
                    if (Header.Condition == "DAMAGE" || Header.Condition=="LOST")
                    {
                        read.Condition = Header.Condition;
                    }
                    else
                    {
                        read.Condition = "Good";
                        read.StatusUse = SYDocumentStatus.OPEN.ToString();
                    }
                    DB.HRAssetRegisters.Attach(read);
                    DB.Entry(read).Property(x => x.StatusUse).IsModified = true;
                    DB.Entry(read).Property(x => x.Condition).IsModified = true;
                }
                DB.HRAssetTransfers.Attach(ObjMatch);

                if (Header.IsDedSalary == true && checkEdit == 1)
                {
					if (ListDed != null)
                    {
						//var existingDeductions = DB.PREmpDeducs.Where(u => u.EmpCode == Header.EmpCode && u.StatusAssetDed != SYDocumentStatus.CLEARED.ToString()).ToList();
						foreach (var row in ListDed)
                        {
                            var ded = DB.PREmpDeducs.Where(u => u.TranNo == row.TranNo).FirstOrDefault();
                            if (ded != null)
                            {
                                ded.Amount = row.Amount;
                                ded.Remark = row.Remark;
                                ded.ChangedBy = User.UserName;
                                ded.ChangedOn = DateTime.Now;
                                DB.PREmpDeducs.Attach(ded);
                                DB.Entry(ded).Property(x => x.Amount).IsModified = true;
                                DB.Entry(ded).Property(x => x.Remark).IsModified = true;
                                DB.Entry(ded).Property(x => x.ChangedBy).IsModified = true;
                                DB.Entry(ded).Property(x => x.ChangedOn).IsModified = true;
                            }
                            else
                            {
                                //var TranNo = DB.PREmpDeducs.OrderByDescending(u => u.TranNo).FirstOrDefault();
                                var _ded = new PREmpDeduc();
                                if (row.TranNo != null)
                                {
                                    _ded.TranNo = row.TranNo;
                                }
                                else
                                {
                                    _ded.TranNo = 1;
                                }
                                _ded.EmpCode = Header.EmpCode;
                                _ded.EmpName = Header.EmployeName;
                                _ded.DedCode = Header.DedType;
                                var DedDes = RewardType.Where(w => w.Code == Header.DedType).First();
                                _ded.DedDescription = DedDes.Description;
                                _ded.FromDate = row.FromDate;
                                _ded.ToDate = row.ToDate;
                                _ded.Amount = row.Amount;
                                _ded.Status = 1;
                                _ded.AssetTransferID = ID;
                                _ded.StatusAssetDed = SYDocumentStatus.OPEN.ToString();
                                _ded.CreateBy = User.UserName;
                                _ded.CreateOn = DateTime.Now;
                                DB.PREmpDeducs.Add(_ded);
                            }

                        }
						//Remove entries where TranNo does not exist in ListDed
						//var validTranNo = ListDed.Select(x => x.TranNo).ToList();
						//var toRemove = existingDeductions.Where(d => !validTranNo.Contains(d.TranNo) && d.DedCode == Header.DedType).ToList();

						//if (toRemove.Any())
						//{
						//	DB.PREmpDeducs.RemoveRange(toRemove);
						//}
					}
                }

                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ReturnDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Attachment).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.IsDedSalary).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Condition).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.Receiver).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.Location).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.RemarkDate).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.RemarkDateDes).IsModified = true;
				if (checkEdit == 1)
                {
                   DB.Entry(ObjMatch).Property(x => x.FromDate).IsModified = true;
                   DB.Entry(ObjMatch).Property(x => x.ToDate).IsModified = true;
                   DB.Entry(ObjMatch).Property(x => x.DedType).IsModified = true;
                   DB.Entry(ObjMatch).Property(x => x.Period).IsModified = true;
                   DB.Entry(ObjMatch).Property(x => x.Amount).IsModified = true;
                   DB.Entry(ObjMatch).Property(x => x.DedAmount).IsModified = true;
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
		public string UpdateAmoutDeu(string id)
		{
			try
			{
				DB = new HumicaDBContext();
				int ID = Convert.ToInt32(id);
				var ObjMatch = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
				if (ObjMatch == null||ListDed.Count() == 0|| Header.DedAmount == 0 || Header.Amount == 0 || Header.DedType == null)
				{
					return "DOC_INV";
				}
				// Update header values
				ObjMatch.DedType = Header.DedType;
				ObjMatch.FromDate = Header.FromDate;
				ObjMatch.ToDate = Header.ToDate;
				ObjMatch.Period = Header.Period;
				ObjMatch.DedAmount = Header.DedAmount;
				ObjMatch.Amount = Header.Amount;

				// Mark modified properties
				DB.Entry(ObjMatch).Property(x => x.FromDate).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.ToDate).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.DedType).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.Period).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.Amount).IsModified = true;
				DB.Entry(ObjMatch).Property(x => x.DedAmount).IsModified = true;

				var RewardTypeList = DB.PR_RewardsType.Where(w => w.ReCode == "Ded").ToList();
				var lastTranNo = DB.PREmpDeducs.OrderByDescending(u => u.TranNo).FirstOrDefault()?.TranNo ?? 0;
				if (ListDed != null)
				{
					foreach (var row in ListDed)
					{
						lastTranNo++;
						var ded = new PREmpDeduc
						{
							TranNo = lastTranNo,
							EmpCode = Header.EmpCode,
							EmpName = Header.EmployeName,
							DedCode = Header.DedType,
							FromDate = row.FromDate,
							ToDate = row.ToDate,
							Amount = row.Amount,
							Status = 1,
							AssetTransferID = ID,
							StatusAssetDed = SYDocumentStatus.OPEN.ToString(),
							CreateBy = User.UserName,
							CreateOn = DateTime.Now
						};
						var DedDes = RewardTypeList.FirstOrDefault(w => w.Code == Header.DedType);
						if (DedDes != null)
						{
							ded.DedDescription = DedDes.Description;
						}
						DB.PREmpDeducs.Add(ded);
					}
				}
				DB.SaveChanges();
				return SYConstant.OK;
			}
			catch (DbEntityValidationException e)
			{
				SYEventLog log = new SYEventLog
				{
					ScreenId = ScreenId,
					UserId = User.UserName,
					DocurmentAction = id,
					Action = SYActionBehavior.EDIT.ToString()
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
					DocurmentAction = id,
					Action = SYActionBehavior.EDIT.ToString()
				};
				SYEventLogObject.saveEventLog(log, e, true);
				return "EE001";
			}
		}
		public string Delete(string id)
        {
            try
            {
                DB = new HumicaDBContext();

                int ID = Convert.ToInt32(id);
                var ObjMatch = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
                string RETURN = SYDocumentStatus.RETURN.ToString();
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                if (ObjMatch.Status == RETURN)
                {
                    var checkdup = DB.HRAssetRegisters.Where(w => w.AssetCode == ObjMatch.AssetCode).ToList();
                    foreach (var read in checkdup.ToList())
                    {
                        read.StatusUse = SYDocumentStatus.ASSIGN.ToString();
                        DB.HRAssetRegisters.Attach(read);
                        DB.Entry(read).Property(x => x.StatusUse).IsModified = true;
                    }
					if (ObjMatch.TransferType == "IsStaff")
					{
						var AssetStaff = DB.HRAssetStaffs.Where(w => w.AssetCode == ObjMatch.AssetCode && w.EmpCode == ObjMatch.EmpCode).ToList();
						foreach (var read in AssetStaff.ToList())
						{
							read.Status = SYDocumentStatus.ASSIGN.ToString();
							DB.HRAssetStaffs.Attach(read);
							DB.Entry(read).Property(x => x.Status).IsModified = true;
						}
					}
					else if(ObjMatch.TransferType == "IsDept")
					{
						var AssetStaff = DB.HRAssetDepartments.Where(w => w.AssetCode == ObjMatch.AssetCode && w.HandlerCode == ObjMatch.EmpCode).ToList();
						foreach (var read in AssetStaff.ToList())
						{
							read.Status = SYDocumentStatus.ASSIGN.ToString();
							DB.HRAssetDepartments.Attach(read);
							DB.Entry(read).Property(x => x.Status).IsModified = true;
						}
					}
					else if (ObjMatch.TransferType == "IsNone")
					{
						var AssetStaff = DB.HRAssetNoneStaffs.Where(w => w.AssetCode == ObjMatch.AssetCode && w.HandlerCode == ObjMatch.EmpCode).ToList();
						foreach (var read in AssetStaff.ToList())
						{
							read.Status = SYDocumentStatus.ASSIGN.ToString();
							DB.HRAssetNoneStaffs.Attach(read);
							DB.Entry(read).Property(x => x.Status).IsModified = true;
						}
					}
					int _ID = Convert.ToInt32(id);
                    var ItemDed = DB.PREmpDeducs.Where(w => w.AssetTransferID == _ID).ToList();
                    if (ItemDed != null)
                    {
                        foreach(var obj in ItemDed)
                        {
                            DB.PREmpDeducs.Attach(obj);
                            DB.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }
                    DB.HRAssetTransfers.Remove(ObjMatch);
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
                    objmatch.ReturnDate = Header.ReturnDate;
                    objmatch.Status = SYDocumentStatus.RETURN.ToString();
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.HRAssetStaffs.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.ReturnDate).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                    var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "DED").ToList();
                    var DedType = DB.PR_RewardsType.Where(w => w.Code == Header.DedType).FirstOrDefault();
                    var TranNo = DB.PREmpDeducs.OrderByDescending(u => u.TranNo).FirstOrDefault();
                    long TN = 0;
                    TN = (TranNo == null) ? 0 : TranNo.TranNo;
                    if (Header.IsDedSalary==true)
                    {
                        foreach(var item in ListDed)
                        {
                            var obj = new PREmpDeduc();
                            int status = 0;
                            int FDate = item.FromDate.Value.Month;
                            int TDate = item.ToDate.Value.Month;

                            if (FDate == TDate && item.FromDate.Value.Year == item.ToDate.Value.Year)
                            {
                                status = 1;
                            }

                            obj.Status = status;
                            obj.LCK = 0;
                            obj.DedCode = Header.DedType;
                            obj.DedDescription = DedType.Description;
                            obj.EmpCode = Header.EmpCode;
                            obj.EmpName = HeaderStaff.AllName;
                            obj.CreateBy = User.UserName;
                            obj.CreateOn = DateTime.Now;
                            obj.TranNo = TN + 1;
                            obj.FromDate = item.FromDate;
                            obj.ToDate = item.ToDate;
                            obj.Amount = item.Amount;
                            obj.Remark = item.Remark;
                            DB.PREmpDeducs.Add(obj);
                            TN++;
                        }
                    }
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
		public List<AssetTransferPending> getDataPending()
		{
			var result = new List<AssetTransferPending>();
			var statusAssign = SYDocumentStatus.ASSIGN.ToString();

			var staffAssigns = DB.HRAssetStaffs
				.Where(s => s.Status == statusAssign)
				.Select(s => new AssetTransferPending
				{
					ID = s.ID,
					ReferenceNum = s.ReferenceNum,
					EmpCode = s.EmpCode,
					EmployeName = s.EmployeName,
					AssignDate = s.AssignDate,
					ReturnDate = s.ReturnDate,
					AssetCode = s.AssetCode,
					AssetDescription = s.AssetDescription,
					Status = s.Status,
					Type = "IsStaff"
				}).ToList();
			result.AddRange(staffAssigns);

			var deptAssigns = DB.HRAssetDepartments
				.Where(d => d.Status == statusAssign)
				.Select(d => new AssetTransferPending
				{
					ID = d.ID,
					ReferenceNum = d.ReferenceNum,
					EmpCode = d.HandlerCode,
					EmployeName = d.HandlerName,
					AssignDate = d.AssignDate,
					ReturnDate = d.ReturnDate,
					AssetCode = d.AssetCode,
					AssetDescription = d.AssetDescription,
					Status = d.Status,
					Type = "IsDept"
				}).ToList();
			result.AddRange(deptAssigns);

			var noneAssigns = DB.HRAssetNoneStaffs
				.Where(d => d.Status == statusAssign)
				.Select(d => new AssetTransferPending
				{
					ID = d.ID,
					ReferenceNum = d.ReferenceNum,
					EmpCode = d.HandlerCode,
					EmployeName = d.HandlerName,
					AssignDate = d.AssignDate,
					ReturnDate = d.ReturnDate,
					AssetCode = d.AssetCode,
					AssetDescription = d.AssetDescription,
					Status = d.Status,
					Type = "IsNone"
				}).ToList();
			result.AddRange(noneAssigns);

			return result.OrderByDescending(x => x.ReferenceNum).ToList();
		}

	}
	public class AssetTransferDetail
	{
		public string ReferenceNum { get; set; }
		public string AssetCode { get; set; }
		public string AssetDescription { get; set; }
		public DateTime AssignDate { get; set; }
		public string Receiver { get; set; }
		public DateTime? ReturnDate { get; set; }
		public bool IsDedSalary { get; set; }
		public string Condition { get; set; }
		public string Location { get; set; }
		public string Remark { get; set; }
		public string AttachFile { get; set; }
		public string Type { get; set; }
	}
	public class AssetTransferPending
	{
		public string ReferenceNum { get; set; }
		public int ID { get; set; }
		public string EmpCode { get; set; }
		public string EmployeName { get; set; }
		public DateTime AssignDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public string AssetCode { get; set; }
		public string AssetDescription { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
	}
}
