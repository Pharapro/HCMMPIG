namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpSelfEvaluationItem")]
    public partial class HREmpSelfEvaluationItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string EvaluationCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string QuestionCode { get; set; }

        public string Description { get; set; }

        public string SecDescription { get; set; }

        public string Comment { get; set; }
    }
}
