using System;
using System.Collections.Generic;

namespace Humica.Logic
{
    public class AttendanceRaw : IEqualityComparer<AttendanceRaw>
    {
        public string EnrollNumber { get; set; }
        public DateTime ScanDate { get; set; }
        public string Branch { get; set; }

        private Func<AttendanceRaw, object> _funcDistinct;
        public AttendanceRaw() { }
        public AttendanceRaw(Func<AttendanceRaw, object> funcDistinct)
        {
            this._funcDistinct = funcDistinct;
        }
        public bool Equals(AttendanceRaw x, AttendanceRaw y)
        {
            return _funcDistinct(x).Equals(_funcDistinct(y));
        }

        public int GetHashCode(AttendanceRaw obj)
        {
            return this._funcDistinct(obj).GetHashCode();
        }
    }
}
