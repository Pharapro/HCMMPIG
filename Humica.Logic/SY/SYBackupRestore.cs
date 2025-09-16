using Humica.Core.DB;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Logic.SY
{
    [Serializable]
    public class SYBackupRestore
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public List<MDUploadTemplate> ListUploadTempalte { get; set; }
        public List<CFForm> ListFormItem { get; set; }
        public List<SYMenuItem> ListSubScreen { get; set; }
        public List<ExEventAuditLog> ListAuditLog { get; set; }
        public string BizType { get; set; }
        public long MaxSecondExecute { get; set; }
        public bool IsSaleContract { get; set; }
        public bool IsReconRealEstate { get; set; }
        public bool IsReconReceipt { get; set; }
        public bool IsInventory { get; set; }
        public bool IsLock { get; set; }
        public bool IsLog { get; set; }
        public string InventoryAccount { get; set; }
        public string ScreenFilter { get; set; }
        public string DocumentFilter { get; set; }
        public string UserName { get; set; }

        public SYBackupRestore()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}
