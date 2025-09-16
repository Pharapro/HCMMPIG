namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_NSSF_Transfer
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string SOCSO { get; set; }

        [StringLength(20)]
        public string IDCard { get; set; }

        [StringLength(140)]
        public string OthAllName { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(100)]
        public string NationDesc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [StringLength(100)]
        public string DeptDesc { get; set; }

        [StringLength(70)]
        public string OthFirstName { get; set; }

        [StringLength(70)]
        public string OthLastName { get; set; }

        [StringLength(70)]
        public string FirstName { get; set; }

        [StringLength(70)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string PostDesc { get; set; }

        public decimal? NSSFGROSS { get; set; }

        public decimal? NSSFGROSSUSD { get; set; }

        [StringLength(500)]
        public string NATIONID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal AVGGrSOSC { get; set; }

        public decimal? SOSEC { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }
    }
}
