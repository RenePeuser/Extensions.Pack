using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    /// <summary>Represents the extension methods for <see cref="Dictionary{TKey,TValue}" />.</summary>
    public static class TcDictionaryExtensions
    {
        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="key">The key for which the expected value have to look for.</param>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <returns>The value of the expected key.</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var result = dictionary.GetValueOrDefaults(key, default);

            if (result.Exists.IsFalse())
            {
                throw new ArgumentException("Value for expected key does not exists.");
            }

            return result.Value;
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="key">The key for which the expected value have to look for.</param>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <returns>The value of the expected key.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => key);

            return dictionary.GetValueOrDefaults(key, default).Value;
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="key">The key for which the expected value have to look for.</param>
        /// <param name="defaultValue">The custom default value.</param>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <returns>The value of the expected key.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => key);

            return dictionary.GetValueOrDefaults(key, defaultValue).Value;
        }

        /// <summary>Gets the key.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns>The generic key of the dictionary.</returns>
        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => value);

            var result = dictionary.GetKeyOrDefaultInternal(value, default);

            if (result.Exists.IsFalse())
            {
                throw new ArgumentException("Values of dictionary does not contains expected value.");
            }

            return result.Value;
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="key">The key for which the expected value have to look for.</param>
        /// <param name="defaultValue">The custom default value.</param>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <returns>The value of the expected key.</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => key);

            var result = dictionary.GetValueOrDefaults(key, defaultValue);

            if (result.Exists.IsFalse())
            {
                throw new ArgumentException("Values of dictionary does not contains expected value.");
            }

            return result.Value;
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The custom default value.</param>
        /// <returns>The value of the expected key.</returns>
        public static TKey GetKeyOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, TKey defaultValue)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => value);

            return dictionary.GetKeyOrDefaultInternal(value, defaultValue).Value;
        }

        /// <summary>Determines whether the specified value contains value.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value contains value; otherwise, <c>false</c>.</returns>
        public static bool ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => value);

            return dictionary.Values.Contains(value);
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The custom default value.</param>
        /// <returns>The value of the expected key.</returns>
        private static TcDictionaryResult<TKey> GetKeyOrDefaultInternal<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TValue value,
            TKey defaultValue)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => value);

            var contains = dictionary.ContainsValue(value);
            var result = contains ? dictionary.First(item => item.Value.EqualsTo(value)).Key : defaultValue;

            return new TcDictionaryResult<TKey>(result, contains);
        }

        /// <summary>Its the same method which use the dictionary class internally.</summary>
        /// <typeparam name="TKey">The generic type of the key.</typeparam>
        /// <typeparam name="TValue">The generic type of the value.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="key">The key for which the expected value have to look for.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value of the expected key.</returns>
        private static TcDictionaryResult<TValue> GetValueOrDefaults<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue)
        {
            Throw.IfNull(() => dictionary);
            Throw.IfNull<object>(() => key);

            TValue result;
            var exists = dictionary.TryGetValue(key, out result);

            if (!exists)
            {
                result = defaultValue;
            }

            return new TcDictionaryResult<TValue>(result, exists);
        }

        /// <summary>Representing the generic dictionary result.</summary>
        /// <typeparam name="T">Generic type.</typeparam>
        public class TcDictionaryResult<T>
        {
            /// <summary>Initializes a new instance of the <see cref="TcDictionaryResult{T}" /> class.</summary>
            /// <param name="value">The value.</param>
            /// <param name="exists">If set to <c>true</c> [exists].</param>
            internal TcDictionaryResult(T value, bool exists)
            {
                Value = value;
                Exists = exists;
            }

            /// <summary>Gets the value.</summary>
            /// <value>The value.</value>
            public T Value { get; }

            /// <summary>Gets a value indicating whether this <see cref="TcDictionaryResult{T}" /> is exists.</summary>
            /// <value><c>true</c> if exists; otherwise, <c>false</c>.</value>
            public bool Exists { get; }
        }
    }
}
