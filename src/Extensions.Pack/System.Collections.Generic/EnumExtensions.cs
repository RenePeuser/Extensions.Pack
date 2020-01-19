using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Extensions.Pack
{
    /// <summary>Represents extensions for types of <see cref="Enum" />.</summary>
    public static class EnumExtensions
    {
        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="System.Collections.Generic.List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="ignoreTypes">The types which have to be ignored from the original type list.</param>
        /// <returns>The list with all fields of the generic enumeration type except the types which have to be ignored.</returns>
        public static List<T> GetEnumValuesExceptOf<T>(params T[] ignoreTypes)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Throw.IfNull(() => ignoreTypes);

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
            T result;

            if (!Enum.TryParse(value, true, out result))
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
            Throw.IfNull(() => value);

            T result;

            if (!Enum.TryParse(value, true, out result))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
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
        public static int ToIntOrDefault<T>(this T? value, int defaultValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (value == null)
            {
                return defaultValue;
            }

            return value.Cast<int>();
        }

        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="System.Collections.Generic.List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <returns>The list with all fields of the generic enumeration type.</returns>
        public static List<T> GetEnumValuesOf<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            var type = typeof(T);
            Throw.If(() => type, t => t.IsEnum.IsNot(), $"The given type: '{type.Name}' is not an enum");

            return Enum.GetValues(typeof(T)).ToListOfType<T>();
        }

        /// <summary>Converts the values of an <see cref="Enum" /> type into a <see cref="System.Collections.Generic.List{T}" />. It helps to get all fields from an enumeration in a list.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="type">The enum type.</param>
        /// <returns>The list with all fields of the generic enumeration type.</returns>
        public static List<T> GetEnumValuesOf<T>(this Type type)
        {
            Throw.IfNull(() => type);
            Throw.If(() => type, t => t.IsEnum.IsNot(), $"The given type: '{type.Name}' is not an enum");

            return Enum.GetValues(type).ToListOfType<T>();
        }

        /// <summary>Gets the custom attribute of an given enum value.</summary>
        /// <typeparam name="T">The generic type of the expected enumeration.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The custom attribute.</returns>
        public static T GetAttributeOfType<T>(this Enum enumValue)
            where T : Attribute
        {
            Throw.IfNull(() => enumValue);

            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);

            return type.GetField(name).GetCustomAttributes(false).OfType<T>().SingleOrDefault();
        }
    }
}
