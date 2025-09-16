namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRGroupDepartment")]
    public partial class HRGroupDepartment
    {
        public string Company { get; set; }

        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(10)]
        public string SortKey { get; set; }
        public string Image { get; set; }
        public string VatinNumber { get; set; }
        public string NSSFNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
