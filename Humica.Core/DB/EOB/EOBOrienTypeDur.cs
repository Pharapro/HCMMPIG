namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBOrienTypeDur")]
    public partial class EOBOrienTypeDur
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string OrienteerCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string OrienteerName { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public decimal? Duration { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
