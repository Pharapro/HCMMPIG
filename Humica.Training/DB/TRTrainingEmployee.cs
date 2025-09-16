
namespace Humica.Training.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingEmployee")]
    public class TRTrainingEmployee
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string TrainNo { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CalendarID { get; set; }

        [Key]
        [Column(Order = 3)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        public int? InYear { get; set; }

        [StringLength(50)]
        public string CourseID { get; set; }
        [StringLength(200)]
        public string GroupDescription { get; set; }

        [StringLength(200)]
        public string CourseName { get; set; }

        [StringLength(15)]
        public string CourseCategoryID { get; set; }

        [StringLength(200)]
        public string CourseCategory { get; set; }

        [StringLength(50)]
        public string TrainingType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(250)]
        public string EmpName { get; set; }
        [StringLength(250)]
        public string Department { get; set; }
        [StringLength(250)]
        public string Position { get; set; }

        public int? InviteTraining { get; set; }

        [StringLength(50)]
        public string InviteTrainingBy { get; set; }

        public DateTime? InviteTrainingDate { get; set; }

        public decimal? ScoreTheory { get; set; }
        public decimal? ScorePractice { get; set; }

        public bool? IsInvite { get; set; }
        public bool? IsAccept { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ReStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }
        public bool? IsAssign { get; set; }
        public string TrainerName { get; set; }
        public decimal? HAmount { get; set; }
    }
}
