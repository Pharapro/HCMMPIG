namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HROrgChart")]
    public partial class HROrgChart
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Designation { get; set; }

        [StringLength(50)]
        public string ReportingManager { get; set; }
    }
}
