namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIRating")]
    public partial class HRKPIRating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public decimal Percentage { get; set; }
    }
}
