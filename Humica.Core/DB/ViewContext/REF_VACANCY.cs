namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class REF_VACANCY
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Position { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string RequestNo { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string Department { get; set; }
    }
}
