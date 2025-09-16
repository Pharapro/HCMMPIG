namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRTransferToMgrItem")]
    public partial class PRTransferToMgrItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Dept { get; set; }

        [StringLength(50)]
        public string JobCode { get; set; }

        [StringLength(50)]
        public string HOD { get; set; }
    }
}
