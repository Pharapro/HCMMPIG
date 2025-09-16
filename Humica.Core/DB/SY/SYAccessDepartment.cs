namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYAccessDepartment")]
    public partial class SYAccessDepartment
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string DeptCode { get; set; }
    }
}
