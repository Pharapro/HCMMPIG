namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_PR_EmpSalary
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(70)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string TelegramID { get; set; }

        [StringLength(10)]
        public string TerminateStatus { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        [StringLength(120)]
        public string SMSTele { get; set; }
    }
}
