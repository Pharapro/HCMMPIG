using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System;
using System.Collections.Generic;
using static Humica.Employee.ClsEmployee;

namespace Humica.Employee
{
    public interface IClsEmployee : IClsApplication
    {
        FTFilterEmployee Filter { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        HRStaffProfile Header { get; set; }
        List<HREmpIdentity> ListEmpIdentity { get; set; }
        bool Fist_Last { get; set; }
        bool Fist_Last_KH { get; set; }
        bool IsEmpAuto { get; set; }
        string Salary { get; set; }
        string SalaryTax { get; set; }
        string SalaryNSSF { get; set; }
        string IsSalary { get; set; }
        bool IsTax { get; set; }
        List<HREmpJobDescription> ListEmpJobDescription { get; set; }
        List<HREmpFamily> ListEmpFamily { get; set; }
		List<HREmpEduc> ListEducation { get; set; }
		List<HREmpContract> ListContract { get; set; }
		List<HRDelayProbation> ListDelayProbation { get; set; }
		List<HREmpDisciplinary> ListEmpDisciplinary { get; set; }
		List<HREmpCertificate> ListEmpCertificate { get; set; }
		List<HREmpPhischk> ListEmpMedical { get; set; }
		string ScreenId { get; set; }
        HREmpCareer HeaderCareer { get; set; }
        List<MDUploadTemplate> ListTemplate { get; set; }
        List<HRStaffProfile> ListStaffProfile { get; set; }
        List<HREmpIdentity> ListEmpIdentityUP { get; set; }
        string MessageError { get; set; }
        bool IsInAtive { get; set; }
        string NewSalary { get; set; }
        string OldSalary { get; set; }
        string Increase { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        List<HR_Career> ListCareerHis { get; set; }
        List<HR_EmpCareer_View> ListEmpCareer { get; set; }
        List<HREmpCareer> ListCareer { get; set; }
        string NewEmpCode { get; set; }
        string ToDate { get; set; }
        List<HRStaffProfile> ListEmp { get; set; }
        List<HREmpBankAcc> ListSaparate { get; set; }
        List<ClassCareerBankList> ListSalrySaparate { get; set; }
        List<HREmpCareerBankList> ListCareerBankList { get; set; }
        List<HR_HisBanKList> ListHisBanKList { get; set; }

        string CreateMulti();
        string CreatStaff();
        string DeleteEmp(string EmpCode);
        DateTime GetProbationDate(DateTime StartDate, string ProType);
        void IsValidChecking(int ID);
        bool IsHideSalary(string Level);
        void OnCreatingLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataJobLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataListLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataLoadingOther(params object[] keys);
        Dictionary<string, dynamic> OnDataPersonal(params object[] keys);
        void OnDetailLoading(params object[] keys);
        string OnGridModifyIdentity(HREmpIdentity MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false);
        string OnGridModifyJobDescription(HREmpJobDescription MModel, string Action, bool IsEdit = false);
        string OnGridModifyFamily(HREmpFamily MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false);
		string OnGridModifyEducation(HREmpEduc MModel, string Action, bool IsEdit = false, bool IsAddNewFile=false);
		string OnGridModifyContract(HREmpContract MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false);
		string OnGridModifyDisciplinary(HREmpDisciplinary MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false);
		string OnGridModifyCertificate(HREmpCertificate MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false);
		string OnGridModifyMedical(HREmpPhischk MModel, string Action, bool IsEdit = false);
		void OnLoandindEmployee();
        string UpdateStaff(string ID);
        Dictionary<string, dynamic> OnDataFamily(params object[] keys);
		Dictionary<string, dynamic> OnDataEducation(params object[] keys);
		Dictionary<string, dynamic> OnDataContract(params object[] keys);
		Dictionary<string, dynamic> OnDataDisciplinary(params object[] keys);
		Dictionary<string, dynamic> OnDataCertificate(params object[] keys);
		Dictionary<string, dynamic> OnDataMedical(params object[] keys);
        void OnDetailCarrerLoading(params object[] keys);
        void OnLoandindEmpCarrer();
        string EditCareerStaff(string ID);
        //void OnloadCareerHistory(string EmpCode);
        Dictionary<string, dynamic> OnDataListCarrer(params object[] keys);
        string Delete_EmpCareerHistory(string ID);
        void OnCreatingEmpCarrer();
        List<HREmpCareer> GetEmployee(string EmpCode);
        string CreateCareerStaff();
        string uploadCareer();
        string uploadSalary();
    }
}