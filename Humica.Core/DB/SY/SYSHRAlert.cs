namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYSHRAlert")]
    public partial class SYSHRAlert
    {
        [Key]
        public long TranNo { get; set; }

        public bool? BDCHK { get; set; }

        public int? BDValue { get; set; }

        public bool? CECHK { get; set; }

        public int? CEValue { get; set; }

        public bool? TMCHK { get; set; }

        public int? TMValue { get; set; }

        public bool? PPCHK { get; set; }

        public int? PPValue { get; set; }

        public bool? WBCHK { get; set; }

        public int? WBValue { get; set; }

        public bool? VISACHK { get; set; }

        public int? VISAValue { get; set; }

        public bool? PBCHK { get; set; }

        public int? PBValue { get; set; }

        public bool? RACHK { get; set; }

        public int? RAValue { get; set; }

        public bool? INSCHK { get; set; }

        public int? INSValue { get; set; }

        public bool? CHKPROBRPT { get; set; }

        public int? PROBRPT30 { get; set; }

        public int? PROBRPT60 { get; set; }

        public int? PROBRPT80 { get; set; }

        public int? PROBRPT90 { get; set; }

        public bool? OMFUCHK { get; set; }

        public int? OMFUValue { get; set; }

        public bool? DRIVERCHK { get; set; }

        public int? DRIVERValue { get; set; }

        public int? TONEFUValue { get; set; }

        public bool? TONEFUCHK { get; set; }

        public bool? MATCHK { get; set; }

        public int? MATValue { get; set; }

        public bool OpenShirtDateCHK { get; set; }

        public int OPENSHIRTDATEValue { get; set; }
    }
}
