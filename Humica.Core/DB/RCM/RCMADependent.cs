namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMADependent")]
    public partial class RCMADependent
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(20)]
        public string NationalId { get; set; }

        [StringLength(30)]
        public string DependentName { get; set; }

        [StringLength(15)]
        public string Relationship { get; set; }

        [StringLength(7)]
        public string Gender { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(15)]
        public string PhoneNo { get; set; }

        [StringLength(30)]
        public string Occupation { get; set; }
    }
}
