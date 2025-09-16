namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBTypiDurType")]
    public partial class EOBTypiDurType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Duration { get; set; }
    }
}
