namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMInterveiwRegion")]
    public partial class RCMInterveiwRegion
    {
        
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int InOrder { get; set; }

        public bool? IsRating { get; set; }

        public bool? IsComment { get; set; }

        public decimal Rating { get; set; }
    }
}
