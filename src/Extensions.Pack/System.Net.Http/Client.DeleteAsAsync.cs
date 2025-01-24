namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static async Task DeleteAsAsync(this HttpClient httpClient, string url)
        {
            using var httpResponseMessage = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return;
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> DeleteAsAsync<T>(this HttpClient httpClient, string url)
        {
            using var httpResponseMessage = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> DeleteAsErrorResultAsync<T>(this HttpClient httpClient, string url)
        {
            using var httpResponseMessage = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false));
        }
    }
}
