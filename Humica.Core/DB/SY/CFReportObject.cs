namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CFReportObject")]
    public partial class CFReportObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ScreenID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        public string DocType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Company { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Version { get; set; }

        [StringLength(180)]
        public string ReportObject { get; set; }

        [StringLength(150)]
        public string PathStore { get; set; }

        [StringLength(250)]
        public string ObjectName { get; set; }

        public int RowLimit { get; set; }

        public bool IsActive { get; set; }
    }
}
