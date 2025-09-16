namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBOrienDesc")]
    public partial class EOBOrienDesc
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Department { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string OrienteerCode { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public string OrienteerName { get; set; }
        public string Description { get; set; }
        public decimal? Duration { get; set; }
    }
}
