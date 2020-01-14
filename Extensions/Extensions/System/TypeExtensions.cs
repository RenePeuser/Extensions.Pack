using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>Extension class for <see cref="Type" /> extensions.</summary>
    public static class TcTypeExtensions
    {
        private const BindingFlags EXPECTED_BINDING_FLAGS =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly IEnumerable<Type> sDictionaryGenericTypeDefinitions = new[] { typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>) };

        /// <summary>Enumeration of available type definitions of <see cref="Action" />.</summary>
        private static readonly IEnumerable<Type> sActionDeclarations = new[] { typeof(Action), typeof(Action<>), typeof(Action<,>), typeof(Action<,,>), typeof(Action<,,,>), typeof(Action<,,,,>), typeof(Action<,,,,,>), typeof(Action<,,,,,,>), typeof(Action<,,,,,,,>), typeof(Action<,,,,,,,,>), typeof(Action<,,,,,,,,,>), typeof(Action<,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,,,>) };

        /// <summary>Enumeration of available type definitions of <see cref="Func{TResult}" />.</summary>
        private static readonly IEnumerable<Type> sFuncDeclarations = new[] { typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>), typeof(Func<,,,,,>), typeof(Func<,,,,,,>), typeof(Func<,,,,,,,>), typeof(Func<,,,,,,,,>), typeof(Func<,,,,,,,,,>), typeof(Func<,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,,>) };

        /// <summary>Check if the type is decorated with <see cref="ImmutableObjectAttribute" />.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Indicates whether the type is decorated with ImmutableAttribute.</returns>
        public static bool IsImmutable(this Type type)
        {
            Throw.IfNull(() => type);

            var result = CustomAttributeExtensions.GetCustomAttribute<ImmutableObjectAttribute>(type);

            if (result == null)
            {
                return false;
            }

            return result.Immutable;
        }

        /// <summary>Check if the type is not decorated with <see cref="ImmutableObjectAttribute" />.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Indicates whether the type is decorated with ImmutableAttribute.</returns>
        public static bool IsNotImmutable(this Type type)
        {
            Throw.IfNull(() => type);

            return !IsImmutable(type);
        }

        /// <summary>Check a type for <see cref="CLSCompliantAttribute" />.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Indicates whether the type is CLS compliant. <see cref="Type" />.</returns>
        public static bool IsClsCompliant(this Type type)
        {
            Throw.IfNull(() => type);

            var attribute = CustomAttributeExtensions.GetCustomAttribute<CLSCompliantAttribute>(type);

            if (attribute == null)
            {
                return false;
            }

            return attribute.IsCompliant;
        }

        /// <summary>Checks if the this class is a sub type of a specific type.</summary>
        /// <param name="type">The source type.</param>
        /// <typeparam name="T">The generic type for check.</typeparam>
        /// <returns><c>True</c> if <c>T</c>; is a subclass of the type; otherwise <c>false</c>.</returns>
        public static bool IsSubClassOf<T>(this Type type)
        {
            Throw.IfNull(() => type);

            return type.IsSubclassOf(typeof(T));
        }

        /// <summary>Checks if an object is a specific type.</summary>
        /// <param name="type">The source type to check.</param>
        /// <typeparam name="T">Generic type for the check.</typeparam>
        /// <returns>True, if it is the specific type.</returns>
        public static bool IsTypeOf<T>(this Type type)
        {
            Throw.IfNull(() => type);

            return type == typeof(T);
        }

        /// <summary>Determines whether this <see cref="Type" /> is <see cref="Action{TResult}" />.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>True</c> if <c>Type</c>; is a <see cref="Action{TResult}" />; otherwise <c>false</c>.</returns>
        public static bool IsTask(this Type type)
        {
            Throw.IfNull(() => type);

            return typeof(Task).IsAssignableFrom(type);
        }

        /// <summary>Determines whether this <see cref="Type" /> is undefined.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>True</c> if <c>Type</c>; is undefined; otherwise <c>false</c>.</returns>
        public static bool IsUndefined(this Type type)
        {
            Throw.IfNull(() => type);

            if (type.FullName == null)
            {
                return true;
            }

            var genericArguments = type.GetGenericArguments();

            return genericArguments.Any(item => item.IsUndefined());
        }

        /// <summary>Gets the constructor of a specific type.</summary>
        /// <param name="type">The source type which has to be created which has to be of type of <c>T</c>.</param>
        /// <returns>The <see cref="ConstructorInfo" />.</returns>
        public static ConstructorInfo GetConstructor(this Type type)
        {
            Throw.IfNull(() => type);

            var constructor = type.GetConstructors(EXPECTED_BINDING_FLAGS).FirstOrDefault();

            return constructor;
        }

        /// <summary>Determines whether the specified type is undefined.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <returns><c>true</c> if the specified type is a system type; otherwise, <c>false</c>.</returns>
        public static bool IsSystemType(this Type type)
        {
            Throw.IfNull(() => type);

            return type.FullName.StartWith(nameof(System));
        }

        /// <summary>Determines whether the specified type is static.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <returns><c>true</c> if the specified type is static; otherwise, <c>false</c>.</returns>
        public static bool IsStatic(this Type type)
        {
            Throw.IfNull(() => type);

            return type.IsClass && type.IsAbstract && type.IsSealed;
        }

        /// <summary>Determines whether type is an <see cref="IEnumerable" />.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <returns><c>true</c> if type is <see cref="IEnumerable" />; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type type)
        {
            Throw.IfNull(() => type);

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>Checks if a specific interface type is an <see cref="IDictionary" />.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <returns>The expected interface, if it exists. <see cref="Type" />.</returns>
        public static bool IsDictionary(this Type type)
        {
            Throw.IfNull(() => type);

            if (type == typeof(IDictionary))
            {
                return true;
            }

            if (IsInterfaceImplemented<IDictionary>(type))
            {
                return true;
            }

            if (type.IsGenericType)
            {
                var genericArguments = type.GenericTypeArguments;

                if (genericArguments.Length != 2)
                {
                    return false;
                }

                var keyValuePair = typeof(KeyValuePair<,>).MakeGenericType(type.GenericTypeArguments);
                var genericKeyValuePair = typeof(IEnumerable<>).MakeGenericType(keyValuePair);

                if (type.GetInterfaces().Contains(genericKeyValuePair))
                {
                    return true;
                }

                return sDictionaryGenericTypeDefinitions.Contains(type.GetGenericTypeDefinition());
            }

            return false;
        }

        /// <summary>Checks if a specific interface type is an <see cref="IEnumerator" />.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <returns>The expected interface, if it exists.</returns>
        public static bool IsEnumerator(this Type type)
        {
            Throw.IfNull(() => type);

            return IsInterfaceImplemented<IEnumerator>(type) || type == typeof(IEnumerator);
        }

        /// <summary>Checks if a specific interface type is implemented.</summary>
        /// <param name="type">The type which has to be checked for expectation.</param>
        /// <typeparam name="T">Generic type of the expected interface.</typeparam>
        /// <returns>The expected interface, if it exists. <see cref="Type" />.</returns>
        public static bool IsInterfaceImplemented<T>(this Type type)
            where T : class
        {
            Throw.IfNull(() => type);

            var result = GetInterface<T>(type);

            return result != null;
        }

        /// <summary>Get an expected interface form a specific type.</summary>
        /// <param name="type">The source type which has to be created which has to be of type of <c>T</c>.</param>
        /// <typeparam name="T">Generic type of the expected interface.</typeparam>
        /// <returns>The expected interface, if it exists. <see cref="Type" />.</returns>
        public static Type GetInterface<T>(this Type type)
            where T : class
        {
            Throw.IfNull(() => type);

            var genericType = typeof(T);
            var result = type.GetInterface(genericType.Name);

            return result;
        }

        /// <summary>Creates an <see cref="Array" /> depending on the <see cref="Type" />.</summary>
        /// <param name="type">The source type to extract all properties.</param>
        /// <param name="length">The length of the expected array.</param>
        /// <returns>The <see cref="Array" /> with the expected length.</returns>
        public static Array ToArray(this Type type, uint length)
        {
            Throw.IfNull(() => type);

            return Array.CreateInstance(type, length);
        }

        /// <summary>Gets all properties form a specific type.</summary>
        /// <param name="type">The source type to extract all properties.</param>
        /// <returns>The enumeration of <see cref="PropertyInfo" />.</returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            Throw.IfNull(() => type);

            var properties = type.GetProperties();
            var inheritInterfaces = type.GetInterfaces();
            var inheritProperties = inheritInterfaces.SelectMany(item => item.GetProperties());
            var allProperties = properties.Concat(inheritProperties);

            return allProperties;
        }

        /// <summary>Gets all properties form a specific type.</summary>
        /// <param name="type">The source type to extract all properties.</param>
        /// <returns>The enumeration of <see cref="PropertyInfo" />.</returns>
        public static bool IsDelegate(this Type type)
        {
            Throw.IfNull(() => type);

            return typeof(Delegate).IsAssignableFrom(type);
        }

        /// <summary>Gets all methods form a specific type.</summary>
        /// <param name="type">The source type to extract all properties.</param>
        /// <returns>The enumeration of <see cref="PropertyInfo" />.</returns>
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            Throw.IfNull(() => type);

            var methods = type.GetMethods().Where(m => m.ReturnType != typeof(void) && !m.IsSpecialName);
            var inheritInterfaces = type.GetInterfaces();
            var inheritMethods = inheritInterfaces.SelectMany(item => item.GetMethods().Where(m => m.ReturnType != typeof(void) && !m.IsSpecialName));
            var allMethods = methods.Concat(inheritMethods);

            return allMethods;
        }

        /// <summary>Get constructor with the max parameters Normally all constructors should be implemented for better maintainability that the last constructor has the most arguments => HighLander-Principle.</summary>
        /// <param name="type">The source type to extract all properties.</param>
        /// <returns>The enumeration of <see cref="PropertyInfo" />.</returns>
        public static ConstructorInfo GetConstructorWithMaxParameters(this Type type)
        {
            Throw.IfNull(() => type);

            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Select(c => new { Constructur = c, Parameters = c.GetParameters() })
                .OrderBy(c => c.Parameters.Length)
                .ToList();
            var constructorWithMaxParameters = constructors.Last().Constructur;

            return constructorWithMaxParameters;
        }

        /// <summary>Determines whether the specified type is a <see cref="Func{TResult}" />.</summary>
        /// <param name="type">The type to check for type of <see cref="Action" />.</param>
        /// <returns><c>true</c> if the specified type is <see cref="Func{TResult}" />; otherwise, <c>false</c>.</returns>
        public static bool IsFunc(this Type type)
        {
            Throw.IfNull(() => type);

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            return sFuncDeclarations.Contains(genericTypeDefinition);
        }

        /// <summary>Invokes the generic method.</summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="arguments">The arguments type info.</param>
        /// <returns>The return value of the invoked method. </returns>
        public static object InvokeGenericMethod(this Type classType, string methodName, Type[] argumentTypes, params object[] arguments)
        {
            Throw.IfNull(() => classType);
            Throw.IfNullOrWhiteSpace(() => methodName);
            Throw.IfNull(() => argumentTypes);
            Throw.IfNull(() => arguments);

            var expectedMethod = classType.GetMethods(EXPECTED_BINDING_FLAGS).First(m => m.Name == methodName);
            var genericMethod = expectedMethod.MakeGenericMethod(argumentTypes);
            var result = genericMethod.Invoke(null, arguments);

            return result;
        }

        /// <summary>Invokes the generic method.</summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="arguments">The arguments type info.</param>
        /// <returns>The return value of the invoked method.</returns>
        public static object InvokeGenericMethod<TClass>(this TClass source, string methodName, Type argumentTypes, params object[] arguments)
            where TClass : class
        {
            Throw.IfNull(() => source);
            Throw.IfNullOrWhiteSpace(() => methodName);
            Throw.IfNull(() => argumentTypes);
            Throw.IfNull(() => arguments);

            var expectedMethod = typeof(TClass).GetMethods(EXPECTED_BINDING_FLAGS).First(m => m.Name == methodName);
            var genericMethod = expectedMethod.MakeGenericMethod(argumentTypes);
            var result = genericMethod.Invoke(source, arguments);

            return result;
        }

        /// <summary>Invokes a method where parameter length is as expected.</summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="arguments">The method parameters.</param>
        /// <returns>The return value of the invoked method. </returns>
        public static object InvokeExpectedMethod(this Type classType, string methodName, Type[] argumentTypes, params object[] arguments)
        {
            Throw.IfNull(() => classType);
            Throw.IfNullOrWhiteSpace(() => methodName);
            Throw.IfNull(() => argumentTypes);
            Throw.IfNull(() => arguments);

            var argumentsTypes = arguments.Select(a => a.GetType());

            var expectedMethod = classType.GetMethods(EXPECTED_BINDING_FLAGS)
                .First(item => item.Name == methodName && item.GetParameters().Select(p => p.ParameterType).SequenceEqualsTo(argumentsTypes));
            var genericMethod = expectedMethod.MakeGenericMethod(argumentTypes);
            var result = genericMethod.Invoke(null, arguments);

            return result;
        }

        /// <summary>Determines whether the specified type is an <see cref="Action" />.</summary>
        /// <param name="type">The type to check for type of <see cref="Action" />.</param>
        /// <returns><c>true</c> if the specified type is <see cref="Action" />; otherwise, <c>false</c>.</returns>
        internal static bool IsAction(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type == typeof(Action);
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            return sActionDeclarations.Contains(genericTypeDefinition);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit = false) where T : Attribute
        {
            Throw.IfNull(() => type);

            var attributes = type.GetCustomAttributes(typeof(T), inherit).ToListOfType<T>();
            return attributes;
        }

        public static bool HasCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            Throw.IfNull(() => type);

            var attribute = type.GetCustomAttribute<T>(inherit);
            var hasCustomAttribute = attribute != null;
            return hasCustomAttribute;
        }

        public static T GetCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }
    }
}
