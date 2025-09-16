namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIAssignMember")]
    public partial class HRKPIAssignMember
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AssignCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string EmpCode { get; set; }
        [StringLength(200)]
        public string EmployeeName { get; set; }
        [StringLength(250)]
        public string Department { get; set; }
        [StringLength(250)]
        public string Position { get; set; }
        [StringLength(50)]
        public string DocRef { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
    }
}
