using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.EForm
{
    public class ClsEFPortalType : IClsEFPortalType
    {
        protected IUnitOfWork unitOfWork;
        public List<EFPortalType> ListPortalType { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsEFPortalType()
        {
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListPortalType = unitOfWork.Set<EFPortalType>().ToList();
        }
        public void OnIndexLoadingPortalType()
        {
            ListPortalType = unitOfWork.Set<EFPortalType>().ToList();
        }
        public string OnGridModifyPortalType(EFPortalType MModel, string Action)
        {
            try
            {
                if (Action == "ADD")
                {
                    if (string.IsNullOrEmpty(MModel.Code))
                    {
                        return "CODE";
                    }
                    unitOfWork.Add(MModel);
                }
                else if (Action == "EDIT")
                {
                    unitOfWork.Update(MModel);
                }
                else if (Action == "DELETE")
                {
                    var objCheck = unitOfWork.Set<EFPortalType>().FirstOrDefault(w => w.Code == MModel.Code);
                    if (objCheck != null)
                    {
                        unitOfWork.Delete(objCheck);
                    }
                    else
                    {
                        return "INV_DOC";
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
