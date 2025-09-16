namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMIntVQuestionnaire")]
    public partial class RCMIntVQuestionnaire
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        public string Description { get; set; }

        public int? IntVStep { get; set; }

        [StringLength(20)]
        public string QType { get; set; }
    }
}
