using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Extensions.Pack
{
    internal static class MediaTypeNamesExtensions
    {
        internal static class Application
        {
            internal const string MergePatchJson = "application/merge-patch+json";
        }
    }

    public static partial class HttpExtensions
    {
        public static Task<T> PatchAsJsonStringAsync<T>(this HttpClient httpClient,
                                                        string url,
                                                        string payloadAsJson)
        {
            return httpClient.PatchAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PatchAsJsonStringAsync<T>(this HttpClient httpClient,
                                                              string url,
                                                              string payloadAsJson,
                                                              Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var httpResponseMessage = await httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson)).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PatchAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                       string url,
                                                                       string payloadAsJson)
        {
            return httpClient.PatchAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PatchAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                             string url,
                                                                             string payloadAsJson,
                                                                             Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var httpResponseMessage = await httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson)).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient httpClient,
                                                                          string url,
                                                                          T content)
        {
            var jsonContent = content.ToJson(JsonSerializerOptions);
            var patchResponse = await httpClient.PatchAsync(url, new StringContent(jsonContent, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson)).ConfigureAwait(false);

            return patchResponse;
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient httpClient,
                                                                 string url,
                                                                 string payloadAsJson)
        {
            return httpClient.PatchAsJsonAsync(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient httpClient,
                                                                       string url,
                                                                       string payloadAsJson,
                                                                       Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);
            var patchResponse = await httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson)).ConfigureAwait(false);

            return patchResponse;
        }
    }
}
