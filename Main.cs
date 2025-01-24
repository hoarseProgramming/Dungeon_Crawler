internal class Program
{
    private static async Task Main(string[] args)
    {
        var AppData = new AppData();

        await AppData.LoadSavedGames();
        await AppData.RunMainMenu();
    }
}