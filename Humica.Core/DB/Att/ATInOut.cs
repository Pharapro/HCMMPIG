namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATInOut")]
    public partial class ATInOut
    {
        public long ID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string CardNo { get; set; }

        public int? STATUS { get; set; }

        public DateTime FullDate { get; set; }

        public int? LCK { get; set; }

        public int? FLAG { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
    }
}
