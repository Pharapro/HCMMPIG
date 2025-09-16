namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_EMPSVC_VIEW
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public DateTime? ResignDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Probation { get; set; }

        public double? Point { get; set; }

        public DateTime? SVC_StartDate { get; set; }

        public DateTime? SVC_END { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
