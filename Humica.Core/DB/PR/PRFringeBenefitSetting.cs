namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRFringeBenefitSetting")]
    public partial class PRFringeBenefitSetting
    {
        [Key]
        [StringLength(50)]
        public string EmployeeType { get; set; }
        public decimal Rate { get; set; }

        public bool IsEmployee { get; set; }
        public bool IsSalaryPaid { get; set; }
    }
}
