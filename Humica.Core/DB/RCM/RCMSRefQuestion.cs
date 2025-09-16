namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMSRefQuestion")]
    public partial class RCMSRefQuestion
    {
        public int ID { get; set; }

        public int step { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecDescription { get; set; }
    }
}
