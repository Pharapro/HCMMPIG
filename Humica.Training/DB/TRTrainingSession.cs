namespace Humica.Training
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingSession")]
    public partial class TRTrainingSession
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TrainingCalendarID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string TrainNo { get; set; }

        [StringLength(15)]
        public string TrainerType { get; set; }

        [StringLength(15)]
        public string TrainerID { get; set; }
        [StringLength(250)]
        public string TrainerNames { get; set; }

        public int? TopicID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public double? Duration { get; set; }

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
        public string AttachFile { get; set; }

        //[NotMapped]
        //public string TrainerNames
        //{
        //    get
        //    {
        //        var trainer = _db.TRTrainerInfo.Where(w => w.TrainNo.ToString() == this.TrainerID).FirstOrDefault();
        //        if (trainer != null)
        //        {
        //            if (this.TrainerType == Training.TrainerType.EXT)
        //            {
        //                var objTrainerType = _db.TRTrainerType.Where(w => w.Code == Training.TrainerType.EXT).FirstOrDefault();
        //                if (objTrainerType == null) return string.Empty;
        //                //var trainer = _db.TRTrainerInfo.Where(w => w.TrainerCode == this.TrainerID && w.TrainerTypeID == objTrainerType.TrainNo.ToString()).FirstOrDefault();
        //                return trainer == null ? string.Empty : trainer.TrainerName;
        //            }
        //            var staff = _CoreDB.HRStaffProfiles.Find(trainer.TrainerCode);
        //            if (staff != null)
        //            {
        //                return staff.AllName;
        //            }
        //        }
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        this.TrainerID = value;
        //    }
        //}

    }
}
