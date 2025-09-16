using System;

namespace Humica.Training.DB
{
    public class TRTrainingRegistration
    {
        public int TrainNo { get; set; }
        public string TRTrainingCalendar { get; set; }
        public int? CourseTypeID { get; set; }
        public int? CourseCateID { get; set; }
        public DateTime? AnnounceDate { get; set; }
        public string AnnouncerCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
    }
}
