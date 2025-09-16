namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SSBSetting")]
    public partial class SSBSetting
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CompanyCD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string SSBCD { get; set; }

        [StringLength(150)]
        public string SSBNo { get; set; }

        public decimal? MaxSalary { get; set; }

        public decimal? EmployeePercentage { get; set; }

        public decimal? CompanyPercentage { get; set; }

        public bool? Status { get; set; }
    }
}
