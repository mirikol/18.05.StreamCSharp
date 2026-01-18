public static class Printer
{
    public static Action<PrinterContext> HasPrinted;

    public static void Print(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        HasPrinted?.Invoke(new PrinterContext(text, foregroundColor, backgroundColor));
        //ConsoleColor oldForegroundColor = Console.ForegroundColor;
        //ConsoleColor oldBackgroundColor = Console.BackgroundColor;

        //Console.ForegroundColor = foregroundColor;
        //Console.BackgroundColor = backgroundColor;

        //Console.WriteLine(text);

        //Console.ForegroundColor = oldForegroundColor;
        //Console.BackgroundColor = oldBackgroundColor;
    }
}

public struct PrinterContext
{
    public string Text;
    public ConsoleColor ForegroundColor;
    public ConsoleColor BackgroundColor;

    public PrinterContext(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = backgroundColor;
    }
}