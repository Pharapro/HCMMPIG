namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressRequestItem")]
    public partial class HRDressRequestItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        public int Qty { get; set; }

        [StringLength(150)]
        public string Description { get; set; }
    }
}
