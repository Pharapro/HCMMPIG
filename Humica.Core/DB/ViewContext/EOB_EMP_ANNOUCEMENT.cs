namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EOB_EMP_ANNOUCEMENT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(5)]
        public string Title { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string gen { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string gen1 { get; set; }

        [StringLength(100)]
        public string post { get; set; }

        [StringLength(100)]
        public string dept { get; set; }
    }
}
