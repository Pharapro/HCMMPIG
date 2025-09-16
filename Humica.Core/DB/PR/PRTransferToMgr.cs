namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRTransferToMgr")]
    public partial class PRTransferToMgr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int Increment { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MgrCode { get; set; }

        [StringLength(140)]
        public string MgrName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DEPT { get; set; }

        [StringLength(50)]
        public string POST { get; set; }

        [StringLength(10)]
        public string BankName { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        [StringLength(40)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(40)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
