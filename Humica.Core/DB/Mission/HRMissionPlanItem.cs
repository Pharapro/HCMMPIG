namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissionPlanItem")]
    public partial class HRMissionPlanItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string MissionCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string Code { get; set; }

        public decimal Amount { get; set; }

        [StringLength(10)]
        public string Remark { get; set; }
    }
}
