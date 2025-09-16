namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMSAdvertise")]
    public partial class RCMSAdvertise
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(30)]
        public string Contact { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone1 { get; set; }

        [StringLength(20)]
        public string Phone2 { get; set; }

        [StringLength(250)]
        public string TokenCode { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string AppID { get; set; }

        [StringLength(100)]
        public string AppName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(15)]
        public string AdsType { get; set; }
    }
}
