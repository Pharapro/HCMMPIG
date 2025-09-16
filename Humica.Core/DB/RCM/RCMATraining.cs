namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMATraining")]
    public partial class RCMATraining
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(50)]
        public string TrainingTopic { get; set; }

        [StringLength(50)]
        public string TrainingProvider { get; set; }

        [StringLength(50)]
        public string TrainingPlace { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }
    }
}
