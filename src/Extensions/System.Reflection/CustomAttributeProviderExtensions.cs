using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    /// <summary>Extension methods for <see cref="ICustomAttributeProvider" />.</summary>
    public static class TcCustomAttributeProviderExtensions
    {
        /// <summary>Get all attributes from a specific <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider customAttributeProvider)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            return customAttributeProvider.GetCustomAttributes<T>(false);
        }

        /// <summary>Get all attributes from a specific <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks. </param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            var attributes = customAttributeProvider.GetCustomAttributes(typeof(T), inherit).ToListOfType<T>();

            return attributes;
        }

        /// <summary>Get all attributes from a specific <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static bool HasCustomAttribute<T>(this ICustomAttributeProvider customAttributeProvider)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            var hasCustomAttribute = customAttributeProvider.HasCustomAttribute<T>(false);

            return hasCustomAttribute;
        }

        /// <summary>Get all attributes from a specific <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks. </param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static bool HasCustomAttribute<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            var attribute = customAttributeProvider.GetCustomAttribute<T>(inherit);
            var hasCustomAttribute = attribute != null;

            return hasCustomAttribute;
        }

        /// <summary>Gets the first attribute of a specific attribute type from the <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider customAttributeProvider)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            return customAttributeProvider.GetCustomAttribute<T>(false);
        }

        /// <summary>Gets the first attribute of a specific attribute type from the <see cref="ICustomAttributeProvider" />.</summary>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider" /> which is decorated with the expected attributes.</param>
        /// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks.</param>
        /// <typeparam name="T">Generic type of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
            where T : Attribute
        {
            Throw.IfNull(() => customAttributeProvider);

            return customAttributeProvider.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }
    }
}
