namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class VIEW_ATInOut
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string CardNo { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime FullDate { get; set; }

        [StringLength(10)]
        public string BRANCH { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }

        [StringLength(10)]
        public string JOBCODE { get; set; }
        public string Office { get; set; }
        public string Groups { get; set; }

        public int? STATUS { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public string ChangedBy { get; set; }
      
    }
}
