using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Represents the extensions for the <see cref="string" /> class.</summary>
    public static class StringExtensions
    {
        /// <summary>Checks if the source string is null or empty.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if source string is null or empty; otherwise <c>False</c>.</returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? source)
        {
            return source.IsNullOrEmpty();
        }

        /// <summary>Checks if the source string is NOT null or empty.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if source string is null or empty; otherwise <c>False</c>.</returns>
        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? source)
        {
            return !source.IsNullOrEmpty();
        }

        /// <summary>Checks if the source string is empty.</summary>
        /// <param name="source">The source string.</param>
        /// <returns><c>True</c> if source string is empty; otherwise <c>False</c>.</returns>
        public static bool IsEmpty(this string source)
        {
            return source == string.Empty;
        }

        /// <summary>Checks if the source string is not empty.</summary>
        /// <param name="source">The source string.</param>
        /// <returns><c>True</c> if source string is not empty; otherwise <c>False</c>.</returns>
        public static bool IsNotEmpty(this string source)
        {
            return !source.IsEmpty();
        }

        /// <summary>Comfort way to use starts with, using <see cref="StringComparison.OrdinalIgnoreCase" /> as the string comparison.</summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if source string starts with expected value; otherwise <c>False</c>.</returns>
        public static bool StartWith(this string source,
                                     string value)
        {
            Throw.IfNull(source);
            Throw.IfNull(value);

            return source.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Comfort way to use ends with, using <see cref="StringComparison.OrdinalIgnoreCase" /> as the string comparison.</summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if source string starts with expected value; otherwise <c>False</c>.</returns>
        public static bool EndWith(this string source,
                                   string value)
        {
            Throw.IfNull(source);
            Throw.IfNull(value);

            var result = source.EndsWith(value, StringComparison.OrdinalIgnoreCase);

            return result;
        }

        /// <summary>Checks if the source string is null or whitespace.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if source string is null or only a whitespace; otherwise <c>False</c>.</returns>
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? source)
        {
            return source.IsNullOrWhiteSpace();
        }

        /// <summary>Checks if the source string is null or whitespace.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if source string is null or only a whitespace; otherwise <c>False</c>.</returns>
        public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? source)
        {
            return source.IsNullOrWhiteSpace().IsFalse();
        }

        /// <summary>Determines whether this string is a whitespace.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if source string is a whitespace; otherwise <c>False</c>.</returns>
        public static bool IsWhitespace(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            return source.IsNullOrWhiteSpace();
        }

        /// <summary>Converts a string (representing a boolean value) to a boolean value.</summary>
        /// <param name="value">The string representing a boolean value.</param>
        /// <returns>The boolean value.</returns>
        public static bool ToBool(this string value)
        {
            Throw.IfNull(value);

            if (bool.TryParse(value, out var boolValue))
            {
                return boolValue;
            }

            var exceptionText = string.Format(CultureInfo.InvariantCulture, "Value {0} cannot be converted to a boolean value.", value);

            throw new ArgumentException(exceptionText, nameof(value));
        }

        /// <summary>Converts a string (representing a boolean value) to a boolean value.</summary>
        /// <param name="value">The string representing a boolean value.</param>
        /// <returns>The boolean value.</returns>
        public static bool ToBoolOrDefault(this string value)
        {
            if (value == null)
            {
                return false;
            }

            return bool.TryParse(value, out var boolValue) && boolValue;
        }

        /// <summary>Determines string is not null, not empty and no whitespace.</summary>
        /// <param name="source">The source string, which has to be check.</param>
        /// <returns><c>true</c> if the specified source is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValid(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            if (source.IsNullOrWhiteSpace())
            {
                return false;
            }

            return true;
        }

        /// <summary>Determines string is null, empty or whitespace.</summary>
        /// <param name="source">The source string, which has to be check.</param>
        /// <returns><c>true</c> if the specified source is valid; otherwise, <c>false</c>.</returns>
        public static bool IsNotValid(this string source)
        {
            return !source.IsValid();
        }

        /// <summary>Parse a string to convert to <see cref="DateTime" />.</summary>
        /// <param name="source">The source string, which has to be converted.</param>
        /// <returns>The converted <see cref="DateTime" />.</returns>
        public static DateTime ToDateTime(this string source)
        {
            Throw.IfNullOrWhiteSpace(source);

            return source.ToDateTime(DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>Parse a string to convert to <see cref="DateTime" />.</summary>
        /// <param name="source">The source string, which has to be converted.</param>
        /// <param name="dateTimeFormatInfo">The date time format information.</param>
        /// <returns>The converted <see cref="DateTime" />.</returns>
        public static DateTime ToDateTime(this string source,
                                          DateTimeFormatInfo dateTimeFormatInfo)
        {
            Throw.IfNullOrWhiteSpace(source);
            Throw.IfNull(dateTimeFormatInfo);

            return source.ToDateTime(DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
        }

        /// <summary>Parse a string to convert to <see cref="DateTime" />.</summary>
        /// <param name="source">The source string, which has to be converted.</param>
        /// <param name="dateTimeFormatInfo">The date time format information.</param>
        /// <param name="dateTimeStyles">The date time styles.</param>
        /// <returns>The converted <see cref="DateTime" />.</returns>
        public static DateTime ToDateTime(this string source,
                                          DateTimeFormatInfo dateTimeFormatInfo,
                                          DateTimeStyles dateTimeStyles)
        {
            Throw.IfNullOrWhiteSpace(source);

            var result = DateTime.TryParse(source, dateTimeFormatInfo, dateTimeStyles,
                                           out var dateTime);

            if (!result)
            {
                throw new InvalidOperationException(string.Format(dateTimeFormatInfo, "Can not parse string: {0} to type DateTime", source));
            }

            return dateTime;
        }

        /// <summary>Formats the specified string with the given format arguments. Uses <see cref="CultureInfo.InvariantCulture" /> for formatting.</summary>
        /// <param name="source">The source.</param>
        /// <param name="formatArguments">The format arguments.</param>
        /// <returns>The resulting string.</returns>
        public static string FormatInvariantCulture(this string source,
                                                    params object[] formatArguments)
        {
            Throw.IfNullOrWhiteSpace(source);
            Throw.IfNull(formatArguments);

            var formatedString = string.Format(CultureInfo.InvariantCulture, source, formatArguments);

            return formatedString;
        }

        /// <summary>Splits the blocks.</summary>
        /// <param name="value">The value.</param>
        /// <param name="expectedBlocks">The expected bytes.</param>
        /// <param name="blockLength">Length of the block.</param>
        /// <returns>The expected string blocks.</returns>
        public static IEnumerable<string> SplitBlocks(this string value,
                                                      int expectedBlocks,
                                                      int blockLength)
        {
            Throw.IfNullOrWhiteSpace(value);

            if (value.Length.NotEqualsTo(expectedBlocks.MultiplyBy(blockLength)))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                                          "Can not split string in expected blocks, because of string length: '{0}' which is not equals as the product of expected blocks: '{1}' and bloock length: '{2}'",
                                                          value.Length,
                                                          expectedBlocks,
                                                          blockLength),
                                            nameof(value));
            }

            for (var i = 0; i < expectedBlocks; i++)
            {
                yield return value.Substring(i.MultiplyBy(blockLength), blockLength);
            }
        }

        /// <summary>Splits the specified block length.</summary>
        /// <param name="value">The value.</param>
        /// <param name="blockLength">Length of the block.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> Split(this string value,
                                                int blockLength)
        {
            if (value.IsNotValid())
            {
                yield break;
            }

            if (blockLength.IsLessOrEqual(default))
            {
                throw new ArgumentException("The length of a block must not be 0 or smaller");
            }

            var expectedBlocks = value.Length.DivideBy(blockLength.ToDouble()).Ceiling();

            for (var i = 0; i < expectedBlocks; i++)
            {
                yield return value.SubstringUpTo(i.MultiplyBy(blockLength), blockLength);
            }
        }

        /// <summary>Takes up to.</summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string SubstringUpTo(this string value,
                                           int length)
        {
            Throw.IfNullOrWhiteSpace(value);

            return value.SubstringUpTo(0, length);
        }

        /// <summary>Takes up to.</summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string SubstringUpTo(this string value,
                                           int startIndex,
                                           int length)
        {
            Throw.IfNullOrWhiteSpace(value);

            var stringLength = value.Length;
            var maxLength = startIndex.Plus(length).IsLessOrEqual(stringLength) ? length : stringLength - startIndex;

            return value.Substring(startIndex, maxLength);
        }

        /// <summary>Repeats the specified count.</summary>
        /// <param name="source">The source.</param>
        /// <param name="count">The count.</param>
        /// <returns>System.String.</returns>
        public static string Repeat(this string source,
                                    int count)
        {
            Throw.IfNull(source);
            Throw.IfLessThan(count, 0);

            return Enumerable.Repeat(source, count).Flatten();
        }

        /// <summary>Concatenates the specified strings to.</summary>
        /// <param name="value">The source.</param>
        /// <param name="values">The strings to concatenate.</param>
        /// <returns>The concatenated string.</returns>
        public static string ConcatWith(this string value,
                                        params string[] values)
        {
            Throw.IfNull(values);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(value);
            values.ForEach(s => stringBuilder.Append(s));

            return stringBuilder.ToString();
        }

        /// <summary>Determines whether the specified value is a valid binary string.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid binary string; otherwise, <c>false</c>.</returns>
        public static bool IsValidBinaryString(this string value)
        {
            Throw.IfNullOrWhiteSpace(value);

            return value.All(character => character.EqualsTo('0') || character.EqualsTo('1'));
        }

        /// <summary>Determines whether the specified value is not a valid binary string.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid binary string; otherwise, <c>false</c>.</returns>
        public static bool IsNotValidBinaryString(this string value)
        {
            Throw.IfNullOrWhiteSpace(value);

            return !value.IsValidBinaryString();
        }

        /// <summary>Gets the type information from a full qualified type name in a specific assembly.</summary>
        /// <param name="fullqualifiedTypeName">The full qualified type name.</param>
        /// <param name="fullQualifiedAssemblyName">The full qualified assembly name.</param>
        /// <returns>The type info.</returns>
        public static Type? FromAssembly(this string fullqualifiedTypeName,
                                         string fullQualifiedAssemblyName)
        {
            Throw.IfNullOrWhiteSpace(fullqualifiedTypeName);
            Throw.IfNullOrWhiteSpace(fullQualifiedAssemblyName);

            return Type.GetType(fullqualifiedTypeName + ", " + fullQualifiedAssemblyName);
        }

        /// <summary>Compares two strings and ignores the case.</summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>True if the strings are the same (ignoring case), else false.</returns>
        public static bool EqualsToIgnoringCase(this string value1,
                                                string value2)
        {
            Throw.IfNull(value1);
            Throw.IfNull(value2);

            return value1.ToUpperInvariant().EqualsTo(value2.ToUpperInvariant());
        }

        /// <summary>Nots the contains.</summary>
        public static bool DoesNotContain(this string value,
                                          string notExpected)
        {
            Throw.IfNullOrWhiteSpace(value);

            return !value.Contains(notExpected);
        }

        public static bool ContainsNotAnyOf(this string source,
                                            params string[] notContainStrings)
        {
            return !notContainStrings.Any(source.Contains);
        }

        public static bool EqualsAnyOf(this string source,
                                       params string[] notContainStrings)
        {
            return notContainStrings.Any(s => s.ToLower(CultureInfo.InvariantCulture).EqualsTo(source));
        }

        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => $"{input.First().ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)}{input[1..]}"
            };
        }

        public static string FirstCharToLower(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture) + input[1..]
            };
        }

        public static Uri ToUri(this string source)
        {
            return new Uri(source);
        }

        public static string BuildUriPathWith(this string basePath,
                                              params string[] pathSegments)
        {
            var normalizeBasPath = basePath.TrimEnd('/');
            var normalizePathSegments = pathSegments.Select(segment => segment.TrimStart('/'));

            return normalizeBasPath.Concat(normalizePathSegments).Flatten("/");
        }

        public static int ToIntOrDefault(this string value,
                                         int defaultValue = 0)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return defaultValue;
            }

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static bool TryChangeType<T>(this string source,
                                            out T? targetType)
        {
            if (source.TryChangeType(typeof(T), out var objectResult))
            {
                if (objectResult.IsNull())
                {
                    targetType = default;

                    return false;
                }

                targetType = objectResult.Cast<T>();

                return true;
            }

            targetType = default;

            return false;
        }

        public static bool TryChangeType(this string source,
                                         Type newType,
                                         out object? targetType)
        {
            try
            {
                targetType = Convert.ChangeType(source, newType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                targetType = null;

                return false;
            }

            return true;
        }

        internal static string GetJsonStringFrom(this string expectedObjectAsJson,
                                                 Assembly callingAssembly)

        {
            var trimmedJsonValue = expectedObjectAsJson.Trim() // Trim whitespaces
                                                       .TrimEnd(Environment.NewLine.ToCharArray()) // Trim line breaks at the end if exists
                                                       .Trim('"'); // Trim " if exists cause not needed

            if (trimmedJsonValue.EndWith(".json"))
            {
                trimmedJsonValue = callingAssembly.GetFileContentFrom(trimmedJsonValue).Trim().TrimEnd(Environment.NewLine.ToCharArray());
            }

            return trimmedJsonValue;
        }
    }
}
