using src.Game;
using src.Game.Players;
using System.Text; 
using System.Runtime.InteropServices;
namespace src
{
  class Program
  {
    // --- Windows Konsol Ayarları İçin Gerekli Tanımlamalar ---
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    const int STD_OUTPUT_HANDLE = -11;
    const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    static async Task Main(string[] args)
    {
        SetupConsole();
        var starter = new Starter();
        await starter.start();
    }

    static void SetupConsole()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        uint mode;
        GetConsoleMode(handle, out mode);
        SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }
  }
}
