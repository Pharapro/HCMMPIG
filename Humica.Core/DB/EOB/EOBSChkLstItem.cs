namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBSChkLstItem")]
    public partial class EOBSChkLstItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
