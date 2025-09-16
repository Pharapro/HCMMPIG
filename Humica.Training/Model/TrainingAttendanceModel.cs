using Humica.Core.DB;
using Humica.Training.DB;
using System.ComponentModel.DataAnnotations;

namespace Humica.Training
{
    public class TrainingAttendanceModel
    {
        Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        [Key]
        public string TrainNo { get; set; }
        public string SessionID { get; set; }
        public TRTrainingAttendance TrainingAttendance { get; set; }
        public TRTrainingPlan TrainingPlan { get; set; }
        public TRTrainingCalender TrainingCalender { get; set; }
        public HRStaffProfile HRStaffProfile { get; set; }
        public TRTrainingSession TRTrainingSession { get; set; }
        public TRTrainingCourse TrainingCourse { get; set; }
        public string AttendText => this.TrainingAttendance == null ? string.Empty : GetAttendanceDesc(TrainingAttendance.IsAttend);
        public string DepartmentDes
        {
            get => this.HRStaffProfile == null ? string.Empty : DB.HRDepartments.Find(this.HRStaffProfile.DEPT)?.Description;
        }
        public string Posititon
        {
            get => this.HRStaffProfile == null ? string.Empty : DB.HRPositions.Find(this.HRStaffProfile.JobCode)?.Description;
        }
        string GetAttendanceDesc(bool? value)
        {
            string description = string.Empty;
            switch (value)
            {
                case false:
                    description = "Absent";
                    break;
                case true:
                    description = "Present";
                    break;
            }
            return description;
        }
    }
}
