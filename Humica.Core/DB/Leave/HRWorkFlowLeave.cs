namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRWorkFlowLeave")]
    public partial class HRWorkFlowLeave
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int Step { get; set; }

        [StringLength(250)]
        public string AssignBy { get; set; }

        [StringLength(250)]
        public string ApproveBy { get; set; }

        public decimal? Qty { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
