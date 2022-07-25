using System;

namespace Extensions.Pack
{
    internal static class RangeExtensions
    {
        internal static bool IsInRange(this Range range, IComparable value)
        {
            return value.CompareTo(range.Start.Value) >= 0 && value.CompareTo(range.End.Value) <= 0;
        }

        internal static bool IsNotInRange(this Range range, IComparable value)
        {
            return range.IsInRange(value).Negate();
        }
    }
}