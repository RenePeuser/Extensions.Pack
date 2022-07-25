using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Extensions.Pack
{
    public class JsonDeserilizeException<T> : Exception
    {
        public JsonDeserilizeException(string jsonString) : base($"The {typeof(T).Name} could not deserialized from json string: {jsonString}. Please check json structure")
        {
        }
    }

    public static class JsonSerializeExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };

        public static string ToJson<T>(this T source)
        {
            return JsonSerializer.Serialize(source, JsonSerializerOptions);
        }

        public static string ToJson<T>(this T source, JsonConverter customConverter)
        {
            return JsonSerializer.Serialize(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
        }

        public static T? FromJsonStringOrDefault<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
        }

        public static T? FromJsonStringOrDefault<T>(this string source, JsonConverter customConverter)
        {
            return JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
        }

        public static T FromJsonStringAs<T>(this string source)
        {
            var result = source.FromJsonStringOrDefault<T>();
            if (result is null)
            {
                throw new JsonDeserilizeException<T>(source);
            }

            return result;
        }

        public static T FromJsonStringAs<T>(this string source, JsonConverter customConverter)
        {
            var result = source.FromJsonStringOrDefault<T>(customConverter);
            if (result is null)
            {
                throw new JsonDeserilizeException<T>(source);
            }

            return result;
        }
    }
}
