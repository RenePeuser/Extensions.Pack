using System;
using System.Linq;
using System.Xml.Linq;

namespace Extensions.Pack
{
    /// <summary>Represents the extensions fro the <see cref="XElement" /> class.</summary>
    public static class XElementExtensions
    {
        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="int" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="int" /> or the default value.</returns>
        public static int ToInt(this XElement element, int value = 0)
        {
            return element.To(value, Convert.ToInt32);
        }

        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="double" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="double" /> or the default value.</returns>
        public static double ToDouble(this XElement element, double value = 0)
        {
            return element.To(value, Convert.ToDouble);
        }

        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="decimal" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="decimal" /> or the default value.</returns>
        public static decimal ToDecimal(this XElement element, decimal value = 0)
        {
            return element.To(value, Convert.ToDecimal);
        }

        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="float" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="float" /> or the default value.</returns>
        public static float ToFloat(this XElement element, float value = 0)
        {
            return element.To(value, Convert.ToSingle);
        }

        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="bool" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="bool" /> or the default value.</returns>
        public static bool ToBool(this XElement element, bool value = false)
        {
            return element.To(value, Convert.ToBoolean);
        }

        /// <summary>Converts the value of the <see cref="XElement" /> to <see cref="string" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="defaultValue">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="string" /> or the default value.</returns>
        public static string ValueOrDefault(this XElement element, string defaultValue = null)
        {
            return element.To(defaultValue, Convert.ToString);
        }

        /// <summary>Gets a specific <see cref="XAttribute" /> from a specific <see cref="XElement" /> by its attribute name.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="attributeName">The attribute name of the expected attribute.</param>
        /// <returns>The converted <see cref="string" /> or the default value.</returns>
        public static XAttribute AttributeBy(this XElement element, string attributeName)
        {
            Throw.IfNull(() => element);
            Throw.IfNull(() => attributeName);

            var attribute = element.Attributes().FirstOrDefault(a => a.Name.LocalName.EqualsTo(attributeName));

            return attribute;
        }

        /// <summary>Converts the value of an <see cref="XElement" /> by an specific converter, or returns the default value.</summary>
        /// <typeparam name="T">The generic type, which has to be the convert result.</typeparam>
        /// <param name="element">The element which contains the value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>The converted or default value.</returns>
        private static T To<T>(this XElement element, T defaultValue, Func<string, T> converter)
        {
            if (element == null)
            {
                return defaultValue;
            }

            if (element.Value.IsEmpty())
            {
                return defaultValue;
            }

            var result = converter(element.Value);

            return result;
        }
    }
}
