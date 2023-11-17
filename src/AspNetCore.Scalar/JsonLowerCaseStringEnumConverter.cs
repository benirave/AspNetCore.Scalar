using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCore.Scalar
{
    public class JsonLowerCaseStringEnumConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), reader.GetString(), true);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}