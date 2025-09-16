namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRSettingRound")]
    public partial class HRSettingRound
    {
        [Key]
        public int TranNo { get; set; }

        public bool Salary { get; set; }

        public bool OT { get; set; }

        public bool Allowance { get; set; }

        public bool Deduction { get; set; }

        public bool GrossPay { get; set; }

        public bool TaxAm { get; set; }

        public bool NetPay { get; set; }

        [StringLength(50)]
        public string Rounding { get; set; }
    }
}
