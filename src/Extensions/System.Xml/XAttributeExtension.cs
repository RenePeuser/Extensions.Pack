using System;
using System.Xml.Linq;

namespace Extensions
{
    /// <summary>Represents the extensions fro the <see cref="XAttribute" /> class.</summary>
    public static class XAttributeExtension
    {
        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="int" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="defaultValue">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="int" /> or the default value.</returns>
        public static int ToInt(this XAttribute element, int defaultValue = 0)
        {
            return element.To(defaultValue, Convert.ToInt32);
        }

        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="double" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="defaultValue">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="double" /> or the default value.</returns>
        public static double ToDouble(this XAttribute element, double defaultValue = 0)
        {
            return element.To(defaultValue, Convert.ToDouble);
        }

        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="decimal" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="defaultValue">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="decimal" /> or the default value.</returns>
        public static decimal ToDecimal(this XAttribute element, decimal defaultValue = 0)
        {
            return element.To(defaultValue, Convert.ToDecimal);
        }

        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="float" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="float" /> or the default value.</returns>
        public static float ToFloat(this XAttribute element, float value = 0)
        {
            return element.To(value, Convert.ToSingle);
        }

        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="bool" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="value">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="bool" /> or the default value.</returns>
        public static bool ToBool(this XAttribute element, bool value = false)
        {
            return element.To(value, Convert.ToBoolean);
        }

        /// <summary>Converts the value of the <see cref="XAttribute" /> to <see cref="string" />.</summary>
        /// <param name="element">The element which contains the expected value.</param>
        /// <param name="defaultValue">The default value, which has to be return if no value or not exists.</param>
        /// <returns>The converted <see cref="string" /> or the default value.</returns>
        public static string ValueOrDefault(this XAttribute element, string defaultValue = null)
        {
            return element.To(defaultValue, Convert.ToString);
        }

        /// <summary>Converts the value of an <see cref="XAttribute" /> by an specific converter, or returns the default value.</summary>
        /// <typeparam name="T">The generic type, which has to be the convert result.</typeparam>
        /// <param name="element">The element which contains the value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>The converted or default value.</returns>
        private static T To<T>(this XAttribute element, T defaultValue, Func<string, T> converter)
        {
            if (element == null)
            {
                return defaultValue;
            }

            var result = converter(element.Value);

            return result;
        }
    }
}
