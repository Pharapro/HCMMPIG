namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExDocApproval")]
    public partial class ExDocApproval
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string DocumentNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string DocumentType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Approver { get; set; }
        [Key]
        [Column(Order = 3)]
        public int ApproveLevel { get; set; }

        [StringLength(150)]
        public string ApproverName { get; set; }

        [StringLength(20)]
        public string WFObject { get; set; }

        [StringLength(20)]
        public string ApprovedBy { get; set; }

        [StringLength(150)]
        public string ApprovedName { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(30)]
        public string WorkGroup { get; set; }

        public string Comment { get; set; }

        public DateTime? LastChangedDate { get; set; }
        [StringLength(50)]
        public string InternalStatus { get; set; }
    }
}
