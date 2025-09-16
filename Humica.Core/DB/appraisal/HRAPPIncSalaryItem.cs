namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAPPIncSalaryItem")]
    public partial class HRAPPIncSalaryItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [Column(Order = 1)]
        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(250)]
        public string EmployeeName { get; set; }

        [StringLength(250)]
        public string Department { get; set; }

        [StringLength(250)]
        public string Position { get; set; }

        public decimal Increase { get; set; }

        public decimal NewSalary { get; set; }
        [StringLength(20)]
        public string DocumentRef { get; set; }
    }
}