using System.Text.RegularExpressions;

namespace Humica.Core.Helper
{
    public static class StringHelper
    {
        public static string ExtractNumberFromString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            // Use a regular expression to find all numeric characters in the string
            string numericPart = Regex.Match(input, @"\d+").Value;

            // Convert the numeric part to an integer
            if (int.TryParse(numericPart, out int result))
            {
                return result.ToString();
            }
            else
            {
                return "";
            }
        }
        public static string getLenOfPrefix(int len, int currentNumber)
        {
            string r = "";
            for (int i = currentNumber.ToString().Length; i < len; i++)
            {
                r = r + "0";
            }
            r = r + currentNumber.ToString();
            return r;
        }
    }
}
