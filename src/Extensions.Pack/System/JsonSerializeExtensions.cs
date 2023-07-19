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

        public static string ToJsonIntended<T>(this T source)
        {
            return JsonSerializer.Serialize(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() }, WriteIndented = true });
        }

        public static string ToJson<T>(this T source, JsonConverter customConverter)
        {
            return JsonSerializer.Serialize(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
        }

        public static T? FromJsonStringOrDefault<T>(this string source)
        {
            try
            {
                // Try catch because of invalid json strings !
                return JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T? FromJsonStringOrDefault<T>(this string source, JsonConverter customConverter)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
            }
            catch (Exception)
            {
                // Try catch because of invalid json strings !
                return default;
            }
        }

        public static T FromJsonStringAs<T>(this string source)
        {
            var result = JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
            if (result.IsNull())
            {
                throw new JsonDeserilizeException<T>(source);
            }

            return result;
        }

        public static T FromJsonStringAs<T>(this string source, JsonConverter customConverter)
        {
            var result = JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(), customConverter } });
            if (result.IsNull())
            {
                throw new JsonDeserilizeException<T>(source);
            }

            return result;
        }
    }
}
