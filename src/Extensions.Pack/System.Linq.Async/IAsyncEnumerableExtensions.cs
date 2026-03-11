using System.Collections.Immutable;

namespace Extensions.Pack
{
    public static class IAsyncEnumerableExtensions
    {
        public static async ValueTask<ImmutableList<TSource>> ToImmutableListAsync<TSource>(this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
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
