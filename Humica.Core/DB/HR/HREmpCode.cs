namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpCode")]
    public partial class HREmpCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(20)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(20)]
        public string NumberRank { get; set; }

        [StringLength(2)]
        public string NumberRankItem { get; set; }

        [StringLength(15)]
        public string BU { get; set; }

        [StringLength(15)]
        public string Branch { get; set; }

    }
}
