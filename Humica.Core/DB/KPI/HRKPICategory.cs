namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPICategory")]
    public partial class HRKPICategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string CategoryCode { get; set; }

        [StringLength(50)]
        public string CatgoryDescription { get; set; }

        public bool? IsActive { get; set; }
    }
}
