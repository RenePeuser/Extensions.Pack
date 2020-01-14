using System;

namespace Extensions
{
    /// <summary>The extension class for comparable types.</summary>
    public static class ComparableExtensions
    {
        /// <summary>Determines whether the source is equal to the specified target.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>True</c> if source is equal to the specified target; otherwise, <c>False</c>.</returns>
        public static bool IsEqualTo<T>(this T source, T target)
            where T : IComparable
        {
            Throw.IfNull<object>(() => source);
            Throw.IfNull<object>(() => target);

            return source.CompareTo(target) == 0;
        }

        /// <summary>Determines whether the source is less than the specified target.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>True</c> if source is less than the specified target; otherwise, <c>False</c>.</returns>
        public static bool IsLessThan<T>(this T source, T target)
            where T : IComparable
        {
            Throw.IfNull<object>(() => source);
            Throw.IfNull<object>(() => target);

            return source.CompareTo(target) < 0;
        }

        /// <summary>Determines whether the source is less than or equal to the specified target.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>True</c> if source is less than or equal to the specified target; otherwise, <c>False</c>.</returns>
        public static bool IsLessOrEqual<T>(this T source, T target)
            where T : IComparable
        {
            Throw.IfNull<object>(() => source);
            Throw.IfNull<object>(() => target);

            return !source.IsGreaterThan(target);
        }

        /// <summary>Determines whether the source is greater than the specified target.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>True</c> if source is greater than the specified target; otherwise, <c>False</c>.</returns>
        public static bool IsGreaterThan<T>(this T source, T target)
            where T : IComparable
        {
            Throw.IfNull<object>(() => source);
            Throw.IfNull<object>(() => target);

            return source.CompareTo(target) > 0;
        }

        /// <summary>Determines whether the source is greater than or equal to the specified target.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>True</c> if source is greater than or equal to the specified target; otherwise, <c>False</c>.</returns>
        public static bool IsGreaterOrEqual<T>(this T source, T target)
            where T : IComparable
        {
            Throw.IfNull<object>(() => source);
            Throw.IfNull<object>(() => target);

            return !source.IsLessThan(target);
        }

        /// <summary>Determines whether the source is in range of the lower and upper limit including the interval corners.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        /// <returns><c>True</c> if the source is in range of the lower and upper limit including the interval corners; otherwise, <c>False</c>.</returns>
        public static bool IsInRange<T>(this T source, T lowerLimit, T upperLimit)
            where T : IComparable
        {
            Throw.IfLessThan(() => upperLimit, lowerLimit);

            return source.IsLessOrEqual(upperLimit) && source.IsGreaterOrEqual(lowerLimit);
        }

        /// <summary>Determines whether the source is in range of the lower and upper limit excluding the interval corners.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        /// <returns><c>True</c> if the source is in range of the lower and upper limit excluding the interval corners; otherwise, <c>False</c>.</returns>
        public static bool IsInRangeExcluding<T>(this T source, T lowerLimit, T upperLimit)
            where T : IComparable
        {
            Throw.IfLessOrEqual(() => upperLimit, lowerLimit);

            return source.IsLessThan(upperLimit) && source.IsGreaterThan(lowerLimit);
        }

        /// <summary>Determines whether the source is out of range of the lower and upper limit.</summary>
        /// <typeparam name="T">The generic comparable type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        /// <returns><c>True</c> if the source is out of range of the lower and upper limit including the interval corners; otherwise, <c>False</c>.</returns>
        public static bool IsOutOfRange<T>(this T source, T lowerLimit, T upperLimit)
            where T : IComparable
        {
            Throw.IfLessThan(() => upperLimit, lowerLimit);

            return !source.IsInRange(lowerLimit, upperLimit);
        }
    }
}
