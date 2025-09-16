namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREvaluateRating")]
    public partial class HREvaluateRating
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Region { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int Rating { get; set; }
    }
}
