namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATLaEaPolicy")]
    public partial class ATLaEaPolicy
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string DedType { get; set; }

        public int OTFrom { get; set; }

        public int OTTo { get; set; }

        public decimal Rate { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(15)]
        public string RuleType { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
