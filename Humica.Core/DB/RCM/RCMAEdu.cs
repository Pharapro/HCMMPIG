namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAEdu")]
    public partial class RCMAEdu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(150)]
        public string EduType { get; set; }

        [StringLength(50)]
        public string EduCenter { get; set; }

        [StringLength(70)]
        public string Major { get; set; }

        [StringLength(50)]
        public string Result { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
