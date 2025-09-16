namespace Humica.Training
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingAgenda")]
    public partial class TRTrainingAgenda
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CalendarID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(15)]
        public string TrainerType { get; set; }

        [StringLength(15)]
        public string TrainerID { get; set; }

        [StringLength(250)]
        public string TrainerNames { get; set; }

        public int? TopicID { get; set; }

        public DateTime? TrainingDate { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public int? InMonth { get; set; }

        public int? Week { get; set; }
    }
}
