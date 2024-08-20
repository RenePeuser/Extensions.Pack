using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static Task<T> PatchAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PatchAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PatchAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString<T>(callingAssembly);

            var patchResponse = await httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await patchResponse.GetResponseInfoAsync(nameof(patchResponse.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PatchAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PatchAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PatchAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString<T>(callingAssembly);

            var patchResponse = await httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (patchResponse.IsSuccessStatusCode.IsFalse())
            {
                return await patchResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await patchResponse.GetResponseInfoAsync("Not successful").ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient httpClient, string url, T content)
        {
            var jsonContent = content.ToJson();
            var patchResponse = await httpClient.PatchAsync(url, new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            return patchResponse;
        }
    }
}
