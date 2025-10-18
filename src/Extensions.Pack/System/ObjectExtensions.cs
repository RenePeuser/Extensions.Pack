using System.Diagnostics.CodeAnalysis;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Represents the extensions for the <see cref="object" />.</summary>
    public static class ObjectExtensions
    {
        /// <summary>Get all attributes from a specific object.</summary>
        /// <param name="source">The source object.</param>
        /// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks.</param>
        /// <typeparam name="T">Generic object of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static T? GetCustomAttribute<T>(this object source,
                                               bool inherit = false)
            where T : Attribute
        {
            Throw.IfNull(source);

            return CustomAttributeProviderExtensions.GetCustomAttribute<T>(source.GetType(), inherit);
        }

        /// <summary>Get all attributes from a specific object.</summary>
        /// <param name="source">The source object.</param>
        /// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks.</param>
        /// <typeparam name="T">Generic object of the expected attribute to find.</typeparam>
        /// <returns>An enumeration of the expected attributes.</returns>
        public static bool HasCustomAttribute<T>(this object source,
                                                 bool inherit = false)
            where T : Attribute
        {
            Throw.IfNull(source);

            return CustomAttributeProviderExtensions.HasCustomAttribute<T>(source.GetType(), inherit);
        }

        /// <summary>Checks it the object is decorated with immutable attribute with true.</summary>
        /// <param name="source">The source.</param>
        /// <returns>The expected interface, if it exists. <see cref="Type" />.</returns>
        public static bool IsImmutable(this object source)
        {
            Throw.IfNull(source);

            return source.GetType().IsImmutable();
        }

        /// <summary>Compares object for not reference equality, also if they are null.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool NotRefEquals(this object source,
                                        object target)
        {
            // Argument checking is not necessary because call extension on null object is allowed => RefrenceEquals will be called
            return !source.RefEquals(target);
        }

        /// <summary>Compares object for reference equality, also if they are null.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool RefEquals(this object source,
                                     object target)
        {
            // Argument checking is not necessary because compare null objects are valid
            return ReferenceEquals(source, target);
        }

        /// <summary>Cast an object to a specific type safely.</summary>
        /// <param name="source">The source object which have to be casted.</param>
        /// <typeparam name="T">The generic type which will be expected.</typeparam>
        /// <returns><c>T</c> of the safe casted object; otherwise <c>null</c>.</returns>
        public static T? As<T>(this object? source)
        {
            if (source is T result)
            {
                return result;
            }

            return default;
        }

        /// <summary>Cast an object to a specific type unsafe.</summary>
        /// <param name="source">The source object which have to be casted.</param>
        /// <typeparam name="T">The generic type which will be expected.</typeparam>
        /// <returns><c>T</c> of the safe casted object; otherwise <c>null</c>.</returns>
        public static T Cast<T>(this object source)
        {
            Throw.IfNull(source);

            return (T)source;
        }

        /// <summary>Checks whether a source object is of a given type.</summary>
        /// <param name="source">The source object that has to be checked.</param>
        /// <typeparam name="T">The expected generic type.</typeparam>
        public static bool Is<T>(this object source)
        {
            return source is T;
        }

        /// <summary>Checks whether a source object is not of a given type.</summary>
        /// <param name="source">The source object that has to be checked.</param>
        /// <typeparam name="T">The expected generic type.</typeparam>
        public static bool IsNot<T>(this object source)
        {
            return Is<T>(source).Negate();
        }

        /// <summary>Checks if an object is a specific type.</summary>
        /// <param name="source">The source object to check.</param>
        /// <typeparam name="T">Generic type for the check.</typeparam>
        /// <returns><c>True</c> if object is the expected type; otherwise <c>false</c>.</returns>
        public static bool IsTypeOf<T>(this object source)
        {
            Throw.IfNull(source);

            return source.GetType().IsTypeOf<T>();
        }

        /// <summary>Checks if an object is not a specific type.</summary>
        /// <param name="source">The source object to check.</param>
        /// <typeparam name="T">Generic type for the check.</typeparam>
        /// <returns><c>True</c> if object is the expected type; otherwise <c>false</c>.</returns>
        public static bool IsNotTypeOf<T>(this object source)
        {
            Throw.IfNull(source);

            return !source.IsTypeOf<T>();
        }

        /// <summary>Checks if the this class is a sub type of a specific type.</summary>
        /// <param name="source">The source type.</param>
        /// <typeparam name="T">The generic type for check.</typeparam>
        /// <returns><c>True</c> if <c>T</c>; is a subclass of the type; otherwise <c>false</c>.</returns>
        public static bool IsSubClassOf<T>(this object source)
        {
            Throw.IfNull(source);

            return source.GetType().IsSubclassOf(typeof(T));
        }

        /// <summary>Checks if a specific interface type is implemented.</summary>
        /// <param name="source">The source object.</param>
        /// <typeparam name="T">Generic type of the expected interface.</typeparam>
        /// <returns><c>True</c> if <c>T</c>; implements the interface type of <c>T</c>; otherwise <c>false</c>.</returns>
        public static bool IsInterfaceImplemented<T>(this object source)
            where T : class
        {
            Throw.IfNull(source);

            return source.GetType().IsInterfaceImplemented<T>();
        }

        /// <summary>Determines whether the specified source is not null.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the source is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull([NotNullWhen(true)] this object? source)
        {
            return source is not null;
        }

        /// <summary>Determines whether the specified source is null.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the source is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull([NotNullWhen(false)] this object? source)
        {
            // No argument checking here, because the extension checks for null.
            return source is null;
        }

        /// <summary>If the given object is the specified type, the <paramref name="action" /> is executed, otherwise nothing happens.</summary>
        /// <typeparam name="TType">The type to check for.</typeparam>
        /// <param name="source">The object which should be checked.</param>
        /// <param name="action">The action to execute.</param>
        public static void IfType<TType>(this object source,
                                         Action<TType?> action)
            where TType : class
        {
            Throw.IfNull(action);

            var expectedType = source.As<TType>();

            expectedType.IfNotNullThen(() => action(expectedType));
        }
    }
}
