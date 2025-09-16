namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExCFWFDepartmentApprover")]
    public partial class ExCFWFDepartmentApprover
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string WFObject { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Department { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string Employee { get; set; }

        [StringLength(150)]
        public string EmployeeName { get; set; }

        public bool? IsSkip { get; set; }

        public int ApproveLevel { get; set; }

        public bool IsSelected { get; set; }

        [StringLength(50)]
        public string InternalStatus { get; set; }
    }
}
