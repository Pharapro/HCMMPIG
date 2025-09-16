namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpUnderManager")]
    public partial class HREmpUnderManager
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DocumentType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EmpCode { get; set; }
        [StringLength(250)]
        public string EmployeeName { get; set; }
        public string Position { get; set; }

       [StringLength(15)]
        public string ChangedTo { get; set; }
    }
}
