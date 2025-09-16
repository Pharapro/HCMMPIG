using System;
using System.ComponentModel.DataAnnotations;

namespace Humica.Training.DB
{
    public class TRTrainingCourseType
    {
        public int TrainNo { get; set; }
        [Required, MaxLength(15)]
        public string Code { get; set; }
        public string Description { get; set; }
        public string SecondDescription { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
    }
}
