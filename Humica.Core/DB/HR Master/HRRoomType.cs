namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRRoomType")]
    public partial class HRRoomType
    {
        [Key]
        [StringLength(15)]
        public string RoomCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
