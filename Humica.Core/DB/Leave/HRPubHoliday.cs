namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPubHoliday")]
    public partial class HRPubHoliday
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime PDate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
