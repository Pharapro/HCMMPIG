namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPINorm")]
    public partial class HRKPINorm
    {
        [Key]
        public int TranNo { get; set; }

        public decimal FromActual { get; set; }

        public decimal ToActual { get; set; }

        public decimal Achievement { get; set; }
    }
}
