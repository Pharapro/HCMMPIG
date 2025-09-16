namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAppLevelMidPoint")]
    public partial class HRAppLevelMidPoint
    {
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string JobLevel { get; set; }

        public decimal? JobLevelMidPoint { get; set; }
    }
}
