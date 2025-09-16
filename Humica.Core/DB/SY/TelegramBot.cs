namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TelegramBot")]
    public partial class TelegramBot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(250)]
        public string TokenID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string ChatID { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(50)]
        public string DocumentType { get; set; }
    }
}
