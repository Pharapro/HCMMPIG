namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSocialSecurity")]
    public partial class PRSocialSecurity
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        [StringLength(50)]
        public string IsValueBased { get; set; }

        [StringLength(50)]
        public string SalaryType { get; set; }
        public int? Age { get; set; }
        public bool? IsEmpoyee { get; set; }
        [StringLength(20)]
        public string IncomeType { get; set; }
    }
}
