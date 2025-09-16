namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRSetEntitleD")]
    public partial class HRSetEntitleD
    {
        [Key]
        public int TranNo { get; set; }

        public int LineItem { get; set; }

        [StringLength(10)]
        public string CodeH { get; set; }

        public int? FromMonth { get; set; }

        public int? ToMonth { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        public double Entitle { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public decimal SeniorityBalance { get; set; }

        public bool? IsProRate { get; set; }
    }
}
