namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("HRNotifications")]
    public partial class HRNotification
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string AlertType { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(150)]
        public string Department { get; set; }

        [StringLength(150)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
    }
}
