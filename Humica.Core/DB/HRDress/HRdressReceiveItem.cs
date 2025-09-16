namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRdressReceiveItem")]
    public partial class HRDressReceiveItem
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string DocNO { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string ItemCode { get; set; }

        public int QTY { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        public int? ReturnItem { get; set; }

        [StringLength(15)]
        public string StatusTransfer { get; set; }

        public int? TransferItem { get; set; }

        [StringLength(15)]
        public string StatusDeduct { get; set; }

        public int? DeductItem { get; set; }
    }
}
