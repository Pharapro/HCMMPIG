namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TokenResource")]
    public partial class TokenResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MUserID { get; set; }

        public string DeviceID { get; set; }

        public string Model { get; set; }

        public string FirebaseID { get; set; }

        public string Token { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiredDate { get; set; }

        public bool IsLock { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LoginDate { get; set; }
    }
}
