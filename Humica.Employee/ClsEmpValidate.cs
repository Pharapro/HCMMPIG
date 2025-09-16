using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Data.Entity;
using System.Linq;

namespace Humica.Employee
{
    public class ClsEmpValidate
    {
        protected IUnitOfWork unitOfWork;
        public int? PositionID { get; set; }
        public bool IsNotCalSalary { get; set; }
        public ClsEmpValidate()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        #region Position Reporting
        public string GetPosition(string Code)
        {
            string PostCode = Code;
            PositionID = null;
            var Postion = unitOfWork.Repository<HRPositionStructure>().Queryable().FirstOrDefault(w => w.ID.ToString() == Code);
            if (Postion != null)
            {
                PositionID = Postion.ID;
                PostCode = Postion.Code;
            }
            return PostCode;
        }
        public string GetPositionView(string Code)
        {
            string PostCode = Code;
            var Postion = unitOfWork.Repository<HRPositionStructure>().Queryable().FirstOrDefault(w => w.Code == Code);
            if (Postion != null)
            {
                PostCode = Postion.ID.ToString();
            }
            return PostCode;
        }
        #endregion
        public string Validate_Car_Move(string EmpCode, DateTime EffectDate, string CareerID = null)
        {
            var objEmpCar = unitOfWork.Repository<HREmpCareerMovement>().Queryable().Where(w => w.EmpCode == EmpCode && w.EffectDate >= EffectDate);
            if (objEmpCar.Any())
            {
                if (string.IsNullOrEmpty(CareerID))
                    return "INV_DATE";
            }
            return SYConstant.OK;
        }
        public string Validate_EffectDate(string EmpCode, DateTime EffectDate, string DocReference = null)
        {
            int TranNo = 0;
            if (!string.IsNullOrEmpty(DocReference))
            {
                TranNo = Convert.ToInt32(DocReference);
            }
            var existsInvalidDate = unitOfWork.Repository<HREmpCareer>().Queryable().Any(w => w.EmpCode == EmpCode &&
                  w.EffectDate.HasValue && DbFunctions.TruncateTime(w.EffectDate.Value) >= EffectDate.Date
                  && (int)w.TranNo != TranNo);
            if (existsInvalidDate)
            {
                return "INV_DATE";
            }
            return SYConstant.OK;
        }
        public bool IsHideSalary(string Level, string UserName)
        {
            var ListLevel = unitOfWork.Repository<SYHRModifySalary>().Queryable().Where(w => w.UserName == UserName && w.Level == Level).ToList();
            if (ListLevel.Count > 0)
            {
                return true;
            }
            return false;
        }

        public string Validate_CareerType(string CareerCode,string Resigntype)
        {
            IsNotCalSalary = false;
            var objCareer = unitOfWork.Repository<HRCareerHistory>().Queryable().Where(w => w.Code == CareerCode);
            if (objCareer.Any(w => w.NotCalSalary == true))
            {
                IsNotCalSalary = true;
                if (!unitOfWork.Repository<HRTerminType>().Queryable().Any(w => w.Code == Resigntype))
                {
                    return "SEPARATE_TYPE_EN";
                }
            }
            return SYConstant.OK;
        }
    }
}
