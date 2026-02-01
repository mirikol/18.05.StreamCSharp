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

    public PrinterContext(string text, ConsoleColor foregroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = ConsoleColor.Black;
    }
}