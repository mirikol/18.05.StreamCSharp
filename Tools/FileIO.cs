public static class FileIO
{
    public static string Load(string path)
    {
        if (!File.Exists(path))
        {
            Logger.LogError($"File {path} is not exist");
            return "";
        }

        return File.ReadAllText(path);
    }

    public static void Save(string directory, string file, string data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, file);
        File.WriteAllText(path, data);
        Logger.Log($"File {path} saved successfully");
    }
}