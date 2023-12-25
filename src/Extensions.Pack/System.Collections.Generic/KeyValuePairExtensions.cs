using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>The key value pair extensions.</summary>
    public static class KeyValuePairExtensions
    {
        /// <summary>Returns a <see cref="string" /> that represents the name of the right operand of the original expression and the result value from the invoked compiled function.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="compiledExpression">The compiled expression.</param>
        /// <param name="argument">The argument to invoke the function.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public static string ToString<T>(this KeyValuePair<string, Func<T, object>> compiledExpression, T argument)
        {
            Throw.IfNull<object>(argument);

            var result = string.Format(CultureInfo.InvariantCulture, "{0}[{1}] ", compiledExpression.Key, compiledExpression.Value.Invoke(argument));

            return result;
        }

        /// <summary>Converts an enumeration of key value pair to a dictionary.</summary>
        /// <param name="keyValuePairs">The enumeration of key value pairs.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>A dictionary.</returns>
        public static OrderedDictionary ToOrderedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            Throw.IfNull(keyValuePairs);

            var result = Create<OrderedDictionary, TKey, TValue>(keyValuePairs);

            return result;
        }

        /// <summary>Converts an enumeration of key value pair to a <see cref="ListDictionary" />.</summary>
        /// <param name="keyValuePairs">The enumeration of key value pairs.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>A dictionary.</returns>
        public static ListDictionary ToListDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            Throw.IfNull(keyValuePairs);

            var result = Create<ListDictionary, TKey, TValue>(keyValuePairs);

            return result;
        }

        /// <summary>Converts an enumeration of key value pair to a <see cref="HybridDictionary" />.</summary>
        /// <param name="keyValuePairs">The enumeration of key value pairs.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>A dictionary.</returns>
        public static HybridDictionary ToHybridDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            Throw.IfNull(keyValuePairs);

            var result = Create<HybridDictionary, TKey, TValue>(keyValuePairs);

            return result;
        }

        /// <summary>Converts an enumeration of key value pair to a <see cref="StringDictionary" />.</summary>
        /// <param name="keyValuePairs">The enumeration of key value pairs.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>A dictionary.</returns>
        public static StringDictionary ToStringDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            Throw.IfNull(keyValuePairs);

            var stringDictionary = new StringDictionary();
            keyValuePairs.ForEach(item => stringDictionary.Add(item.Key!.ToString()!, item.Value!.ToString()!));

            return stringDictionary;
        }

        /// <summary>Converts an enumeration of key value pair to a <see cref="NameValueCollection" />.</summary>
        /// <param name="keyValuePairs">The enumeration of key value pairs.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>A dictionary.</returns>
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            Throw.IfNull(keyValuePairs);

            var nameValueCollection = new NameValueCollection();
            keyValuePairs.ForEach(item => nameValueCollection.Add(item.Key!.ToString(), item.Value!.ToString()));

            return nameValueCollection;
        }

        /// <summary>Creates the specific dictionary type which is needed.</summary>
        /// <typeparam name="TDictionary">The generic type of the specific <see cref="IDictionary" />.</typeparam>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <param name="keyValuePairs">The source <see cref="KeyValuePair{TKey, TValue}" /> which has to be created to a specific <see cref="Dictionary{TKey, TValue}" />.</param>
        /// <returns>The created type .</returns>
        private static TDictionary Create<TDictionary, TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
            where TDictionary : IDictionary, new()
        {
            var expectedDicitonary = new TDictionary();
            keyValuePairs.ForEach(item => expectedDicitonary.Add(item.Key!.ToString()!, item.Value!.ToString()));

            return expectedDicitonary;
        }
    }
}
