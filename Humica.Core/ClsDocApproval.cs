using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Humica.Core
{
    public class ClsDocApproval
    {
        private IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsDocApproval()
        {
            OnLoad();
        }
        public List<ExDocApproval> OnLoadDoc(string DocType, string UserName)
        {
            string Open = SYDocumentStatus.OPEN.ToString();
            List<ExDocApproval> ListDocApproval = new List<ExDocApproval>();
            ListDocApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == DocType &&
            w.Approver == UserName && w.Status != Open).ToList();
            return ListDocApproval;
        }
        public List<ExDocApproval> OnLoadDocPending(string DocType, string UserName)
        {
            string Open = SYDocumentStatus.OPEN.ToString();
            List<ExDocApproval> ListDocApproval = new List<ExDocApproval>();
            var ListDocApp = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentType == DocType).ToList();
            var LstDocAppBy = ListDocApp.Where(w => w.Approver == UserName && w.Status == Open).ToList();
            ListDocApp = ListDocApp.Where(w => LstDocAppBy.Where(x => x.DocumentNo == w.DocumentNo).Any()).ToList();
            List<ExDocApproval> listTemp = new List<ExDocApproval>();
            foreach (var Appro in ListDocApp.OrderBy(w => w.ApproveLevel).ToList())
            {
                if (listTemp.Where(w => w.DocumentNo == Appro.DocumentNo).Any())
                {
                    continue;
                }
                if (Appro.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    continue;
                }
                else if (Appro.Status == SYDocumentStatus.REJECTED.ToString())
                {
                    listTemp.Add(Appro);
                    continue;
                }
                var DocAppBy = LstDocAppBy.FirstOrDefault(w => w.DocumentNo == Appro.DocumentNo);
                if (DocAppBy != null)
                {
                    if (DocAppBy.ApproveLevel > ListDocApp.Where(w => w.DocumentNo == Appro.DocumentNo && w.Status == Open).Min(w => w.ApproveLevel))
                    {
                        continue;
                    }
                    ListDocApproval.Add(Appro);
                }
            }
            return ListDocApproval;
        }

        public List<ExDocApproval> SetAutoApproval(string ScreenID, string DocType, string Branch, string deparment = "")
        {
            var ListApproval = new List<ExDocApproval>();
            var objDoc = unitOfWork.Repository<ExCfWorkFlowItem>().Find(ScreenID, DocType);
            if (objDoc != null)
            {
                CFWorkFlow objCF = unitOfWork.Repository<CFWorkFlow>().Find(objDoc.ApprovalFlow);
                if (objCF != null)
                {
                    if (objCF.ByDepartment == true)
                    {
                        List<ExCFWFDepartmentApprover> listDefaultApproval = unitOfWork.Repository<ExCFWFDepartmentApprover>().Where(w => //w.Company == ClsConstant.DEFAULT_PLANT
                       w.Department == deparment
                      && w.WFObject == objDoc.ApprovalFlow
                      && w.IsSelected == true).ToList();

                        foreach (ExCFWFDepartmentApprover read in listDefaultApproval)
                        {
                            ExDocApproval objApp = new ExDocApproval();
                            GetApproval(objApp, read.Employee, read.EmployeeName, DocType, read.ApproveLevel,
                                objDoc.ApprovalFlow, read.InternalStatus);
                            ListApproval.Add(objApp);
                        }
                    }
                    else
                    {
                        List<ExCfWFApprover> listDefaultApproval = unitOfWork.Repository<ExCfWFApprover>().Queryable().Where(w => //w.Company == ClsConstant.DEFAULT_PLANT && 
                        w.Branch == Branch
                        && w.WFObject == objDoc.ApprovalFlow
                        && w.IsSelected == true).ToList();

                        foreach (ExCfWFApprover read in listDefaultApproval)
                        {
                            ExDocApproval objApp = new ExDocApproval();
                            GetApproval(objApp, read.Employee, read.EmployeeName, DocType, read.ApproveLevel,
                                objDoc.ApprovalFlow, read.InternalStatus);
                            ListApproval.Add(objApp);
                        }
                    }
                }
            }
            return ListApproval;
        }

        public void GetApproval(ExDocApproval objApp, string Approver, string EmployeeName, string DocType,
            int ApproveLevel, string ApprovalFlow, string InternalStatus)
        {
            objApp.Approver = Approver;
            objApp.ApproverName = EmployeeName;
            objApp.DocumentType = DocType;
            objApp.ApproveLevel = ApproveLevel;
            objApp.WFObject = ApprovalFlow;
            objApp.InternalStatus = InternalStatus;
        }
    }
}
