namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREvalSelfEvaluate")]
    public partial class HREvalSelfEvaluate
    {
        [Key]
        [StringLength(20)]
        public string QuestionCode { get; set; }

        public string Description { get; set; }

        public string SecDescription { get; set; }

        public int InOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
