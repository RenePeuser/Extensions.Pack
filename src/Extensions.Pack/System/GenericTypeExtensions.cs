using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Class for extensions of generic types.</summary>
    public static class GenericTypeExtensions
    {
        /// <summary>The default tolerance used to distinguish between two different doubles. The value has proven to be sufficient.</summary>
        private const double DefaultDoubleTolerance = 0.000001;

        /// <summary>
        /// Compares two nullable value types (e.g. Guid?, Enum?) for equality.
        /// </summary>
        public static bool EqualsTo<T>(this T? source,
                                       T? target) where T : struct
        {
            return EqualityComparer<T?>.Default.Equals(source, target);
        }

        /// <summary>
        /// Compares a non-nullable value type with a nullable one (e.g. Guid vs Guid?).
        /// </summary>
        public static bool EqualsTo<T>(this T source,
                                       T? target) where T : struct
        {
            return target.HasValue && EqualityComparer<T>.Default.Equals(source, target.Value);
        }

        /// <summary>
        /// Negated comparison for two nullable value types (e.g. Guid? vs Guid?).
        /// </summary>
        public static bool NotEqualsTo<T>(this T? source,
                                          T? target) where T : struct
        {
            return !EqualityComparer<T?>.Default.Equals(source, target);
        }

        /// <summary>
        /// Negated comparison for a non-nullable and a nullable value type (e.g. Guid vs Guid?).
        /// </summary>
        public static bool NotEqualsTo<T>(this T source,
                                          T? target) where T : struct
        {
            return !target.HasValue || !EqualityComparer<T>.Default.Equals(source, target.Value);
        }

        // ─────────────────────────────────────────────────────────────
        // 🧩 REFERENCE TYPES (classes, strings, records, etc.)
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Compares two reference types safely (handles nulls automatically).
        /// </summary>
        public static bool EqualsTo<T>(this T? source,
                                       T? target)
        {
            return EqualityComparer<T?>.Default.Equals(source, target);
        }

        /// <summary>
        /// Negated comparison for two reference types (handles nulls automatically).
        /// </summary>
        public static bool NotEqualsTo<T>(this T? source,
                                          T? target)
            where T : class
        {
            return !EqualsTo(source, target);
        }

        /// <summary>Simplify the usage of the equality comparer for double with default tolerance.</summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise <c>false</c>.</returns>
        public static bool EqualsTo(this double source,
                                    double target)
        {
            return EqualsTo(source, target, DefaultDoubleTolerance);
        }

        /// <summary>Simplify the usage of the equality comparer for double with default tolerance.</summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <returns><c>false</c> if the objects are equal; otherwise <c>true</c>.</returns>
        public static bool NotEqualsTo(this double source,
                                       double target)
        {
            return NotEqualsTo(source, target, DefaultDoubleTolerance);
        }

        /// <summary>Simplify the usage of the equality comparer for double with custom tolerance.</summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <param name="tolerance">The tolerance for difference between the two doubles.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise <c>false</c>.</returns>
        public static bool EqualsTo(this double source,
                                    double target,
                                    double tolerance)
        {
            return Math.Abs(source - target).IsLessThan(tolerance);
        }

        /// <summary>Simplify the usage of the equality comparer for double with custom tolerance.</summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <param name="tolerance">The tolerance for difference between the two doubles.</param>
        /// <returns><c>false</c> if the objects are equal; otherwise <c>true</c>.</returns>
        public static bool NotEqualsTo(this double source,
                                       double target,
                                       double tolerance)
        {
            return !source.EqualsTo(target, tolerance);
        }

        /// <summary>Simplify the usage of the equality comparer to compare two URIs (<c>Uri.Compare</c> is called) with inverted result.</summary>
        /// <remarks>
        ///     Hint: The <see cref="NotEqualsTo{T}(T,T)" /> is not working properly for every Uri. Hint: e.g. <![CDATA[{ADDRESS}{LANGUAGE}/#/?unit-system={UNIT_SYSTEM}, like http://localhost:8080/DE-de/#/?unit-system=metric]]>
        ///     Hint: is failing.
        /// </remarks>
        /// <param name="source">The source URI to compare.</param>
        /// <param name="target">The target URI to compare.</param>
        /// <returns><c>false</c> if the objects are equal; otherwise <c>true</c>.</returns>
        public static bool NotEquals(this Uri source,
                                     Uri target)
        {
            return !EqualsTo(source, target);
        }

        public static bool EqualsTo(this Uri source,
                                    Uri target)
        {
            var compareResult = Uri.Compare(source, target, UriComponents.AbsoluteUri,
                                            UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

            var isNotEqual = compareResult.EqualsTo(0);

            return isNotEqual;
        }

        /// <summary>Convert a single instance to an IList.</summary>
        /// <typeparam name="T">The generic type for items of the enumeration.</typeparam>
        /// <param name="item">The single item, which have to be converted to a list of type.</param>
        /// <returns>Returns a <see cref="IList{T}" /> containing a single item.</returns>
        public static IList<T> ToIList<T>(this T item)
        {
            return new List<T> { item };
        }

        public static List<T> AsList<T>(this T item)
        {
            return new List<T> { item };
        }

        public static ImmutableList<T> AsImmutableList<T>(this T item)
        {
            return ImmutableList.Create(item);
        }

        /// <summary>Determines whether source is any of the expected values.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns><c>true</c> if the source value is at least one of the expected values; otherwise, <c>false</c>.</returns>
        public static bool EqualsAny<T>(this T source,
                                        params object[] expectedValues)
            where T : class
        {
            if (source.EqualsTo<object>(expectedValues))
            {
                return true;
            }

            if (expectedValues.IsNull())
            {
                return false;
            }

            var result = expectedValues.Any(item => item.EqualsTo(source));

            return result;
        }

        /// <summary>Invokes a method if the object is not null. If we have Roslyn Compiler than we can use ?. Operator instead of this extension.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns>The source object back again.</returns>
        public static T? IfNotNullThen<T>(this T? source,
                                          Func<T, Action> action)
            where T : class
        {
            if (source.IsNull())
            {
                return null;
            }

            Throw.IfNull(action);

            action(source)();

            return source;
        }

        /// <summary>Invokes a method if the object is not null. If we have .Net 4.6 we can use ?. Operator instead of this extension.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns>The source object back again.</returns>
        public static T? IfNotNullThen<T>(this T? source,
                                          Action action)
            where T : class
        {
            if (source is null)
            {
                return default;
            }

            Throw.IfNull(action);

            action();

            return source;
        }

        /// <summary>Invokes a method if the object is not null. If we have .Net 4.6 we can use ?. Operator instead of this extension.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns>The source object back again.</returns>
        public static T? IfNotNullThen<T>(this T? source,
                                          Action<T> action)
            where T : class
        {
            if (source.IsNull())
            {
                return null;
            }

            Throw.IfNull(action);

            action(source);

            return source;
        }

        /// <summary>Invokes a method if the object is not null. If we have Roslyn Compiler than we can use ?. Operator instead of this extension.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns>The source object back again.</returns>
        public static T? IfNullThen<T>(this T? source,
                                       Func<T, Action> action)
            where T : class
        {
            if (source != null)
            {
                return source;
            }

            Throw.IfNull(action);

            action(null!)();

            return null;
        }

        /// <summary>Invokes a method if the object is not null. If we have .Net 4.6 we can use ?. Operator instead of this extension.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns>The source object back again.</returns>
        public static T? IfNullThen<T>(this T? source,
                                       Action action)
            where T : class
        {
            if (source != null)
            {
                return source;
            }

            Throw.IfNull(action);

            action();

            return null;
        }

        /// <summary>Creates a string from an enumeration of objects.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="title">The title.</param>
        /// <param name="infoSelector">The information selector.</param>
        /// <returns>A string which was prepared by the info selector.</returns>
        public static string ToString<T>(this T source,
                                         string title,
                                         params Expression<Func<T, object>>[] infoSelector)
            where T : class
        {
            Throw.IfNull(source);
            Throw.IfNull(title);
            Throw.IfNull(infoSelector);

            var compiledExpressions = infoSelector.ToCompiledExpressionWithInfo();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(title);
            stringBuilder.AppendLine(source.ToString(compiledExpressions));

            return stringBuilder.ToString();
        }

        /// <summary>Creates a string from an enumeration of objects.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="infoSelector">The information selector.</param>
        /// <returns>A string which was prepared by the info selector.</returns>
        public static string ToString<T>(this T source,
                                         params Expression<Func<T, object>>[] infoSelector)
            where T : class
        {
            Throw.IfNull(source);
            Throw.IfNull(infoSelector);

            var compiledExpressions = infoSelector.ToCompiledExpressionWithInfo();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(source.ToString(compiledExpressions));

            return stringBuilder.ToString();
        }

        /// <summary>Creates a string from an enumeration of expressions.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// string
        /// <param name="compiledExpressions">The compiled expressions.</param>
        /// <returns>A string which was prepared by the info selector.</returns>
        public static string ToString<T>(this T source,
                                         Dictionary<string, Func<T, object>> compiledExpressions)
        {
            Throw.IfNull<object>(source);
            Throw.IfNull(compiledExpressions);

            var stringBuilder = new StringBuilder();
            compiledExpressions.ForEach(item => stringBuilder.Append(item.ToString(source)));

            return stringBuilder.ToString();
        }

        /// <summary>Abstract extensions for the <see cref="Enumerable.Repeat{TResult}" />.</summary>
        /// <typeparam name="T">The generic type of the source object.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="count">The repeat count.</param>
        /// <returns>The created enumeration of the specific type with the specific expected count of items.</returns>
        public static IEnumerable<T> Repeat<T>(this T source,
                                               int count)
        {
            Throw.IfLessThan(count, 0);

            return Enumerable.Repeat(source, count);
        }

        /// <summary>Converts an <see cref="IConvertible" /> to an <see cref="int" />.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The converted value.</returns>
        public static int ToInt<T>(this T source)
            where T : IConvertible
        {
            Throw.IfNull<object>(source);

            return Convert.ToInt32(source, CultureInfo.InvariantCulture);
        }

        /// <summary>Creates.</summary>
        /// <typeparam name="T">The generic type of the enumerations.</typeparam>
        /// <param name="source">The source which hast to be concatenated with the expected enumeration.</param>
        /// <param name="items">The items.</param>
        /// <returns>The concatenated enumeration.</returns>
        public static IEnumerable<T> Concat<T>(this T source,
                                               IEnumerable<T> items)
        {
            Throw.IfNull<object>(source);
            Throw.IfNull<object>(items);

            yield return source;

            foreach (var item in items)
            {
                yield return item;
            }
        }

        /// <summary>Indicating weather a nullable type has not a value.</summary>
        /// <typeparam name="T">The generic type of the nullable.</typeparam>
        /// <param name="nullable">The nullable.</param>
        /// <returns><c>true</c> if the type has no value; otherwise <c>false</c>.</returns>
        public static bool NotHasValue<T>(this T? nullable)
            where T : struct
        {
            return !nullable.HasValue;
        }

        public static Task<InMemoryFileAsStream> ToCsvAsync<T>(this IEnumerable<T> elements) where T : class
        {
            return Task.Run(() => elements.ToCsv(typeof(T).Name));
        }

        public static Task<InMemoryFileAsStream> ToCsvAsync<T>(this IEnumerable<T> elements,
                                                               string fileName) where T : class
        {
            return Task.Run(() => elements.ToCsv(fileName));
        }

        public static InMemoryFileAsStream ToCsv<T>(this IEnumerable<T> elements) where T : class
        {
            return elements.ToCsv(typeof(T).Name);
        }

        public static InMemoryFileAsStream ToCsv<T>(this IEnumerable<T> elements,
                                                    string fileName) where T : class
        {
            Throw.IfNull(elements);
            Throw.IfNullOrWhiteSpace(fileName);

            fileName = fileName.EndWith(".csv") ? fileName : $"{fileName}.csv";

            var type = typeof(T);
            var properties = type.GetProperties();
            var stringBuilder = new StringBuilder();

            // 1. Write header
            stringBuilder.AppendLine(properties.Select(p => p.Name).Flatten(";"));

            // 2. Write values
            foreach (var element in elements)
            {
                stringBuilder.AppendLine(properties.Select(p => p.GetValue(element)!.ToString()!).Flatten(";"));
            }

            // Do not dispose, because the stream have to be used outside this method.
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));

            return new InMemoryFileAsStream(memoryStream, fileName);
        }
    }
}
