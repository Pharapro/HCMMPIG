namespace Humica.Training
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingPlan")]
    public partial class TRTrainingPlan
    {
        [Key]
        public int TrainNo { get; set; }

        public int? InYear { get; set; }

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
        public string VenueCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MinTrainee { get; set; }

        public int? MaxTrainee { get; set; }

        public decimal? TotalCost { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(15)]
        public string RequesterCode { get; set; }

        [StringLength(15)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(15)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        public string TrainingCate { get; set; }
        public string Restatus { get; set; }
    }
}
