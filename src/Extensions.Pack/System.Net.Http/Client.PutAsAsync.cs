using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static async Task<T> PutAsAsync<T>(this HttpClient httpClient, string url, object body)
        {
            var putResponse = await httpClient.PutAsJsonAsync(url, body).ConfigureAwait(false);
            if (putResponse.IsSuccessStatusCode)
            {
                return await putResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await putResponse.GetResponseInfoAsync(nameof(putResponse.IsSuccessStatusCode)).ConfigureAwait(false));
        }

        public static async Task<T> PutAsErrorResultAsync<T>(this HttpClient httpClient, string url, object body)
        {
            var putResponse = await httpClient.PutAsJsonAsync(url, body).ConfigureAwait(false);
            if (putResponse.IsSuccessStatusCode.IsFalse())
            {
                return await putResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await putResponse.GetResponseInfoAsync("Not successful").ConfigureAwait(false);
            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PutAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PutAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PutAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString<T>(callingAssembly);

            var putResponse = await httpClient.PutAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (putResponse.IsSuccessStatusCode)
            {
                return await putResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await putResponse.GetResponseInfoAsync(nameof(putResponse.IsSuccessStatusCode)).ConfigureAwait(false));
        }

        public static Task<T> PutAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PutAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PutAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString<T>(callingAssembly);

            var putResponse = await httpClient.PutAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (putResponse.IsSuccessStatusCode.IsFalse())
            {
                return await putResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await putResponse.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<HttpResponseMessage> PutAsJsonStringAsync(this HttpClient httpClient, string url, string jsonContent)
        {
            return httpClient.PutAsJsonStringAsync(url, jsonContent, Assembly.GetCallingAssembly());
        }

        public static Task<HttpResponseMessage> PutAsJsonStringAsync(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString<object>(callingAssembly);

            return httpClient.PutAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json));
        }
    }
}
