using System.Runtime.InteropServices;

public static class ConsoleUtils
{
    public static void SetConsoleSizeSafe(int width, int height)
    {
        // For Unix-like systems (xterm-compatible)
        Console.Write($"\x1b[8;{height};{width}t");

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        try
        {
            // Save current and max sizes
            int oldBufWidth = Console.BufferWidth;
            int oldBufHeight = Console.BufferHeight;
            int maxWidth = Console.LargestWindowWidth;
            int maxHeight = Console.LargestWindowHeight;

            width = Math.Min(width, maxWidth);
            height = Math.Min(height, maxHeight);

            if (oldBufWidth < width || oldBufHeight < height)
            {
                Console.SetBufferSize(Math.Max(oldBufWidth, width), Math.Max(oldBufHeight, height));
            }

            Console.SetWindowSize(width, height);
        }
        catch (Exception ex)
        {
            string message =
              $"Failed to set console size to {width}x{height}.\n" +
              $"[Current buffer] {Console.BufferWidth}x{Console.BufferHeight}\n" +
              $"[Current window] {Console.WindowWidth}x{Console.WindowHeight}\n" +
              $"[Max window size] {Console.LargestWindowWidth}x{Console.LargestWindowHeight}\n" +
              $"Exception: {ex.Message}";

            throw new InvalidOperationException(message, ex);
        }
    }
}