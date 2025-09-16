namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAReference")]
    public partial class RCMAReference
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(30)]
        public string RefName { get; set; }

        [StringLength(30)]
        public string Occupation { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(15)]
        public string Phone1 { get; set; }

        [StringLength(15)]
        public string Phone2 { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
    }
}
