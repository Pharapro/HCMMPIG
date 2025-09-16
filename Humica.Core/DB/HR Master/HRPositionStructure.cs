namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPositionStructure")]
    public partial class HRPositionStructure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        
        public string Description { get; set; }

        [StringLength(50)]
        public string ReportingID { get; set; }
        public bool? BaseHOD { get; set; }
    }
}
