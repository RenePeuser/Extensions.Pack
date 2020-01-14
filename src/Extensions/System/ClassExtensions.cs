using System;
using System.Linq.Expressions;

namespace Extensions
{
    /// <summary>Represents the extension methods for the all generic types.</summary>
    public static class TcClassExtensions
    {
        /// <summary>Extract the name of a property, method and so on.</summary>
        /// <param name="source">The class from which the expected name have to be extracted.</param>
        /// <param name="expression">The expression for the property or method and so on.</param>
        /// <typeparam name="TClass">Generic type for the class source object.</typeparam>
        /// <typeparam name="TNameOf">Generic type of the expected property or method which have to be extracted.</typeparam>
        /// <returns>The name of the expected property.</returns>
        /// <remarks>Argument checking is not necessary, because this extension calls another extension which do argument checking.</remarks>
        public static string NameOf<TClass, TNameOf>(this TClass source, Expression<Func<TClass, TNameOf>> expression)
            where TClass : class
        {
            return expression.NameOf();
        }
    }
}
