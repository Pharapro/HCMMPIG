namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExCfWorkFlowItem")]
    public partial class ExCfWorkFlowItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ScreenID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string DocType { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(30)]
        public string NumberRank { get; set; }

        [StringLength(2)]
        public string NumberRankItem { get; set; }

        [StringLength(20)]
        public string ApprovalFlow { get; set; }

        public bool? IsCompleteVendorSubmit { get; set; }

        [StringLength(20)]
        public string DefaultPOType { get; set; }

        public bool? IsAbleToForceClose { get; set; }

        public bool? IsRequireBidding { get; set; }

        public bool? PRCanOverQtyRequest { get; set; }

        public bool? IsAllowMultiProperty { get; set; }

        public bool? IsRequiredApproval { get; set; }

        public bool? IsCancelWholeQuotation { get; set; }

        [StringLength(50)]
        public string Telegram { get; set; }

        public string Email { get; set; }
    }
}
