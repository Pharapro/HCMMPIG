namespace Humica.Core.DB
{
    using Humica.EF.MD;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class HumicaDBContext : SMSystemEntity
    {
        public HumicaDBContext()
            : base()
        {
            this.Database.CommandTimeout = 1800;
        }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<ATLateEarlyDeduct> ATLateEarlyDeducts { get; set; }
        public virtual DbSet<ATGenMeal> ATGenMeals { get; set; }
        public virtual DbSet<ATMealSetup> ATMealSetups { get; set; }
        public virtual DbSet<ATMealSetupItem> ATMealSetupItems { get; set; }
        public virtual DbSet<ATOTSetting> ATOTSettings { get; set; }
        public virtual DbSet<ATRequestOnSite> ATRequestOnSites { get; set; }
        public virtual DbSet<ATRequestOnSiteItem> ATRequestOnSiteItems { get; set; }

        #region Purchase
        public virtual DbSet<HRPOReceipt> HRPOReceipts { get; set; }
        public virtual DbSet<HRPOReceiptItem> HRPOReceiptItems { get; set; }

        #endregion
        #region Dress
        public virtual DbSet<HRUniform> HRUniforms { get; set; }
        public virtual DbSet<HRDressDeduct> HRDressDeducts { get; set; }
        public virtual DbSet<HRDressDeductItem> HRDressDeductItems { get; set; }
        public virtual DbSet<HRDressReceive> HRDressReceives { get; set; }
        public virtual DbSet<HRDressReceiveItem> HRDressReceiveItems { get; set; }
        public virtual DbSet<HRDressRequest> HRDressRequests { get; set; }
        public virtual DbSet<HRDressRequestItem> HRDressRequestItems { get; set; }
        public virtual DbSet<HRDressReturn> HRDressReturns { get; set; }
        public virtual DbSet<HRDressReturnItem> HRDressReturnItems { get; set; }
        public virtual DbSet<HRDressTransfer> HRDressTransfers { get; set; }
        public virtual DbSet<HRDressTransferItem> HRDressTransferItems { get; set; }
        #endregion
        public virtual DbSet<EFEmpPortal> EFEmpPortals { get; set; }

        public virtual DbSet<EFPortalType> EFPortalTypes { get; set; }
        public virtual DbSet<CFExVendor> CFExVendors { get; set; }
        public virtual DbSet<HISApproveSalary> HISApproveSalaries { get; set; }
        public virtual DbSet<HISApproveSalaryFD> HISApproveSalaryFDs { get; set; }
        public virtual DbSet<HISGLBenCharge> HISGLBenCharges { get; set; }
        public virtual DbSet<HisInsurance> HisInsurances { get; set; }
        public virtual DbSet<HRAnnounceType> HRAnnounceTypes { get; set; }
        public virtual DbSet<HisPayHi> HisPayHis { get; set; }
        public virtual DbSet<HISPaySlip> HISPaySlips { get; set; }
        public virtual DbSet<HisPendingAppSalary> HisPendingAppSalaries { get; set; }
        public virtual DbSet<HisPendingAppSalaryFP> HisPendingAppSalaryFPs { get; set; }
        public virtual DbSet<HISSVCMonthly> HISSVCMonthlies { get; set; }
        public virtual DbSet<HisTransferToMgr> HisTransferToMgrs { get; set; }
        public virtual DbSet<HRAnnouncement> HRAnnouncements { get; set; }
        public virtual DbSet<HRAPPIncSalary> HRAPPIncSalaries { get; set; }
        public virtual DbSet<HRAPPIncSalaryItem> HRAPPIncSalaryItems { get; set; }
        public virtual DbSet<HRApprForm> HRApprForms { get; set; }
        public virtual DbSet<HRApproverLeave> HRApproverLeaves { get; set; }
        public virtual DbSet<HRAssetClass> HRAssetClasses { get; set; }
        public virtual DbSet<HRAssetRegister> HRAssetRegisters { get; set; }
        public virtual DbSet<HRAssetStaff> HRAssetStaffs { get; set; }
		public virtual DbSet<HRAssetDepartment> HRAssetDepartments { get; set; }
		public virtual DbSet<HRAssetNoneStaff> HRAssetNoneStaffs { get; set; }
		public virtual DbSet<HRAssetNoneStaffRecord> HRAssetNoneStaffRecords { get; set; }
		public virtual DbSet<HRAssetType> HRAssetTypes { get; set; }
        public virtual DbSet<HRAssetTransfer> HRAssetTransfers { get; set; }
		public virtual DbSet<HRBookingCarSchedule> HRBookingCarSchedules { get; set; }
		public virtual DbSet<HRCarType> HRCarTypes { get; set; }
		public virtual DbSet<HRBookingCar> HRBookingCars { get; set; }
		public virtual DbSet<HRDriver> HRDrivers { get; set; }
		public virtual DbSet<HRAssetLocation> HRAssetLocations { get; set; }
		public virtual DbSet<HRBank> HRBanks { get; set; }
        public virtual DbSet<HRBankAccount> HRBankAccounts { get; set; }
        public virtual DbSet<HRBankBranch> HRBankBranches { get; set; }
        public virtual DbSet<HRBloodType> HRBloodTypes { get; set; }
        public virtual DbSet<HRBookingRoom> HRBookingRooms { get; set; }
        public virtual DbSet<HRBookingSchedule> HRBookingSchedules { get; set; }
        public virtual DbSet<HRCategory> HRCategories { get; set; }
        public virtual DbSet<HRCertificationType> HRCertificationTypes { get; set; }
        public virtual DbSet<HRClaimLeave> HRClaimLeaves { get; set; }
        public virtual DbSet<HRComplain> HRComplains { get; set; }
        public virtual DbSet<HRContractType> HRContractTypes { get; set; }
        public virtual DbSet<HRCountry> HRCountries { get; set; }
        public virtual DbSet<HRCurrency> HRCurrencies { get; set; }
        public virtual DbSet<HRCompanyGroup> HRCompanyGroups { get; set; }
        public virtual DbSet<HRCompanyTree> HRCompanyTrees { get; set; }
        public virtual DbSet<HRDelayProbation> HRDelayProbations { get; set; }
        public virtual DbSet<HRDelegateRule> HRDelegateRules { get; set; }
        public virtual DbSet<HRDepartment> HRDepartments { get; set; }
        public virtual DbSet<HRDisciplinAction> HRDisciplinActions { get; set; }
        public virtual DbSet<HRDisciplinType> HRDisciplinTypes { get; set; }
        public virtual DbSet<HRDivision> HRDivisions { get; set; }
        public virtual DbSet<HREduType> HREduTypes { get; set; }
        public virtual DbSet<HREmpCareer> HREmpCareers { get; set; }
        public virtual DbSet<HREmpCareerBankList> HREmpCareerBankLists { get; set; }
        public virtual DbSet<HREmpBankAcc> HREmpBankAccs { get; set; }
        public virtual DbSet<HREmpCareerMovement> HREmpCareerMovements { get; set; }
        public virtual DbSet<HREmpCertificate> HREmpCertificates { get; set; }
        public virtual DbSet<HREmpCode> HREmpCodes { get; set; }
        public virtual DbSet<HREmpContract> HREmpContracts { get; set; }
        public virtual DbSet<HREmpDeposit> HREmpDeposits { get; set; }
        public virtual DbSet<HREmpDepositItem> HREmpDepositItems { get; set; }
        public virtual DbSet<HREmpDisciplinary> HREmpDisciplinaries { get; set; }
        public virtual DbSet<HREmpEduc> HREmpEducs { get; set; }
        public virtual DbSet<HREmpFamily> HREmpFamilies { get; set; }
        public virtual DbSet<HREmpInsurance> HREmpInsurances { get; set; }
        public virtual DbSet<HREmpJobDescription> HREmpJobDescriptions { get; set; }
        public virtual DbSet<HREmpLeave> HREmpLeaves { get; set; }
        public virtual DbSet<HREmpLeaveB> HREmpLeaveBs { get; set; }
        public virtual DbSet<HREmpLeaveD> HREmpLeaveDs { get; set; }
        public virtual DbSet<HREmpLoan> HREmpLoans { get; set; }
        public virtual DbSet<HREmpLoanH> HREmpLoanHs { get; set; }
        public virtual DbSet<HREmpPhischk> HREmpPhischks { get; set; }
        public virtual DbSet<HREmpResign> HREmpResigns { get; set; }
        public virtual DbSet<HREmpSupense> HREmpSupenses { get; set; }
        public virtual DbSet<HREmpUnderManager> HREmpUnderManagers { get; set; }
        public virtual DbSet<HREmpType> HREmpTypes { get; set; }
		public virtual DbSet<HREmpMultiPosition> HREmpMultiPositions { get; set; }
		public virtual DbSet<HRIdentityType> HRIdentityTypes { get; set; }
        public virtual DbSet<HRGroup> HRGroups { get; set; }
        public virtual DbSet<HRGroupDepartment> HRGroupDepartments { get; set; }
        public virtual DbSet<HRHeadCount> HRHeadCounts { get; set; }
        public virtual DbSet<HRHospital> HRHospitals { get; set; }
        public virtual DbSet<HRInsuranceCompany> HRInsuranceCompanies { get; set; }
        public virtual DbSet<HRInsuranceType> HRInsuranceTypes { get; set; }
        public virtual DbSet<HRJobGrade> HRJobGrades { get; set; }
        public virtual DbSet<HRJournalType> HRJournalTypes { get; set; }
        public virtual DbSet<HRKPIAction> HRKPIActions { get; set; }
        public virtual DbSet<HRKPIList> HRKPILists { get; set; }
        public virtual DbSet<HRKPIActivitiesHeader> HRKPIActivitiesHeaders { get; set; }
        public virtual DbSet<HRKPIActivitiesItem> HRKPIActivitiesItems { get; set; }
        public virtual DbSet<HRKPIActivityAttach> HRKPIActivityAttaches { get; set; }
        public virtual DbSet<HRKPIAssignHeader> HRKPIAssignHeaders { get; set; }
        public virtual DbSet<HREmpEditLeaveEntitle> HREmpEditLeaveEntitles { get; set; }
        public virtual DbSet<HRKPIAssignItem> HRKPIAssignItems { get; set; }
        public virtual DbSet<HRKPIAssignIndicator> HRKPIAssignIndicators { get; set; }
        public virtual DbSet<HRKPIAssignMember> HRKPIAssignMembers { get; set; }
        public virtual DbSet<HRKPICategory> HRKPICategories { get; set; }
        public virtual DbSet<HRKPIEmpConcern> HRKPIEmpConcerns { get; set; }
        public virtual DbSet<HRKPIEmployeeAction> HRKPIEmployeeActions { get; set; }
        public virtual DbSet<HRKPIEvaItem> HRKPIEvaItems { get; set; }
        public virtual DbSet<HRKPIEvalIndicator> HRKPIEvalIndicators { get; set; }
        public virtual DbSet<HRKPIEvaluation> HRKPIEvaluations { get; set; }
        public virtual DbSet<HRKPITimeSheet> HRKPITimeSheets { get; set; }
        public virtual DbSet<HRKPIType> HRKPITypes { get; set; }
        public virtual DbSet<HRKPIYearlyTracking> HRKPIYearlyTrackings { get; set; }
        public virtual DbSet<HRAppCompareRatio> HRAppCompareRatios { get; set; }
        public virtual DbSet<HRAppLevelMidPoint> HRAppLevelMidPoints { get; set; }
        public virtual DbSet<HRAppPerformanceRate> HRAppPerformanceRates { get; set; }
        public virtual DbSet<HRAppSalaryBudget> HRAppSalaryBudgets { get; set; }
        public virtual DbSet<HRKPIJobFolloUp> HRKPIJobFolloUps { get; set; }
        public virtual DbSet<HRKPIManagemGuideLine> HRKPIManagemGuideLines { get; set; }
        public virtual DbSet<HRKPIMonthlyTracking> HRKPIMonthlyTrackings { get; set; }
        public virtual DbSet<HRKPINonFinance> HRKPINonFinances { get; set; }
        public virtual DbSet<HRKPIGrade> HRKPIGrades { get; set; }
        public virtual DbSet<HRKPINorm> HRKPINorms { get; set; }
        public virtual DbSet<HRKPIIndicator> HRKPIIndicators { get; set; }
        public virtual DbSet<HRKPIPlanHeader> HRKPIPlanHeaders { get; set; }
        public virtual DbSet<HRKPIPlanIndividual> HRKPIPlanIndividuals { get; set; }
        public virtual DbSet<HRKPIPlanIndivItem> HRKPIPlanIndivItems { get; set; }
        public virtual DbSet<HRKPIPlanItem> HRKPIPlanItems { get; set; }
        public virtual DbSet<HRLeaveImport> HRLeaveImports { get; set; }
        public virtual DbSet<HRLeaveProRate> HRLeaveProRates { get; set; }
        public virtual DbSet<HRLeaveType> HRLeaveTypes { get; set; }
        public virtual DbSet<HRLine> HRLines { get; set; }
        public virtual DbSet<HRLocation> HRLocations { get; set; }
        public virtual DbSet<HRMaterial> HRMaterials { get; set; }
        public virtual DbSet<HRMedicalType> HRMedicalTypes { get; set; }
        public virtual DbSet<HRMissionClaim> HRMissionClaims { get; set; }
        public virtual DbSet<HRMissionClaimItem> HRMissionClaimItems { get; set; }
        public virtual DbSet<HRMissionPlan> HRMissionPlans { get; set; }
        public virtual DbSet<HRMissionPlanItem> HRMissionPlanItems { get; set; }
        public virtual DbSet<HRMissionPlanMem> HRMissionPlanMems { get; set; }
        public virtual DbSet<HRMissItem> HRMissItems { get; set; }
        public virtual DbSet<HRMissType> HRMissTypes { get; set; }
        public virtual DbSet<HRMissMemo> HRMissMemoes { get; set; }
        public virtual DbSet<HRMissOilRating> HRMissOilRatings { get; set; }
        public virtual DbSet<HRMissProvince> HRMissProvinces { get; set; }
        public virtual DbSet<HRResgierVehicle> HRResgierVehicles { get; set; }
        public virtual DbSet<HRNation> HRNations { get; set; }
        public virtual DbSet<HRNotification> HRNotifications { get; set; }
        public virtual DbSet<HROffice> HROffices { get; set; }
        public virtual DbSet<HROrgChart> HROrgCharts { get; set; }
        public virtual DbSet<HRPHCKHResult> HRPHCKHResults { get; set; }
        public virtual DbSet<HRFunction> HRFunctions { get; set; }
        public virtual DbSet<HRPosition> HRPositions { get; set; }
        public virtual DbSet<HRPositionFamily> HRPositionFamilies { get; set; }
        public virtual DbSet<HRPositionStructure> HRPositionStructures { get; set; }
        public virtual DbSet<HRProbationType> HRProbationTypes { get; set; }
        public virtual DbSet<HRProvice> HRProvices { get; set; }
        public virtual DbSet<HRPubHoliday> HRPubHolidays { get; set; }
        public virtual DbSet<HRRelationshipType> HRRelationshipTypes { get; set; }
        public virtual DbSet<HRReqLateEarly> HRReqLateEarlies { get; set; }
        public virtual DbSet<HRReqMaternity> HRReqMaternities { get; set; }
        public virtual DbSet<HRRequestOT> HRRequestOTs { get; set; }
        public virtual DbSet<HRRoomType> HRRoomTypes { get; set; }
        public virtual DbSet<HRSection> HRSections { get; set; }
        public virtual DbSet<HRSetEntitleD> HRSetEntitleDs { get; set; }
        public virtual DbSet<HRSetEntitleH> HRSetEntitleHs { get; set; }
        public virtual DbSet<HRSettingRound> HRSettingRounds { get; set; }
        public virtual DbSet<HRStaffProfile> HRStaffProfiles { get; set; }
        public virtual DbSet<ATSwitchShift> ATSwitchShifts { get; set; }
        public virtual DbSet<HRWorkFlow> HRWorkFlows { get; set; }
        public virtual DbSet<HRWorkFlowHeader> HRWorkFlowHeaders { get; set; }
        public virtual DbSet<HRWorkFlowLeave> HRWorkFlowLeaves { get; set; }
        public virtual DbSet<HRWorkingType> HRWorkingTypes { get; set; }
        public virtual DbSet<HumanResourceSetting> HumanResourceSettings { get; set; }
        public virtual DbSet<MDUploadBatchNo> MDUploadBatchNoes { get; set; }
        public virtual DbSet<MDUploadImage> MDUploadImages { get; set; }
        public virtual DbSet<MDUploadTemplate> MDUploadTemplates { get; set; }
        public virtual DbSet<OBDesc> OBDescs { get; set; }
        public virtual DbSet<PRBenefitType> PRBenefitTypes { get; set; }
        public virtual DbSet<PRChartofAccount> PRChartofAccounts { get; set; }
        public virtual DbSet<PRCostCenter> PRCostCenters { get; set; }
        public virtual DbSet<PRFeeSetting> PRFeeSettings { get; set; }
        public virtual DbSet<PRGLmapping> PRGLmappings { get; set; }
        public virtual DbSet<PRInteAccount> PRInteAccounts { get; set; }
        public virtual DbSet<PRInteAccountItem> PRInteAccountItems { get; set; }
        public virtual DbSet<PRParameter> PRParameters { get; set; }
        public virtual DbSet<PRTaxSetting> PRTaxSettings { get; set; }
        public virtual DbSet<PRTransferToMgr> PRTransferToMgrs { get; set; }
        public virtual DbSet<PRTransferToMgrItem> PRTransferToMgrItems { get; set; }
        public virtual DbSet<SSBSetting> SSBSettings { get; set; }
        public virtual DbSet<SYAccessDepartment> SYAccessDepartments { get; set; }
        public virtual DbSet<SYHRModifySalary> SYHRModifySalarys { get; set; }
        public virtual DbSet<SYHRAnnouncement> SYHRAnnouncements { get; set; }
        public virtual DbSet<SYHRSetting> SYHRSettings { get; set; }
        public virtual DbSet<SYSHRAlert> SYSHRAlerts { get; set; }
        public virtual DbSet<TelegramBot> TelegramBots { get; set; }
        public virtual DbSet<SYSocialMedia> SYSocialMedias { get; set; }
        public virtual DbSet<SYHisScript> SYHisScript { get; set; }
        public virtual DbSet<TokenResource> TokenResources { get; set; }
        public virtual DbSet<SYRoundingRule> SYRoundingRules { get; set; }

        #region Attendance
        public virtual DbSet<ATPayperiod> ATPayperiods { get; set; }
        public virtual DbSet<ATEmpCountABS> ATEmpCountABSs { get; set; }
        public virtual DbSet<ATBatch> ATBatches { get; set; }
        public virtual DbSet<ATEmpActivity> ATEmpActivities { get; set; }
        public virtual DbSet<ATBatchItem> ATBatchItems { get; set; }
        public virtual DbSet<ATDevSetting> ATDevSettings { get; set; }
        public virtual DbSet<ATEmpRelWork> ATEmpRelWorks { get; set; }
        public virtual DbSet<ATEmpSchedule> ATEmpSchedules { get; set; }
        public virtual DbSet<ATImpRoster> ATImpRosters { get; set; }
        public virtual DbSet<ATImpRosterHeader> ATImpRosterHeaders { get; set; }
        public virtual DbSet<ATInOut> ATInOuts { get; set; }
        public virtual DbSet<ATLaEaPolicy> ATLaEaPolicies { get; set; }
        public virtual DbSet<ATOTPolicy> ATOTPolicies { get; set; }
        public virtual DbSet<ATPolicy> ATPolicies { get; set; }
        public virtual DbSet<ATPolicyLaEa> ATPolicyLeEas { get; set; }
        public virtual DbSet<ATEmpMissscan> ATEmpMissscans { get; set; }
        public virtual DbSet<HRLeaveDedPolicy> HRLeaveDedPolicies { get; set; }
        public virtual DbSet<HRLeaveHourPolicy> HRLeaveHourPolicies { get; set; }
        public virtual DbSet<ATShift> ATShifts { get; set; }
        #endregion
        public virtual DbSet<CFReportObject> CFReportObjects { get; set; }
        public virtual DbSet<CFReasonCode> CFReasonCodes { get; set; }
        public virtual DbSet<CFUploadPath> CFUploadPaths { get; set; }
        #region  Onbard
        public virtual DbSet<EOBConfirmAlert> EOBConfirmAlerts { get; set; }
        public virtual DbSet<EOBEmpChkLst> EOBEmpChkLsts { get; set; }
        public virtual DbSet<EOBEmpChkLstItem> EOBEmpChkLstItems { get; set; }
        public virtual DbSet<EOBEmpHealthCheckUp> EOBEmpHealthCheckUps { get; set; }
        public virtual DbSet<EOBEmpHealthCheckUpRecord> EOBEmpHealthCheckUpRecords { get; set; }
        public virtual DbSet<EOBSChkLst> EOBSChkLsts { get; set; }
        public virtual DbSet<EOBSChkLstItem> EOBSChkLstItems { get; set; }
        public virtual DbSet<EOBPreBoItem> EOBPreBoItems { get; set; }
        public virtual DbSet<EOBPreBoStage> EOBPreBoStages { get; set; }
        public virtual DbSet<EOBPreBoStep> EOBPreBoSteps { get; set; }
        public virtual DbSet<EOBSocMedAcc> EOBSocMedAccs { get; set; }
        public virtual DbSet<EOBOrienChList> EOBOrienChLists { get; set; }
        public virtual DbSet<EOBOrienChListItem> EOBOrienChListItems { get; set; }
        public virtual DbSet<EOBTypiDurType> EOBTypiDurTypes { get; set; }
        public virtual DbSet<EOBOrienTypeDur> EOBOrienTypeDurs { get; set; }
        public virtual DbSet<EOBOrienDesc> EOBOrienDescs { get; set; }

        #endregion
        public virtual DbSet<ExCfFileTemplate> ExCfFileTemplates { get; set; }
        public virtual DbSet<ExCFUploadMapping> ExCFUploadMappings { get; set; }
        public virtual DbSet<ExCfWFApprover> ExCfWFApprovers { get; set; }
        public virtual DbSet<ExCFWFDepartmentApprover> ExCFWFDepartmentApprovers { get; set; }
        public virtual DbSet<ExCfWFSalaryApprover> ExCfWFSalaryApprovers { get; set; }
        public virtual DbSet<ExCfWorkFlowItem> ExCfWorkFlowItems { get; set; }
        public virtual DbSet<ExDocApproval> ExDocApprovals { get; set; }
        public virtual DbSet<HRCareerHistory> HRCareerHistories { get; set; }
        public virtual DbSet<HREmpAchieve> HREmpAchieves { get; set; }
        public virtual DbSet<HREmpIdentity> HREmpIdentities { get; set; }
        public virtual DbSet<HRTerminType> HRTerminTypes { get; set; }
        public virtual DbSet<PR_RewardsType> PR_RewardsType { get; set; }
        public virtual DbSet<PRADVPay> PRADVPays { get; set; }
        public virtual DbSet<PRBankFee> PRBankFees { get; set; }
        public virtual DbSet<HRMaterialAccount> HRMaterialAccounts { get; set; }
        public virtual DbSet<PRCostCenterGroup> PRCostCenterGroups { get; set; }
        public virtual DbSet<PRCostCenterGroupItem> PRCostCenterGroupItems { get; set; }
        public virtual DbSet<PRIncomeType> PRIncomeTypes { get; set; }
        public virtual DbSet<PROTRate> PROTRates { get; set; }
        public virtual DbSet<PRPayPeriodItem> PRPayPeriodItems { get; set; }
        public virtual DbSet<PRPayrollSetting> PRPayrollSettings { get; set; }
        public virtual DbSet<HRIncreaseSalary> HRIncreaseSalaries { get; set; }
        public virtual DbSet<PRIntegratePO> PRIntegratePOes { get; set; }
        public virtual DbSet<PRIntegratePOItem> PRIntegratePOItems { get; set; }
        public virtual DbSet<HRMClaimType> HRMClaimTypes { get; set; }
        public virtual DbSet<HRMItem> HRMItems { get; set; }
        public virtual DbSet<HRMTravelby> HRMTravelbys { get; set; }
        public virtual DbSet<HRMType> HRMTypes { get; set; }
        #region Eform
        public virtual DbSet<HREFBenefit> HREFBenefits { get; set; }
        public virtual DbSet<HREFEmpResign> HREFEmpResigns { get; set; }
        public virtual DbSet<HREFLeave> HREFLeaves { get; set; }
        public virtual DbSet<HREFProbationType> HREFProbationTypes { get; set; }
        public virtual DbSet<HREFReqChangShift> HREFReqChangShifts { get; set; }
        public virtual DbSet<HREFReqProbation> HREFReqProbations { get; set; }
        public virtual DbSet<HREFRequestTransferStaff> HREFRequestTransferStaffs { get; set; }
        public virtual DbSet<HREFTransferAndPromotionType> HREFTransferAndPromotionTypes { get; set; }
        #endregion

        #region PO
        public virtual DbSet<HRPODetail> HRPODetails { get; set; }
        public virtual DbSet<HRPOHeader> HRPOHeaders { get; set; }
        public virtual DbSet<HRPORequest> HRPORequests { get; set; }
        public virtual DbSet<HRPORequestItem> HRPORequestItems { get; set; }
        public virtual DbSet<HRReqPayment> HRReqPayments { get; set; }
        public virtual DbSet<HRReqPaymentItem> HRReqPaymentItems { get; set; }

        #endregion
        #region Evaluate
        public virtual DbSet<HREmpEvalRating> HREmpEvalRatings { get; set; }
        public virtual DbSet<HREmpEvaluate> HREmpEvaluates { get; set; }
        public virtual DbSet<HREmpEvaluateScore> HREmpEvaluateScores { get; set; }
        public virtual DbSet<HREvaluateFactor> HREvaluateFactors { get; set; }
        public virtual DbSet<HREvaluateRating> HREvaluateRatings { get; set; }
        public virtual DbSet<HREvaluateRegion> HREvaluateRegions { get; set; }
        public virtual DbSet<HREvaluateType> HREvaluateTypes { get; set; }
        public virtual DbSet<HREmpSelfEvaluation> HREmpSelfEvaluations { get; set; }
        public virtual DbSet<HREmpSelfEvaluationItem> HREmpSelfEvaluationItems { get; set; }
        public virtual DbSet<HREvalSelfEvaluate> HREvalSelfEvaluates { get; set; }
        public virtual DbSet<HREmpEvaluateProcess> HREmpEvaluateProcesses { get; set; }
        #endregion
        #region appraisal
        public virtual DbSet<HREmpAppProcess> HREmpAppProcesses { get; set; }
        public virtual DbSet<HREmpAppProcessItem> HREmpAppProcessItems { get; set; }
        public virtual DbSet<HREmpAppraisal> HREmpAppraisals { get; set; }
        public virtual DbSet<HREmpAppraisalItem> HREmpAppraisalItems { get; set; }
        public virtual DbSet<HREmpAppraisalSummary> HREmpAppraisalSummarys { get; set; }
        public virtual DbSet<HRAppGrade> HRAppGrades { get; set; }
        public virtual DbSet<HRApprFactor> HRApprFactors { get; set; }
        public virtual DbSet<HRApprRegion> HRApprRegions { get; set; }
        public virtual DbSet<HRApprSelfAssQCM> HRApprSelfAssQCMs { get; set; }
        public virtual DbSet<HRApprType> HRApprTypes { get; set; }
        public virtual DbSet<HRKPITracking> HRKPITrackings { get; set; }
        public virtual DbSet<HRApprRating> HRApprRatings { get; set; }
        public virtual DbSet<HRApprSelfAssessment> HRApprSelfAssessments { get; set; }
        public virtual DbSet<EFEmpPortalItem> EFEmpPortalItems { get; set; }
        public virtual DbSet<HRAPPMatrixIncreaseSalary> HRAPPMatrixIncreaseSalaries { get; set; }

        #endregion

        #region HisGen First
        public virtual DbSet<BiMonthlySalarySetting> BiMonthlySalarySettings { get; set; }
        public virtual DbSet<HISGenFirstPay> HISGenFirstPays { get; set; }
        public virtual DbSet<HISGenFirstPaySalaryD> HISGenFirstPaySalaryDs { get; set; }
        public virtual DbSet<HisGenLeaveDFirstPay> HisGenLeaveDFirstPays { get; set; }
        public virtual DbSet<HisGenMonthlySalaryRetro> HisGenMonthlySalaryRetroes { get; set; }
        public virtual DbSet<HisGenOTFirstPay> HisGenOTFirstPays { get; set; }
        public virtual DbSet<HISPaySlipFirstPay> HISPaySlipFirstPays { get; set; }
        public virtual DbSet<PRBiExchangeRate> PRBiExchangeRates { get; set; }
        public virtual DbSet<PRFiscalYear> PRFiscalYears { get; set; }
        public DbSet<PRFringeBenefitSetting> PRFringeBenefitSettings { get; set; }
        #endregion

        #region Payroll
        public virtual DbSet<HisCostCharge> HisCostCharges { get; set; }
        public virtual DbSet<HisEmpPayment> HisEmpPayments { get; set; }
        public virtual DbSet<HisEmpReward> HisEmpRewards { get; set; }
        public virtual DbSet<HisEmpRewardFP> HisEmpRewardFPs { get; set; }
        public virtual DbSet<HisEmpSocialSecurity> HisEmpSocialSecurities { get; set; }
        public virtual DbSet<HISGenAllowance> HISGenAllowances { get; set; }
        public virtual DbSet<HISGenBonu> HISGenBonus { get; set; }
        public virtual DbSet<HISGenDeduction> HISGenDeductions { get; set; }
        public virtual DbSet<HisGenFee> HisGenFees { get; set; }
        public virtual DbSet<HISGenLeaveDeduct> HISGenLeaveDeducts { get; set; }
        public virtual DbSet<HISGenOTHour> HISGenOTHours { get; set; }
        public virtual DbSet<HISGenSalary> HISGenSalaries { get; set; }
        public virtual DbSet<HISGenSalaryD> HISGenSalaryDs { get; set; }
        public virtual DbSet<PREmpAllw> PREmpAllws { get; set; }
        public virtual DbSet<PREmpBonu> PREmpBonus { get; set; }
        public virtual DbSet<PREmpCCCharge> PREmpCCCharges { get; set; }
        public virtual DbSet<PREmpClaimBen> PREmpClaimBens { get; set; }
        public virtual DbSet<PREmpDeduc> PREmpDeducs { get; set; }
        public virtual DbSet<PREmpHold> PREmpHolds { get; set; }
        public virtual DbSet<PREmpLateDeduct> PREmpLateDeducts { get; set; }
        public virtual DbSet<PREmpOTCondition> PREmpOTConditions { get; set; }
        public virtual DbSet<PREmpOverTime> PREmpOverTimes { get; set; }
        public virtual DbSet<PREmpSVC> PREmpSVCs { get; set; }
        public virtual DbSet<PREmpWorking> PREmpWorkings { get; set; }
        public virtual DbSet<PRExceptType> PRExceptTypes { get; set; }
        public virtual DbSet<PRExchRate> PRExchRates { get; set; }
        public virtual DbSet<PRPensionFundSetting> PRPensionFundSettings { get; set; }
        public virtual DbSet<PRProvidentFund> PRProvidentFunds { get; set; }
        public virtual DbSet<PRSeverancePay> PRSeverancePays { get; set; }
        public virtual DbSet<PRSeveranceRate> PRSeveranceRates { get; set; }
        public virtual DbSet<PRSincerity> PRSincerities { get; set; }
        public virtual DbSet<PRSocialSecurity> PRSocialSecurities { get; set; }

        public virtual DbSet<PRSocsec> PRSocsecs { get; set; }
        public virtual DbSet<PRSVCValue> PRSVCValues { get; set; }
        public virtual DbSet<HisSalaryPay> HisSalaryPays { get; set; }
        #endregion
        #region RCM
        public virtual DbSet<RCMIntVQuestionnaire> RCMIntVQuestionnaires { get; set; }
        public virtual DbSet<RCMADependent> RCMADependents { get; set; }
        public virtual DbSet<RCMAEdu> RCMAEdus { get; set; }
        public virtual DbSet<RCMALanguage> RCMALanguages { get; set; }
        public virtual DbSet<RCMAReference> RCMAReferences { get; set; }
        public virtual DbSet<RCMATraining> RCMATrainings { get; set; }
        public virtual DbSet<RCMAWorkHistory> RCMAWorkHistories { get; set; }
        public virtual DbSet<RCMAdvType> RCMAdvTypes { get; set; }
        public virtual DbSet<RCMAdvertising> RCMAdvertisings { get; set; }
        public virtual DbSet<RCMSAdvertise> RCMSAdvertises { get; set; }
        public virtual DbSet<RCMSJobDesc> RCMSJobDescs { get; set; }
        public virtual DbSet<RCMSLang> RCMSLangs { get; set; }
        public virtual DbSet<RCMSQuestionnaire> RCMSQuestionnaires { get; set; }
        public virtual DbSet<RCMGuideRecruit> RCMGuideRecruits { get; set; }
        public virtual DbSet<RCMRecruitRequest> RCMRecruitRequests { get; set; }
        public virtual DbSet<RCMApplicant> RCMApplicants { get; set; }
        public virtual DbSet<RCMERecruit> RCMERecruits { get; set; }
        public virtual DbSet<RCMHire> RCMHires { get; set; }
        public virtual DbSet<RCMPInterview> RCMPInterviews { get; set; }
        public virtual DbSet<RCMRefCheckPerson> RCMRefCheckPersons { get; set; }
        public virtual DbSet<RCMRSkillRequire> RCMRSkillRequires { get; set; }
        public virtual DbSet<RCMVacancy> RCMVacancies { get; set; }
        public virtual DbSet<RCMVInterviewer> RCMVInterviewers { get; set; }
        public virtual DbSet<RCMAIdentity> RCMAIdentities { get; set; }
        public virtual DbSet<RCMSourcingExpend> RCMSourcingExpends { get; set; }
        public virtual DbSet<RCMRefQuestionnaire> RCMRefQuestionnaires { get; set; }
        public virtual DbSet<RCMSRefQuestion> RCMSRefQuestions { get; set; }
        public virtual DbSet<RCMInterveiwRegion> RCMInterveiwRegions { get; set; }
        public virtual DbSet<RCMInterveiwFactor> RCMInterveiwFactors { get; set; }
        public virtual DbSet<RCMInterveiwRating> RCMInterveiwRatings { get; set; }
        public virtual DbSet<RCMEmpEvaluateScore> RCMEmpEvaluateScores { get; set; }
        #endregion
        public virtual DbSet<SyBackgroundService> SyBackgroundServices { get; set; }
        public virtual DbSet<SyServiceURL> SyServiceURLs { get; set; }
        public virtual DbSet<SYSHRBuiltinAcc> SYSHRBuiltinAccs { get; set; }
        public virtual DbSet<Temp_BonusAnnual> Temp_BonusAnnual { get; set; }
        public virtual DbSet<ZEmailSentLog> ZEmailSentLogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Attendance
            modelBuilder.Entity<ATDevSetting>()
                .Property(e => e.DeviceID)
                .IsUnicode(false);

            modelBuilder.Entity<ATDevSetting>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ATDevSetting>()
                .Property(e => e.DevType)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LateDesc1)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LateFlag1)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.DepDesc1)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.DepFlag1)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LateDesc2)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LateFlag2)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.DepDesc2)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.DEPFLAG2)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.SHIFT)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.USERLCK)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.LeaveDesc)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.GMSTATUS)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.Remark2)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.USERDEL)
                .IsUnicode(false);

            modelBuilder.Entity<ATEmpSchedule>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<ATImpRoster>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<ATImpRoster>()
                .Property(e => e.Shift)
                .IsUnicode(false);

            modelBuilder.Entity<ATImpRosterHeader>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ATImpRosterHeader>()
                .Property(e => e.ChangedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ATInOut>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<ATInOut>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<ATOTPolicy>()
                .Property(e => e.OTFrom)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ATOTPolicy>()
                .Property(e => e.OTTo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ATOTPolicy>()
                .Property(e => e.OTHour)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ATShift>()
                .Property(e => e.Code)
                .IsUnicode(false);
            #endregion

            #region  Onbard
            modelBuilder.Entity<EOBConfirmAlert>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<EOBConfirmAlert>()
                .Property(e => e.SendingSelected)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpChkLst>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpChkLstItem>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpChkLstItem>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpHealthCheckUp>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpHealthCheckUp>()
                .Property(e => e.HealthCheckUpType)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpHealthCheckUp>()
                .Property(e => e.AckBy)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpHealthCheckUp>()
                .Property(e => e.CheckedBy)
                .IsUnicode(false);

            modelBuilder.Entity<EOBEmpHealthCheckUpRecord>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<EOBSChkLst>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<EOBSChkLstItem>()
                .Property(e => e.Code)
                .IsUnicode(false);
            #endregion

            modelBuilder.Entity<HRDressReturnItem>()
                .Property(e => e.ReceivedItemID)
                .IsUnicode(false);

            #region Payroll
            modelBuilder.Entity<HisEmpPayment>()
                .Property(e => e.Amount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenAllowance>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenAllowance>()
                .Property(e => e.AllwCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenAllowance>()
                .Property(e => e.PayTo)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenAllowance>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.BonusCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.BonusAM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.RatePerDay)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedAm)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedAMPM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.Reckey)
                .IsUnicode(false);
            modelBuilder.Entity<HISGenLeaveDeduct>()
               .Property(e => e.EmpCode)
               .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.LeaveDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.ForMular)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.BaseSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTFormula)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.TermStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.TermRemark)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.CostCenter)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.HomeFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.ICNO)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EPF)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.TaxNo)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.BankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.USERGEN)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.ESalary)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EAmtoBrTax)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EGrossNoTIP)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.EGrossPay)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.ENetNoTIP)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.ENetWage)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.TXPayType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_Salary)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_GrossPay)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_AmToBeTax)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_Tax)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_TaxRate)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HISGenSalary>()
                .Property(e => e.SFT_NetPay)
                .HasPrecision(18, 5);
            modelBuilder.Entity<HISGenSalary>().Property(e => e.AlwBeforTax).HasPrecision(18, 5);
            modelBuilder.Entity<HISGenSalary>().Property(e => e.Seniority).HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.CareerCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Homefunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.ActWorkDay)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.BasicSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenSalaryD>()
                .Property(e => e.ActWorkHours)
                .HasPrecision(18, 4);
            modelBuilder.Entity<PREmpAllw>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<PREmpAllw>()
                    .Property(e => e.AllwCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpAllw>()
                    .Property(e => e.Hospital)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpAllw>()
                    .Property(e => e.InvoceNo)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpBonu>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpBonu>()
                    .Property(e => e.BonCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpBonu>()
                    .Property(e => e.Amount)
                    .HasPrecision(18, 4);

            modelBuilder.Entity<PREmpCCCharge>()
                    .Property(e => e.Charge)
                    .HasPrecision(18, 0);

            modelBuilder.Entity<PREmpDeduc>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpDeduc>()
                    .Property(e => e.DedCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpLateDeduct>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpLateDeduct>()
                    .Property(e => e.DedCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpOverTime>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpOverTime>()
                    .Property(e => e.OTType)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpSVC>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpWorking>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpWorking>()
                    .Property(e => e.Hours)
                    .HasPrecision(18, 5);

            modelBuilder.Entity<PREmpWorking>()
                    .Property(e => e.Remark)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpWorking>()
                    .Property(e => e.CreatedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<PREmpWorking>()
                    .Property(e => e.ChangedBy)
                    .IsUnicode(false);
            modelBuilder.Entity<PRSeverancePay>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PRSincerity>()
                    .Property(e => e.EmpCode)
                    .IsUnicode(false);

            modelBuilder.Entity<PRSVCValue>()
                    .Property(e => e.SVCACC)
                    .IsUnicode(false);

            modelBuilder.Entity<PRSVCValue>()
                    .Property(e => e.TIPACC)
                    .IsUnicode(false);

            modelBuilder.Entity<PRSVCValue>()
                    .Property(e => e.SVCPACC)
                    .IsUnicode(false);


            #endregion

            modelBuilder.Entity<HRAssetClass>()
                .Property(e => e.AssetClassCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRAssetClass>()
                .Property(e => e.AssetTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRAssetRegister>()
                .Property(e => e.UsefulLifeYear)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRAssetRegister>()
                .Property(e => e.AcquisitionCost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HRAssetRegister>()
                .Property(e => e.Condition)
                .IsFixedLength()
                .IsUnicode(false);

            #region HR
            modelBuilder.Entity<HRCareerHistory>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRCareerHistory>()
                .Property(e => e.TemplatePath)
                .IsUnicode(false);

            modelBuilder.Entity<HRCareerHistory>()
                .Property(e => e.TemplatePathKH)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCertificate>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCertificate>()
                .Property(e => e.CertType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.CareerCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.SECT)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.HomeFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.JobDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.JobSpec)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.SupCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.SuperVisor)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.Appby)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.AppDate)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.VeriFyBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.VeriFYDate)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.APPLBY)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.resigntype)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.EstartSAL)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.EIncrease)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.ESalary)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpCareer>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);
            modelBuilder.Entity<HREmpDisciplinary>()
                 .Property(e => e.EmpCode)
                 .IsUnicode(false);

            modelBuilder.Entity<HREmpDisciplinary>()
                .Property(e => e.DiscType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpDisciplinary>()
                .Property(e => e.DiscAction)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeave>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeave>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeave>()
                .Property(e => e.DocNo)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeaveB>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeaveB>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeaveB>()
                .Property(e => e.LToDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HREmpLeaveD>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeaveD>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLeaveD>()
                .Property(e => e.Shift)
                .IsUnicode(false);
            modelBuilder.Entity<HRLeaveType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRLeaveType>()
                .Property(e => e.Foperand)
                .IsUnicode(false);

            modelBuilder.Entity<HRLeaveType>()
                .Property(e => e.Operator)
                .IsUnicode(false);
            modelBuilder.Entity<HREmpIdentity>()
                 .Property(e => e.EmpCode)
                 .IsUnicode(false);

            modelBuilder.Entity<HREmpIdentity>()
                .Property(e => e.IdentityTye)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Costcent)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.SubCostCent)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.EmpSource)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Marital)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Nation)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Race)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Religion)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.TranType)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Phone2)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PhoneExt)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.FAX)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BankAccName)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Heigth)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Weigth)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BloodType)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Performance)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.LOCT)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Line)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.SECT)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.HomeFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.POSTDESC)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.JOBSPEC)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.HODCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.LEAVESCHM)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.TerminateStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.TerminateRemark)
                .IsUnicode(false);


            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.ConPhone)
                .IsUnicode(false);


            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PerPhone)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.CareerDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PayParam)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.EMPSKILL)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.EMPHOBBIE)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.ESalary)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.ROSTER)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.BONUSTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.Email2)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PAEMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.ALERTLEAVE)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.TXPayType)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.PostFamily)
                .IsUnicode(false);

            modelBuilder.Entity<HRStaffProfile>()
                .Property(e => e.HODPost)
                .IsUnicode(false);
            #endregion

            #region Evaluate
            modelBuilder.Entity<HREvaluateFactor>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HREvaluateType>()
                .Property(e => e.Code)
                .IsUnicode(false);
            #endregion

            #region appraisal
            modelBuilder.Entity<HRApprFactor>()
                .Property(e => e.Code)
                .IsUnicode(false);
            modelBuilder.Entity<HRApprType>()
                    .Property(e => e.Code)
                    .IsUnicode(false);
            modelBuilder.Entity<HRApprSelfAssessment>()
                .Property(e => e.QuestionCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRTerminType>()
                .Property(e => e.Code)
                .IsUnicode(false);
            #endregion
            modelBuilder.Entity<PRADVPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);
            modelBuilder.Entity<PRBankFee>()
                .Property(e => e.BrankCode)
                .IsUnicode(false);
            modelBuilder.Entity<PRCostCenterGroupItem>()
                .Property(e => e.CostCenterType)
                .IsUnicode(false);

            #region HisGen First
            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TermStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TermRemark)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.ICNO)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.USERGEN)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTAXCH)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTAXSP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxALWAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxALWAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxDEDAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxDEDAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxBONAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxBONAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.CareerCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Homefunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.ActWorkDay)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.BasicSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.ActWorkHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.LeaveDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.ForMular)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.WorkedDay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.ActualWorkedDay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.Rate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.EarnedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.BaseSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTFormula)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.EarnDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.DeductDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            #endregion

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.OTCode)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.OTType)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.Foperand)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.Foperator)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.Soperand)
                .IsUnicode(false);

            modelBuilder.Entity<PROTRate>()
                .Property(e => e.Soperator)
                .IsUnicode(false);

            modelBuilder.Entity<PR_RewardsType>()
                .Property(e => e.ReCode)
                .IsUnicode(false);

            modelBuilder.Entity<PR_RewardsType>()
                .Property(e => e.Code)
                .IsUnicode(false);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Temp_BonusAnnual>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_BonusAnnual>()
                .Property(e => e.BonusCode)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_BonusAnnual>()
                .Property(e => e.BonusDesc)
                .IsUnicode(false);

            // Views 
            //modelBuilder.Entity<ExDocApproval>().HasKey(b => new { b.DocumentNo,b.DocumentType,b.Approver });

            modelBuilder.Entity<HRIncreaseSalary>().HasKey(b => new { b.DocNo });

            modelBuilder.Entity<HISApproveSalary>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<HisCostCharge>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisCostCharge>()
                .Property(e => e.CostCenter)
                .IsUnicode(false);

            modelBuilder.Entity<HisCostCharge>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HisEmpReward>().Property(e => e.Amount).HasPrecision(18, 5);
            modelBuilder.Entity<HisEmpRewardFP>().Property(e => e.Amount).HasPrecision(18, 5);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.BonusCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.BonusAM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenBonu>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.RatePerDay)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedAm)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DedAMPM)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenDeduction>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.Levels)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.FeeFrom)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.FeeTo)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenFee>()
                .Property(e => e.DedAmount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TermStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TermRemark)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.ICNO)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.USERGEN)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTAXCH)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTAXSP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxALWAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxALWAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxDEDAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxDEDAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.TaxBONAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPay>()
                .Property(e => e.UTaxBONAM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.CareerCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.LINE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.CATE)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Sect)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.LevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.JobGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.PersGrade)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Homefunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Functions)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.SubFunction)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.ActWorkDay)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.BasicSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenFirstPaySalaryD>()
                .Property(e => e.ActWorkHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.LeaveDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.ForMular)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenLeaveDeduct>()
                .Property(e => e.RECKEY)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.LeaveDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.ForMular)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenLeaveDFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.WorkedDay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.ActualWorkedDay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.Rate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenMonthlySalaryRetro>()
                .Property(e => e.EarnedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.BaseSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.OTFormula)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HisGenOTFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.BaseSalary)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.WorkHour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTHour)
                .HasPrecision(18, 4);
            modelBuilder.Entity<HISGenOTHour>().Property(e => e.OTRate).HasPrecision(18, 5);
            modelBuilder.Entity<HISGenOTHour>().Property(e => e.Amount).HasPrecision(18, 5);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.OTFormula)
                .IsUnicode(false);

            modelBuilder.Entity<HISGenOTHour>()
                .Property(e => e.Measure)
                .IsUnicode(false);

            modelBuilder.Entity<HISGLBenCharge>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISGLBenCharge>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HisInsurance>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisInsurance>()
                .Property(e => e.InsurType)
                .IsUnicode(false);

            modelBuilder.Entity<HisInsurance>()
                .Property(e => e.InsurComp)
                .IsUnicode(false);

            modelBuilder.Entity<HisInsurance>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HisPayHi>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HisPayHi>()
                .Property(e => e.SGROUP)
                .IsUnicode(false);

            modelBuilder.Entity<HisPayHi>()
                .Property(e => e.PayType)
                .IsUnicode(false);

            modelBuilder.Entity<HisPayHi>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlip>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlip>()
                .Property(e => e.EarnDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlip>()
                .Property(e => e.DeductDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlip>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlip>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.EarnDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.DeductDesc)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.DeletedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HISPaySlipFirstPay>()
                .Property(e => e.Reckey)
                .IsUnicode(false);

            modelBuilder.Entity<HISSVCMonthly>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HISSVCMonthly>()
                .Property(e => e.CretaeBy)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.ApprNo)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.APPRTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.PGRADE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.DISCUSNOTE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.NEWPOST)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.HRSIGNCODE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.DDSIGNCODE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.DRSIGNCODE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.APPRAISEE)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.APPRAISEECOMM)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.APPRAISER)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.APPRAISERCOMM)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.HOD)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.HODCOMM)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.DDDR)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.DDDRCOMM)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.IMPLEMENT)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.COOPERATION)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprForm>()
                .Property(e => e.BEHAVIOR)
                .IsUnicode(false);

            modelBuilder.Entity<HRApproverLeave>()
                .Property(e => e.BranchID)
                .IsUnicode(false);

            modelBuilder.Entity<HRApproverLeave>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HRApproverLeave>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<HRApproverLeave>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRApproverLeave>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<HRApprSelfAssessment>()
                .Property(e => e.QuestionCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRBank>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRBankBranch>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRBloodType>()
                .Property(e => e.Code)
                .IsUnicode(false);


            modelBuilder.Entity<HRCategory>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRCertificationType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRCertificationType>()
                .Property(e => e.TemplatePath)
                .IsUnicode(false);

            modelBuilder.Entity<HRCertificationType>()
                .Property(e => e.TemplatePathKH)
                .IsUnicode(false);

            modelBuilder.Entity<HRContractType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRContractType>()
                .Property(e => e.TemplatePath)
                .IsUnicode(false);

            modelBuilder.Entity<HRContractType>()
                .Property(e => e.TemplateNameKhm)
                .IsUnicode(false);

            modelBuilder.Entity<HRCountry>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRDepartment>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRDisciplinAction>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRDisciplinAction>()
                .Property(e => e.TemplatePath)
                .IsUnicode(false);

            modelBuilder.Entity<HRDisciplinAction>()
                .Property(e => e.TemplatePathKH)
                .IsUnicode(false);

            modelBuilder.Entity<HRDisciplinType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRDivision>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HREduType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpContract>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpContract>()
                .Property(e => e.ConType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpContract>()
                .Property(e => e.ContactText)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpContract>()
                .Property(e => e.Conterm)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpDepositItem>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpEduc>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpEduc>()
                .Property(e => e.EduType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpEvaluateProcess>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpEvaluateProcess>()
                .Property(e => e.AssignedTo)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpFamily>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpFamily>()
                .Property(e => e.RelCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpFamily>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpFamily>()
                .Property(e => e.IDCard)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpFamily>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.InsurType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.InsurComp)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpInsurance>()
                .Property(e => e.ChangedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLoan>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpLoanH>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.MedicalType)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.HospCHK)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.Result)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.Createdby)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpPhischk>()
                .Property(e => e.ChangedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpResign>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpResign>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpSelfEvaluation>()
                .Property(e => e.EvaluationCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpSelfEvaluation>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpSelfEvaluation>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpSelfEvaluation>()
                .Property(e => e.ChangedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HREmpType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HREvaluateFactor>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRHeadCount>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRHospital>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRInsuranceCompany>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<HRInsuranceCompany>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HRInsuranceType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRInsuranceType>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<HRJobGrade>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRKPIActivitiesHeader>()
                .Property(e => e.TotalAcheivement)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesHeader>()
                .Property(e => e.KPIAverage)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesHeader>()
                .Property(e => e.TotalScore)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesItem>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesItem>()
                .Property(e => e.Difference)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesItem>()
                .Property(e => e.Acheivement)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIActivitiesItem>()
                .Property(e => e.AcheivementByEachKPI)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignHeader>()
                .Property(e => e.TotalAchievement)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignHeader>()
                .Property(e => e.KPIAverage)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignHeader>()
                .Property(e => e.TotalScore)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignItem>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignItem>()
                .Property(e => e.Actual)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignItem>()
                .Property(e => e.Score)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIAssignItem>()
                .Property(e => e.Acheivement)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRKPIEmpConcern>()
                .Property(e => e.EmpName)
                .IsFixedLength();

            modelBuilder.Entity<HRKPIEmployeeAction>()
                .Property(e => e.EmpName)
                .IsFixedLength();

            modelBuilder.Entity<HRKPIJobFolloUp>()
                .Property(e => e.EmpName)
                .IsFixedLength();

            modelBuilder.Entity<HRKPIManagemGuideLine>()
                .Property(e => e.EmpName)
                .IsFixedLength();

            modelBuilder.Entity<HRKPIPlanItem>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HRLine>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRLocation>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRMedicalType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRMissionClaimItem>()
                .Property(e => e.Remark)
                .IsFixedLength();

            modelBuilder.Entity<HRMissionPlanItem>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<HRMissionPlanItem>()
                .Property(e => e.Remark)
                .IsFixedLength();

            modelBuilder.Entity<HRNation>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HROrgChart>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<HROrgChart>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<HRPHCKHResult>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRPosition>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRPositionFamily>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRProvice>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRRelationshipType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRRequestOT>()
                .Property(e => e.OTRNo)
                .IsUnicode(false);

            modelBuilder.Entity<HRRoomType>()
                .Property(e => e.RoomCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRSection>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<HRSetEntitleD>()
                .Property(e => e.CodeH)
                .IsUnicode(false);

            modelBuilder.Entity<HRSetEntitleD>()
                .Property(e => e.LeaveCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRSetEntitleH>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<ATSwitchShift>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkFlow>()
                .Property(e => e.BranchID)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkFlow>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkFlow>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkFlow>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkFlowHeader>()
                .Property(e => e.BranchID)
                .IsUnicode(false);

            modelBuilder.Entity<HRWorkingType>()
                .Property(e => e.Code)
                .IsUnicode(false);


            modelBuilder.Entity<PRBenefitType>()
                 .Property(e => e.Code)
                 .IsUnicode(false);

            modelBuilder.Entity<PRChartofAccount>()
                .Property(e => e.DC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PRChartofAccount>()
                .Property(e => e.GrpAcc)
                .IsUnicode(false);

            modelBuilder.Entity<PRChartofAccount>()
                .Property(e => e.GrpBen)
                .IsUnicode(false);

            modelBuilder.Entity<PRChartofAccount>()
                .Property(e => e.BenCode)
                .IsUnicode(false);
            modelBuilder.Entity<PRFeeSetting>()
                .Property(e => e.Levels)
                .IsUnicode(false);

            modelBuilder.Entity<PRFeeSetting>()
                .Property(e => e.FeeFrom)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PRFeeSetting>()
                .Property(e => e.FeeTo)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PRFeeSetting>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PRInteAccountItem>()
                .Property(e => e.CreditAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PRInteAccountItem>()
                .Property(e => e.DebitAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PRParameter>()
                .Property(e => e.Code)
                .IsUnicode(false);
            modelBuilder.Entity<PRTaxSetting>()
                .Property(e => e.TaxFrom)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PRTaxSetting>()
                .Property(e => e.TaxTo)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PRTaxSetting>()
                .Property(e => e.TaxPercent)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PRTaxSetting>()
                .Property(e => e.Amdeduct)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PRTransferToMgr>()
                .Property(e => e.MgrCode)
                .IsUnicode(false);

            modelBuilder.Entity<PRTransferToMgr>()
                .Property(e => e.Branch)
                .IsUnicode(false);

            modelBuilder.Entity<PRTransferToMgr>()
                .Property(e => e.DEPT)
                .IsUnicode(false);

            modelBuilder.Entity<PRTransferToMgr>()
                .Property(e => e.POST)
                .IsUnicode(false);

            modelBuilder.Entity<PRTransferToMgrItem>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HRKPIAssignHeader>().Property(e => e.FinalResult).HasPrecision(18, 4);
            modelBuilder.Entity<HRKPIAssignIndicator>().Property(e => e.Acheivement).HasPrecision(18, 4);

            #region RCM

            modelBuilder.Entity<RCMPInterview>()
                .Property(e => e.DocType)
                .IsUnicode(false);
            modelBuilder.Entity<RCMSAdvertise>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<RCMSAdvertise>().Property(e => e.Password).IsUnicode(false);
            modelBuilder.Entity<RCMSAdvertise>().Property(e => e.AppID).IsUnicode(false);
            modelBuilder.Entity<RCMSAdvertise>().Property(e => e.AppName).IsUnicode(false);

            modelBuilder.Entity<RCMSJobDesc>().Property(e => e.Code).IsUnicode(false);

            modelBuilder.Entity<RCMSLang>().Property(e => e.Code).IsUnicode(false);

            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.GuideRecruitNo).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.DEPT).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.Gender).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.WorkingType).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.CreatedBy).IsUnicode(false);
            modelBuilder.Entity<RCMGuideRecruit>().Property(e => e.ChangedBy).IsUnicode(false);

            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.RequestNo).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.Branch).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.DEPT).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.POST).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.RecruitType).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.WorkingType).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.Gender).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.JobLevel).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.TermEmp).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.RecruitFor).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.RequestedBy).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.CreatedBy).IsUnicode(false);
            modelBuilder.Entity<RCMRecruitRequest>().Property(e => e.ChangedBy).IsUnicode(false);

            modelBuilder.Entity<RCMADependent>().Property(e => e.ApplicantID).IsUnicode(false);

            modelBuilder.Entity<RCMAIdentity>().Property(e => e.ApplicantID).IsUnicode(false);
            modelBuilder.Entity<RCMAIdentity>().Property(e => e.IdentityType).IsUnicode(false);

            modelBuilder.Entity<RCMApplicant>().Property(e => e.VacNo).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.FirstName).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.LastName).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.AllName).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.WorkingType).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Gender).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.ExpectSalary).HasPrecision(18, 0);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Country).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Nationality).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Height).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Weight).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Phone1).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Phone2).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Email).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Source).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.TestScore).HasPrecision(18, 0);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.TestStatus).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.BloodType).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.ApplyDept).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.CurStage).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.Sect).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.PostOffer).IsUnicode(false);
            modelBuilder.Entity<RCMApplicant>().Property(e => e.IntVStatus).IsUnicode(false);

            modelBuilder.Entity<RCMADependent>().Property(e => e.ApplicantID).IsUnicode(false);

            modelBuilder.Entity<RCMAReference>().Property(e => e.Email).IsUnicode(false);

            modelBuilder.Entity<RCMAWorkHistory>().Property(e => e.StartSalary).HasPrecision(18, 0);
            modelBuilder.Entity<RCMAWorkHistory>().Property(e => e.EndSalary).HasPrecision(18, 0);

            modelBuilder.Entity<RCMIntVQuestionnaire>().Property(e => e.ApplicantID).IsUnicode(false);

            modelBuilder.Entity<RCMHire>().Property(e => e.ApplyPosition).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.EmpCode).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.EmployeeType).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Branch).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Location).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Division).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Department).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Category).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Level).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Position).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Line).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.JobGrade).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.ROSTER).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.Section).IsUnicode(false);
            modelBuilder.Entity<RCMHire>().Property(e => e.TXPayType).IsUnicode(false);

            modelBuilder.Entity<RCMRefCheckPerson>().Property(e => e.ApplicantID).IsUnicode(false);

            modelBuilder.Entity<RCMRSkillRequire>().Property(e => e.RequestNo).IsUnicode(false);

            modelBuilder.Entity<RCMVacancy>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.Position).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.VacancyType).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.Branch).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.Dept).IsUnicode(false);
            modelBuilder.Entity<RCMVacancy>().Property(e => e.WorkingType).IsUnicode(false);

            modelBuilder.Entity<RCMAdvertising>().Property(e => e.VacNo).IsUnicode(false);
            modelBuilder.Entity<RCMAdvertising>().Property(e => e.AdsType).IsUnicode(false);
            modelBuilder.Entity<RCMAdvertising>().Property(e => e.Advertising).IsUnicode(false);

            modelBuilder.Entity<RCMVInterviewer>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<RCMVInterviewer>().Property(e => e.EmpCode).IsUnicode(false);
            modelBuilder.Entity<RCMVInterviewer>().Property(e => e.Position).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.ApplicantID).IsUnicode(false);

            modelBuilder.Entity<RCMPInterview>().Property(e => e.VacNo).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.ApplyPost).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.PositionOffer).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.AlertToInterviewer).IsUnicode(false);
            modelBuilder.Entity<RCMPInterview>().Property(e => e.DocType).IsUnicode(false);

            #endregion
            #region  SY
            modelBuilder.Entity<SSBSetting>().Property(e => e.MaxSalary).HasPrecision(18, 4);
            modelBuilder.Entity<SYAccessDepartment>().Property(e => e.DeptCode).IsUnicode(false);
            modelBuilder.Entity<SYHisScript>().Property(e => e.UserID).IsUnicode(false);
            modelBuilder.Entity<SYHisScript>().Property(e => e.Script).IsUnicode(false);
            #endregion

            modelBuilder.Entity<SYHRSetting>().Property(e => e.ComRisk).HasPrecision(18, 5);
            modelBuilder.Entity<SYHRSetting>().Property(e => e.StaffRisk).HasPrecision(18, 5);
            modelBuilder.Entity<SYHRSetting>().Property(e => e.ComHealthCare).HasPrecision(18, 5);
            modelBuilder.Entity<SYHRSetting>().Property(e => e.StaffHealthCare).HasPrecision(18, 5);
        }

    }
}
