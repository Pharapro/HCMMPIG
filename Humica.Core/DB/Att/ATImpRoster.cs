namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATImpRoster")]
    public partial class ATImpRoster
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string DocumentNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        public long InYear { get; set; }

        public int InMonth { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InDate { get; set; }

        [StringLength(10)]
        public string Shift { get; set; }
    }
}
