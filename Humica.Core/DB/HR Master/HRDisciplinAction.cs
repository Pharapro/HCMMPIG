namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDisciplinAction")]
    public partial class HRDisciplinAction
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(150)]
        public string SecDescription { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(500)]
        public string TemplatePath { get; set; }

        [StringLength(500)]
        public string TemplatePathKH { get; set; }
    }
}
