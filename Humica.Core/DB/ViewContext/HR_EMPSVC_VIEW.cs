namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_EMPSVC_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public DateTime? SVCStartDate { get; set; }

        public DateTime? SVCEndDate { get; set; }

        public decimal? Day { get; set; }

        public decimal? NoDay { get; set; }

        public decimal? Deduct { get; set; }

        public decimal? Adding { get; set; }

        public decimal? Amount { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }
    }
}
