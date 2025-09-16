namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisCostCharge")]
    public partial class HisCostCharge
    {
        [StringLength(20)]
        public string CodeCCGoup { get; set; }

        [StringLength(250)]
        public string GroupDescription { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string CostCenter { get; set; }

        public decimal Charge { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(15)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
