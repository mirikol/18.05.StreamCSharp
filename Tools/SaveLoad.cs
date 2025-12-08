public static class SaveLoad<T> where T : class
{
    private static readonly Dictionary<Type, string> typeToDirectory = new Dictionary<Type, string>()
    {
        {typeof(UnitSave), Path.Combine("Content", "Unit") },
        {typeof(UnitModel), Path.Combine("Content", "Unit", "Model") },
        {typeof(IArmor), Path.Combine("Content", "Equipment", "Armor") },
        {typeof(IWeapon), Path.Combine("Content", "Equipment", "Weapon") },
        {typeof(ArenaModel), Path.Combine("Content", "Levels") }
    };

    public static T Load(string name)
    {
        if (TryGetDirectory(out string directory))
        {
            var json = FileIO.Load(Path.Combine(directory, name + ".json"));
            return JsonIO<T>.Load(json);
        }
        else
        {
            Logger.LogError($"Load error {typeof(T).Name} ({name})");
            return null;
        }
    }

    public static void Save(T saveable, string name)
    {
        if (TryGetDirectory(out string directory))
        {
            var json = JsonIO<T>.Save(saveable);
            FileIO.Save(Path.Combine(directory), name + ".json", json);
        }
        else
        {
            Logger.LogError($"Save error {typeof(T).Name} ({name})");
        }
    }

    private static bool TryGetDirectory(out string directory)
    {
        foreach (var type in typeToDirectory.Keys)
        {
            if (type.IsAssignableFrom(typeof(T)) || typeof(T) == type)
            {
                directory = typeToDirectory[type];
                return true;
            }
        }

        directory = "";
        return false;
    }
}