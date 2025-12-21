public class DrawUtils
{
    public struct BorderStyle
    {
        public char Horizontal;
        public char Vertical;
        public char TopLeft;
        public char TopRight;
        public char BottomLeft;
        public char BottomRight;

        public BorderStyle(char h, char v, char tl, char tr, char bl, char br)
        {
            Horizontal = h;
            Vertical = v;
            TopLeft = tl;
            TopRight = tr;
            BottomLeft = bl;
            BottomRight = br;
        }
    }

    public static readonly BorderStyle SingleLine = new BorderStyle('-', '|', '+', '+', '+', '+');
    public static readonly BorderStyle DoubleLine = new BorderStyle('═', '║', '╔', '╗', '╚', '╝');
    public static readonly BorderStyle HeavyLine = new BorderStyle('━', '┃', '┏', '┓', '┗', '┛');
    public static readonly BorderStyle Rounded = new BorderStyle('─', '│', '╭', '╮', '╰', '╯');

    private ScreenBuffer _target;
    private int _fgColor = 15;
    private int _bgColor = 0;

    public DrawUtils(ScreenBuffer target)
    {
        _target = target;
    }

    public DrawUtils SetTarget(ScreenBuffer target)
    {
        _target = target;
        return this;
    }

    public DrawUtils SetColor(int fg, int bg = 0)
    {
        _fgColor = fg;
        _bgColor = bg;
        return this;
    }

    public DrawUtils ResetColor()
    {
        _fgColor = 15;
        _bgColor = 0;
        return this;
    }

    public DrawUtils DrawText(int x, int y, string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _target.Set(x + i, y, text[i], _fgColor, _bgColor);
        }

        return this;
    }

    public DrawUtils DrawTextLines(int x, int y, string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            DrawText(x, y + i, lines[i]);
        }

        return this;
    }

    public DrawUtils DrawRect(int x, int y, int width, int height, BorderStyle style, bool fill = false)
    {
        int xMax = x + width - 1;
        int yMax = y + height - 1;

        for (int i = x + 1; i < xMax; i++)
        {
            _target.Set(i, y, style.Horizontal, _fgColor, _bgColor);
            _target.Set(i, yMax, style.Horizontal, _fgColor, _bgColor);
        }

        for (int j = y + 1; j < yMax; j++)
        {
            _target.Set(x, j, style.Vertical, _fgColor, _bgColor);
            _target.Set(xMax, j, style.Vertical, _fgColor, _bgColor);
        }

        _target.Set(x, y, style.TopLeft, _fgColor, _bgColor);
        _target.Set(xMax, y, style.TopRight, _fgColor, _bgColor);
        _target.Set(x, yMax, style.BottomLeft, _fgColor, _bgColor);
        _target.Set(xMax, yMax, style.BottomRight, _fgColor, _bgColor);

        if (fill)
        {
            for (int i = x + 1; i < xMax; i++)
            {
                for (int j = y + 1; j < yMax; j++)
                {
                    _target.Set(i, j, ' ', _fgColor, _bgColor);
                }
            }
        }

        return this;
    }

    public DrawUtils Clear()
    {
        _target.Clear();
        return this;
    }

    public DrawUtils DrawTextCentered(int x, int y, string str)
    {
        return DrawText(x - str.Length / 2, y, str);
    }
}