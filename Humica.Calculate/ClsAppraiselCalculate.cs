using Humica.Core.Helper;

namespace Humica.Calculate
{
    public class ClsAppraiselCalculate
    {
        public decimal Target { get; set; }
        public decimal Actual { get; set; }
        public decimal Weight { get; set; }
        public decimal Achievement { get; set; }
        public decimal CalculateScore(string Symbol)
        {
            Achievement = 0;
            decimal Score = 0;
            if (Actual == 0)
            {
                if (Symbol == "<") Achievement = 1;
                else if (Symbol == "|<1|") Achievement = 1;
            }
            else
            {
                if (Symbol == "<")
                {
                    Achievement = ClsRounding.Rounding(((Target - Actual) / Target), 4, "N");
                    if (Achievement < 0) Achievement = 0;
                }
                else if (Symbol == "|<1|") Achievement = 0;
                else
                {
                    Achievement = ClsRounding.Rounding((Actual / Target), 4, "N");
                }
            }
            Score = ClsRounding.Rounding(Achievement * Weight * 100, 2, "N");
            if (Weight * 100 <= Score)
            {
                Score = Weight * 100;
            }
            return Score;
        }
    }
}
