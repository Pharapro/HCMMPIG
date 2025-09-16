namespace Humica.Training.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingInvitation")]
    public partial class TRTrainingInvitation
    {
        [Key]
        public long TrainNo { get; set; }

        public long? CalendarID { get; set; }
        public int InYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        [StringLength(15)]
        public string CourseID { get; set; }

        [StringLength(200)]
        public string CourseName { get; set; }
        [StringLength(15)]
        public string TrainingTypeID { get; set; }

        [StringLength(200)]
        public string TrainingType { get; set; }

        [StringLength(15)]
        public string CourseCategoryID { get; set; }

        [StringLength(200)]
        public string CourseCategory { get; set; }

        [StringLength(15)]
        public string StaffLevel { get; set; }

        [StringLength(60)]
        public string Position { get; set; }

        public decimal ScoreTheory { get; set; }

        public decimal ScorePractice { get; set; }

        public int? Target { get; set; }

        public int? Capacity { get; set; }

        [Column(TypeName = "date")]
        public DateTime ScheduleFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime ScheduleTo { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(30)]
        public string TrainingGroup { get; set; }

        [StringLength(50)]
        public string Venue { get; set; }

        [StringLength(150)]
        public string Attatchment { get; set; }

        public int? RequirementCode { get; set; }

        [StringLength(20)]
        public string Reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }
        public DateTime? DeadlineAcc { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string ChangedBy { get; set; }

    }
}
