internal class Program
{
    private static void Main()
    {
        try
        {
            Logger.enabled = true;
            Arena arena = new Arena();
            arena.Start();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }
}