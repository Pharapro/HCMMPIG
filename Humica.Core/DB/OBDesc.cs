namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OBDesc")]
    public partial class OBDesc
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Desc1 { get; set; }

        [StringLength(250)]
        public string Desc2 { get; set; }
    }
}
