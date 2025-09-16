namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFProbationType")]
    public partial class HREFProbationType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(150)]
        public string SecDescription { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
    }
}
