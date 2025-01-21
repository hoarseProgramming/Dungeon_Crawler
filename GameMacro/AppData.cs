using Dungeon_Crawler.GameMacro;

internal class AppData
{
    public AppData(List<Game> savedGames)
    {
        this.SavedGames = savedGames;
    }

    private Game newGame;

    public List<Game> SavedGames;

    private Settings settings = new();
    public void RunMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
        ConsoleKeyInfo input = new();


        while (input.Key != ConsoleKey.Escape)
        {
            if (input.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (settings.IsDefaultSettings)
                {
                    StartNewGame(settings);
                }
                else
                {
                    if (settings.ShouldAnimateDiceThrows)
                    {
                        Console.WriteLine("You have choosen to animate dice throws.");
                        Console.WriteLine("Please maximise your console window now. Press any key when ready.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    StartNewGame(settings);
                }
            }
            else if (input.Key == ConsoleKey.L)
            {
                ChooseSavedGame();
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                settings = GetSettings();
            }
            else if (input.Key == ConsoleKey.Backspace)
            {
                settings = new Settings();
            }

            Console.Clear();

            WriteMenuBorders();

            Console.SetCursorPosition(13, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Hoarse Dungeon");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, 6);
            Console.Write("Start new game".PadRight(17));
            Console.Write("|".PadRight(7) + "\"Enter\"");

            Console.SetCursorPosition(2, 7);
            Console.Write("Load Game".PadRight(17));
            Console.Write("|".PadRight(7) + "\"L\"");

            Console.SetCursorPosition(2, 8);
            Console.Write("Settings".PadRight(17));
            Console.Write("|".PadRight(7) + "\"Space\"");

            Console.SetCursorPosition(2, 9);
            Console.Write("Exit".PadRight(17));
            Console.Write("|".PadRight(7) + "\"ESC\"");

            Console.SetCursorPosition(2, 11);

            if (settings.IsDefaultSettings)
            {
                Console.Write("Current settings: Default");
            }
            else
            {
                Console.Write("Current settings: Modified");

                Console.SetCursorPosition(2, 12);
                Console.Write("\"Backspace\": Load default settings.");
            }


            input = Console.ReadKey(true);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    public void StartNewGame(Settings settings)
    {
        newGame = new();
        newGame.CreateNewGame(settings);
        newGame.RunGameLoop();
    }
    public void ChooseSavedGame()
    {
        Console.Clear();

        WriteMenuBorders();
        if (SavedGames[0].Settings.ShouldAnimateDiceThrows)
        {
            Console.WriteLine("You have chosen to animate dice throws.");
            Console.WriteLine("Please maximise your console window now. Press any key when ready.");
            Console.ReadKey();
            Console.Clear();
        }
        SavedGames[0].CurrentLevel.DrawLevel();
        SavedGames[0].RunGameLoop();
    }
    public static Settings GetSettings()
    {
        Console.Clear();
        Settings settings = new(isDefaultSettings: false);

        Console.WriteLine("Enter hero name. Simply press enter for default name (highly recommended)");
        string heroName = Console.ReadLine();

        if (heroName != "")
        {
            settings.ChosenHeroName = heroName;
        }

        Console.WriteLine("Do you want to animate dice throws! YES/NO (YES is Highly recommended)");
        string input = String.Empty;
        while (input.ToLower() != "yes" && input.ToLower() != "no")
        {
            Console.SetCursorPosition(0, 3);
            input = Console.ReadLine();
        }

        bool shouldAnimate = false;
        if (input.ToLower() == "yes")
        {
            shouldAnimate = true;
        }
        settings.ShouldAnimateDiceThrows = shouldAnimate;

        return settings;
    }

    private void WriteMenuBorders()
    {
        int cursorX = 0;
        int cursorY = 0;
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 39; x++)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                if (y == 0 || y == 4 || y == 14)
                {
                    Console.Write("#");
                    cursorX++;
                }
                else
                {
                    if (x == 0 || x == 38)
                    {
                        Console.Write("#");
                        cursorX++;
                    }
                    else
                    {
                        Console.Write(" ");
                        cursorX++;
                    }
                }
            }

            cursorY++;
            cursorX = 0;
        }
    }
}