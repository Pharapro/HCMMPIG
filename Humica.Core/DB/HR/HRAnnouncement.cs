namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAnnouncement")]
    public partial class HRAnnouncement
    {
        [Key]
        public int ID { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidDate { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public string AttachFile { get; set; }
        public string Document { get; set; }
        [StringLength(50)]
        public string AnnounceType { get; set; }
    }
}
