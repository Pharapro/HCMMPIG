namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_Family_View
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(100)]
        public string RelationShip { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(75)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string IDCard { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        public bool? TaxDeduc { get; set; }

        public bool? InSchool { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }
		public DateTime EffectiveDate { get; set; }
		public DateTime? ToDate { get; set; }
	}
}
