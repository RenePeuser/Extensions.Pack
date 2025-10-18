namespace Extensions.Pack
{
    public static class DecimalExtensions
    {
        public static decimal CalcPercent(this decimal source,
                                          decimal divisor)
        {
            if (divisor.Equals(0))
            {
                return 0;
            }

            var percent = divisor / source * 100;

            return Math.Round(percent, 0);
        }

        public static bool IsMinValue(this decimal value)
        {
            return value == decimal.MinValue;
        }

        public static bool IsMaxValue(this decimal value)
        {
            return value == decimal.MaxValue;
        }

        public static int ToInt(this decimal value)
        {
            return value.Cast<int>();
        }

        public static double ToDouble(this decimal value)
        {
            return value.Cast<double>();
        }
    }
}
