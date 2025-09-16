using System;
using System.Collections.Generic;

namespace Humica.Core
{
    public static class DateTimeHelper
    {
        public static DateTime? SetTimeInDate(this DateTime? date)
        {
            if (date == null) return null;
            DateTime _dateNow = DateTime.Now;
            return _dateNow.Date + date.Value.TimeOfDay;
        }
        public static DateTime SetTimeInDate(this DateTime date, DateTime? Time)
        {
            return date + Time.Value.TimeOfDay;
        }
        public static DateTime StartDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        public static DateTime EndDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
        public static DateTime Value(this DateTime? value)
        {
            return (value.HasValue ? value.Value : DateTimeHelper.MaxValue);
        }
        public static DateTime ValueMinDate(this DateTime? value)
        {
            return (value.HasValue ? value.Value : DateTimeHelper.MinValue);
        }
        public static DateTime MinValue
        {
            get { return new DateTime(1900, 01, 01); }
        }
        public static DateTime MaxValue
        {
            get { return new DateTime(5000, 12, 31); }
        }
        public static DateTime Time(this DateTime? value)
        {
            DateTime dateTime = MinValue;
            if (value.HasValue)
            {
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, value.Value.Hour, value.Value.Minute, value.Value.Second);
            }
            else
            {
                return dateTime;
            }
        }
        public static DateTime DateInMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
        public static DateTime DateInHourMin(DateTime dateTime)
        {
            dateTime = dateTime.AddMilliseconds(-dateTime.Millisecond).AddSeconds(-dateTime.Second);
            return dateTime;
        }
        public static int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        public static DateTime? ObjectToDateTime(object obj)
        {
            DateTime? result = null;
            if (obj != null)
            {
                try
                {
                    result = DateTime.Parse(obj.ToString());

                    // Date range:	January 1, 1753, through December 31, 9999
                    if (result.Value.Year < 1753)
                        result = null;
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }
        public static int CountMonth(DateTime startDate, DateTime endDate)
        {
            int monthCount = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
            return monthCount + 1;
        }
        public static int GetMonth(DateTime startDate, DateTime endDate)
        {
            int monthCount = 0;
            for (DateTime date = startDate.StartDateOfMonth(); date <= endDate; date = date.AddMonths(1))
            {
                monthCount += 1;
            }
            return monthCount;
        }
        public static int GetDay(DateTime startDate, DateTime endDate)
        {
            int monthCount = 0;
            monthCount = (endDate - startDate).Days;
            return monthCount;
        }
        public static void TimeSpanToDate(DateTime d1, DateTime d2, out int years, out int months, out int days)
        {
            if (d1 < d2)
            {
                DateTime d3 = d2;
                d2 = d1;
                d1 = d3;
            }

            months = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month);

            if (d1.Day < d2.Day)
            {
                months--;
                days = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day;
            }
            else
            {
                days = d1.Day - d2.Day;
            }
            years = months / 12;
            months -= years * 12;
        }
        public static void GetWeekStartEnd(ref DateTime Start, ref DateTime End)
        {
            DateTime today = DateTime.Now;
            int diff = today.DayOfWeek - DayOfWeek.Monday;
            Start = today.AddDays(-diff).Date;
            End = Start.AddDays(6).Date;
        }
        public static (DateTime Start, DateTime End) GetWeekStartEnd()
        {
            DateTime today = DateTime.Now;
            int diff = today.DayOfWeek - DayOfWeek.Monday;
            DateTime start = today.AddDays(-diff).Date;
            DateTime end = start.AddDays(6).Date;
            return (start, end);
        }
        public static double GetServiceLength(DateTime FromDate, DateTime toDate, ServiceLengthType type)
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
    public struct DateTimeSpan
    {
        public int Years { get; }
        public int Months { get; }
        public int Days { get; }
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }
        public int Milliseconds { get; }

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        enum Phase { Years, Months, Days, Done }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }
    }
}