namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMVInterviewer")]
    public partial class RCMVInterviewer
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(30)]
        public string EmpName { get; set; }

        [StringLength(20)]
        public string Position { get; set; }

        public int IntStep { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
