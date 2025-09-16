namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATOTPolicy")]
    public partial class ATOTPolicy
    {
        [Key]
        public int TranNo { get; set; }

        public decimal OTFrom { get; set; }

        public decimal OTTo { get; set; }

        public decimal OTHour { get; set; }
    }
}
