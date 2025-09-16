namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_OrgChart_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Code { get; set; }
        public string EmpCode { get; set; }
        [StringLength(140)]
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public string ReportingID { get; set; }
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Images { get; set; }
        public string LevelCode { get; set; }
        public string Email { get; set; }
    }
}
