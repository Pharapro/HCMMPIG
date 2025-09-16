namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRProvidentFund")]
    public partial class PRProvidentFund
    {
        [Key]
        public long TranNo { get; set; }

        public int YEAR { get; set; }

        public double? Resignation { get; set; }

        public double? Death { get; set; }
    }
}
