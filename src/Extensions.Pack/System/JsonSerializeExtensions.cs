using System.Text.Json;
using System.Text.Json.Serialization;

namespace Extensions.Pack
{
    public static class JsonSerializeExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };

        public static string ToJsonString<T>(this T source)
        {
            return source.ToJson();
        }

        public static string ToJson<T>(this T source)
        {
            return JsonSerializer.Serialize(source, JsonSerializerOptions);
        }

        public static string ToJson<T>(this T source, JsonConverter customConverter)
        {
            return JsonSerializer.Serialize(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
        }

        public static string ToJsonString<T>(this T source, JsonConverter customConverter)
        {
            return source.ToJson(customConverter);
        }

        public static T FromJson<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
        }
        public static T FromJsonAs<T>(this string source)
        {
            return source.FromJson<T>();
        }

        public static T FromJsonStringAs<T>(this string source)
        {
            return source.FromJson<T>();
        }

        public static T FromJson<T>(this string source, JsonConverter customConverter)
        {
            return JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
        }
    }
}
