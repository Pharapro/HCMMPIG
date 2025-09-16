namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRBankAccount")]
    public partial class HRBankAccount
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Bank { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string AccountNo { get; set; }

        [StringLength(250)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string ReferenceCode { get; set; }
    }
}
