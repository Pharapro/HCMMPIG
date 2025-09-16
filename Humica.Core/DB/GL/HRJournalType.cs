namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRJournalType")]
    public partial class HRJournalType
    {
        [Key]
        [StringLength(20)]
        public string JournalType { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(30)]
        public string NumberRank { get; set; }

        [StringLength(2)]
        public string NumberRankItem { get; set; }

        [StringLength(20)]
        public string ReturnDoc { get; set; }
        public bool? IsDebitNote { get; set; }
    }
}
