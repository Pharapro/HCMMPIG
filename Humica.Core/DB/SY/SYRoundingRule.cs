namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SYRoundingRule
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(10)]
        public string RoundMethod { get; set; }

        public int RoundNumber { get; set; }

        public int RoundPlaces { get; set; }

        public bool IncludeIsDaily { get; set; }

        [Column(TypeName = "date")]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
