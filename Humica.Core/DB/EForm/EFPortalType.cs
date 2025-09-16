namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EFPortalType")]
    public partial class EFPortalType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Description2 { get; set; }
    }
}
