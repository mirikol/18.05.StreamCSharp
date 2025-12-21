public interface IConsoleRenderer
{
    /// <summary>
    /// Returns currently active buffer. Don't store this value, as it will change after every rendering to console.
    /// Use actual value every time you render something to the buffer.
    /// </summary>
    ScreenBuffer Buffer { get; }
    void Clear();
    void SetSize(int width, int height);
    void Render();
}