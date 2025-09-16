namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_EmpClaimBen_View
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string BenefitType { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime TODate { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "date")]
        public DateTime PayDate { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal Amount { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
    }
}
