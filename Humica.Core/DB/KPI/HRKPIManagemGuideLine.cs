namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIManagemGuideLine")]
    public partial class HRKPIManagemGuideLine
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AVTCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string KPICode { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(50)]
        public string MMSAssignTo { get; set; }

        [StringLength(50)]
        public string KPIElement { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(250)]
        public string Attach { get; set; }
    }
}
