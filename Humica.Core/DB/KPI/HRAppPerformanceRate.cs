namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAppPerformanceRate")]
    public partial class HRAppPerformanceRate
    {
        public int ID { get; set; }

        public decimal FromScore { get; set; }

        public decimal ToScore { get; set; }

        public int Result { get; set; }
        public decimal Rate { get; set; }
    }
}
