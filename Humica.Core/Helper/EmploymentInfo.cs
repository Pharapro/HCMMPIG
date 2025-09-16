using System;
namespace Humica.Core
{
    public class EmploymentInfo
    {
        public double GetEmploymentServiceLength(DateTime FromDate, DateTime toDate, ServiceLengthType type)
        {
            double days = (toDate - FromDate).TotalDays;
            switch (type)
            {
                case ServiceLengthType.Day: return days;
                case ServiceLengthType.Year: return days / 365;
                case ServiceLengthType.Month:
                    {
                        var dateTimespan = DateTimeSpan.CompareDates(FromDate, toDate);
                        return (dateTimespan.Years * 12) + dateTimespan.Months;
                    }
                default: return 0;
            }
        }

    }
    public enum ServiceLengthType
    {
        Year,
        Day,
        Month
    }

}