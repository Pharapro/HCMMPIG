namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEvalRating")]
    public partial class HREmpEvalRating
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string EvaluateID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RatingID { get; set; }

        [StringLength(20)]
        public string Region { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
        public int Rating { get; set; }
    }
}