namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIActivityAttach")]
    public partial class HRKPIActivityAttach
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AVTCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string AssignCode { get; set; }

        [StringLength(30)]
        public string EmpCode { get; set; }

        [StringLength(250)]
        public string AttachFile { get; set; }

        [StringLength(150)]
        public string Description { get; set; }
    }
}
