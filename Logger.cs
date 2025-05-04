public static class Logger
{
    public static bool enabled = true;

    public static void Log(string message)
    {
        if (!enabled)
        {
            return;
        }

        ConsoleColor oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[LOG]({DateTime.Now}): {message}");
        Console.ForegroundColor = oldColor;
    }
    public static void LogWarning(string message)
    {
        if (!enabled)
        {
            return;
        }

        ConsoleColor oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING]({DateTime.Now}): {message}");
        Console.ForegroundColor = oldColor;
    }
    public static void LogError(string message)
    {
        if (!enabled)
        {
            return;
        }

        ConsoleColor oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR]({DateTime.Now}): {message}");
        Console.ForegroundColor = oldColor;
    }
    public static void LogException(Exception exception)
    {
        if (!enabled)
        {
            return;
        }

        ConsoleColor oldForegroundColor = Console.ForegroundColor;
        ConsoleColor oldBackgroundColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"[EXCEPTION]({DateTime.Now}): {exception.Message}");
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }
}
