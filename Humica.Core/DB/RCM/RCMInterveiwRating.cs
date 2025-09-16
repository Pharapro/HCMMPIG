namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMInterveiwRating")]
    public partial class RCMInterveiwRating
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public bool IsCategory { get; set; }
    }
}
