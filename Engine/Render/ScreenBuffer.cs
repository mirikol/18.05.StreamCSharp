public class ScreenBuffer
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ScreenCell?[,] Surface { get; private set; } = new ScreenCell?[0, 0];

    public ScreenBuffer(int width, int height)
    {
        SetSize(width, height);
    }

    public void SetSize(int width, int height)
    {
        Width = width;
        Height = height;
        Surface = new ScreenCell?[height, width];
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Surface[y, x] = new ScreenCell(' ', 7, 0);
            }
        }
    }

    public void Set(int x, int y, char symbol, int fg, int bg)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            Surface[y, x] = new ScreenCell(symbol, fg, bg);
        }
    }

    private void Set(int x, int y, ScreenCell cell)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            Surface[y, x] = cell;
        }
    }

    public void DrawFrom(ScreenBuffer source, int offsetX, int offsetY)
    {
        int maxY = Math.Min(source.Height, Height - offsetY);
        int maxX = Math.Min(source.Width, Width - offsetX);

        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                var cell = source.Surface[y, x];

                if (cell != null)
                {
                    Set(x + offsetX, y + offsetY, cell.Value);
                }
            }
        }
    }
}