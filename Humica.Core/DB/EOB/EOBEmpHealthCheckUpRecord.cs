namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBEmpHealthCheckUpRecord")]
    public partial class EOBEmpHealthCheckUpRecord
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string MedicalCheck { get; set; }

        [StringLength(70)]
        public string Result { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
