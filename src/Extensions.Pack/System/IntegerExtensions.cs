using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Represents the extensions for the <see cref="int" />.</summary>
    public static class IntegerExtensions
    {
        /// <summary>Converts a nullable integer value into a value of <see cref="int" /> type.</summary>
        /// <param name="value">The value of the nullable <see cref="int" />.</param>
        /// <returns>The converted <see cref="int" /> value if the type has a value; otherwise default of <see cref="int" />.</returns>
        public static int ToInt(this int? value)
        {
            return value ?? default;
        }

        /// <summary>Converts an <see cref="int" /> to a <see cref="bool" />.</summary>
        /// <param name="value">The source.</param>
        /// <returns><c>True</c> if greater than 0 and not <c>null</c>; otherwise <c>False</c>.</returns>
        public static bool ToBool(this int value)
        {
            return value.IsGreaterThan(0);
        }

        /// <summary>Divides an int value by a double value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>System.Int32.</returns>
        public static double DivideBy(this int value, double divisor)
        {
            if (divisor.IsZero())
            {
                throw new ArgumentException("Division by 0 is not allowed");
            }

            if (divisor.IsNan())
            {
                throw new ArgumentException("Divisor is not a number");
            }

            return value / divisor;
        }

        /// <summary>Divides a int value by an int value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>System.Int32.</returns>
        public static int DivideBy(this int value, int divisor)
        {
            if (divisor.EqualsTo(0))
            {
                throw new ArgumentException("Division by 0 is not allowed");
            }

            return value / divisor;
        }

        /// <summary>Multiplies a int value by another int value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>System.Int32.</returns>
        public static int MultiplyBy(this int value, int multiplier)
        {
            return value * multiplier;
        }

        /// <summary>Subtracts the specified subtrahend.</summary>
        /// <param name="value">The value.</param>
        /// <param name="subtrahend">The subtrahend.</param>
        /// <returns>System.Int32.</returns>
        public static int Minus(this int value, int subtrahend)
        {
            return value - subtrahend;
        }

        /// <summary>Creates the sum of two integers.</summary>
        /// <param name="value">The value.</param>
        /// <param name="addend">The addend.</param>
        /// <returns>System.Int32.</returns>
        public static int Plus(this int value, int addend)
        {
            return value + addend;
        }

        /// <summary>To the double.</summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Double.</returns>
        public static double ToDouble(this int value)
        {
            return value;
        }

        /// <summary>Executes a specified action several times.</summary>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="action">The action to execute.</param>
        public static void Times(this int source, Action action)
        {
            Throw.IfNull(action);
            Throw.IfLessThan(source, 0);

            source.Times(index => action());
        }

        /// <summary>Executes a specified action several times. The action takes the index of the for loop as a parameter, which starts at zero.</summary>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="action">The action to execute.</param>
        public static void Times(this int source, Action<int> action)
        {
            Throw.IfNull(action);
            Throw.IfLessThan(source, 0);

            source.Times(action, 0);
        }

        /// <summary>Executes a specified action several times. The action takes the index of the for loop as a parameter, which starts at a specified start index.</summary>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="startIndex">The start index.</param>
        public static void Times(this int source, Action<int> action, int startIndex)
        {
            Throw.IfNull(action);

            for (var i = startIndex; i.IsLessThan(source.Plus(startIndex)); i++)
            {
                action(i);
            }
        }

        /// <summary>Returns the result of a function several times as an <see cref="IEnumerable{T}" />. The function takes the index of the for loop as a parameter, which starts at zero.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="func">The function whose result gets returned.</param>
        /// <returns>The enumerable containing the yielded results.</returns>
        public static IEnumerable<T> Times<T>(this int source, Func<T> func)
        {
            Throw.IfNull(func);
            Throw.IfLessThan(source, 0);

            return source.Times(index => func());
        }

        /// <summary>Returns the result of a function several times as an <see cref="IEnumerable{T}" />. The function takes the index of the for loop as a parameter, which starts at zero.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="func">The function whose result gets returned.</param>
        /// <returns>The enumerable containing the yielded results.</returns>
        public static IEnumerable<T> Times<T>(this int source, Func<int, T> func)
        {
            Throw.IfNull(func);
            Throw.IfLessThan(source, 0);

            return source.Times(func, 0);
        }

        /// <summary>Returns the result of a function several times as an <see cref="IEnumerable{T}" />. The function takes the index of the for loop as a parameter, which starts at a specified start index.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The value representing how often the action will be executed.</param>
        /// <param name="func">The function whose result gets returned.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The enumerable containing the yielded results.</returns>
        public static IEnumerable<T> Times<T>(this int source, Func<int, T> func, int startIndex)
        {
            Throw.IfNull(func);

            for (var i = startIndex; i.IsLessThan(source.Plus(startIndex)); i++)
            {
                yield return func(i);
            }
        }
    }
}
