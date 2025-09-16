namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATBatchItem")]
    public partial class ATBatchItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string BatchNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string ShiftCode { get; set; }

        public bool? Mon { get; set; }

        public bool? Tue { get; set; }

        public bool? Wed { get; set; }

        public bool? Thu { get; set; }

        public bool? Fri { get; set; }

        public bool? Sat { get; set; }

        public bool? Sun { get; set; }
    }
}
