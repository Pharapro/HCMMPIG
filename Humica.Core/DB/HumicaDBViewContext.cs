using System.Data.Entity;

namespace Humica.Core.DB
{
    public partial class HumicaDBViewContext : HumicaDBContext
    {
        public HumicaDBViewContext() : base()

        {
            this.Database.CommandTimeout = 1800;
        }
        public virtual DbSet<HR_CompanyGroup_View> HR_CompanyGroup_View { get; set; }

        public virtual DbSet<HR_PayHisStory> HR_PayHisStory { get; set; }
        public virtual DbSet<HR_VIEW_EmpEvaluationForm> HR_VIEW_EmpEvaluationForm { get; set; }
        public virtual DbSet<HR_STAFF_VIEW> HR_STAFF_VIEW { get; set; }
        public virtual DbSet<HR_EmpCareer_View> HR_EmpCareer_View { get; set; }
        public virtual DbSet<REF_VACANCY> REF_VACANCY { get; set; }
        public virtual DbSet<HR_VIEW_EmpLeave> HR_VIEW_EmpLeave { get; set; }
        public virtual DbSet<HR_PO_VIEW> HR_PO_VIEW { get; set; }
        public virtual DbSet<HR_Family_View> HR_Family_View { get; set; }
        public virtual DbSet<HR_LineMgr_View> HR_LineMgr_View { get; set; }
        public virtual DbSet<HR_ListEmployee> HR_ListEmployee { get; set; }
        public virtual DbSet<HR_NSSF_Transfer> HR_NSSF_Transfer { get; set; }
        public virtual DbSet<HR_OrgChart_View> HR_OrgChart_View { get; set; }
        public virtual DbSet<HR_PendingLeave> HR_PendingLeave { get; set; }
        public virtual DbSet<HR_PR_EmpSalary> HR_PR_EmpSalary { get; set; }
        public virtual DbSet<HR_PR_VIEW> HR_PR_VIEW { get; set; }
        public virtual DbSet<HR_RP_View> HR_RP_View { get; set; }
        public virtual DbSet<HR_View_EmpAllowance> HR_View_EmpAllowance { get; set; }
        public virtual DbSet<HR_View_EmpGenSalary> HR_View_EmpGenSalary { get; set; }
        public virtual DbSet<HR_VIEW_EMPLOAN> HR_VIEW_EMPLOAN { get; set; }
        public virtual DbSet<PR_GLCharge_View> PR_GLCharge_View { get; set; }
        public virtual DbSet<PR_TEMP_SYGL_VIEW> PR_TEMP_SYGL_VIEW { get; set; }
        public virtual DbSet<HR_EMPSVC_VIEW> HR_EMPSVC_VIEW { get; set; }
        public virtual DbSet<PR_EmpClaimBen_View> PR_EmpClaimBen_View { get; set; }
        public virtual DbSet<EOB_EMP_ANNOUCEMENT> EOB_EMP_ANNOUCEMENT { get; set; }
        public virtual DbSet<AT_EmpSchedule> AT_EmpSchedule { get; set; }
        public virtual DbSet<HR_BooingRoom_View> HR_BooingRoom_View { get; set; }
        public virtual DbSet<VIEW_ATEmpSchedule> VIEW_ATEmpSchedule { get; set; }
        public virtual DbSet<VIEW_ATInOut> VIEW_ATInOut { get; set; }
        public virtual DbSet<PR_EMPSVC_VIEW> PR_EMPSVC_VIEW { get; set; }
        public virtual DbSet<PR_InetAccount_view> PR_InetAccount_view { get; set; }
        public virtual DbSet<PR_POPending_view> PR_POPending_view { get; set; }
        public virtual DbSet<HR_EmpLateEarly_View> HR_EmpLateEarly_View { get; set; }
        public virtual DbSet<HLCheckShiftImports> HLCheckShiftImports { get; set; }
        public virtual DbSet<NSSF_Latter> NSSF_Latter { get; set; }
        public virtual DbSet<RPT_AttScheduleSummary> RPT_AttScheduleSummary { get; set; }
        public virtual DbSet<HR_POReceipt_View> HR_POReceipt_View { get; set; }
        public virtual DbSet<VIEW_EmpOnsite> VIEW_EmpOnsite { get; set; }
		public virtual DbSet<HR_AssetDepartment_View> HR_AssetDepartment_View { get; set; }
		public virtual DbSet<HR_AssetStaff_View> HR_AssetStaff_View { get; set; }
		public virtual DbSet<HR_AssetNoneStaff_View> HR_AssetNoneStaff_View { get; set; }
		public virtual DbSet<HR_AssetRegisterVehicle_View> HR_AssetRegisterVehicle_View { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            #region HR_STAFF_VIEW
            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.TerminateStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.PayParam)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.BranchID)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Peraddress)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.PostFamily)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.BankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.BankAccName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.TXPayType)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.ServicesLenght)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<HR_STAFF_VIEW>()
                .Property(e => e.Email)
                .IsUnicode(false);
            #endregion

            #region EOB
            modelBuilder.Entity<EOB_EMP_ANNOUCEMENT>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<EOB_EMP_ANNOUCEMENT>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<EOB_EMP_ANNOUCEMENT>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<EOB_EMP_ANNOUCEMENT>()
                .Property(e => e.gen)
                .IsUnicode(false);

            modelBuilder.Entity<EOB_EMP_ANNOUCEMENT>()
                .Property(e => e.gen1)
                .IsUnicode(false);
            #endregion

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.FAX)
                .IsUnicode(false);

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.Health)
                .HasPrecision(38, 2);

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.SOSEC)
                .HasPrecision(38, 2);

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.PensionFund)
                .HasPrecision(38, 2);

            modelBuilder.Entity<NSSF_Latter>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<RPT_AttScheduleSummary>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<RPT_AttScheduleSummary>()
                .Property(e => e.ResourceID)
                .IsUnicode(false);

            modelBuilder.Entity<HLCheckShiftImports>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EmpLateEarly_View>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EmpLateEarly_View>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EmpLateEarly_View>()
                .Property(e => e.DedCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EmpLateEarly_View>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<PR_EMPSVC_VIEW>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<PR_EMPSVC_VIEW>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<PR_InetAccount_view>()
                .Property(e => e.Amount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<AT_EmpSchedule>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<AT_EmpSchedule>()
                .Property(e => e.ResourceID)
                .IsUnicode(false);

            modelBuilder.Entity<AT_EmpSchedule>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<AT_EmpSchedule>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.SHIFT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.DivisionCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.GMSTATUS)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.TerminateStatus)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.Remark2)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.LEAVEDESC)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATEmpSchedule>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.BRANCH)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_ATInOut>()
                .Property(e => e.JOBCODE)
                .IsUnicode(false);

            modelBuilder.Entity<PR_EmpClaimBen_View>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EMPSVC_VIEW>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_EMPSVC_VIEW>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                 .Property(e => e.EmpCode)
                 .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                .Property(e => e.RewardType)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                .Property(e => e.Reward)
                .HasPrecision(38, 2);

            modelBuilder.Entity<REF_VACANCY>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<REF_VACANCY>()
                .Property(e => e.RequestNo)
                .IsUnicode(false);

            modelBuilder.Entity<REF_VACANCY>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<REF_VACANCY>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<REF_VACANCY>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<HR_VIEW_EmpLeave>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_VIEW_EmpLeave>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_VIEW_EmpLeave>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PO_VIEW>()
                .Property(e => e.Requestor)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.IDCard)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<HR_Family_View>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_LineMgr_View>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.SEX)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.CareerCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.MV_LEVEL)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.TERMINATESTATUS)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.WORKID)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.SECT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_ListEmployee>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.NSSFGROSS)
                .HasPrecision(37, 4);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.SOSEC)
                .HasPrecision(20, 2);

            modelBuilder.Entity<HR_NSSF_Transfer>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HR_OrgChart_View>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<HR_OrgChart_View>()
                .Property(e => e.Images)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PayHisStory>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PendingLeave>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PendingLeave>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PendingLeave>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PendingLeave>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_EmpSalary>()
                .Property(e => e.TerminateStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HR_PR_VIEW>()
                .Property(e => e.Requestor)
                .IsUnicode(false);

            modelBuilder.Entity<HR_RP_View>()
                .Property(e => e.Requestor)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpAllowance>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpAllowance>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpAllowance>()
                .Property(e => e.AllwCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpAllowance>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.PayParam)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.CAREERDESC)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.SECT)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_View_EmpGenSalary>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_VIEW_EMPLOAN>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HR_VIEW_EMPLOAN>()
                .Property(e => e.AllName)
                .IsUnicode(false);

            modelBuilder.Entity<PR_GLCharge_View>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PR_GLCharge_View>()
                .Property(e => e.DEBITAM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PR_GLCharge_View>()
                .Property(e => e.CREDITAM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PR_GLCharge_View>()
                .Property(e => e.SortKey)
                .IsUnicode(false);

            modelBuilder.Entity<PR_GLCharge_View>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<PR_TEMP_SYGL_VIEW>()
                .Property(e => e.GroupAcc)
                .IsUnicode(false);

            modelBuilder.Entity<PR_TEMP_SYGL_VIEW>()
                .Property(e => e.AccCode)
                .IsUnicode(false);

            modelBuilder.Entity<PR_TEMP_SYGL_VIEW>()
                .Property(e => e.SortKey)
                .IsUnicode(false);

            modelBuilder.Entity<PR_POPending_view>()
               .Property(e => e.Amount)
               .HasPrecision(38, 4);

            modelBuilder.Entity<PR_POPending_view>()
                .Property(e => e.DocType)
                .IsUnicode(false);
            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<VIEW_EmpOnsite>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);
        }
    }
}
