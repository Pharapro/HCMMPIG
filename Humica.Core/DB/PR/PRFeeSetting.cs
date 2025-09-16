namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRFeeSetting")]
    public partial class PRFeeSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranNo { get; set; }

        [Key]
        [StringLength(50)]
        public string Levels { get; set; }

        public decimal FeeFrom { get; set; }

        public decimal FeeTo { get; set; }

        public decimal Rate { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreatOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
