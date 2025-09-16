namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRPayrollSetting")]
    public partial class PRPayrollSetting
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string BaseCurrency { get; set; }
        public decimal? Spouse { get; set; }
        public decimal? Child { get; set; }
        [StringLength(50)]
        public string TaxBase { get; set; }
    }
}
