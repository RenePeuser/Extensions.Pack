using System.Text.Json;
using System.Text.Json.Serialization;

namespace Extensions.Pack
{
    public class JsonDeserializeException<T> : Exception
    {
        public JsonDeserializeException(string jsonString) : base($"The {typeof(T).Name} could not deserialized from json string: {jsonString}. Please check json structure")
        {
        }
    }

    public static class JsonSerializeExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private static readonly JsonSerializerOptions JsonSerializerWriteIntended = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };

        public static string ToJson<T>(this T source)
        {
            return JsonSerializer.Serialize(source, JsonSerializerOptions);
        }

        public static string ToJson<T>(this T source,
                                       JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.Serialize(source, jsonSerializerOptions);
        }

        public static string ToJsonIntended<T>(this T source)
        {
            return JsonSerializer.Serialize(source, JsonSerializerWriteIntended);
        }

        public static string ToJsonIntended<T>(this T source,
                                               JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.Serialize(source, jsonSerializerOptions);
        }

        public static string ToJson<T>(this T source,
                                       JsonConverter customConverter)
        {
#pragma warning disable CA1869
            return JsonSerializer.Serialize(source, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                                                        {
                                                            new JsonStringEnumConverter(),
                                                            customConverter
                                                        }
            });
#pragma warning restore CA1869
        }

        public static T? FromJsonStringOrDefault<T>(this string source)
        {
            try
            {
                // Try catch because of invalid json strings !
                return JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
            }
#pragma warning disable CA1031
            catch (Exception)
#pragma warning restore CA1031
            {
                return default;
            }
        }

        public static T? FromJsonStringOrDefault<T>(this string source,
                                                    JsonSerializerOptions jsonSerializerOptions)
        {
            try
            {
                // Try catch because of invalid json strings !
                return JsonSerializer.Deserialize<T>(source, jsonSerializerOptions);
            }
#pragma warning disable CA1031
            catch (Exception)
#pragma warning restore CA1031
            {
                return default;
            }
        }

        public static T? FromJsonStringOrDefault<T>(this string source,
                                                    JsonConverter customConverter)
        {
            try
            {
#pragma warning disable CA1869
                return JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                                                                 {
                                                                     new JsonStringEnumConverter(),
                                                                     customConverter
                                                                 }
                });
#pragma warning restore CA1869
            }
#pragma warning disable CA1031
            catch (Exception)
#pragma warning restore CA1031
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
                throw new JsonDeserializeException<T>(source);
            }

            return result;
        }

        public static T FromJsonStringAs<T>(this string source,
                                            JsonSerializerOptions jsonSerializerOptions)
        {
            var result = JsonSerializer.Deserialize<T>(source, jsonSerializerOptions);

            if (result.IsNull())
            {
                throw new JsonDeserializeException<T>(source);
            }

            return result;
        }

        public static T FromJsonStringAs<T>(this string source,
                                            JsonConverter customConverter)
        {
#pragma warning disable CA1869
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                                            {
                                                new JsonStringEnumConverter(),
                                                customConverter
                                            }
            };
#pragma warning restore CA1869

            var result = JsonSerializer.Deserialize<T>(source, jsonSerializerOptions);

            if (result.IsNull())
            {
                throw new JsonDeserializeException<T>(source);
            }

            return result;
        }
    }
}
