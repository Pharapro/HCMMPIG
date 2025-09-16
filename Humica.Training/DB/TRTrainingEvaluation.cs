namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRTrainingEvaluation")]
    public partial class TRTrainingEvaluation
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string QuestionCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        public string Description { get; set; }
        public bool? IsRequiredComment { get; set; }
        public int InOrder { get; set; }
    }
}
