namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYHRSetting")]
    public partial class SYHRSetting
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TelegLeave { get; set; }

        [StringLength(50)]
        public string TelegOT { get; set; }

        [StringLength(20)]
        public string DeductLate { get; set; }

        [StringLength(50)]
        public string NSSFSalaryType { get; set; }

        [StringLength(50)]
        public string DeductEalary { get; set; }

        public decimal? MaxContribute { get; set; }

        public decimal? MinContribue { get; set; }

        [StringLength(10)]
        public string Hr_manager { get; set; }
        public decimal? SeniorityException { get; set; }
        public bool? IsTax { get; set; }
        public string EmpType { get; set; }
        public string SeniorityType { get; set; }
        public decimal? ComRisk { get; set; }
        public decimal? StaffRisk { get; set; }
        public decimal? ComHealthCare { get; set; }
        public decimal? StaffHealthCare { get; set; }
        public string StampRejectImage { get; set; }
        public string MisScanAL { get; set; }
        public string MisScanUP { get; set; }
        public int CountMisscan { get; set; }
    }
}
