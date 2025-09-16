namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpAppProcessItem")]
    public partial class HREmpAppProcessItem
    {
        [Key]
        [Column(Order = 0)]
        public int AppraisalProcessNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Category { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public decimal Weight { get; set; }

        public decimal MaxScore { get; set; }

        public decimal Result { get; set; }

        public int Inorder { get; set; }
    }
}
