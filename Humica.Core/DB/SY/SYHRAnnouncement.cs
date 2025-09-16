namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYHRAnnouncement")]
    public partial class SYHRAnnouncement
    {
        public long ID { get; set; }

        [StringLength(150)]
        public string Type { get; set; }

        [StringLength(150)]
        public string Subject { get; set; }

        public string Description { get; set; }

        [StringLength(30)]
        public string DocumentNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        public bool IsRead { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
