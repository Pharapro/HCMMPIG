using System;

namespace Humica.Core
{
    public static class NumberExtension
    {
        public static int Value(this int? value)
        {
            return value ?? 0;
        }
        public static bool IsNullOrZero(this bool? value)
        {
            return (value ?? false) == false;
        }
        public static bool IsNullOrZero(this int? value)
        {
            return (value ?? 0) == 0;
        }
        public static decimal ToDecimal(this decimal? value)
        {
            return value ?? 0;
        }
        public static decimal Value(this decimal? value)
        {
            if (value.HasValue)
            {
                decimal _value = (decimal)value;
                value = Math.Round(_value, 7, MidpointRounding.AwayFromZero);
            }
            return value ?? 0;
        }
    }
}
