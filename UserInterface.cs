
public static class UserInterface
{
    public static void RunMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.CursorVisible = false;
        List<Object> settings = new();
        ConsoleKeyInfo input = new();
        while (input.Key != ConsoleKey.Escape)
        {
            if (input.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (settings.Count == 0)
                {
                    RunGameLoop(GetDefaultSettings());
                }
                else
                {
                    if ((bool)settings[0] == true)
                    {
                        Console.WriteLine("You have choosen to animate dice throws.");
                        Console.WriteLine("Please maximise your console window now. Press any key when ready.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    RunGameLoop(settings);
                }
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                settings = GetSettings();
            }
            else if (input.Key == ConsoleKey.Backspace)
            {
                settings = new List<object>();
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Start Game - Press Enter");
            Console.WriteLine("Settings - Press Space (Highly recommended)");
            if (settings.Count == 0)
            {
                Console.WriteLine("Current settings: Default");
            }
            else
            {
                Console.WriteLine("Current settings: Modified. Press backspace to load default settings.");
            }
            Console.WriteLine("Exit - Press ESC");
            input = Console.ReadKey(true);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    public static void RunGameLoop(List<Object> settings)
    {
        LevelData levelOne = new LevelData();
        levelOne.Load("Level1.txt", settings);

        while (levelOne.Hero.IsAlive)
        {
            levelOne.NewTurn();
            levelOne.DrawLevel();
        }
    }
    public static List<Object> GetSettings()
    {
        Console.Clear();
        List<Object> settings = new();

        Console.WriteLine("Enter hero name. Simply press enter for default name (highly recommended)");
        string heroName = Console.ReadLine();

        Console.WriteLine("Do you want to animate dice throws! YES/NO (YES is Highly recommended)");
        string input = String.Empty;
        while (input.ToLower() != "yes" && input.ToLower() != "no")
        {
            Console.SetCursorPosition(0, 3);
            input = Console.ReadLine();

        }

        bool shouldAnimate = false;
        if (input == "yes")
        {
            shouldAnimate = true;
        }
        settings.Add(shouldAnimate);

        if (heroName == "")
        {
            settings.Add("Fredrik \"SOLID\" the DRY");
        }
        else
        {
            settings.Add(heroName);
        }

        return settings;
    }
    public static List<Object> GetDefaultSettings()
    {
        List<Object> defaultSettings = new();
        defaultSettings.Add(false);
        defaultSettings.Add("Fredrik \"SOLID\" the DRY");

        return defaultSettings;
    }
}