namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_POPending_view
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string CompanyCode { get; set; }
        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string Branch { get; set; }
        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string Warehouse { get; set; }
        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string Project { get; set; }

        public decimal? Amount { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(12)]
        public string DocType { get; set; }
    }
}
