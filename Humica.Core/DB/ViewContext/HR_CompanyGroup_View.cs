namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_CompanyGroup_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string CompanyGroup { get; set; }
    }
}
