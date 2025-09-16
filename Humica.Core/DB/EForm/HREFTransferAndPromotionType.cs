namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFTransferAndPromotionType")]
    public partial class HREFTransferAndPromotionType
    {
        [Key]
        [StringLength(30)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public bool IsPro { get; set; }
    }
}
