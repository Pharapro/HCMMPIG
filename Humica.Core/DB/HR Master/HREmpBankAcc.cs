namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpBankAcc")]
    public partial class HREmpBankAcc
    {
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineItem { get; set; }
        [StringLength(150)]
        public string Company { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [StringLength(50)]
        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public decimal Salary { get; set; }

        public bool? IsTax { get; set; }

        public bool? IsNSSF { get; set; }

        public bool? Association { get; set; }

        public bool? IsActive { get; set; }

        public long? Reference { get; set; }
    }
}
