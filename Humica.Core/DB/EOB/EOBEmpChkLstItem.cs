namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBEmpChkLstItem")]
    public partial class EOBEmpChkLstItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
    }
}
