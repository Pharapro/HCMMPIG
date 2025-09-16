namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CFReasonCode")]
    public partial class CFReasonCode
    {
        [Key]
        [StringLength(5)]
        public string ReasonCode { get; set; }

        [StringLength(1)]
        public string Indicator { get; set; }

        [StringLength(150)]
        public string Description1 { get; set; }

        public string Description2 { get; set; }
    }
}
