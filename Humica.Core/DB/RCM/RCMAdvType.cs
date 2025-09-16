namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAdvType")]
    public partial class RCMAdvType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(150)]
        public string SecondDescription { get; set; }
        [StringLength(150)]
        public string Remark { get; set; }
    }
}
