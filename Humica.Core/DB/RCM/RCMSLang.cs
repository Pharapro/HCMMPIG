namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMSLang")]
    public partial class RCMSLang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecondDescription { get; set; }
    }
}
