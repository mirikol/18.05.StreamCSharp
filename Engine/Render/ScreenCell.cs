public struct ScreenCell(char c, int color, int bgColor) : IEquatable<ScreenCell>
{
    public char Char = c;
    public int Color = color;
    public int BgColor = bgColor;

    public bool Equals(ScreenCell other)
    {
        return Char == other.Char &&
               Color == other.Color &&
               BgColor == other.BgColor;
    }

    public override bool Equals(object? obj) => obj is ScreenCell other && Equals(other);

    public override int GetHashCode() =>
      HashCode.Combine(Char, Color, BgColor);

    public static bool operator ==(ScreenCell a, ScreenCell b) => a.Equals(b);
    public static bool operator !=(ScreenCell a, ScreenCell b) => !a.Equals(b);
}