namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBOrienChList")]
    public partial class EOBOrienChList
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? InOrder { get; set; }
    }
}
