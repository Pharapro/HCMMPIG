namespace Humica.Core
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPINonFinance")]
    public partial class HRKPINonFinance
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        [StringLength(200)]
        public string PerformanceType { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
