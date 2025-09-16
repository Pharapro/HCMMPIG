namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BiMonthlySalarySetting")]
    public partial class BiMonthlySalarySetting
    {

        [Key]
        [StringLength(10)]
        public string PayrollParameterID { get; set; }

        public int? IsAccrualTax { get; set; }

        public int? IsCalOvertime { get; set; }

        public int? IsCalNightWork { get; set; }

        public int? IsCalAllowan { get; set; }
        public int? IsCalBounus { get; set; }
        public int? IsCalDeduction { get; set; }

        public int? IsCalLeaveDed { get; set; }

        public int? FirstStartDay { get; set; }

        public int? FirstEndDay { get; set; }

        public int? DateJoinFromDay { get; set; }

        public int? DateJoinToDay { get; set; }

        public int? DateResignFromDay { get; set; }

        public int? DateResignToDay { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string CreatedUser { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedUser { get; set; }

        public bool? IsPreviousMonth { get; set; }
    }
}
