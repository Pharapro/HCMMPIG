namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYHisScript")]
    public partial class SYHisScript
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        public DateTime? TranDate { get; set; }

        [Column(TypeName = "text")]
        public string Script { get; set; }
    }
}
