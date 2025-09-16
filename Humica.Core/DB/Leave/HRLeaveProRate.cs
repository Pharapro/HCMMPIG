namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRLeaveProRate")]
    public partial class HRLeaveProRate
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LinItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string LeaveType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 3)]
        public string EntitleType { get; set; }

        public int ActWorkDayFrom { get; set; }

        public int ActWorkDayTo { get; set; }

        public decimal Rate { get; set; }

    }
}
