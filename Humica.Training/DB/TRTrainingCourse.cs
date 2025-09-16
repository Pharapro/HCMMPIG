namespace Humica.Training.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingCourse")]
    public partial class TRTrainingCourse
    {
        [Key]
        public int TrainNo { get; set; }

        [StringLength(15)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(500)]
        public string SecondDescription { get; set; }

        [StringLength(1500)]
        public string Objective { get; set; }

        [StringLength(15)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(15)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
