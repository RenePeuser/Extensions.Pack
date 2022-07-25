using System;

namespace Extensions.Pack
{
    /// <summary>Represents extensions for types of <see cref="double" />.</summary>
    public static class DoubleExtensions
    {
        /// <summary>Checks double for not a number.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if <c>double</c>; is not a number; otherwise <c>false</c>.</returns>
        public static bool IsNan(this double value)
        {
            return double.IsNaN(value);
        }

        /// <summary>Checks double if not not a number.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if <c>double</c>; is a number; otherwise <c>false</c>.</returns>
        public static bool IsNotNan(this double value)
        {
            return !value.IsNan();
        }

        /// <summary>Converts a nullable <see cref="double" /> to double.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if <c>double</c>; is a number; otherwise <c>false</c>.</returns>
        public static double ToDouble(this double? value)
        {
            return value ?? default;
        }

        /// <summary>Converts a double <see cref="double" /> to value or default. If double is not a number the return default(double) otherwise its value.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if <c>double</c>; is a number; otherwise <c>false</c>.</returns>
        public static double ToValueOrDefault(this double value)
        {
            return value.IsNan() ? default : value;
        }

        /// <summary>Determines whether this instance is zero.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the specified source is zero; otherwise, <c>false</c>.</returns>
        public static bool IsZero(this double source)
        {
            var result = source.EqualsTo(default);

            return result;
        }

        /// <summary>To the decimal.</summary>
        /// <param name="source">The source.</param>
        /// <returns>The converted decimal.</returns>
        public static decimal ToDecimal(this double source)
        {
            return new decimal(source);
        }

        /// <summary>Ceilings the specified value.</summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public static int Ceiling(this double value)
        {
            return (int)Math.Ceiling(value);
        }

        /// <summary>Compares double values with default tolerance but excludes NaN from comparison with tolerance.</summary>
        /// <param name="source">The source for the comparison.</param>
        /// <param name="target">The target for the comparison.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise <c>false</c>.</returns>
        public static bool DoubleNotEqualsToExcludingNan(this double source, double target)
        {
            bool result;

            if (source.IsNotNan() && target.IsNotNan())
            {
                // Compare double values with tolerance (only applicable, if no value is NaN).
                result = source.NotEqualsTo(target);
            }
            else
            {
                // Compare double values without tolerance (at least one of the values is NaN).
                result = !source.Equals(target);
            }

            return result;
        }
    }
}
