using System.Diagnostics;
using Argument.Check;

namespace Extensions.Pack
{
    public static partial class HttpExtensions
    {
        public static Task<bool> UrlExistsAsync(this HttpClient httpClient,
                                                string url)
        {
            var httpClientAvoidClosure = httpClient;
            var relativeUrlAvoidClosure = url;

            return Task.Run(() => UrlExists(httpClientAvoidClosure, relativeUrlAvoidClosure));
        }

        public static bool UrlExists(this HttpClient httpClient,
                                     string url)
        {
            Throw.IfNull(httpClient);
            Throw.IfNullOrWhiteSpace(url);

            var absolutePath = url.StartWith("http") ? url : Path.Combine(httpClient.BaseAddress?.ToString()!, url);
#pragma warning disable SYSLIB0014
            var webRequest = System.Net.WebRequest.Create(absolutePath);
#pragma warning restore SYSLIB0014
            webRequest.Method = "HEAD";

            try
            {
                using var response = (System.Net.HttpWebResponse)webRequest.GetResponse();

                return response.StatusCode.ToString().EqualsTo("OK");
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                Debug.WriteLine($"Url: '{absolutePath}' does not exists or was not reachable");

                return false;
            }
        }
    }
}
