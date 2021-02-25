using System;
using System.Linq;

namespace Extensions.Pack
{
    /// <summary>The extension class for arrays.</summary>
    public static class ArrayExtensions
    {
        /// <summary>Checks an array if it contains any of the given values.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">Array of source values.</param>
        /// <param name="values">The values to check.</param>
        /// <returns><c>True</c> if the source includes at least one of the given values; otherwise, <c>False</c>.</returns>
        public static bool HasAny<T>(this T[] source, params object[] values)
        {
            Throw.IfNull(() => source);

            var result = source.Any(item => item.EqualsTo<object>(values) || values.Any(value => item.EqualsTo(value)));

            return result;
        }

        /// <summary>Gets a non-null array.</summary>
        /// <typeparam name="T">Type of the array items.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>The array of original items, or empty array if original array was null.</returns>
        public static T[] GetNonNullArray<T>(this T[] array)
        {
            return array ?? Array.Empty<T>();
        }
    }
}
