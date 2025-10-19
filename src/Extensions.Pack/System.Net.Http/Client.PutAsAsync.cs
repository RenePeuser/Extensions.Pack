using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Argument.Check;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        // Quickfix until refactoring
        public static JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new JsonStringEnumConverter() }
        };

        public static async Task<T> PutAsAsync<T>(this HttpClient httpClient,
                                                  string url,
                                                  object body)
        {
            using var httpResponseMessage = await httpClient.PutAsJsonAsync(url, body).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false));
        }

        public static async Task<T> PutAsErrorResultAsync<T>(this HttpClient httpClient,
                                                             string url,
                                                             object body)
        {
            using var httpResponseMessage = await httpClient.PutAsJsonAsync(url, body).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<T> PutAsJsonStringAsync<T>(this HttpClient httpClient,
                                                      string url,
                                                      string payloadAsJson)
        {
            return httpClient.PutAsJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PutAsJsonStringAsync<T>(this HttpClient httpClient,
                                                            string url,
                                                            string payloadAsJson,
                                                            Assembly callingAssembly)
        {
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpResponseMessage = await httpClient.PutAsync(url, stringContent).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            throw new UnexpectedResultException(await httpResponseMessage.GetResponseInfoAsync(nameof(httpResponseMessage.IsSuccessStatusCode)).ConfigureAwait(false));
        }

        public static Task<T> PutAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                     string url,
                                                                     string payloadAsJson)
        {
            return httpClient.PutAsErrorResultWithJsonStringAsync<T>(url, payloadAsJson, Assembly.GetCallingAssembly());
        }

        public static async Task<T> PutAsErrorResultWithJsonStringAsync<T>(this HttpClient httpClient,
                                                                           string url,
                                                                           string payloadAsJson,
                                                                           Assembly callingAssembly)
        {
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpResponseMessage = await httpClient.PutAsync(url, stringContent).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode.IsFalse())
            {
                return await httpResponseMessage.ParseResultAsync<T>().ConfigureAwait(false);
            }

            var responseInfoAsync = await httpResponseMessage.GetResponseInfoAsync("Not successful").ConfigureAwait(false);

            throw new UnexpectedResultException(responseInfoAsync);
        }

        public static Task<HttpResponseMessage> PutAsJsonStringAsync(this HttpClient httpClient,
                                                                     string url,
                                                                     string jsonContent)
        {
            return httpClient.PutAsJsonStringAsync(url, jsonContent, Assembly.GetCallingAssembly());
        }

        public static Task<HttpResponseMessage> PutAsJsonStringAsync(this HttpClient httpClient,
                                                                     string url,
                                                                     string payloadAsJson,
                                                                     Assembly callingAssembly)
        {
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

#pragma warning disable CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage
            var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
#pragma warning restore CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage

            return httpClient.PutAsync(url, stringContent);
        }
    }
}
