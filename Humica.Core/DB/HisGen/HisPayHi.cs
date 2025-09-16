namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("HisPayHis")]
    public partial class HisPayHi
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string SGROUP { get; set; }

        [StringLength(20)]
        public string PayType { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public decimal JAN { get; set; }

        public decimal FEB { get; set; }

        public decimal MAR { get; set; }

        public decimal APR { get; set; }

        public decimal MAY { get; set; }

        public decimal JUN { get; set; }

        public decimal JUL { get; set; }

        public decimal AUG { get; set; }

        public decimal SEP { get; set; }

        public decimal OCT { get; set; }

        public decimal NOV { get; set; }

        public decimal DECE { get; set; }
    }
}
