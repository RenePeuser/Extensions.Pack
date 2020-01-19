using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Extensions.Pack
{
    /// <summary>Represents the unit tests for the <see cref="MethodInfo" /> class.</summary>
    public static class MethodInfoExtensions
    {
        /// <summary>Determines whether the method is an async method which returns a <see cref="Task" />.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns><c>true</c> if method is async; otherwise, <c>false</c>.</returns>
        public static bool IsAsyncMethod(this MethodInfo methodInfo)
        {
            Throw.IfNull(() => methodInfo);

            return methodInfo.ReturnType.IsTask();
        }

        /// <summary>Determines whether the method is not an async method which returns a <see cref="Task" />.</summary>
        public static bool IsNotAsyncMethod(this MethodInfo methodInfo)
        {
            Throw.IfNull(() => methodInfo);

            return methodInfo.ReturnType.IsTask().IsFalse();
        }

        /// <summary>Helps to invoke a generic methods without knowing internals.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns><c>true</c> if method is async; otherwise, <c>false</c>.</returns>
        public static object InvokeGeneric(this MethodInfo methodInfo, Type genericType, object[] arguments)
        {
            Throw.IfNull(() => methodInfo);

            return methodInfo.InvokeGeneric(null, genericType, arguments);
        }

        /// <summary>Helps to invoke a generic methods without knowing internals.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns><c>true</c> if method is async; otherwise, <c>false</c>.</returns>
        public static object InvokeGeneric(this MethodInfo methodInfo, object instance, Type genericType, object[] arguments)
        {
            Throw.IfNull(() => methodInfo);

            var genericMethod = methodInfo.MakeGenericMethod(genericType);

            return genericMethod.Invoke(instance, arguments);
        }

        /// <summary>Helps to invoke a generic methods without knowing internals.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns><c>true</c> if method is async; otherwise, <c>false</c>.</returns>
        public static object InvokeGeneric(this MethodInfo methodInfo, Type genericType)
        {
            Throw.IfNull(() => methodInfo);

            return methodInfo.InvokeGeneric(genericType, new object[] { });
        }

        /// <summary>Gets the parameters of a method info as an enumeration of <see cref="ParameterExpression" />.</summary>
        /// <param name="methodInfo">The methodInfo.</param>
        /// <returns>IEnumerable&lt;ParameterExpression&gt;.</returns>
        public static IEnumerable<ParameterExpression> GetParamsAsExpressions(this MethodInfo methodInfo)
        {
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                yield return Expression.Parameter(parameterInfo.ParameterType, parameterInfo.Name);
            }
        }
    }
}
