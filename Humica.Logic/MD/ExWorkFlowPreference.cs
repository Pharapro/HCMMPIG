using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic.MD
{
    public class ExWorkFlowPreference
    {
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        //public List<ExCfWorkFlow> ListWF { get; set; }
        public List<CFWorkFlow> ListWF { get; set; }
        public List<ExCfWFApprover> ListWFApprover { get; set; }
        public List<ExCfWFSalaryApprover> ListSalaryApprover { get; set; }
        public List<ExCfWorkFlowItem> ListWFItem { get; set; }
        public string ScreenID { get; set; }
        //public List<ExCfFileTemplate> ListFileTemplate { get; set; }
        //public List<ExCFUploadMapping> ListMapping { get; set; }
        public List<SYScreenDevelop> ListScreenDevelop { get; set; }
        public string Table { get; set; }
        public ExWorkFlowPreference()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ExCfWFApprover> getApproverListByDocType(string DocType, string Location, string _ScreenID)
        {
            var listResult = new List<ExCfWFApprover>();
            var objDoc = DB.ExCfWorkFlowItems.Find(_ScreenID, DocType);
            if (objDoc != null)
            {
                var objWf = SMS.CFWorkFlows.Find(objDoc.ApprovalFlow);
                if (objWf != null)
                {
                    listResult = DB.ExCfWFApprovers.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location).OrderBy(w => w.ApproveLevel).ToList();
                }
            }
            return listResult;
        }
        public List<ExCfWFApprover> getApproverListByDocType(string DocType, string Location, string _ScreenID , string Department)
        {
            var listResult = new List<ExCfWFApprover>();
            var objDoc = DB.ExCfWorkFlowItems.Find(_ScreenID, DocType);
            if (objDoc != null)
            {
                var objWf = SMS.CFWorkFlows.Find(objDoc.ApprovalFlow);
                if (objWf != null && !string.IsNullOrEmpty(Department))
                {
                    listResult = DB.ExCfWFApprovers.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location && w.Department== Department).OrderBy(w => w.ApproveLevel).ToList();
                }
                else if (objWf != null)
                {
                    listResult = DB.ExCfWFApprovers.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location ).OrderBy(w => w.ApproveLevel).ToList();
                }
            }
            return listResult;
        }
        public List<ExCfWFSalaryApprover> getApproverListByDocType(string DocType, string _ScreenID)
        {
            var listResult = new List<ExCfWFSalaryApprover>();
            var objDoc = DB.ExCfWorkFlowItems.Find(_ScreenID, DocType);
            if (objDoc != null)
            {
                var objWf = SMS.CFWorkFlows.Find(objDoc.ApprovalFlow);
                if (objWf != null)
                {
                    listResult = DB.ExCfWFSalaryApprovers.Where(w => w.WFObject == objWf.WFObject).OrderBy(w => w.ApproveLevel).ToList();
                }
            }
            return listResult;
        }
        //public List<ExCfDocMatClass> getDocMatListByDocType(string DocType, string Location, RequisitionList RLType)
        //{
        //    var listResult = new List<ExCfDocMatClass>();
        //    if (RLType == RequisitionList.Request)
        //    {
        //        var objDoc = DBX.ExCfRequests.Find(DocType);
        //        if (objDoc != null)
        //        {
        //            listResult = DBX.ExCfDocMatClasses.Where(w => w.DocurmentType == objDoc.DocumentType).ToList();
        //        }

        //    }
        //    if (RLType == RequisitionList.PO)
        //    {
        //        var objDoc = DBX.ExCfPOTypes.Find(DocType);
        //        if (objDoc != null)
        //        {
        //            listResult = DBX.ExCfDocMatClasses.Where(w => w.DocurmentType == objDoc.POType).ToList();
        //        }

        //    }
        //    return listResult;
        //}

        //public List<ExCfWFGroup> getWorkGroupListByDocType(string DocType, string Location, RequisitionList RLType)
        //{
        //    var listResult = new List<ExCfWFGroup>();
        //    if (RLType == RequisitionList.Request)
        //    {
        //        var objDoc = DBX.ExCfRequests.Find(DocType);
        //        if (objDoc != null)
        //        {
        //            var objWf = DBX.ExCfWorkFlows.Find(objDoc.ApprovalFlow);
        //            if (objWf != null)
        //            {
        //                listResult = DBX.ExCfWFGroups.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location).ToList();
        //            }
        //        }

        //    }
        //    if (RLType == RequisitionList.PR)
        //    {
        //        var objDoc = DBX.ExCfPRTypes.Find(DocType);
        //        if (objDoc != null)
        //        {
        //            var objWf = DBX.ExCfWorkFlows.Find(objDoc.ApprovalFlow);
        //            if (objWf != null)
        //            {
        //                listResult = DBX.ExCfWFGroups.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location).ToList();
        //            }
        //        }

        //    }
        //    if (RLType == RequisitionList.PO)
        //    {
        //        var objDoc = DBX.ExCfPOTypes.Find(DocType);
        //        if (objDoc != null)
        //        {
        //            var objWf = DBX.ExCfWorkFlows.Find(objDoc.ApprovalFlow);
        //            if (objWf != null)
        //            {
        //                listResult = DBX.ExCfWFGroups.Where(w => w.WFObject == objWf.WFObject && w.Branch == Location).ToList();
        //            }
        //        }

        //    }
        //    return listResult;
        //}
        public string getObjectName(RequisitionList objList)
        {
            if (objList == RequisitionList.Bidding)
            {
                return "ExPOBidding";
            }
            if (objList == RequisitionList.Receipt)
            {
                return "ExPOReceipt";
            }
            return "ExPORequest";
        }

        //public static Staff getStaffProfileSign(string DocType, string DocNo, string property, string staffid = "", int AppLevel = 0)
        //{
        //    var DBX = new DBGenerateX();
        //    var DB = new SMSystemEntity();

        //    var obj = new Staff();
        //    if (AppLevel == 0)
        //    {
        //        obj = DB.Staffs.Find(staffid, property);
        //    }
        //    else
        //    {
        //        var listApp = DBX.ExDocApprovals.Where(w => w.DocumentNo == DocNo && w.DocumentType == DocType).ToList();

        //        var checkList = listApp.Where(w => w.ApproveLevel == AppLevel).ToList();
        //        if (checkList.Count > 0)
        //        {
        //            var objSearch = checkList.First();
        //            obj = DB.Staffs.Find(objSearch.ApprovedBy, property);
        //            if (obj != null)
        //            {
        //                if (checkList.First().ApprovedDate.HasValue)
        //                {
        //                    obj.ModifiedOn = checkList.First().ApprovedDate;
        //                }
        //            }
        //        }
        //    }
        //    if (obj == null)
        //    {
        //        obj = new Staff();
        //    }
        //    return obj;
        //}

        //public static Staff getStaffProfileSignByYser(string userId)
        //{
        //    var DBX = new DBGenerateX();
        //    var DB = new SMSystemEntity();
        //    var obj = new Staff();
        //    var listUser = DB.SYUsers.Where(w => w.UserName == userId).ToList();
        //    if (listUser.Count > 0)
        //    {
        //        var id = listUser.First().UserID;
        //        var listUserStaff = DB.CFStaffUsers.Where(w => w.UserID == id).ToList();
        //        if (listUserStaff.Count > 0)
        //        {
        //            obj = DB.Staffs.Find(listUserStaff.First().StaffID, listUser.First().CompanyOwner);
        //        }
        //    }

        //    if (obj == null)
        //    {
        //        obj = new Staff();
        //    }
        //    return obj;
        //}

    }
    public class TableName
    {
        public string ObjectName { get; set; }
        public string Description { get; set; }
        public TableName()
        {

        }
        public TableName(string objName, string Des)
        {
            ObjectName = objName;
            Description = Des;
        }
        public List<TableName> getListRequisition()
        {
            return new List<TableName> {
                new TableName("ExPOBidding", "Bidding"),
                new TableName("ExPOHeader", "Purchasing"),
                new TableName("ExPOReceipt", "Receipt"),
                new TableName("ExPRHeader", "Requisition"),
                new TableName("ExPORequest", "Request")
            };
        }
    }

    public enum RequisitionList
    {
        Request, Bidding, Receipt, Payment
    }
}