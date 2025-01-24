using Dungeon_Crawler;
using Dungeon_Crawler.GameMacro;

internal class AppData
{

    public bool HasEstablishedConnectionToDatabase = false;

    private Game? SelectedGame;

    public Game[] SavedGames;

    private Settings settings = new();
    public async Task RunMainMenu()
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
                    await StartNewGame(settings);
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
                    await StartNewGame(settings);
                }
            }
            else if (input.Key == ConsoleKey.L)
            {
                if (HasEstablishedConnectionToDatabase) await SelectSaveFileForLoading();
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                settings = GetSettings();
                Console.Clear();
            }
            else if (input.Key == ConsoleKey.R)
            {
                if (!HasEstablishedConnectionToDatabase) await LoadSavedGames();
            }
            else if (input.Key == ConsoleKey.D)
            {
                settings = new Settings();
            }

            ConsoleWriter.WriteMainMenu(HasEstablishedConnectionToDatabase);

            input = Console.ReadKey(true);
        }
    }
    public async Task StartNewGame(Settings settings)
    {
        SelectedGame = new();
        SelectedGame.CreateNewGame(settings, this);
        await SelectedGame.RunGameLoop();
    }
    public async Task SelectSaveFileForLoading()
    {
        ConsoleWriter.WriteSaveFiles(SavedGames);

        ConsoleKeyInfo input = new();

        while (
            input.Key != ConsoleKey.D1 &&
            input.Key != ConsoleKey.D2 &&
            input.Key != ConsoleKey.D3 &&
            input.Key != ConsoleKey.Backspace
            )
        {
            input = Console.ReadKey(true);
        }

        if (input.Key != ConsoleKey.Backspace)
        {
            if (input.Key == ConsoleKey.D1)
            {
                SelectedGame = SavedGames[0];
            }
            else if (input.Key == ConsoleKey.D2)
            {
                SelectedGame = SavedGames[1];
            }
            else if (input.Key == ConsoleKey.D3)
            {
                SelectedGame = SavedGames[2];
            }

            if (SelectedGame is not null)
            {
                if (SelectedGame.Settings.ShouldAnimateDiceThrows)
                {
                    Console.Clear();
                    Console.WriteLine("You have chosen to animate dice throws.");
                    Console.WriteLine("Please maximise your console window now. Press any key when ready.");
                    Console.ReadKey();
                    Console.Clear();
                }

                Console.Clear();
                SelectedGame.CurrentLevel.DrawLevel();
                await SelectedGame.RunGameLoop();
            }

            Console.Clear();
        }
    }
    public int SelectSaveFileForSaving()
    {
        Console.Clear();

        ConsoleWriter.WriteSaveFiles(SavedGames);

        ConsoleKeyInfo input = new();

        while (input.Key != ConsoleKey.D1 &&
            input.Key != ConsoleKey.D2 &&
            input.Key != ConsoleKey.D3 &&
            input.Key != ConsoleKey.Backspace)
        {
            input = Console.ReadKey(true);

            if (input.Key == ConsoleKey.D1 ||
                input.Key == ConsoleKey.D2 ||
                input.Key == ConsoleKey.D3)
            {
                int selectedSaveFile = 0;

                if (input.Key == ConsoleKey.D1)
                {
                    selectedSaveFile = 1;
                }
                else if (input.Key == ConsoleKey.D2)
                {
                    selectedSaveFile = 2;
                }
                else if (input.Key == ConsoleKey.D3)
                {
                    selectedSaveFile = 3;
                }

                if (SavedGames[selectedSaveFile - 1] is not null)
                {
                    ConsoleWriter.WriteOverwriteQuery();

                    Console.SetCursorPosition(18, 9);
                    string choice = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.White;

                    if (choice.ToLower() == "yes")
                    {
                        return selectedSaveFile;
                    }
                }
                else
                {
                    return selectedSaveFile;
                }
            }

            Console.Clear();
        }
        return 0;
    }
    public static Settings GetSettings()
    {
        Console.Clear();
        Settings settings = new(isDefaultSettings: false);

        Console.WriteLine("Enter hero name. Press \"ENTER\" for default name.");
        string heroName = Console.ReadLine();

        if (heroName != "")
        {
            settings.ChosenHeroName = heroName;
        }

        Console.WriteLine("Do you want to animate dice throws! YES/NO");
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
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 41; x++)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                if (y == 0 || y == 4 || y == 15)
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
    public async Task LoadSavedGames()
    {
        ConsoleWriter.WriteLoadingStatus();
        Console.CursorVisible = true;

        SavedGames = await DataBaseHandler.LoadGamesFromDataBase();

        if (SavedGames[0]?.Id == 4)
        {
            ConsoleWriter.WriteLoadFailMessage();


            Console.ReadKey(true);
        }
        else if (!SavedGames.Any(g => g?.Id == 1))
        {
            Console.SetCursorPosition(3, 6);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Connected to database!".PadRight(27));

            Console.SetCursorPosition(3, 7);
            Console.Write("Couldn't find any saved games.");

            Console.SetCursorPosition(3, 9);
            Console.Write("Good luck!");

            Console.CursorVisible = false;
            Console.SetCursorPosition(3, 11);
            Console.Write("Press any key to continue");
            HasEstablishedConnectionToDatabase = true;

            Console.ReadKey(true);
        }
        else
        {
            Console.SetCursorPosition(5, 6);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Games loaded successfully!");

            foreach (var game in SavedGames)
            {
                if (game is not null)
                {
                    game.DeMongoGame(this);
                }
            }

            HasEstablishedConnectionToDatabase = true;
        }

    }
}