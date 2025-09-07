using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public static class JsonIO<T> where T : class
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        WriteIndented = true
    };

    public static T Load(string json)
    {
        var data = JsonSerializer.Deserialize<T>(json);
        if (data == null)
        {
            Logger.LogError($"Error while JSON deserialize ({typeof(T).Name})");
            return null;
        }

        return data;
    }

    public static string Save(T saveable)
    {
        return JsonSerializer.Serialize<T>(saveable, jsonSerializerOptions);
    }
}