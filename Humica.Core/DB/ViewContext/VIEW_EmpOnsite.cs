namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VIEW_EmpOnsite
    {
        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(15)]
        public string CompanyCode { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }
        
        [StringLength(10)]
        public string GroupDept { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string JobCode { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }
        
        [StringLength(10)]
        public string SECT { get; set; }

        [StringLength(10)]
        public string Office { get; set; }

        [StringLength(10)]
        public string Groups { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public DateTime? ScanDate { get; set; }

        [StringLength(200)]
        public string Longitude { get; set; }

        [StringLength(200)]
        public string latitude { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        public string MapURL { get; set; }
    }
}
