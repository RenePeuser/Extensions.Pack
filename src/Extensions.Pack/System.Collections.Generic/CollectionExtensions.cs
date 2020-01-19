using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Extensions.Pack
{
    /// <summary>Extension methods for ICollection.</summary>
    public static class CollectionExtensions
    {
        /// <summary>Clears a collection and adds the given items to the collection.</summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection to add the items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        public static void ClearAndAddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => items);

            collection.Clear();
            collection.AddRange(items);
        }

        /// <summary>Clears a collection and adds the given items to the collection.</summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection to add the items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        public static void ClearAndAddRange<T>(this ICollection<T> collection, params T[] items)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => items);

            collection.Clear();
            collection.AddRange(items);
        }

        /// <summary>Adds a range of items to a collection.</summary>
        /// <param name="collection">The second collection where the items have to be added.</param>
        /// <param name="items">The item which have to be added.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => items);

            items.ToList().ForEach(collection.Add);
        }

        /// <summary>Adds a range of items to a collection.</summary>
        /// <param name="collection">The second collection where the items have to be added.</param>
        /// <param name="items">The item which have to be added.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => items);

            items.ForEach(collection.Add);
        }

        /// <summary>Remove a range of items from a collection.</summary>
        /// <param name="collection">The collection where the items have to be removed.</param>
        /// <param name="items">The items which have to be removed from the collection.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => items);

            items.ToList().ForEach(item => collection.Remove(item));
        }

        /// <summary>Remove a range of items from a collection.</summary>
        /// <param name="collection">The collection where the items have to be removed.</param>
        /// <param name="selector">The selector used to specify what items should be removed.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void RemoveRange<T>(this ICollection<T> collection, Func<T, bool> selector)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => selector);

            var itemsToRemove = collection.Where(selector).ToList();
            collection.RemoveRange(itemsToRemove);
        }

        /// <summary>Remove all items which fulfill the predicate.</summary>
        /// <param name="collection">The second collection.</param>
        /// <param name="predicate">Predicate to get the items for removing.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => predicate);

            var itemsToRemove = collection.Where(predicate).ToList();
            collection.RemoveRange(itemsToRemove);
        }

        /// <summary>Adds an item to a collection if it doesn't already contain it. Does not throw an exception if there already is one.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item to add.</param>
        public static void AddOnce<T>(this ICollection<T> collection, T item)
            where T : class
        {
            Throw.IfNull(() => collection);
            Throw.IfNull(() => item);

            if (!collection.Contains(item))
            {
                collection.Add(item);
            }
        }

        /// <summary>Replace a item at a specific index.</summary>
        /// <param name="collection">The second collection.</param>
        /// <param name="index">The index on which position the item have to be replaced.</param>
        /// <param name="newItem">The new item.</param>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        public static void ReplaceAt<T>(this Collection<T> collection, int index, T newItem)
        {
            Throw.IfNull(() => collection);

            collection[index] = newItem;
        }

        /// <summary>Replace a item at its specific index.</summary>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        /// <param name="sourceCollection">The second collection.</param>
        /// <param name="oldItem">The original item.</param>
        /// <param name="newItem">The new item.</param>
        public static void Replace<T>(this Collection<T> sourceCollection, T oldItem, T newItem)
        {
            Throw.IfNull(() => sourceCollection);

            var index = sourceCollection.IndexOf(oldItem);
            sourceCollection.ReplaceAt(index, newItem);
        }

        /// <summary>Unites the source collection with a second collection by replacing all items that appear in both lists.</summary>
        /// <typeparam name="T">Generic type of the collection.</typeparam>
        /// <param name="target">The target collection to be united.</param>
        /// <param name="source">The source collection to unite from.</param>
        public static void UnionByReplacing<T>(this ICollection<T> target, ICollection<T> source)
        {
            Throw.IfNull(() => target);
            Throw.IfNull(() => source);

            foreach (var item in source)
            {
                if (target.Contains(item))
                {
                    target.Remove(item);
                }

                target.Add(item);
            }
        }

        /// <summary>Synchronizes the target collection with a source collection. The items that are not in the source collection will be removed from the target collection.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <param name="target">The first collection which have to be synchronized.</param>
        /// <param name="source">The second collection.</param>
        public static void SyncCollectionFrom<T>(this ICollection<T> target, IEnumerable<T> source)
            where T : class
        {
            Throw.IfNull(() => target);
            Throw.IfNull(() => source);

            var sourceItems = source.ToList();

            InvokeActionForExceptItems(target, sourceItems, target.Add);
            InvokeActionForExceptItems(sourceItems, target, item => target.Remove(item));
        }

        /// <summary>Synchronizes the target collection with a source collection. The items that are not in the source collection will be removed from the target collection.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="target">The first collection which have to be synchronized.</param>
        /// <param name="source">The second collection.</param>
        /// <param name="selector">The selector for the criteria of the synchronization.</param>
        public static void SyncCollectionFrom<T, TProperty>(this ICollection<T> target, IEnumerable<T> source, Func<T, TProperty> selector)
            where T : class
        {
            Throw.IfNull(() => target);
            Throw.IfNull(() => source);
            Throw.IfNull(() => selector);

            var sourceItems = source.ToList();

            InvokeActionForExceptItems(target, sourceItems, selector, target.Add);
            InvokeActionForExceptItems(sourceItems, target, selector, item => target.Remove(item));
        }

        /// <summary>Synchronizes the target collection with a source collection without deleting the items that are not in source.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="target">The first collection which have to be synchronized.</param>
        /// <param name="source">The second collection.</param>
        /// <param name="selector">The selector for the criteria of the synchronization.</param>
        public static void SyncCollectionWithoutDeleteFrom<T, TProperty>(
            this ICollection<T> target,
            IEnumerable<T> source,
            Func<T, TProperty> selector)
            where T : class
        {
            Throw.IfNull(() => target);
            Throw.IfNull(() => source);
            Throw.IfNull(() => selector);

            var sourceItems = source.ToList();
            InvokeActionForExceptItems(target, sourceItems, selector, target.Add);
        }

        /// <summary>Synchronizes the target collection with a source collection without deleting the items that are not in source.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <param name="target">The first collection which have to be synchronized.</param>
        /// <param name="source">The second collection.</param>
        public static void SyncCollectionWithoutDeleteFrom<T>(this ICollection<T> target, IEnumerable<T> source)
            where T : class
        {
            Throw.IfNull(() => target);
            Throw.IfNull(() => source);

            var sourceItems = source.ToList();
            InvokeActionForExceptItems(target, sourceItems, target.Add);
        }

        /// <summary>Invoke action for all items which are except from second to the first.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="action">The action which have to be invoked on each except items.</param>
        private static void InvokeActionForExceptItems<T>(ICollection<T> first, IEnumerable<T> second, Action<T> action)
            where T : class
        {
            var exceptItems = second.Except(first).ToList();
            exceptItems.ForEach(action);
        }

        /// <summary>Invoke action for all items which are except from second to the first.</summary>
        /// <typeparam name="T">The generic type of the collections.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second enumeration.</param>
        /// <param name="selector">The selector for the criteria to except the items.</param>
        /// <param name="action">The action which have to be invoked on each except items.</param>
        private static void InvokeActionForExceptItems<T, TProperty>(
            ICollection<T> first,
            IEnumerable<T> second,
            Func<T, TProperty> selector,
            Action<T> action)
            where T : class
        {
            var exceptItems = second.Except(first, selector).ToList();
            exceptItems.ForEach(action);
        }
    }
}
