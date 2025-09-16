namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class RPT_AttScheduleSummary
    {
        public long? ID { get; set; }

        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime TranDate { get; set; }

        [StringLength(58)]
        public string Subjects { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AllDay { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Color { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string ResourceID { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Lable { get; set; }
    }
}
