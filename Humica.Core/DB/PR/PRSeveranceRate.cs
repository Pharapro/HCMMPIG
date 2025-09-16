namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSeveranceRate")]
    public partial class PRSeveranceRate
    {
        [Key]
        public int TranNo { get; set; }

        public int FromMonth { get; set; }

        public int ToMonth { get; set; }

        public decimal? Rate { get; set; }
    }
}
