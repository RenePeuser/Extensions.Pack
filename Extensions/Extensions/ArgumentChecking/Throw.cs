using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Extensions
{
    internal static class Throw
    {
        [DebuggerHidden]
        internal static void IfGreaterThan<T>(Func<T> argumentFunc, T limit)
            where T : IComparable
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().IsGreaterThan(limit))
            {
                return;
            }

            ThrowGreaterThanException(argumentFunc, limit, arg => arg is T && ((T) arg).IsGreaterThan(limit));
        }

        [DebuggerHidden]
        internal static void IfGreaterOrEqual<T>(Func<T> argumentFunc, T limit)
            where T : IComparable
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().IsGreaterOrEqual(limit))
            {
                return;
            }

            ThrowGreaterOrEqualException(argumentFunc, limit, arg => arg is T && ((T) arg).IsGreaterOrEqual(limit));
        }

        [DebuggerHidden]
        internal static void IfLessThan<T>(Func<T> argumentFunc, T limit)
            where T : IComparable
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().IsLessThan(limit))
            {
                return;
            }

            ThrowLessThanException(argumentFunc, limit, arg => arg is T && ((T) arg).IsLessThan(limit));
        }

        [DebuggerHidden]
        internal static void IfLessOrEqual<T>(Func<T> argumentFunc, T limit)
            where T : IComparable
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().IsLessOrEqual(limit))
            {
                return;
            }

            ThrowLessOrEqualException(argumentFunc, limit, arg => arg is T && ((T) arg).IsLessOrEqual(limit));
        }

        [DebuggerHidden]
        internal static void IfNotValidBinary(Func<string> argumentFunc)
        {
            IfNull(() => argumentFunc);

            IfNullOrWhiteSpace(argumentFunc);
            IfNotValidBinaryInternal(argumentFunc);
        }

        [DebuggerHidden]
        internal static void IfNull<T>(Func<T> argumentFunc)
            where T : class
        {
            if (argumentFunc == null)
            {
                throw new ArgumentNullException(nameof(argumentFunc));
            }

            IfNullInternal(argumentFunc);
        }

        [DebuggerHidden]
        internal static void IfNullOrEmpty<T>(Func<IEnumerable<T>> argumentFunc)
        {
            IfNull(() => argumentFunc);
            IfNullInternal(argumentFunc);
            IfEmpty(argumentFunc);
        }

        [DebuggerHidden]
        internal static void IfNullOrAny<T>(Func<IEnumerable<T>> argumentFunc, Func<T, bool> predicate)
        {
            IfNull(() => argumentFunc);
            IfNull(() => predicate);

            IfNullInternal(argumentFunc);

            if (!argumentFunc().Any(predicate))
            {
                return;
            }

            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "The sequence must not contain any items that satisfy the predicate '{0}'.",
                    predicate.Method.Name),
                argumentFunc.GetParameterName(x => x.Is<IEnumerable<T>>() && x.Cast<IEnumerable<T>>().Any(predicate)));
        }

        [DebuggerHidden]
        internal static void IfNullOrAnyItemIsNull<T>(Func<IEnumerable<T>> argumentFunc)
        {
            IfNull(() => argumentFunc);
            IfNull(argumentFunc);

            if (!argumentFunc().IsAnyItemNull())
            {
                return;
            }

            ThrowIsAnyItemNullException(argumentFunc, arg => arg.Is<IEnumerable<T>>() && arg.Cast<IEnumerable<T>>().IsAnyItemNull());
        }

        [DebuggerHidden]
        internal static void IfAnyItemIsNullOrWhitespace(Func<IEnumerable<string>> argumentFunc)
        {
            IfNull(() => argumentFunc);
            IfNull(argumentFunc);

            if (!argumentFunc().IsAnyItemNullOrWhitespace())
            {
                return;
            }

            ThrowIsAnyItemNullOrWhiteSpaceException(
                argumentFunc,
                arg => arg.Is<IEnumerable<string>>() && arg.Cast<IEnumerable<string>>().IsAnyItemNullOrWhitespace());
        }

        [DebuggerHidden]
        internal static void If<T>(Func<T> argumentFunc, Func<T, bool> validationPredicate)
        {
            IfNull(() => argumentFunc);
            IfNull(() => validationPredicate);

            IfInternal(argumentFunc, validationPredicate, b => b, string.Empty);
        }

        [DebuggerHidden]
        internal static void If<T>(Func<T> argumentFunc, Func<T, bool> validationPredicate, string message)
        {
            IfNull(() => argumentFunc);
            IfNull(() => validationPredicate);

            IfInternal(argumentFunc, validationPredicate, b => b, message);
        }

        [DebuggerHidden]
        internal static void IfNot<T>(Func<T> argumentFunc, Func<T, bool> validationPredicate)
        {
            IfNull(() => argumentFunc);
            IfNull(() => validationPredicate);

            IfInternal(argumentFunc, validationPredicate, b => b.IsFalse(), string.Empty);
        }

        [DebuggerHidden]
        internal static void IfNot<T>(Func<T> argumentFunc, Func<T, bool> validationPredicate, string message)
        {
            IfNull(() => argumentFunc);
            IfNull(() => validationPredicate);

            IfInternal(argumentFunc, validationPredicate, b => b.IsFalse(), message);
        }

        [DebuggerHidden]
        internal static void IfNullOrEmpty(Func<string> argumentFunc)
        {
            IfNull(() => argumentFunc);
            IfNullInternal(argumentFunc);
            IfEmpty(argumentFunc);
        }

        [DebuggerHidden]
        internal static void IfNullOrWhiteSpace(Func<string> argumentFunc)
        {
            IfNull(() => argumentFunc);
            IfNullInternal(argumentFunc);
            IfEmpty(argumentFunc);
            IfWhitespace(argumentFunc);
        }

        [DebuggerHidden]
        internal static void IfEqualsTo<T>(Func<T> argumentFunc, T expectedValue)
        {
            IfNull(() => argumentFunc);
            IfEqualsToInternal(argumentFunc, expectedValue);
        }

        [DebuggerHidden]
        internal static void IfNotEqualsTo<T>(Func<T> argumentFunc, T expectedValue)
        {
            IfNull(() => argumentFunc);

            IfNotEqualsToInternal(argumentFunc, expectedValue);
        }

        [DebuggerHidden]
        internal static void IfLengthIsNot(Func<string> argumentFunc, int length)
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().Length.NotEqualsTo(length))
            {
                return;
            }

            ThrowStringLengthIsNotException(argumentFunc, length, arg => arg.Is<string>() && arg.Cast<string>().Length.NotEqualsTo(length));
        }

        [DebuggerHidden]
        internal static void IfOutOfRange<T>(Func<T> argumentFunc, T min, T max)
            where T : IComparable
        {
            IfNull(() => argumentFunc);

            if (!argumentFunc().IsOutOfRange(min, max))
            {
                return;
            }

            ThrowValueOutOfRangeException(argumentFunc, min, max, arg => arg is T && ((T) arg).IsOutOfRange(min, max));
        }

        [DebuggerHidden]
        internal static void IfNotType<T>(Func<object> argumentFunc)
            where T : class
        {
            IfNull(() => argumentFunc);
            IfNull(argumentFunc);

            if (!argumentFunc().IsNotTypeOf<T>())
            {
                return;
            }

            throw new ArgumentException(
                "The given type is not expected type",
                argumentFunc.GetParameterName(arg => arg.EqualsTo(argumentFunc()) && arg.IsNotTypeOf<T>()));
        }

        [DebuggerHidden]
        internal static void IfNotInterface<T>()
            where T : class
        {
            var type = typeof(T);

            if (!type.IsInterface)
            {
                throw new ArgumentException($"Type: '{type}' has to be an interface");
            }
        }

        [DebuggerHidden]
        internal static void IfInterface<T>()
            where T : class
        {
            var type = typeof(T);

            if (type.IsInterface)
            {
                throw new ArgumentException($"Type: '{type}' has not to be an interface");
            }
        }

        [DebuggerHidden]
        internal static void IfNotContains<T>(Func<T> argumentFunc, IEnumerable<T> source)
        {
            IfNull(() => source);
            IfNull(() => argumentFunc);

            var argument = argumentFunc();

            if (source.Contains(argument))
            {
                return;
            }

            throw new ArgumentException(
                "The given type is not expected type",
                argumentFunc.GetParameterName(arg => arg.EqualsTo(argument) && !source.Contains(argument)));
        }

        [DebuggerHidden]
        private static void IfEmpty<T>(Func<IEnumerable<T>> argument)
        {
            if (argument().Any())
            {
                return;
            }

            throw new ArgumentException(
                "The enumerable must not be empty.",
                argument.GetParameterName(arg => arg.Is<IEnumerable<T>>() && arg.Cast<IEnumerable<T>>().IsEmpty()));
        }

        [DebuggerHidden]
        private static void IfNotValidBinaryInternal(Func<string> argumentFunc)
        {
            var binaryString = argumentFunc();

            if (binaryString.IsValidBinaryString())
            {
                return;
            }

            throw new ArgumentException(
                "The given string: '{0}' is not a valid binary string",
                argumentFunc.GetParameterName(arg => arg.Is<string>() && !arg.Cast<string>().IsValidBinaryString()));
        }

        [DebuggerHidden]
        private static void IfNullInternal<T>(Func<T> argumentFunc)
            where T : class
        {
            if (argumentFunc() != null)
            {
                return;
            }

            throw new ArgumentNullException(argumentFunc.GetParameterName(arg => arg == null));
        }

        [DebuggerHidden]
        private static void IfEmpty(Func<string> argument)
        {
            if (!TcEnumerableExtensions.IsEmpty(argument()))
            {
                return;
            }

            throw new ArgumentException(
                "The string must not be empty.",
                argument.GetParameterName(arg => arg.Is<string>() && TcEnumerableExtensions.IsEmpty(arg.Cast<string>())));
        }

        [DebuggerHidden]
        private static void IfWhitespace(Func<string> argument)
        {
            if (!argument().IsNullOrWhiteSpace())
            {
                return;
            }

            throw new ArgumentException(
                "The string must not be only whitespace.",
                argument.GetParameterName(arg => arg.Is<string>() && arg.Cast<string>().IsWhitespace()));
        }

        [DebuggerHidden]
        private static void ThrowGreaterThanException<T>(Func<T> argumentFunc, T limit, Func<object, bool> predicate)
            where T : IComparable
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Value: '{0}' must not be greater than: '{1}'",
                    argumentFunc(),
                    limit));
        }

        [DebuggerHidden]
        private static void ThrowGreaterOrEqualException<T>(Func<T> argumentFunc, T limit, Func<object, bool> predicate)
            where T : IComparable
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Value: '{0}' must not be greater than or equal to: '{1}'",
                    argumentFunc(),
                    limit));
        }

        [DebuggerHidden]
        private static void ThrowLessThanException<T>(Func<T> argumentFunc, T limit, Func<object, bool> predicate)
            where T : IComparable
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Value: '{0}' must not be less than: '{1}'",
                    argumentFunc(),
                    limit));
        }

        [DebuggerHidden]
        private static void ThrowLessOrEqualException<T>(Func<T> argumentFunc, T limit, Func<object, bool> predicate)
            where T : IComparable
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Value: '{0}' must not be less than or equal to: '{1}'",
                    argumentFunc(),
                    limit));
        }

        [DebuggerHidden]
        private static void ThrowStringLengthIsNotException(Func<string> argumentFunc, int length, Func<object, bool> predicate)
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "String length: '{0}' is not as expected. Expected value: '{1}",
                    argumentFunc(),
                    length));
        }

        [DebuggerHidden]
        private static void ThrowIsAnyItemNullException<T>(Func<IEnumerable<T>> argumentFunc, Func<object, bool> predicate)
        {
            throw new ArgumentNullException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "At least one item in the enumeration '{0}' was null.",
                    argumentFunc()));
        }

        [DebuggerHidden]
        private static void ThrowIsAnyItemNullOrWhiteSpaceException(Func<IEnumerable<string>> argumentFunc, Func<object, bool> predicate)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "At least one string in the enumeration '{0}' was null or a whitespace.",
                    argumentFunc()),
                argumentFunc.GetParameterName(predicate));
        }

        [DebuggerHidden]
        private static void ThrowValueOutOfRangeException<T>(Func<T> argumentFunc, T minimum, T maximum, Func<object, bool> predicate)
            where T : IComparable
        {
            throw new ArgumentOutOfRangeException(
                argumentFunc.GetParameterName(predicate),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Value: '{0}' is not in expected range from '{1}' to '{2}'",
                    argumentFunc(),
                    minimum,
                    maximum));
        }

        [DebuggerHidden]
        private static void IfNotEqualsToInternal<T>(Func<T> argumentFunc, T expectedValue)
        {
            var argumentValue = argumentFunc();

            if (!argumentValue.NotEqualsTo(expectedValue))
            {
                return;
            }

            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "The argument value: '{0}' is not equal to: {1}",
                    argumentValue,
                    expectedValue),
                argumentFunc.GetParameterName(arg => arg.NotEqualsTo(expectedValue)));
        }

        [DebuggerHidden]
        private static void IfEqualsToInternal<T>(Func<T> argumentFunc, T expectedValue)
        {
            var argumentValue = argumentFunc();

            if (!argumentValue.EqualsTo(expectedValue))
            {
                return;
            }

            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "The argument value: '{0}' must not be equal to: {1}",
                    argumentValue,
                    expectedValue),
                argumentFunc.GetParameterName(arg => arg.EqualsTo(expectedValue)));
        }

        [DebuggerHidden]
        private static void IfInternal<T>(Func<T> argumentFunc, Func<T, bool> validationPredicate, Func<bool, bool> isValidFunc, string message)
        {
            var argument = argumentFunc();
            var isValid = validationPredicate(argument);

            if (isValidFunc(isValid))
            {
                throw new ArgumentException(message, argumentFunc.GetParameterName(arg => arg.EqualsTo(argument)));
            }
        }
    }
}
