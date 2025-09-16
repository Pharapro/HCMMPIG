namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRHeadCount")]
    public partial class HRHeadCount
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? EmpNo { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
