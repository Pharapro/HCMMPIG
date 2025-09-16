namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMSQuestionnaire")]
    public partial class RCMSQuestionnaire
    {
        [Key]
        public int TranNo { get; set; }

        public int? Step { get; set; }

        public string Description { get; set; }

        [StringLength(10)]
        public string Position { get; set; }
    }
}
