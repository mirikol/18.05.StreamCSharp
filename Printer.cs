public static class Printer
{
    public static void Print(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        ConsoleColor oldForegroundColor = Console.ForegroundColor;
        ConsoleColor oldBackgroundColor = Console.BackgroundColor;

        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        Console.WriteLine(text);

        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }
}