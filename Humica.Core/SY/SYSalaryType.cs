namespace Humica.Core.SY
{
    public static partial class SYSalaryType
    {
        public const string Yearly = "Y";

        public const string Monthly = "M";

        public const string Weekly = "W";

        public const string Daily = "D";

        public const string Hourly = "H";

        public const string Full = "FL";

        public const string Half = "HF";

    }
    public enum PaymentType
    {
        Monthly,
        Hourly,
        Daily
    }
}