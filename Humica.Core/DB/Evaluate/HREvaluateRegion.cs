namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREvaluateRegion")]
    public partial class HREvaluateRegion
    {
        [StringLength(30)]
        public string EvaluateType { get; set; }

        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public bool? IsQCM { get; set; }

        public bool? IsComment { get; set; }

        public bool? IsRating { get; set; }

        public int InOrder { get; set; }
    }
}
