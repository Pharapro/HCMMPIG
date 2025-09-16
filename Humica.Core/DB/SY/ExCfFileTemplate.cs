using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Humica.Core.DB
{
    [Table("ExCfFileTemplate")]
    public partial class ExCfFileTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ScreenId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Version { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(500)]
        public string FilePath { get; set; }

        public bool? IsActive { get; set; }
    }
}
