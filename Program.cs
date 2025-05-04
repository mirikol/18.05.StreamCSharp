internal class Program
{
    private static void Main()
    {
        Logger.enabled = false;
        Arena arena = new Arena();
        arena.Start();
    }
}