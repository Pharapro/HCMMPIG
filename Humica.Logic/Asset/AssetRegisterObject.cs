using Humica.Core.DB;
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

namespace Humica.Logic.Asset
{
    public class AssetRegisterObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string Code { get; set; }
        public HRAssetRegister Header { get; set; }
        public List<HRAssetRegister> ListHeader { get; set; }
        public List<HRAssetStaff> ListAssetStaff { get; set; }
        public List<HRAssetTransfer> ListAssetTransfer { get; set; }
		public List<AssetRegisterQTYDetail> ListAssetQTYDetail { get; set; }
		public List<ListRecordDetail> ListRecordDetail { get; set; }
		public List<MDUploadTemplate> ListTemplate { get; set; }
        public AssetRegisterObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
		//    public string CreateFixAsset()
		//    {
		//        try
		//        {
		//            DB = new HumicaDBContext();
					//if (ListAssetQTYDetail.Count == 0)
					//{
					//	return "NO_DATA";
					//}
					//foreach (var staffs in ListAssetQTYDetail)
		//            {
					//	var header = new HRAssetRegister();
					//	var objMatch = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == Header.AssetCode);
					//	if (objMatch != null) return "ASSET_EX";

					//	var objClass = DB.HRAssetClasses.Find(Header.AssetClassCode);

					//	header.SerialNumber = staffs.AssetCode;
					//	header.SerialNumber= staffs.SerialNumber;
					//	header.Model = staffs.Model;
					//	header.Qty = staffs.QTY;
					//	header.StatusUse = SYDocumentStatus.OPEN.ToString();
					//	header.IsActive = Header.Status == "A" || Header.Status == "Active";

					//	header.IsCombone = Header.IsCombone;
					//	header.AssetClassCode = Header.AssetClassCode;
					//	header.Description = Header.Description;
					//	header.PropertyType = Header.PropertyType;
					//	header.Status = Header.Status;
					//	header.UsefulLifeYear = Header.UsefulLifeYear;
					//	header.ReceiptDate = Header.ReceiptDate;
					//	header.AcquisitionCost = Header.AcquisitionCost;
					//	header.BranchCode = Header.BranchCode;
					//	header.BuildingCD = Header.BuildingCD;
					//	header.Floor = Header.Floor;
					//	header.Room = Header.Room;
					//	header.DepartmentCD = Header.DepartmentCD;
					//	header.Reason = Header.Reason;
					//	header.TagNbr = Header.TagNbr;
					//	header.Location = Header.Location;
					//	header.WarrantyExpirationDate = Header.WarrantyExpirationDate;
					//	header.OPNumber = Header.OPNumber;
					//	header.ReceiptNumber = Header.ReceiptNumber;
					//	header.BuildingNumber = Header.BuildingNumber;
					//	header.Condition = Header.Condition.ToUpper();
					//	header.CreatedBy = User.UserName;
					//	header.CreatedOn = DateTime.Now;

					//	if (Header.IsCombone == true)
		//                {
					//		if (objClass != null && !string.IsNullOrWhiteSpace(objClass.NumberRank))
		//                    {
					//			header.AssetTypeID = objClass.AssetTypeCode;
					//		}
					//		header.AssetCode=staffs.AssetCode;
					//	}
		//                else
		//                {
					//		if (objClass != null && !string.IsNullOrWhiteSpace(objClass.NumberRank))
					//		{
					//			header.AssetTypeID = objClass.AssetTypeCode;

					//			var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, Header.CreatedOn.Year, true);
					//			if (objNumber?.NextNumberRank == null)
					//			{
					//				return "NUMBER_RANK_NE";
					//			}
					//			header.AssetCode = objNumber.NextNumberRank;
					//		}
					//	}
					//	if (string.IsNullOrWhiteSpace(header.AssetCode))
					//	{
					//		return "CODE_REQUIRED";
					//	}
					//	DB.HRAssetRegisters.Add(header);
					//}
					//DB.SaveChanges();
					//if (Header.IsCombone)
		//            {
		//                var objClass = DB.HRAssetClasses.Find(Header.AssetClassCode);
		//                if (objClass != null && !string.IsNullOrWhiteSpace(objClass.NumberRank))
		//                {
		//                    var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, Header.CreatedOn.Year, true);
		//                    if (objNumber?.NextNumberRank == null)
		//                    {
		//                        return "NUMBER_RANK_NE";
		//                    }
		//                }
		//            }
		//            return SYConstant.OK;
		//        }
		//        catch (DbEntityValidationException e)
		//        {
		//            /*------------------SaveLog----------------------------------*/
		//            SYEventLog log = new SYEventLog();
		//            log.ScreenId = ScreenId;
		//            log.UserId = User.UserName;
		//            log.DocurmentAction = Header.AssetCode;
		//            log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
		//            SYEventLogObject.saveEventLog(log, e);
		//            /*----------------------------------------------------------*/
		//            return "EE001";
		//        }
		//        catch (DbUpdateException e)
		//        {
		//            /*------------------SaveLog----------------------------------*/
		//            SYEventLog log = new SYEventLog();
		//            log.ScreenId = ScreenId;
		//            log.UserId = User.UserName;
		//            log.DocurmentAction = Header.AssetCode;
		//            log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
		//            SYEventLogObject.saveEventLog(log, e, true);
		//            /*----------------------------------------------------------*/
		//            return "EE001";
		//        }
		//        catch (Exception e)
		//        {
		//            /*------------------SaveLog----------------------------------*/
		//            SYEventLog log = new SYEventLog();
		//            log.ScreenId = ScreenId;
		//            log.UserId = User.UserName;
		//            log.DocurmentAction = Header.AssetCode;
		//            log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
		//            SYEventLogObject.saveEventLog(log, e, true);
		//            /*----------------------------------------------------------*/
		//            return "EE001";
		//        }
		//    }
		public string CreateFixAsset(out List<string> savedCodes)
		{
			savedCodes = new List<string>();
			using (var db = new HumicaDBContext())
			{
				try
				{
					if (ListAssetQTYDetail == null || ListAssetQTYDetail.Count == 0)
					{
						return "NO_DATA";
					}
					if (Header == null || User == null)
					{
						return "DOC_INV";
					}
					var assetCodes = ListAssetQTYDetail.Select(s => s.AssetCode).ToList();
					var existingCodes = db.HRAssetRegisters.Where(r => assetCodes.Contains(r.AssetCode)).Select(r => r.AssetCode).ToList();
					if (existingCodes.Any())
					{
						return "ASSET_EX";
					}
					var objClass = db.HRAssetClasses.Find(Header.AssetClassCode);
					if (objClass == null)
					{
						return "DOC_INV";
					}
					foreach (var staff in ListAssetQTYDetail)
					{
						var header = new HRAssetRegister
						{
							AssetCode = staff.AssetCode,
							SerialNumber = staff.SerialNumber,
							Model = staff.Model,
							Qty = staff.QTY,
							StatusUse = SYDocumentStatus.OPEN.ToString(),
							IsActive = Header.Status == "A" || Header.Status == "Active",
							IsCombone = Header.IsCombone,
							AssetClassCode = Header.AssetClassCode,
							Description = Header.Description,
							PropertyType = Header.PropertyType,
							Status = Header.Status,
							UsefulLifeYear = Header.UsefulLifeYear,
							ReceiptDate = Header.ReceiptDate,
							AcquisitionCost = Header.AcquisitionCost,
							BranchCode = Header.BranchCode,
							BuildingCD = Header.BuildingCD,
							Floor = Header.Floor,
							Room = Header.Room,
							DepartmentCD = Header.DepartmentCD,
							Reason = Header.Reason,
							TagNbr = Header.TagNbr,
							Location = Header.Location,
							WarrantyExpirationDate = Header.WarrantyExpirationDate,
							OPNumber = Header.OPNumber,
							ReceiptNumber = Header.ReceiptNumber,
							BuildingNumber = Header.BuildingNumber,
							Condition = Header.Condition?.ToUpper(),
							CreatedBy = User.UserName,
							CreatedOn = DateTime.Now
						};

						if (Header.IsCombone)
						{
							header.AssetCode = staff.AssetCode;
							header.AssetTypeID = !string.IsNullOrWhiteSpace(objClass.NumberRank) ? objClass.AssetTypeCode : null;
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(objClass.NumberRank))
							{
								var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, Header.CreatedOn.Year, true);
								if (objNumber?.NextNumberRank == null)
								{
									return "NUMBER_RANK_NE";
								}
								header.AssetCode = objNumber.NextNumberRank;
								header.AssetTypeID = objClass.AssetTypeCode;
							}
						}

						if (string.IsNullOrWhiteSpace(header.AssetCode))
						{
							return "CODE_REQUIRED";
						}
						db.HRAssetRegisters.Add(header);
						savedCodes.Add(header.AssetCode);
					}
					if (!string.IsNullOrWhiteSpace(objClass.NumberRank) && Header.IsCombone)
					{
						var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, Header.CreatedOn.Year, true);
						if (objNumber?.NextNumberRank == null)
							return "NUMBER_RANK_NE";
					}
					db.SaveChanges();
					return SYConstant.OK;
				}
				catch (DbEntityValidationException e)
				{
					SYEventLog log = new SYEventLog
					{
						ScreenId = ScreenId,
						UserId = User.UserName,
						DocurmentAction = Header.AssetCode,
						Action = Humica.EF.SYActionBehavior.ADD.ToString()
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
						DocurmentAction = Header.AssetCode,
						Action = Humica.EF.SYActionBehavior.ADD.ToString()
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
						DocurmentAction = Header.AssetCode,
						Action = Humica.EF.SYActionBehavior.ADD.ToString()
					};
					SYEventLogObject.saveEventLog(log, e, true);
					return "EE001";
				}
			}
		}
		public string EditFixAsset(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                var obj = DB.HRAssetRegisters.SingleOrDefault(w => w.AssetCode == id);
                if (obj != null)
                {
                    //Asset Summary
                    obj.BranchCode = Header.BranchCode;
                    obj.Description = Header.Description;
                    if (Header.Status == "Active" || Header.Status == "A")
                    {
                        obj.IsActive = true;
                        obj.Status = Header.Status;
                    }
                    else
                    {
                        obj.IsActive = false;
                        obj.Status = Header.Status;
                    }
                    obj.StatusUse = Header.StatusUse;
                    obj.PropertyType = Header.PropertyType;
                    obj.UsefulLifeYear = Header.UsefulLifeYear;
                    obj.Qty = Header.Qty;
                    obj.ReceiptDate = Header.ReceiptDate;
                    obj.AcquisitionCost = Header.AcquisitionCost;

                    //TrackingInfo
                    obj.BuildingCD = Header.BuildingCD;
                    obj.Floor = Header.Floor;
                    obj.Room = Header.Room;
                    obj.DepartmentCD = Header.DepartmentCD;
                    obj.Reason = Header.Reason;
                    obj.TagNbr = Header.TagNbr;
					obj.Location = Header.Location;

					//Purchase Info
					obj.Model = Header.Model;
                    obj.SerialNumber = Header.SerialNumber;
                    obj.WarrantyExpirationDate = Header.WarrantyExpirationDate;
                    obj.Condition = Header.Condition;
                    obj.OPNumber = Header.OPNumber;
                    obj.ReceiptNumber = Header.ReceiptNumber;
                    obj.BuildingNumber = Header.BuildingNumber;

                    //Photo
                    obj.Images = Header.Images;

                    obj.ChangedBy = User.UserName;
                    obj.ChangedOn = DateTime.Now;

                    if (obj.Status == "A")
                    {
                        obj.IsActive = true;
                    }

                    DB.Entry(obj).State = EntityState.Modified;

                }
                int row1 = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();
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
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();
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
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteFixAsset(string ID)
        {
            try
            {
                Header = new HRAssetRegister();
                var objMatch = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == ID);
                if (objMatch == null)
                {
                    return "ASSET_EX";
                }
                DB.HRAssetRegisters.Remove(objMatch);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();
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
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();
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
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();
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

                    foreach (var staffs in ListHeader.ToList())
                    {
                        var objClass = DB.HRAssetClasses.Find(staffs.AssetClassCode);
                        string baseAssetCode = staffs.AssetCode?.Replace("/", "-");

                        int qty = staffs.Qty == null ? 1 : Convert.ToInt32(staffs.Qty);
                        qty = qty <= 0 ? 1 : qty;
						if (staffs.IsCombone == true && objClass != null)
						{
							if (!string.IsNullOrEmpty(objClass.NumberRank))
							{
								var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, DateTime.Now.Year, true);
								if (string.IsNullOrEmpty(objNumber?.NextNumberRank))
								{
									return "NUMBER_RANK_NE";
								}
								baseAssetCode = objNumber.NextNumberRank;
							}
						}
						for (int i = 1; i <= qty; i++)
                        {
                            Header = new HRAssetRegister();
                            Header.AssetCode = qty > 1 ? $"{baseAssetCode}-{i}" : baseAssetCode;
							var objMatch = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == Header.AssetCode);
							if (objMatch != null) return "Asset code already exit";
							Header.AssetClassCode = staffs.AssetClassCode;
                            Header.Description = staffs.Description;
                            Header.PropertyType = staffs.PropertyType;
                            Header.Status = "Active";
                            Header.StatusUse = SYDocumentStatus.OPEN.ToString();
                            Header.UsefulLifeYear = staffs.UsefulLifeYear;
                            Header.Qty = staffs.Qty;
                            Header.ReceiptDate = staffs.ReceiptDate;
                            Header.AcquisitionCost = staffs.AcquisitionCost;
                            Header.BranchCode = staffs.BranchCode;
                            Header.BuildingCD = staffs.BuildingCD;
                            Header.Floor = staffs.Floor;
                            Header.Room = staffs.Room;
                            Header.DepartmentCD = staffs.DepartmentCD;
                            Header.Reason = staffs.Reason;
                            Header.TagNbr = staffs.TagNbr;
                            Header.Model = staffs.Model;
                            Header.SerialNumber = staffs.SerialNumber;
                            Header.WarrantyExpirationDate = staffs.WarrantyExpirationDate;
                            Header.OPNumber = staffs.OPNumber;
                            Header.ReceiptNumber = staffs.ReceiptNumber;
                            Header.BuildingNumber = staffs.BuildingNumber;
                            Header.Condition = staffs.Condition.ToUpper();
                            Header.CreatedBy = User.UserName;
                            Header.CreatedOn = DateTime.Now;
                            Header.IsActive = true;
							if (staffs.IsCombone == true)
							{
								Header.AssetCode = $"{baseAssetCode}-{i}";
							}
							else if (objClass != null && !string.IsNullOrEmpty(objClass.NumberRank))
							{
								var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, DateTime.Now.Year, true);
								if (string.IsNullOrEmpty(objNumber?.NextNumberRank))
								{
									return "NUMBER_RANK_NE";
								}
								Header.AssetCode = objNumber.NextNumberRank;
							}
                            if (string.IsNullOrEmpty(Header.AssetCode))
                            {
                                return "CODE_REQUIRED";
                            }

                            DB.HRAssetRegisters.Add(Header);
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
                log.DocurmentAction = Header.AssetCode;
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
                log.DocurmentAction = Header.AssetCode;
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
                log.DocurmentAction = Header.AssetCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string GetAssetCode(string id)
        {
            string getAssetCode = null;
			var objClass = DB.HRAssetClasses.Find(id);
			if (objClass != null && !string.IsNullOrWhiteSpace(objClass.NumberRank))
			{
				var objNumber = new CFNumberRank(objClass.NumberRank, DocConfType.FixedAsset, Header.CreatedOn.Year, true);
				if (objNumber?.NextNumberRank == null)
				{
					return "NUMBER_RANK_NE";
				}
				getAssetCode = objNumber.NextNumberRank;
			}
            return getAssetCode;
		}
		public List<ListRecordDetail> LoadRecordDetail(string AssetCode)
		{
			var result = new List<ListRecordDetail>();
			var statusAssign = SYDocumentStatus.ASSIGN.ToString();
			var statusReturn = SYDocumentStatus.RETURN.ToString();

			var staffAssigns = DB.HRAssetStaffs
				.Where(s => s.AssetCode == AssetCode)
				.Select(s => new ListRecordDetail
				{
					ReferenceNum = s.ReferenceNum,
					EmpCode = s.EmpCode,
					EmployeName = s.EmployeName,
					AssignDate = s.AssignDate,
					ReturnDate = s.ReturnDate,
					AssetCode = s.AssetCode,
					AssetDescription = s.AssetDescription,
					Status = statusAssign,
					Type = "Asset Staff"
				}).ToList();
			result.AddRange(staffAssigns);

			var deptAssigns = DB.HRAssetDepartments
				.Where(d => d.AssetCode == AssetCode)
				.Select(d => new ListRecordDetail
				{
					ReferenceNum = d.ReferenceNum,
					EmpCode = d.HandlerCode,
					EmployeName = d.HandlerName,
					AssignDate = d.AssignDate,
					ReturnDate = d.ReturnDate,
					AssetCode = d.AssetCode,
					AssetDescription = d.AssetDescription,
					Status = statusAssign,
					Type = "Asset Department"
				}).ToList();
			result.AddRange(deptAssigns);

			var noneAssigns = DB.HRAssetNoneStaffs
				.Where(d => d.AssetCode == AssetCode)
				.Select(d => new ListRecordDetail
				{
					ReferenceNum = d.ReferenceNum,
					EmpCode = d.HandlerCode,
					EmployeName = d.HandlerName,
					AssignDate = d.AssignDate,
					ReturnDate = d.ReturnDate,
					AssetCode = d.AssetCode,
					AssetDescription = d.AssetDescription,
					Status = statusAssign,
					Type = "Asset NoneStaff"
				}).ToList();
			result.AddRange(noneAssigns);

			var transfers = DB.HRAssetTransfers
				.Where(t => t.AssetCode == AssetCode)
				.Select(t => new ListRecordDetail
				{
					ReferenceNum = t.ReferenceNum,
					EmpCode = t.EmpCode,
					EmployeName = t.EmployeName,
					AssignDate = t.AssignDate,
					ReturnDate = t.ReturnDate,
					AssetCode = t.AssetCode,
					AssetDescription = t.AssetDescription,
					Status = statusReturn,
					Type = "Asset Transfer"
				}).ToList();
			result.AddRange(transfers);

			return result.OrderByDescending(x => x.ReferenceNum).ToList();
		}
	}
	public class AssetRegisterQTYDetail
	{
		public int ID { get; set; } 
        public string AssetCode { get; set; } 
		public int QTY { get; set; }
		public string Model { get; set; }
		public string SerialNumber { get; set; }
	}
	public class ListRecordDetail
	{
		public string ReferenceNum { get; set; }
		public string EmpCode { get; set; }
		public DateTime? AssignDate { get; set; }
		public string EmployeName { get; set; }
		public string AssetCode { get; set; }
		public string AssetDescription { get; set; }
		public DateTime? ReturnDate { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
	}
}