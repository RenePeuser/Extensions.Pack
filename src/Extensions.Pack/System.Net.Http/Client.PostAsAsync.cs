using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static async Task<T> PostAsAsync<T>(this HttpClient httpClient,
                                                   string url)
        {
            using var httpResponseMessage = await httpClient.PostAsync(url, null).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var typeResult = await httpResponseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false);

                return typeResult;
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsAsync<T>(this HttpClient httpClient,
                                                   string url,
                                                   object body)
        {
            using var httpResponseMessage = await httpClient.PostAsJsonAsync(url, body).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var typeResult = await httpResponseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false);

                return typeResult;
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsErrorResultAsync<T>(this HttpClient httpClient,
                                                              string url,
                                                              object body)
        {
            using var httpResponseMessage = await httpClient.PostAsJsonAsync(url, body).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static async Task<T> PostAsErrorResultAsync<T>(this HttpClient httpClient,
                                                              string url)
        {
            using var httpResponseMessage = await httpClient.PostAsync(url, null).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PostAsJsonStringAsync<T>(this HttpClient httpClient,
                                                       string url,
                                                       string payloadAsJson)
        {
            return httpClient.PostAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PostAsJsonStringAsync<T>(this HttpClient httpClient,
                                                             string url,
                                                             string payloadAsJson,
                                                             Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var httpResponseMessage = await httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PostAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                      string url,
                                                                      string payloadAsJson)
        {
            return httpClient.PostAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PostAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                            string url,
                                                                            string payloadAsJson,
                                                                            Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var httpResponseMessage = await httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json)).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<HttpResponseMessage> PostAsJsonStringAsync(this HttpClient httpClient,
                                                                      string url,
                                                                      string payloadAsJson)
        {
            return httpClient.PostAsJsonStringAsync(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static Task<HttpResponseMessage> PostAsJsonStringAsync(this HttpClient httpClient,
                                                                      string url,
                                                                      string payloadAsJson,
                                                                      Assembly callingAssembly)
        {
            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            return httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json));
        }
    }
}
