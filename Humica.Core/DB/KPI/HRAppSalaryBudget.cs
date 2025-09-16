namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAppSalaryBudget")]
    public partial class HRAppSalaryBudget
    {
        [Key]
        public int ID { get; set; }

        public int InYear { get; set; }

        public decimal Budget { get; set; }

    }
}
