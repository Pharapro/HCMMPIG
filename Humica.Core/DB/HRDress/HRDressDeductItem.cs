namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressDeductItem")]
    public partial class HRDressDeductItem
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        public int Qty { get; set; }

        public decimal Amount { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public int? ReceivedItemID { get; set; }
    }
}
