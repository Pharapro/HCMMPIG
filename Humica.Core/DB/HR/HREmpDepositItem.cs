namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpDepositItem")]
    public partial class HREmpDepositItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string DepositNum { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime InMonth { get; set; }

        [Column(TypeName = "date")]
        public DateTime PayMonth { get; set; }

        public decimal Deposit { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
