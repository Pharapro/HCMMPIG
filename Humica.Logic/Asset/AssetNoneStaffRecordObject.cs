using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Asset
{
    public class AssetNoneStaffRecordObject
	{
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
		public string MessageError { get; set; }
		public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
		public HRAssetNoneStaffRecord Header { get; set; }
		public List<HRAssetNoneStaffRecord> ListNoneStaffRecord { get; set; }
		public List<MDUploadTemplate> ListTemplate { get; set; }
		public AssetNoneStaffRecordObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
		public string Import()
		{
			try
			{
				if (ListNoneStaffRecord.Count == 0)
				{
					return "NO_DATA";
				}
				var handlerCodes = ListNoneStaffRecord.Select(s => s.HandlerCode).Distinct().ToList();
				var existingCodes = DB.HRAssetNoneStaffRecords.Where(s => handlerCodes.Contains(s.HandlerCode)).Select(s => s.HandlerCode).ToHashSet();
				if (existingCodes.Any())
				{
					var duplicate = ListNoneStaffRecord.First(s => existingCodes.Contains(s.HandlerCode));
					return $"Handler code already exists [{duplicate.HandlerCode}]";
				}
				try
				{
					foreach (var staff in ListNoneStaffRecord)
					{
						var record = new HRAssetNoneStaffRecord
						{
							HandlerCode = staff.HandlerCode,
							HandlerName = staff.HandlerName,
							Company = staff.Company,
							PhoneNumber = staff.PhoneNumber,
							Position = staff.Position,
							Commune = staff.Commune,
							District = staff.District,
							Province = staff.Province
						};
						DB.HRAssetNoneStaffRecords.Add(record);
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
}