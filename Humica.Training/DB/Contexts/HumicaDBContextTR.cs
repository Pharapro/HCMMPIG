namespace Humica.Training.DB
{
    using Humica.Core.DB;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class HumicaDBContextTR : HumicaDBViewContext
    {
        public HumicaDBContextTR() : base()
        {
        }
        public virtual DbSet<TRCourseCategory> TRCourseCategories { get; set; }
        public virtual DbSet<TRTopic> TRTopics { get; set; }
        public virtual DbSet<TRTrainingAgenda> TRTrainingAgendas { get; set; }
        public virtual DbSet<TRTrainingAttendance> TRTrainingAttendances { get; set; }
        public virtual DbSet<TRTrainingCalender> TRTrainingCalenders { get; set; }
        public virtual DbSet<TRTrainingCatalogue> TRTrainingCatalogues { get; set; }
        public virtual DbSet<TRTrainingCatalogueCourse> TRTrainingCatalogueCourses { get; set; }
        public virtual DbSet<TRTrainingCourse> TRTrainingCourses { get; set; }
        public virtual DbSet<TRTrainingEmployee> TRTrainingEmployees { get; set; }
        public virtual DbSet<TRTrainingInvitation> TRTrainingInvitations { get; set; }
        public virtual DbSet<TRTrainingPlan> TRTrainingPlans { get; set; }
        public virtual DbSet<TRPendingDeptInvit> TRPendingDeptInvits { get; set; }
        public virtual DbSet<TRTrainingRequirement> TRTrainingRequirements { get; set; }
        public virtual DbSet<TRTrainingSession> TRTrainingSessions { get; set; }
        public virtual DbSet<TRTrainingType> TRTrainingTypes { get; set; }
        public virtual DbSet<TRTrainingCategory> TRTrainingCategories { get; set; }
        public virtual DbSet<TRTrainingVenue> TRTrainingVenues { get; set; }
        public virtual DbSet<TREmpSurveyScore> TREmpSurveyScores { get; set; }
        public virtual DbSet<TRSurveyFactor> TRSurveyFactors { get; set; }
        public virtual DbSet<TRSurveyRating> TRSurveyRatings { get; set; }
        public virtual DbSet<TRSurveyRegion> TRSurveyRegions { get; set; }
        public virtual DbSet<TRTrainingSurvey> TRTrainingSurveys { get; set; }
        public virtual DbSet<TREmpSurRating> TREmpSurRatings { get; set; }
        public virtual DbSet<EFEmpPortal> EFEmpPortals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRTrainingInvitation>()
                .Property(e => e.ScoreTheory)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TRTrainingInvitation>()
                .Property(e => e.ScorePractice)
                .HasPrecision(18, 4);
            modelBuilder.Entity<TRTrainingRequirement>()
                .Property(e => e.Value)
                .HasPrecision(18, 4);
            // modelBuilder.Entity<User>().Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            // Views 
            //modelBuilder.Entity<ExDocApproval>().HasKey(b => new { b.DocumentNo,b.DocumentType,b.Approver });

            #region modelbuilder 

            #endregion
            modelBuilder.Entity<TRELearningQuestion>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TRELearningAnswer>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TrainigModule>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TrainingCourseMaster>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TrainingProgram>().HasKey(b => new { b.ID, b.ProgramCode });
            modelBuilder.Entity<TrainingCourseItem>().HasKey(b => new { b.InvCode, b.LineItem });
            modelBuilder.Entity<TrainingReleaseProgram>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TrainingAnouncement>().HasKey(b => new { b.Code, b.ID });
            modelBuilder.Entity<TrainingAnouncementItem>().HasKey(b => new { b.ID, b.LineItem });
            modelBuilder.Entity<TrainingRequestHeader>().HasKey(b => new { b.ID, b.RequestCode });
            modelBuilder.Entity<TrainingRequestItem>().HasKey(b => new { b.ID, b.LineItem, b.RequestCode });
            modelBuilder.Entity<TrainingExamHeader>().HasKey(b => new { b.ExamCode, b.ID });
            modelBuilder.Entity<TrainingExamItem>().HasKey(b => new { b.LineItem, b.ExamCode, b.StaffID, b.AnswerCode, b.Topic, b.Coursecode, b.TrainingType, b.QuestionCode });
            modelBuilder.Entity<TRTrainingCourseType>().HasKey(b => new { b.TrainNo });
            modelBuilder.Entity<TRTrainerType>().HasKey(b => new { b.TrainNo });
            modelBuilder.Entity<TRTrainerInfo>().HasKey(b => new { b.TrainNo });
            //modelBuilder.Entity<trtr>
            #region  Qualification-Education
            modelBuilder.Entity<TREducationProfile>().HasKey(b => new { b.ID });
            modelBuilder.Entity<TRQualificatClass>().HasKey(b => new { b.Code });
            modelBuilder.Entity<TRQualificatName>().HasKey(b => new { b.Code });
            modelBuilder.Entity<TRQualificatType>().HasKey(b => new { b.Code });
            #endregion
        }
        #region  Qualification-Education
        public virtual DbSet<TRQualificatClass> TRQualificatClass { get; set; }
        public virtual DbSet<TRQualificatName> TRQualificatName { get; set; }
        public virtual DbSet<TRQualificatType> TRQualificatType { get; set; }
        public virtual DbSet<TREducationProfile> TREducationProfile { get; set; }

        #endregion

        public virtual DbSet<TRELearningQuestion> TRELearningQuestion { get; set; }
        public virtual DbSet<TRELearningAnswer> TRELearningAnswer { get; set; }
        public virtual DbSet<TrainigModule> TrainigModule { get; set; }
        public virtual DbSet<TrainingCourseMaster> TrainingCourseMaster { get; set; }
        public virtual DbSet<TrainingProgram> TrainingProgram { get; set; }
        public virtual DbSet<TrainingCourseItem> TrainingCourseItem { get; set; }
        public virtual DbSet<TrainingReleaseProgram> TrainingReleaseProgram { get; set; }
        public virtual DbSet<TrainingAnouncement> TrainingAnouncement { get; set; }
        public virtual DbSet<TrainingAnouncementItem> TrainingAnouncementItem { get; set; }
        public virtual DbSet<TrainingRequestHeader> TrainingRequestHeader { get; set; }
        public virtual DbSet<TrainingRequestItem> TrainingRequestItem { get; set; }
        public virtual DbSet<TrainingExamHeader> TrainingExamHeader { get; set; }
        public virtual DbSet<TrainingExamItem> TrainingExamItem { get; set; }
        public virtual DbSet<TRTrainingCourseType> TRTrainingCourseType { get; set; }
        public virtual DbSet<TRTrainerType> TRTrainerType { get; set; }
        public virtual DbSet<TRTrainerInfo> TRTrainerInfo { get; set; }
        public virtual DbSet<CFReportObject> CFReportObjects { get; set; }
        public virtual DbSet<ESSTrainingSchedule> ESSTrainingSchedule { get; set; }
        public virtual DbSet<TRTrainingEvaluation> TRTrainingEvaluations { get; set; }

    }
}
