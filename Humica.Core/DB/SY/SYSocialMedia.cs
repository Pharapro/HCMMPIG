namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYSocialMedia")]
    public partial class SYSocialMedia
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(350)]
        public string AccessToken { get; set; }

        
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
