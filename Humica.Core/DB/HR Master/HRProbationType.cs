namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRProbationType")]
    public partial class HRProbationType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public int InMonth { get; set; }

        public int Day { get; set; }
    }
}
