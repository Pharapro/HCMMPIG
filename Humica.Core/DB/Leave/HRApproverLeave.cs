namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApproverLeave")]
    public partial class HRApproverLeave
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string BranchID { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string Department { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(500)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}
