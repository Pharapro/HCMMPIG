namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;

    public partial class HR_LineMgr_View
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }
    }
}
