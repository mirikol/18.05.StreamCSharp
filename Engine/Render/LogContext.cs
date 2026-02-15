public struct LogContext
{
    public string Text;
    public ConsoleColor ForegroundColor;
    public ConsoleColor BackgroundColor;

    public LogContext(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = backgroundColor;
    }

    public LogContext(string text, ConsoleColor foregroundColor)
    {
        Text = text;
        ForegroundColor = foregroundColor;
        BackgroundColor = ConsoleColor.Black;
    }
}