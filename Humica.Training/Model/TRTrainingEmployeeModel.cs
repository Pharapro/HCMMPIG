using Humica.Core.DB;
using Humica.Training.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Humica.Training
{
    [NotMapped]
    public class TRTrainingEmployeeModel : TRTrainingEmployee
    {
        Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemNo { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string Sex { get; set; }
        public string Department { get; set; }
        public HRStaffProfile Staff { get; set; }
        List<TRTrainingEmployeeModel> items { get; set; }
        //public TRTrainingPlan Calendar { get; set; }
        public string DepartmentDes
        {
            get => this.Staff == null ? string.Empty : DB.HRDepartments.Find(this.Staff.DEPT)?.Description;
        }
        public string Posititon
        {
            get => this.Staff == null ? string.Empty : DB.HRPositions.Find(this.Staff.JobCode)?.Description;
        }
        //public string CourseName => this.Calendar == null ? string.Empty :
        //    DBX.TRTrainingCourse.FirstOrDefault(w => w.Code == Calendar.CourseID)?.Description;
    }
}
