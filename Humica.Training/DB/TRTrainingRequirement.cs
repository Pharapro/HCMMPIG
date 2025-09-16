namespace Humica.Training.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingRequirement")]
    public partial class TRTrainingRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public decimal? Value { get; set; }

        [StringLength(2)]
        public string Type { get; set; }

        [StringLength(2)]
        public string Category { get; set; }
    }
}
