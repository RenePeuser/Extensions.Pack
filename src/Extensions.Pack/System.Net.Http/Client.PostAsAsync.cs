using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static async Task<T> PostAsAsync<T>(this HttpClient httpClient, string url)
        {
            var result = await httpClient.PostAsync(url, null).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                var typeResult = await result.Content.ReadAsAsync<T>().ConfigureAwait(false);
                return typeResult;
            }

            var responseInfoAsync = await result.GetResponseInfoAsync(nameof(result.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsAsync<T>(this HttpClient httpClient, string url, object body)
        {
            var result = await httpClient.PostAsJsonAsync(url, body).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                var typeResult = await result.Content.ReadAsAsync<T>().ConfigureAwait(false);
                return typeResult;
            }

            var responseInfoAsync = await result.GetResponseInfoAsync(nameof(result.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsErrorResultAsync<T>(this HttpClient httpClient, string url, object body)
        {
            var result = await httpClient.PostAsJsonAsync(url, body).ConfigureAwait(false);
            if (result.IsSuccessStatusCode is false)
            {
                return await result.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await result.GetResponseInfoAsync("Not successful").ConfigureAwait(false);
            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsErrorResultAsync<T>(this HttpClient httpClient, string url)
        {
            var result = await httpClient.PostAsync(url, null).ConfigureAwait(false);
            if (result.IsSuccessStatusCode is false)
            {
                return await result.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await result.GetResponseInfoAsync("Not successful").ConfigureAwait(false);
            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PostAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PostAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PostAsJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString(callingAssembly);

            var postResponse = await httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (postResponse.IsSuccessStatusCode)
            {
                return await postResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await postResponse.GetResponseInfoAsync(nameof(postResponse.IsSuccessStatusCode)).ConfigureAwait(false);
            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PostAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PostAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PostAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString(callingAssembly);

            var postResponse = await httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);
            if (postResponse.IsSuccessStatusCode is false)
            {
                return await postResponse.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await postResponse.GetResponseInfoAsync("Not successful").ConfigureAwait(false);
            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<HttpResponseMessage> PostAsJsonStringAsync(this HttpClient httpClient, string url, string payloadAsJson)
        {
            return httpClient.PostAsJsonStringAsync(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static Task<HttpResponseMessage> PostAsJsonStringAsync(this HttpClient httpClient, string url, string payloadAsJson, Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonString(callingAssembly);

            return httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json));
        }
    }
}
