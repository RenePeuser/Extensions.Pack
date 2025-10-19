using System.Net.Mime;
using System.Reflection;
using System.Text;
using Argument.Check;

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
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson);

            using var httpResponseMessage = await httpClient.PatchAsync(url, stringContent).ConfigureAwait(false);

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
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

            using var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson);

            using var httpResponseMessage = await httpClient.PatchAsync(url, stringContent).ConfigureAwait(false);

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
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);

            var jsonContent = content.ToJson(JsonSerializerOptions);
#pragma warning disable CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson);
#pragma warning restore CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage
            var patchResponse = await httpClient.PatchAsync(url, stringContent).ConfigureAwait(false);

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
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);
            Throw.IfNullOrWhiteSpace(payloadAsJson);
            Throw.IfNull(callingAssembly);

            var jsonPayload = payloadAsJson.GetJsonStringFrom(callingAssembly);

#pragma warning disable CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage
            var stringContent = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNamesExtensions.Application.MergePatchJson);
#pragma warning restore CA2000 // We can not dispose it here, caller is responsible for disposing the HttpResponseMessage

            var patchResponse = await httpClient.PatchAsync(url, stringContent).ConfigureAwait(false);

            return patchResponse;
        }
    }
}
