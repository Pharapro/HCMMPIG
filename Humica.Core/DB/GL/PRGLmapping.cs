namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRGLmapping")]
    public partial class PRGLmapping
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string BenGrp { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string BenCode { get; set; }
        [StringLength(20)]
        public string Branch { get; set; }
        [StringLength(20)]
        public string Warehouse { get; set; }
        [StringLength(20)]
        public string Project { get; set; }

        [StringLength(10)]
        public string GLCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string MaterialCode { get; set; }
        [StringLength(200)]
        public string BenefitGroup { get; set; }

    }
}
