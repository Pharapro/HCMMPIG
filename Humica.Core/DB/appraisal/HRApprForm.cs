namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApprForm")]
    public partial class HRApprForm
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ApprNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string EmpCode { get; set; }

        public DateTime? APPRFROM { get; set; }

        public DateTime? APPRTO { get; set; }

        [StringLength(10)]
        public string APPRTYPE { get; set; }

        public DateTime? EVALUATEDATE { get; set; }

        [StringLength(100)]
        public string PGRADE { get; set; }

        [StringLength(500)]
        public string DISCUSNOTE { get; set; }

        [StringLength(10)]
        public string NEWPOST { get; set; }

        public double? OLDSALARY { get; set; }

        public double? NEWSALARY { get; set; }

        public DateTime? EFFECTDATE { get; set; }

        [Column(TypeName = "image")]
        public byte[] HRSIGN { get; set; }

        [StringLength(10)]
        public string HRSIGNCODE { get; set; }

        public DateTime? HRSIGNDATE { get; set; }

        [Column(TypeName = "image")]
        public byte[] DDSIGN { get; set; }

        [StringLength(10)]
        public string DDSIGNCODE { get; set; }

        public DateTime? DDSIGNDATE { get; set; }

        [Column(TypeName = "image")]
        public byte[] DRSIGN { get; set; }

        [StringLength(10)]
        public string DRSIGNCODE { get; set; }

        public DateTime? DRSIGNDATE { get; set; }

        [StringLength(10)]
        public string APPRAISEE { get; set; }

        [StringLength(500)]
        public string APPRAISEECOMM { get; set; }

        public DateTime? APPRAISEEDATE { get; set; }

        [StringLength(10)]
        public string APPRAISER { get; set; }

        [StringLength(500)]
        public string APPRAISERCOMM { get; set; }

        public DateTime? APPRAISERDATE { get; set; }

        [StringLength(10)]
        public string HOD { get; set; }

        [StringLength(500)]
        public string HODCOMM { get; set; }

        public DateTime? HODDATE { get; set; }

        [StringLength(10)]
        public string DDDR { get; set; }

        [StringLength(500)]
        public string DDDRCOMM { get; set; }

        public DateTime? DDDRDATE { get; set; }

        [Column(TypeName = "text")]
        public string IMPLEMENT { get; set; }

        [Column(TypeName = "text")]
        public string COOPERATION { get; set; }

        [Column(TypeName = "text")]
        public string BEHAVIOR { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? DATEEDIT { get; set; }

        [StringLength(100)]
        public string PGRADEOTH { get; set; }
    }
}
