namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressReturnItem")]
    public partial class HRDressReturnItem
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        public int Qty { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(50)]
        public string ReceivedItemID { get; set; }
    }
}
