namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRContractType")]
    public partial class HRContractType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? LCK { get; set; }

        [StringLength(500)]
        public string TemplatePath { get; set; }

        [Column(TypeName = "image")]
        public byte[] TemplateDoc { get; set; }

        [StringLength(500)]
        public string TemplateNameKhm { get; set; }

        [Column(TypeName = "image")]
        public byte[] TemplateDocKhm { get; set; }
        public bool? IsUDC { get; set; }
    }
}
