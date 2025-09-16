namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAppCompareRatio")]
    public partial class HRAppCompareRatio
    {
        [Key]
        public int ID { get; set; }

        public decimal FromRatio { get; set; }

        public decimal ToRatio { get; set; }

        public decimal Factor { get; set; }
    }
}
