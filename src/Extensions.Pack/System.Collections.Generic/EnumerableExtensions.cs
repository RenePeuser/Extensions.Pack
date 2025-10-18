using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>Extension class for <see cref="IEnumerable{T}" /> extensions.</summary>
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> FilterNullOrWhitespace(this IEnumerable<string> source)
        {
            Throw.IfNull(source);

            return source.Where(s => s.IsNotNullOrWhiteSpace());
        }

        public static IEnumerable<TResult> AllTypesAre<TResult>(this IEnumerable source)
        {
            Throw.IfNull(source);

            foreach (var item in source)
            {
                // Hint not use 'item is TResult', because here we want explicit type not the derived type !!!
                if (item.GetType() == typeof(TResult))
                {
                    yield return (TResult)item;
                }
            }
        }

        public static IEnumerable<TResult> AllTypesAreEqualsTo<TResult>(this IEnumerable source)
        {
            Throw.IfNull(source);

            foreach (var item in source)
            {
                // Hint not use 'item is TResult', because here we want explicit type not the derived type !!!
                if (item.GetType() == typeof(TResult))
                {
                    yield return (TResult)item;
                }
            }
        }

        /// <summary>Filters out the objects that are <c>null</c> from an enumeration.</summary>
        /// <typeparam name="T">Generic type for the enumeration.</typeparam>
        /// <param name="source">The enumeration to be filtered.</param>
        /// <returns>The filtered enumeration.</returns>
        public static IEnumerable<T> FilterNullObjects<T>(this IEnumerable<T?> source)
            where T : class
        {
            Throw.IfNull(source);

            return source.Where(item => item is not null)!;
        }

        /// <summary>Returns the first element in a sequence that satisfies a specified condition.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable" /> to return an element from.</param>
        /// <returns>The first element in the sequence that passes the test in the specified predicate function.</returns>
        public static TSource FirstOfType<TSource>(this IEnumerable source)
        {
            Throw.IfNull(source);

            var result = source.FirstOrDefaultOfType<TSource>();

            if (result.IsNull())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Enumeration does not contains any item of the specific type: {0}",
                                                                  typeof(TSource).Name));
            }

            return result;
        }

        /// <summary>Checks if enumeration contains an item of a specific type.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c> if enumeration contains an item of the expected type; otherwise <c>false</c>.</returns>
        public static bool AnyOfType<TSource>(this IEnumerable source)
            where TSource : class
        {
            Throw.IfNull(source);

            var result = source.FirstOrDefaultOfType<TSource>();

            return result.IsNotNull();
        }

        /// <summary>The first or default of type.</summary>
        /// <returns>The first object of the enumeration or null.</returns>
        /// <param name="source">An <see cref="IEnumerable" /> to return an element from.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        public static TSource? FirstOrDefaultOfType<TSource>(this IEnumerable source)
        {
            Throw.IfNull(source);

            return source.OfType<TSource>().FirstOrDefault();
        }

        /// <summary>Returns the first element in a sequence that satisfies a specified condition.</summary>
        /// <returns>The first element in the sequence that passes the test in the specified predicate function.</returns>
        /// <param name="source">An <see cref="IEnumerable" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
        public static TSource FirstOfType<TSource>(this IEnumerable source,
                                                   Func<TSource, bool> predicate)
        {
            Throw.IfNull(source);
            Throw.IfNull(predicate);

            var result = source.FirstOrDefaultOfType(predicate);

            if (result.IsNull())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Enumeration does not contains any item of the specific type: {0}",
                                                                  typeof(TSource).Name));
            }

            return result;
        }

        /// <summary>The first or default of type.</summary>
        /// <returns>The first object of the enumeration or null.</returns>
        /// <param name="source">An <see cref="IEnumerable" /> to return an element from.</param>
        /// <param name="predicate">The predicate.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        public static TSource? FirstOrDefaultOfType<TSource>(this IEnumerable source,
                                                             Func<TSource, bool> predicate)
        {
            Throw.IfNull(source);
            Throw.IfNull(predicate);

            return source.OfType<TSource>().FirstOrDefault(predicate);
        }

        /// <summary>Performs the specified action on each element of the <see cref="IEnumerable{T}" />.</summary>
        /// <typeparam name="TSource">Generic Type for the expected type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="Action" /> delegate to perform on each element of the <see cref="IEnumerable{T}" />.</param>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
                                                            Action<TSource> action)
        {
            Throw.IfNull(source);
            Throw.IfNull(action);

            var sourceList = source.ToList();
            sourceList.ForEach(action);

            return sourceList;
        }

        /// <summary>Performs the specified action on each element and each index of the <see cref="IEnumerable{T}" />.</summary>
        /// <typeparam name="TSource">Generic Type for the expected type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="Action" /> delegate to perform on each element of the <see cref="IEnumerable{T}" /> and index.</param>
        public static void ForEachIndex<TSource>(this IEnumerable<TSource> source,
                                                 Action<TSource, int> action)
        {
            Throw.IfNull(source);
            Throw.IfNull(action);

            var index = 0;

            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        /// <summary>Performs the specified action on each element of the <see cref="IEnumerable" />.</summary>
        /// <typeparam name="TSource">Generic Type for the expected type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="Action" /> delegate to perform on each element of the <see cref="IEnumerable" />.</param>
        public static IEnumerable<TSource> ForEachOfType<TSource>(this IEnumerable source,
                                                                  Action<TSource> action)
        {
            Throw.IfNull(source);
            Throw.IfNull(action);

            var sourceList = source.OfType<TSource>().ToList();
            sourceList.ForEach(action);

            return sourceList;
        }

        /// <summary>Not sequence equals on 2 enumerations which works also if the enumerations are null.</summary>
        /// <param name="first">The first enumeration for the sequence equal operation.</param>
        /// <param name="second">The second enumeration for the sequence equal operation.</param>
        /// <typeparam name="T">The generic type of the enumerations.</typeparam>
        /// <returns><c>True</c> if both enumerations are equal; otherwise <c>false</c>.</returns>
        public static bool NotSequenceEqualsTo<T>(this IEnumerable<T> first,
                                                  IEnumerable<T> second)
        {
            // no argument checking is needed because SequenceEqualsTo can handle null values.
            return !first.SequenceEqualsTo(second);
        }

        /// <summary>Sequence equals on 2 enumerations which works also if the enumerations are null.</summary>
        /// <param name="first">The first enumeration for the sequence equal operation.</param>
        /// <param name="second">The second enumeration for the sequence equal operation.</param>
        /// <typeparam name="T">The generic type of the enumerations.</typeparam>
        /// <returns><c>True</c> if both enumerations are equal; otherwise <c>false</c>.</returns>
        public static bool SequenceEqualsTo<T>(this IEnumerable<T> first,
                                               IEnumerable<T> second)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            return first.SequenceEqual(second);
        }

        /// <summary>Determines whether two sequences are equal by comparing the elements by using the default equality comparer for their type.</summary>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <typeparam name="TSource">Expected generic type.</typeparam>
        /// <returns>Indicates whether to enumeration are sequence equal.</returns>
        public static bool SequenceEqualsOfType<TSource>(this IEnumerable first,
                                                         IEnumerable second)
        {
            Throw.IfNull(first);
            Throw.IfNull(second);

            return first.OfType<TSource>().SequenceEqual(second.OfType<TSource>());
        }

        /// <summary>Converts a enumeration to a <see cref="ReadOnlyCollection{T}" /> that is immutable, because no one holds the list that is behind the <see cref="ReadOnlyCollection{T}" />.</summary>
        /// <remarks>Uses <c>source.GetEnumValuesOf().AsReadOnly();</c>.</remarks>
        /// <param name="source">The source enumeration.</param>
        /// <typeparam name="T">Generic type of enumeration.</typeparam>
        /// <returns>A read only collection.</returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            return source.ToList().AsReadOnly();
        }

        /// <summary>Performs the specified action on each element of the <see cref="IEnumerable" />.</summary>
        /// <typeparam name="TSource">Generic Type for the expected type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>A list the generic copy from the enumeration <see cref="IEnumerable" />.</returns>
        public static List<TSource> ToListOfType<TSource>(this IEnumerable source)
        {
            Throw.IfNull(source);

            return source.OfType<TSource>().ToList();
        }

        /// <summary>Returns a typed copy list from the source enumeration or if source is null an empty list.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>A list the generic copy from the enumeration. <see cref="IEnumerable" />.</returns>
        public static List<TSource> ToListOfTypeOrEmpty<TSource>(this IEnumerable source)
        {
            if (source == null)
            {
                return new List<TSource>();
            }

            return source.OfType<TSource>().ToList();
        }

        /// <summary>Converts an enumeration to an observable collection.</summary>
        /// <param name="enumeration">The source enumeration.</param>
        /// <typeparam name="T">Generic type for the enumeration.</typeparam>
        /// <returns>The observable collection.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            Throw.IfNull(enumeration);

            return new ObservableCollection<T>(enumeration);
        }

        /// <summary>Converts an enumeration to an observable collection.</summary>
        /// <param name="enumeration">The source enumeration.</param>
        /// <typeparam name="T">Generic type for the enumeration.</typeparam>
        /// <returns>The observable collection.</returns>
        public static ReadOnlyObservableCollection<T> ToReadOnlyObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            Throw.IfNull(enumeration);

            return enumeration.ToObservableCollection().ToReadOnlyObservableCollection();
        }

        /// <summary>Filters a sequence of values based on a predicate.</summary>
        /// <returns>An <see cref="IEnumerable" /> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <param name="source">An <see cref="IEnumerable" /> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IEnumerable<TSource> WhereOfType<TSource>(this IEnumerable source,
                                                                Func<TSource, bool> predicate)
        {
            Throw.IfNull(source);
            Throw.IfNull(predicate);

            return source.OfType<TSource>().Where(predicate);
        }

        /// <summary>Determines whether is any item null.</summary>
        /// <typeparam name="T">The generic type of the source enumeration.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if any item is null; otherwise <c>false</c>.</returns>
        public static bool IsAnyItemNull<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            return source.IsAnyItem(item => item == null);
        }

        /// <summary>Determines whether is any item null.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if any item is null; otherwise <c>false</c>.</returns>
        public static bool IsAnyItemNullOrWhitespace(this IEnumerable<string> source)
        {
            Throw.IfNull(source);

            return source.IsAnyItem(item => item.IsNullOrWhiteSpace());
        }

        /// <summary>Determines whether is any item null.</summary>
        /// <typeparam name="T">The generic type of the source enumeration.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="check">The expected state check.</param>
        /// <returns><c>True</c>if any item is null; otherwise <c>false</c>.</returns>
        public static bool IsAnyItem<T>(this IEnumerable<T> source,
                                        Predicate<T> check)
        {
            Throw.IfNull(source);

            return source.Any(item => check(item));
        }

        /// <summary>Determines whether is any item null.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if any item is null; otherwise <c>false</c>.</returns>
        public static bool IsAnyItemNull(params object[] source)
        {
            Throw.IfNull(source);

            return source.IsNotAnyItemNull();
        }

        /// <summary>Determines whether all items in a enumeration are not null.</summary>
        /// <typeparam name="T">The generic type of the source enumeration.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if all items are not null; otherwise <c>false</c>.</returns>
        public static bool IsNotAnyItemNull<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            return !source.IsAnyItemNull();
        }

        /// <summary>Disposes all object in an enumeration if they implement <see cref="IDisposable" />.</summary>
        /// <typeparam name="T">The generic type of the enumeration.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The disposed items.</returns>
        public static IEnumerable<T> Dispose<T>(this IEnumerable<T> source)
            where T : IDisposable
        {
            Throw.IfNull(source);

            source.ToList().ForEach(item => item.Dispose());

            return source;
        }

        /// <summary>Removes the specified types of an <see cref="Enum" /> type.</summary>
        /// <typeparam name="T">The generic type of am <see cref="Enum" />.</typeparam>
        /// <param name="source">The source list of <see cref="Enum" /> type.</param>
        /// <param name="itemsToRemove">The types to ignore in the list.</param>
        /// <returns>The list without items of the ignored types.</returns>
        public static List<T> Remove<T>(this IEnumerable<T> source,
                                        params T[] itemsToRemove)
        {
            Throw.IfNull(source);
            Throw.IfNull(itemsToRemove);

            var items = source.ToList();
            items.RemoveRange(itemsToRemove);

            return items;
        }

        /// <summary>Removes the first item from a list that satisfies the equality comparer.</summary>
        /// <returns><c>True</c> if the item was successfully removed. Otherwise <c>False</c>.</returns>
        public static bool Remove<T>(this IList<T> source,
                                     T item,
                                     IEqualityComparer<T> equalityComparer)
        {
            Throw.IfNull(source);
            Throw.IfNull(equalityComparer);

            if (source.Contains(item, equalityComparer))
            {
                return source.Remove(source.First(i => equalityComparer.Equals(item, i)));
            }

            return false;
        }

        /// <summary>Checks if all items of an array of types are contained in the source enumeration.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expectedItems">The expected items.</param>
        /// <returns><c>True</c>if all items containing; otherwise <c>false</c>.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> source,
                                          params T[] expectedItems)
        {
            Throw.IfNull(source);
            Throw.IfNull(expectedItems);

            return source.ContainsAll(expectedItems.ToList());
        }

        /// <summary>Checks if all items of an array of types are contained in the source enumeration.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expectedItems">The expected items.</param>
        /// <returns><c>True</c>if all items containing; otherwise <c>false</c>.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> source,
                                          IEnumerable<T> expectedItems)
        {
            Throw.IfNull(source);
            Throw.IfNull(expectedItems);

            return expectedItems.All(source.Contains);
        }

        /// <summary>Checks if all items of an array of types are contained in the source enumeration.</summary>
        public static bool ContainsAll<T1, T2, TProperty>(
            this IEnumerable<T1> source,
            IEnumerable<T2> expectedItems,
            Func<T1, TProperty> funcSelector1,
            Func<T2, TProperty> funcSelector2)
        {
            Throw.IfNull(source);
            Throw.IfNull(expectedItems);
            Throw.IfNull(funcSelector1);
            Throw.IfNull(funcSelector2);

            return source.ContainsAllInternal(expectedItems, funcSelector1, funcSelector2);
        }

        /// <summary>Checks if all items of an array of types are contained in the source enumeration.</summary>
        public static bool ContainsAllInternal<T1, T2, TProperty>(
            this IEnumerable<T1> source,
            IEnumerable<T2> expectedItems,
            Func<T1, TProperty> funcSelector1,
            Func<T2, TProperty> funcSelector2)
        {
            var list1 = source.ToList();
            var list2 = expectedItems.ToList();

            var itemsToCompare1 = list1.Select(funcSelector1);
            var itemsToCompare2 = list2.Select(funcSelector2);

            return itemsToCompare1.ContainsAll(itemsToCompare2);
        }

        /// <summary>Checks if at least one item of an array of types is not contained in the source enumeration.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expectedItems">The expected items.</param>
        /// <returns><c>True</c>if at least one item is not in source; otherwise <c>false</c>.</returns>
        public static bool ContainsNotAll<T>(this IEnumerable<T> source,
                                             IEnumerable<T> expectedItems)
        {
            Throw.IfNull(source);
            Throw.IfNull(expectedItems);

            return !source.ContainsAll(expectedItems);
        }

        /// <summary>Checks if any items of an array of types are contained in the source enumeration.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expectedItems">The expected items.</param>
        /// <returns><c>True</c>if all items containing; otherwise <c>false</c>.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source,
                                          params T[] expectedItems)
        {
            Throw.IfNull(source);
            Throw.IfNull(expectedItems);

            return expectedItems.Any(source.Contains);
        }

        /// <summary>Checks if enumeration is empty.</summary>
        /// <typeparam name="T">The generic type of the enumeration.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if the enumeration is empty; otherwise <c>false</c>.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            return !source.Any();
        }

        public static bool AreEmpty<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            return !source.Any();
        }

        /// <summary>Excepts the specified target.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The source.</param>
        /// <param name="second">The target.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A list with the expected items which will be found by the selector.</returns>
        public static IEnumerable<T> Except<T, TProperty>(this IEnumerable<T> first,
                                                          IEnumerable<T> second,
                                                          Func<T, TProperty> selector)
            where T : class
        {
            return first.Except(second, selector, selector);
        }

        /// <summary>Intersects the specified target.</summary>
        public static IEnumerable<T> Intersect<T, TProperty>(this IEnumerable<T> first,
                                                             IEnumerable<T> second,
                                                             Func<T, TProperty> selector)
            where T : class
        {
            return first.Intersect(second, selector, selector);
        }

        /// <summary>Excepts the specified second.</summary>
        /// <typeparam name="T1">The type of the 1 enumeration.</typeparam>
        /// <typeparam name="T2">The type of the 2 enumeration.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector1">The selector for the first enumeration.</param>
        /// <param name="selector2">The selector for the second enumeration.</param>
        /// <returns>An enumeration of items which are a result of an except from the first to the second enumeration.</returns>
        public static IEnumerable<T1> Except<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector1);
            Throw.IfNull(selector2);

            return first.InternalExcept(second, selector1, selector2);
        }

        /// <summary>Intersect the specified second.</summary>
        public static IEnumerable<T1> Intersect<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector1);
            Throw.IfNull(selector2);

            return first.InternalIntersect(second, selector1, selector2);
        }

        /// <summary>Did a sequence equals compare by the values of the enumerations.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The source.</param>
        /// <param name="second">The target.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A list with the expected items which will be found by the selector.</returns>
        public static bool SequenceEqualsTo<T, TProperty>(this IEnumerable<T> first,
                                                          IEnumerable<T> second,
                                                          Func<T, TProperty> selector)
            where T : class
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector);

            return first.SequenceEqualsTo(second, selector, selector);
        }

        /// <summary>Did a sequence equals compare by the values of the enumerations.</summary>
        /// <typeparam name="T1">The type of the 1 enumeration.</typeparam>
        /// <typeparam name="T2">The type of the 2 enumeration.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector1">The selector for the first enumeration.</param>
        /// <param name="selector2">The selector for the second enumeration.</param>
        /// <returns>An enumeration of items which are a result of an except from the first to the second enumeration.</returns>
        public static bool SequenceEqualsTo<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
            where T1 : class where T2 : class
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector1);
            Throw.IfNull(selector2);

            var valueOfFirst = first.Select(selector1).ToList();
            var valueOfSecond = second.Select(selector2).ToList();

            return valueOfFirst.SequenceEqual(valueOfSecond);
        }

        /// <summary>Synchronizes the collection from.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The target.</param>
        /// <param name="second">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The merged collection, where the missing items are not deleted.</returns>
        public static IEnumerable<T> MergeItemsWithoutRemove<T, TProperty>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TProperty> selector)
            where T : class
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector);

            var targetItems = first.ToList();
            var sourceItems = second.ToList();
            var itemsToAdd = sourceItems.Except(targetItems, selector).ToList();

            foreach (var item in itemsToAdd)
            {
                var index = sourceItems.IndexOf(item);
                targetItems.Insert(index, item);
            }

            return targetItems;
        }

        /// <summary>Creates a string from an enumeration of expressions.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="title">The title.</param>
        /// <param name="infoSelector">The information selector.</param>
        /// <returns>A string which was prepared by the info selector.</returns>
        public static string ToString<T>(this IEnumerable<T> source,
                                         string title,
                                         params Expression<Func<T, object>>[] infoSelector)
        {
            Throw.IfNull(source);
            Throw.IfNull(title);
            Throw.IfNull(infoSelector);

            var compiledExpressions = infoSelector.ToCompiledExpressionWithInfo();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(title);
            source.ForEach(item => stringBuilder.AppendLine(item.ToString(compiledExpressions)));

            return stringBuilder.ToString();
        }

        /// <summary>Checks if no item exists in an enumeration from a specific predicate.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="predicate">The predicate which is needed to check against each item.</param>
        /// <returns><c>True</c>if all items not expecting the predicate; otherwise <c>false</c>.</returns>
        public static bool None<T>(this IEnumerable<T> source,
                                   Func<T, bool> predicate)
        {
            Throw.IfNull(source);
            Throw.IfNull(predicate);

            return !source.Any(predicate);
        }

        /// <summary>Checks an array if it contains any of the expected values We made the same also for enumerable, the only reason is we don't have to call toList, because an array can never change.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">Array of source values.</param>
        /// <param name="expectedValues">The expected values to check.</param>
        /// <returns><c>True</c> if the source include at least one value; otherwise, <c>false</c>.</returns>
        public static bool HasAny<T>(this IEnumerable<T> source,
                                     params object[] expectedValues)
        {
            Throw.IfNull(source);

            return source.ToArray().HasAny(expectedValues);
        }

        /// <summary>Filters all double items out by the specific selector.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The enumeration without duplicates which are checked by the selector.</returns>
        public static IEnumerable<T> Distinct<T, TProperty>(this IEnumerable<T> source,
                                                            Func<T, TProperty> selector) where TProperty : notnull
        {
            Throw.IfNull(source);
            Throw.IfNull(selector);

            var dictionary = new Dictionary<TProperty, T>();

            foreach (var item in source)
            {
                var value = selector(item);

                dictionary.TryAdd(value, item);
            }

            return dictionary.Values;
        }

        /// <summary>Joins the specified element to source sequence.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="itemToConcat">The element to join.</param>
        /// <returns>New sequence with appended element.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source,
                                               T itemToConcat)
        {
            Throw.IfNull(source);

            foreach (var item in source)
            {
                yield return item;
            }

            yield return itemToConcat;
        }

        /// <summary>Checks if two sequences have the same items independent of their order.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="list1">The first list.</param>
        /// <param name="list2">The second list.</param>
        /// <returns><c>True</c>if all items containing; otherwise <c>false</c>.</returns>
        public static bool HasSameItems<T>(this IEnumerable<T> list1,
                                           IEnumerable<T> list2)
        {
            Throw.IfNull(list1);
            Throw.IfNull(list2);

            var listCopy1 = list1.ToList();
            var listCopy2 = list2.ToList();

            var result = listCopy1.ContainsAll(listCopy2);
            var sameCount = listCopy1.Count == listCopy2.Count;

            return result && sameCount;
        }

        /// <summary>Excepts the specified second.</summary>
        /// <typeparam name="T1">The type of the 1 enumeration.</typeparam>
        /// <typeparam name="T2">The type of the 2 enumeration.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector1">The selector for the first enumeration.</param>
        /// <param name="selector2">The selector for the second enumeration.</param>
        /// <returns>An enumeration of items which are a result of an except from the first to the second enumeration.</returns>
        public static bool HasSameItems<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            Throw.IfNull(first);
            Throw.IfNull(second);
            Throw.IfNull(selector1);
            Throw.IfNull(selector2);

            return first.InternalHasSameItems(second, selector1, selector2);
        }

        /// <summary>Excepts the specified second.</summary>
        /// <typeparam name="T1">The type of the 1 enumeration.</typeparam>
        /// <typeparam name="T2">The type of the 2 enumeration.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector1">The selector for the first enumeration.</param>
        /// <param name="selector2">The selector for the second enumeration.</param>
        /// <returns>An enumeration of items which are a result of an except from the first to the second enumeration.</returns>
        public static bool InternalHasSameItems<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            var list1 = first.ToList();
            var list2 = second.ToList();

            var infoToCompareList1 = list1.Select(selector1);
            var infoToCompareList2 = list2.Select(selector2);

            return infoToCompareList1.HasSameItems(infoToCompareList2);
        }

        /// <summary>Checks if two sequences do not have the same items independent of their order.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="list1">The first list.</param>
        /// <param name="list2">The second list.</param>
        /// <returns><c>True</c>if all items containing; otherwise <c>false</c>.</returns>
        public static bool HasNotSameItems<T>(this IEnumerable<T> list1,
                                              IEnumerable<T> list2)
        {
            Throw.IfNull(list1);
            Throw.IfNull(list2);

            return !list1.HasSameItems(list2);
        }

        /// <summary>Flattens the specified strings.</summary>
        /// <param name="strings">The strings.</param>
        /// <returns>The strings flattened to a single string.</returns>
        public static string Flatten(this IEnumerable<string> strings)
        {
            Throw.IfNull(strings);

            return strings.Flatten(string.Empty);
        }

        /// <summary>Flattens the specified strings.</summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">A separator, which will be used in between the single values.</param>
        /// <returns>The strings flattened to a single string.</returns>
        public static string Flatten(this IEnumerable<string> strings,
                                     string separator)
        {
            Throw.IfNull(strings);
            Throw.IfNull(separator);

            return string.Join(separator, strings);
        }

        /// <summary>Flattens the specified source enumeration.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The flattened array.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            Throw.IfNull(source);

            return source.SelectMany(s => s);
        }

        /// <summary>Determines if one item is equal to another item in different enumerations.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="master">The master.</param>
        /// <param name="slave">The slave.</param>
        /// <param name="selectors">The selectors.</param>
        /// <returns><c>true</c> if [has one that equals] [the specified slave]; otherwise, <c>false</c>.</returns>
        public static bool HasOneThatEquals<T>(this IEnumerable<T> master,
                                               IEnumerable<T> slave,
                                               params Func<T, object>[] selectors)
            where T : class
        {
            Throw.IfNull(master);
            Throw.IfNull(slave);
            Throw.IfNull(selectors);

            using var masterEnumerator = master.GetEnumerator();
            using var slaveEnumerator = slave.GetEnumerator();

            while (masterEnumerator.MoveNext())
            {
                var allValues = masterEnumerator.GetSelectorResults(selectors).ToList();

                while (slaveEnumerator.MoveNext())
                {
                    var slaveValues = slaveEnumerator.GetSelectorResults(selectors);

                    if (allValues.SequenceEqual(slaveValues))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Checks if enumeration is null or empty.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>True</c>if the enumeration is null or empty; otherwise <c>false</c>.</returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? source)
        {
            if (source.IsNull())
            {
                return true;
            }

            return source.OfType<object>().IsEmpty();
        }

        public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? source)
        {
            if (source.IsNull())
            {
                return false;
            }

            return !source.IsEmpty();
        }

        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
        {
            if (source.IsNull())
            {
                return true;
            }

            return source.IsEmpty();
        }

        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this IEnumerable? source)
        {
            if (source.IsNull())
            {
                return false;
            }

            return source.IsNullOrEmpty().IsFalse();
        }

        /// <summary>Index of.</summary>
        public static int IndexOf<T>(this IEnumerable<T> source,
                                     T item)
        {
            Throw.IfNull(source);

            return source.ToList().IndexOf(item);
        }

        /// <summary>Creates a readable string from a linq grouping.</summary>
        public static string ToString<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groups,
                                                    Expression<Func<TValue, object>> groupKey,
                                                    params Expression<Func<TValue, object>>[] infoSelector)
        {
            var stringBuilder = new StringBuilder();

            foreach (var group in groups)
            {
                stringBuilder.AppendLine($"{groupKey.NameOf()}: {group.Key}");

                foreach (var value in group)
                {
                    foreach (var expression in infoSelector)
                    {
                        stringBuilder.AppendLine($"-> {expression.NameOf()} : {expression.Compile().Invoke(value)}");
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public static IImmutableList<IImmutableList<T>> ChunkImmutable<T>(this IEnumerable<T> source,
                                                                          int chunksize)
        {
            return Chunk(source, chunksize).ToImmutableList();

            static IEnumerable<IImmutableList<T>> Chunk(IEnumerable<T> source,
                                                        int chunksize)
            {
                while (source.Any())
                {
                    yield return source.Take(chunksize).ToImmutableList();

                    source = source.Skip(chunksize);
                }
            }
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> source,
                                           Func<T, bool> predicate,
                                           T defaultValue)
        {
            var result = source.SingleOrDefault(predicate);

            return result.IsNull() ? defaultValue : result;
        }

        /// <summary>Converts an <see cref="IEnumerable{T}" /> to a <see cref="StringCollection" />.</summary>
        /// <typeparam name="T">The generic type, which has to be created.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <returns>The converted <see cref="StringCollection" />.</returns>
        public static StringCollection ToStringCollection<T>(this IEnumerable<T> source)
        {
            Throw.IfNull(source);

            var stringCollection = new StringCollection();
            source.ForEach(item => stringCollection.Add(item!.ToString()));

            return stringCollection;
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source,
                                             int page,
                                             int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static string ToFlattenString<T>(this IEnumerable<T> source,
                                                string separator = "") where T : notnull
        {
            return source.Select(item => item.ToString() ?? string.Empty).Flatten(separator);
        }

        /// <summary>Executes an except operation the second list.</summary>
        /// <typeparam name="T1">The type of the 1 enumeration.</typeparam>
        /// <typeparam name="T2">The type of the 2 enumeration.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first enumeration.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector1">The selector for the first enumeration.</param>
        /// <param name="selector2">The selector for the second enumeration.</param>
        /// <returns>An enumeration of items which are a result of an except from the first to the second enumeration.</returns>
        private static IEnumerable<T1> InternalExcept<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            var firstItems = first.ToList();
            var secondItems = second.ToList();

            foreach (var item in firstItems)
            {
                var result = secondItems.FirstOrDefault(i => selector2(i).EqualsTo(selector1(item)));

                if (result == null)
                {
                    yield return item;
                }
            }
        }

        /// <summary>Executes an intersect operation the second list.</summary>
        private static IEnumerable<T1> InternalIntersect<T1, T2, TProperty>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, TProperty> selector1,
            Func<T2, TProperty> selector2)
        {
            var firstItems = first.ToList();
            var secondItems = second.ToList();

            foreach (var item in firstItems)
            {
                var result = secondItems.FirstOrDefault(i => selector2(i).EqualsTo(selector1(item)));

                if (result != null)
                {
                    yield return item;
                }
            }
        }

        /// <summary>Gets the selector results.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <param name="selectors">The selectors.</param>
        /// <returns>The selected values of an specific object.</returns>
        private static IEnumerable<object> GetSelectorResults<T>(this IEnumerator<T> enumerator,
                                                                 params Func<T, object>[] selectors)
            where T : class
        {
            return selectors.Select(func => func(enumerator.Current));
        }
    }
}
