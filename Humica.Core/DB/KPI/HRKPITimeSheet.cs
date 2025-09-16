namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPITimeSheet")]
    public partial class HRKPITimeSheet
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        //[StringLength(250)]
        public string Description { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public decimal? Hours { get; set; }

        //[StringLength(250)]
        public string Remark { get; set; }
        public string Attachment { get; set; }
        [StringLength(100)]
        public string TotalHours { get; set; }
    }
}
