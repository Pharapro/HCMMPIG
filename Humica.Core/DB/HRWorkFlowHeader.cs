namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRWorkFlowHeader")]
    public partial class HRWorkFlowHeader
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string BranchID { get; set; }
    }
}
