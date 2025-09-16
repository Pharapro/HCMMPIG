namespace Humica.Core
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisEmpSocialSecurity")]
    public partial class HisEmpSocialSecurity
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PeriodID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string Code { get; set; }

        public string Description { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal Amount { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        public bool? IsEmpoyee { get; set; }

        public decimal AmountBasic { get; set; }
    }
}
