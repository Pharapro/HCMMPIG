namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRLocation")]
    public partial class HRLocation
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(250)]
        public string OthDesc { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string longitude { get; set; }

        [StringLength(30)]
        public string latitude { get; set; }

        public decimal? Distance { get; set; }

        public int? LCK { get; set; }
        public bool? IsActive { get; set; }

    }
}
