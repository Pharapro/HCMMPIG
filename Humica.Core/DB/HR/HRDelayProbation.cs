namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDelayProbation")]
    public partial class HRDelayProbation
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime Probation { get; set; }

        public int Day { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndProbation { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string CretaedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
