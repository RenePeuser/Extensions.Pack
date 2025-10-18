using System.IO.Compression;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static async Task GetAsAsync(this HttpClient httpClient,
                                            string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return;
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> GetAsAsync<T>(this HttpClient httpClient,
                                                  string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> GetAsErrorResultAsync<T>(this HttpClient httpClient,
                                                             string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false));
        }

        public static async Task<byte[]> GetFileStreamAsByteArray(this HttpClient httpClient,
                                                                  string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var fileStreamResult = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

                await using (fileStreamResult.ConfigureAwait(false))
                {
                    var memoryStream = new MemoryStream();

                    await using (memoryStream.ConfigureAwait(false))
                    {
                        await fileStreamResult.CopyToAsync(memoryStream).ConfigureAwait(false);
                        var byteArray = memoryStream.ToArray();

                        return byteArray;
                    }
                }
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync($"GET '{url}' was not success full.").ConfigureAwait(false));
        }

        public static async Task<InMemoryFileAsByteArray> GetFileAsync(this HttpClient httpClient,
                                                                       string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var fileStreamResult = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

                await using (fileStreamResult.ConfigureAwait(false))
                {
                    var memoryStream = new MemoryStream();

                    await using (memoryStream.ConfigureAwait(false))
                    {
                        await fileStreamResult.CopyToAsync(memoryStream).ConfigureAwait(false);
                        var byteArray = memoryStream.ToArray();

                        return new InMemoryFileAsByteArray(byteArray, "test");
                    }
                }
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync($"GET '{url}' was not success full.").ConfigureAwait(false));
        }

        public static async IAsyncEnumerable<InMemoryFileAsByteArray> GetFilesFromZipResponseAsync(this HttpClient httpClient,
                                                                                                   string url)
        {
            using var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var fileStreamResult = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

                await using (fileStreamResult.ConfigureAwait(false))
                {
                    using var zipArchive = new ZipArchive(fileStreamResult);

                    foreach (var entry in zipArchive.Entries)
                    {
                        using var stream = entry.Open();
                        using var memoryStream = new MemoryStream();

                        await using (memoryStream.ConfigureAwait(false))
                        {
                            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                            var byteArray = memoryStream.ToArray();

                            yield return new InMemoryFileAsByteArray(byteArray, entry.Name);
                        }
                    }
                }
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync($"GET '{url}' was not success full.").ConfigureAwait(false));
        }
    }
}
