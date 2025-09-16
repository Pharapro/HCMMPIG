namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_BooingRoom_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string BookingNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(25)]
        public string RoomID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string RoomType { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime BookingDate { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime StartTime { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime EndTime { get; set; }
    }
}
