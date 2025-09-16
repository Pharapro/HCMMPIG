namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisInsurance")]
    public partial class HisInsurance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int TranNo { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string InsurType { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string InsurComp { get; set; }

        [StringLength(150)]
        public string InsurDescription { get; set; }

        public decimal WorkingDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Period { get; set; }
    }
}
