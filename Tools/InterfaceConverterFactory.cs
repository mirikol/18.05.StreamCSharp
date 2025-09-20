using System.Text.Json.Serialization;
using System.Text.Json;

[AttributeUsage(AttributeTargets.Interface)]
public class JsonInterfaceConverterAttribute : JsonConverterAttribute
{
    public JsonInterfaceConverterAttribute() : base(typeof(InterfaceConverterFactory)) { }
}

public class InterfaceConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsInterface;
    }

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var converterType = typeof(InterfaceConverter<>).MakeGenericType(type);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}

public class InterfaceConverter<TInterface> : JsonConverter<TInterface>
{
    private readonly Dictionary<string, Type> _typeMap;

    public InterfaceConverter()
    {
        _typeMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t);
    }

    public override TInterface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var root = jsonDoc.RootElement;
            if (root.TryGetProperty("$type", out var typeElement))
            {
                var typeName = typeElement.GetString();
                if (_typeMap.TryGetValue(typeName, out var concreteType))
                {
                    return (TInterface)JsonSerializer.Deserialize(root.GetRawText(), concreteType, options);
                }
            }
            throw new JsonException("Cannot determine concrete type");
        }
    }

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        writer.WriteStartObject();
        writer.WriteString("$type", type.Name);

        var json = JsonSerializer.Serialize(value, type, options);
        using (var doc = JsonDocument.Parse(json))
        {
            foreach (var property in doc.RootElement.EnumerateObject())
            {
                property.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}