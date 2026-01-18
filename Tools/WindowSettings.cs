using ANSIConsole;
using Spectre.Console;
using System.Runtime.InteropServices;

public static class WindowSettings
{
    private const int windowWidth = 200;
    private const int windowHeight = 50;

    private const int MF_BYCOMMAND = 0x00000000;
    private const int SC_CLOSE = 0xF060;
    private const int SC_MINIMIZE = 0xF020;
    private const int SC_MAXIMIZE = 0xF030;
    private const int SC_SIZE = 0xF000;
    private const int STD_OUTPUT_HANDLE = -11;

    public static void Initialize()
    {
        if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
        Console.SetWindowSize(windowWidth, windowHeight);
        AnsiConsole.Cursor.Hide();

        ForbidChangeSize();
        DisableScrolling();
    }

    private static void ForbidChangeSize()
    {
        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);

        if (handle != IntPtr.Zero)
        {
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
        }
    }

    private static void DisableScrolling()
    {
        IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (consoleHandle == IntPtr.Zero) return;

        CONSOLE_SCREEN_BUFFER_INFO_EX csbi = new CONSOLE_SCREEN_BUFFER_INFO_EX();
        csbi.cbSize = (uint)Marshal.SizeOf(csbi);
        GetConsoleScreenBufferInfoEx(consoleHandle, ref csbi);

        // Set the buffer size to match the current window size
        // This removes the need for a scrollbar
        csbi.dwSize.X = (short)(csbi.srWindow.Right - csbi.srWindow.Left + 1);
        csbi.dwSize.Y = (short)(csbi.srWindow.Bottom - csbi.srWindow.Top + 1);

        // Ensure the cbSize is correctly set for the P/Invoke call
        SetConsoleScreenBufferInfoEx(consoleHandle, ref csbi);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public ushort wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;
        public ushort wPopupAttributes;
        public int bFullscreenSupported;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] ColorTable;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX info);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX info);

    [DllImport("user32.dll")]
    private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();
}
