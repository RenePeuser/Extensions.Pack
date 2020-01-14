using System;

namespace Extensions
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan SubtractToZero(this TimeSpan timeSpan, TimeSpan subtrahend)
        {
            var result = timeSpan - subtrahend;
            return result <= TimeSpan.Zero ? TimeSpan.Zero : result;
        }

        public static TimeSpan Min(this TimeSpan source, TimeSpan target)
        {
            if (source == TimeSpan.Zero)
            {
                return target;
            }

            return source < target ? source : target;
        }

        public static TimeSpan Max(this TimeSpan source, TimeSpan target)
        {
            return source > target ? source : target;
        }
    }
}
