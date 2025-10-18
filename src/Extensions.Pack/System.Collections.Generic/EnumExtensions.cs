using System.Globalization;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Represents extensions for types of <see cref="Enum" />.</summary>
    public static class EnumExtensions
    {
        public static IEnumerable<string> GetAllNames<T>(this T _) where T : struct, Enum
        {
            return Enum.GetNames<T>();
        }

        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="ignoreTypes">The types which have to be ignored from the original type list.</param>
        /// <returns>The list with all fields of the generic enumeration type except the types which have to be ignored.</returns>
        public static List<T> GetEnumValuesExceptOf<T>(params T[] ignoreTypes)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Throw.IfNull(ignoreTypes);

            var enumValues = GetEnumValuesOf<T>();
            enumValues.RemoveRange(ignoreTypes);

            return enumValues;
        }

        /// <summary>To the enum or default. No exception will be raised.</summary>
        /// <typeparam name="T">The generic enum type.</typeparam>
        /// <param name="value">The <see cref="string" /> source value.</param>
        /// <returns>The converted <see cref="Enum" /> from the source <see cref="string" />.</returns>
        public static T ToEnumOrDefault<T>(this string value)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!Enum.TryParse(value, true, out T result))
            {
                return default;
            }

            return result;
        }

        /// <summary>To the enum.</summary>
        /// <typeparam name="T">The generic enum type.</typeparam>
        /// <param name="value">The <see cref="string" /> source value.</param>
        /// <returns>The converted <see cref="Enum" /> from the source <see cref="string" />.</returns>
        public static T ToEnum<T>(this string value)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Throw.IfNull(value);

            if (!Enum.TryParse(value, true, out T result))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                                          "Convert string: '{0}' to enum: {1} not successful",
                                                          value,
                                                          typeof(T).Name),
                                            nameof(value));
            }

            return result;
        }

        /// <summary>Converts an enum to an int.</summary>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The converted int.</returns>
        public static int ToInt<T>(this T? value)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return value.ToIntOrDefault(default);
        }

        /// <summary>Converts an enum to an int or to a default value if it is null.</summary>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted int.</returns>
        public static int ToIntOrDefault<T>(this T? value,
                                            int defaultValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (value == null)
            {
                return defaultValue;
            }

            return value.Cast<int>();
        }

        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <returns>The list with all fields of the generic enumeration type.</returns>
        public static List<T> GetEnumValuesOf<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            var type = typeof(T);
            Throw.If(type, t => t.IsNotNull() && t.IsEnum.IsNot(), $"The given type: '{type.Name}' is not an enum");

            return Enum.GetValues(typeof(T)).ToListOfType<T>();
        }

        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="type">The enum type.</param>
        /// <returns>The list with all fields of the generic enumeration type.</returns>
        public static List<T> GetEnumValuesOf<T>(this Type type)
        {
            Throw.IfNull(type);
            Throw.If(type, t => t.IsNotNull() && t.IsEnum.IsNot(), $"The given type: '{type.Name}' is not an enum");

            return Enum.GetValues(type).ToListOfType<T>();
        }

        /// <summary>Gets the custom attribute of an given enum value.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The custom attribute.</returns>
        public static T? GetAttributeOfType<T>(this Enum enumValue)
            where T : Attribute
        {
            Throw.IfNull(enumValue);

            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);

            Throw.IfNullOrWhiteSpace(name);

            var fieldInfo = type.GetField(name);
            Throw.IfNull(fieldInfo);

            return fieldInfo.GetCustomAttributes(false).OfType<T>().SingleOrDefault();
        }

        public static bool IsDefined<T>(this T value) where T : Enum
        {
            return Enum.IsDefined(typeof(T), value);
        }

        public static bool IsUnDefined<T>(this T value) where T : Enum
        {
            return value.IsDefined().Negate();
        }
    }
}
