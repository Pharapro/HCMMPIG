using System;

namespace Humica.Core.Helper
{
    public class ClsRounding
    {
        public static decimal Rounding(decimal number, int places, string roundType)
        {
            if (roundType == RoundType.U.ToString())
            {
                number = RoundUp(number, places);
            }
            else if (roundType == RoundType.N.ToString())
            {
                number = RoundNormal(number, places);
            }
            else if (roundType == RoundType.L.ToString())
            {
                number = RoundDown(number, places);
            }
            return number;
        }
        public static decimal RoundUp(decimal number, int places)
        {
            decimal factor = RoundFactor(places);
            number *= factor;
            number = Math.Ceiling(number);
            number /= factor;
            return number;
        }
        public static decimal RoundNormal(decimal number, int places)
        {
            number = Math.Round(number, 7, MidpointRounding.AwayFromZero);
            number = Math.Round(number, places, MidpointRounding.AwayFromZero);
            return number;
        }
        public static decimal RoundDown(decimal number, int places)
        {
            decimal factor = RoundFactor(places);
            number *= factor;
            number = Math.Floor(number);
            number /= factor;
            return number;
        }
        internal static decimal RoundFactor(int places)
        {
            decimal factor = 1m;

            if (places < 0)
            {
                places = -places;
                for (int i = 0; i < places; i++)
                    factor /= 10m;
            }

            else
            {
                for (int i = 0; i < places; i++)
                    factor *= 10m;
            }

            return factor;
        }
        public static int Rounding_UP_KH(decimal? Salary)
        {
            int _result = 0;
            if (Salary == 0) return 0;
            int _netPay = Convert.ToInt32(Salary);
            if (_netPay.ToString().Length < 2) return _result;
            int Values = Convert.ToInt32(_netPay.ToString().Substring(_netPay.ToString().Length - 2, 2));
            int _rounding = 100;
            if (Values >= 50)
            {
                int result = _rounding - Values;
                _result = _netPay + result;
            }
            else
            {
                _result = _netPay - Values;
            }
            return _result;
        }
    }
    public enum RoundType
    {
        U, N, L
    }
}
