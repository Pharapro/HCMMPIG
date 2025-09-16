namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_InetAccount_view
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string Branch { get; set; }

        public decimal? Amount { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }
    }
}
