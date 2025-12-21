public static class AnsiColor
{
    public const int Black = 0;
    public const int Red = 1;
    public const int Green = 2;
    public const int Yellow = 3;
    public const int Blue = 4;
    public const int Magenta = 5;
    public const int Cyan = 6;
    public const int White = 7;

    public const int BrightBlack = 8; // Dark Gray
    public const int BrightRed = 9;
    public const int BrightGreen = 10;
    public const int BrightYellow = 11;
    public const int BrightBlue = 12;
    public const int BrightMagenta = 13;
    public const int BrightCyan = 14;
    public const int BrightWhite = 15;

    private static readonly Dictionary<(int fg, int bg), string> FgbgCache = new();
    private static readonly Dictionary<int, string> FgCache = new();

    /// <summary>
    /// Returns color index from provided intensities of color components. Each intensity can be from 0 to 5.
    /// </summary>
    /// <param name="r">0..5</param>
    /// <param name="g">0..5</param>
    /// <param name="b">0..5</param>
    /// <returns></returns>
    public static int Rgb(int r, int g, int b)
    {
        return 16 + (36 * r) + (6 * g) + b;
    }

    /// <summary>
    /// Returns gray color index from the provided white intensity (0..23).
    /// </summary>
    /// <param name="level">0 (block) to 23 (almost white)</param>
    /// <returns></returns>
    public static int Grayscale(int level)
    {
        return 232 + Math.Clamp(level, 0, 23);
    }

    /// <summary>
    /// Returns ANSI escape sequence for setting both foreground and background colors.
    /// </summary>
    /// <param name="fg"></param>
    /// <param name="bg"></param>
    /// <returns></returns>
    public static string GetCode(int fg, int bg)
    {
        var key = (fg, bg);

        if (FgbgCache.TryGetValue(key, out var code))
        {
            return code;
        }

        code = $"\x1b[38;5;{fg};48;5;{bg}m";
        FgbgCache[key] = code;

        return code;
    }

    /// <summary>
    /// Returns ANSI escape sequence for setting foreground color.
    /// </summary>
    /// <param name="fg"></param>
    /// <returns></returns>
    public static string GetFgCode(int fg)
    {
        if (FgCache.TryGetValue(fg, out var code))
        {
            return code;
        }

        code = $"\x1b[38;5;{fg}m";
        FgCache[fg] = code;

        return code;
    }

    /// <summary>
    /// ANSI-code to reset color and attributes
    /// </summary>
    public static string Reset => "\x1b[0m";
}