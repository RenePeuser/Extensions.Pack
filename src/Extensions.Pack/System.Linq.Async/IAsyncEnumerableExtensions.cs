using System.Collections.Immutable;

namespace Extensions.Pack
{
    public static class IAsyncEnumerableExtensions
    {
        public static async ValueTask<ImmutableList<TSource>> ToImmutableListAsync<TSource>(this IAsyncEnumerable<TSource> source,
                                                                                             CancellationToken cancellationToken = default)
        {
            if (source is IAsyncIListProvider<TSource> listProvider)
            {
                var listItems = await listProvider.ToListAsync(cancellationToken).ConfigureAwait(false);

                return listItems.ToImmutableList();
            }

            return await Core(source, cancellationToken).ConfigureAwait(false);

            static async ValueTask<ImmutableList<TSource>> Core(IAsyncEnumerable<TSource> source,
                                                                 CancellationToken cancellationToken)
            {
                var immutableListBuilder = ImmutableList.CreateBuilder<TSource>();

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    immutableListBuilder.Add(item);
                }

                return immutableListBuilder.ToImmutable();
            }
        }
    }
}
