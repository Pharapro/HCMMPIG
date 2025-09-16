namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAppGrade")]
    public partial class HRAppGrade
    {
        [Key]
        public int TranNo { get; set; }

        public decimal FromScore { get; set; }

        public decimal ToScore { get; set; }

        [StringLength(100)]
        public string Grade { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public decimal Rating { get; set; }
    }
}
