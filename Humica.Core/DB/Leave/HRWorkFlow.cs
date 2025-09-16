namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRWorkFlow")]
    public partial class HRWorkFlow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Line { get; set; }

        [StringLength(10)]
        public string BranchID { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string Department { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string Sign { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Qty1 { get; set; }
    }
}
