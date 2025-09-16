namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CFUploadPath")]
    public partial class CFUploadPath
    {
        [Key]
        [StringLength(20)]
        public string PathCode { get; set; }

        [StringLength(30)]
        public string Description { get; set; }

        [StringLength(250)]
        public string PathDirectory { get; set; }
    }
}
