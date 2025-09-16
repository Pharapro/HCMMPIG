namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EFEmpPortalItem")]
    public partial class EFEmpPortalItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string PortalNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string QuestionCode { get; set; }

        public string Description1 { get; set; }

        public bool IsQCM { get; set; }

        [StringLength(10)]
        public string CorrectValue { get; set; }

        public string Description2 { get; set; }

        public string Comment { get; set; }
    }
}
