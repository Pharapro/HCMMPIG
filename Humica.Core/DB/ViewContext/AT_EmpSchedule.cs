namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AT_EmpSchedule
    {
        [Key]
        [Column(Order = 0)]
        public long TranNo { get; set; }

        [StringLength(146)]
        public string Subjects { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime TranDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AllDay { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string Color { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string ResourceID { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        [StringLength(10)]
        public string DisplayName { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Lable { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }
    }
}
