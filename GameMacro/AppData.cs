using Dungeon_Crawler.GameMacro;

internal class AppData
{
    public AppData(Game[] savedGames)
    {
        SavedGames = savedGames;

        foreach (var game in savedGames)
        {
            if (game is not null)
            {
                game.DeMongoGame(this);
            }
        }
    }

    private Game? SelectedGame;

    public Game[] SavedGames;

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
                SelectSaveFileForLoading();
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                settings = GetSettings();
            }
            else if (input.Key == ConsoleKey.D)
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
                Console.Write("\"D\": Load default settings.");
            }


            input = Console.ReadKey(true);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    public void StartNewGame(Settings settings)
    {
        SelectedGame = new();
        SelectedGame.CreateNewGame(settings, this);
        SelectedGame.RunGameLoop();
    }
    public void SelectSaveFileForLoading()
    {
        Console.Clear();

        WriteMenuBorders();

        ConsoleKeyInfo input = new();

        Console.SetCursorPosition(4, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Select save file: 1, 2 or 3. ");
        Console.ForegroundColor = ConsoleColor.White;

        while (
            input.Key != ConsoleKey.D1 &&
            input.Key != ConsoleKey.D2 &&
            input.Key != ConsoleKey.D3 &&
            input.Key != ConsoleKey.Backspace
            )
        {
            if (SavedGames[0] is not null)
            {
                Console.SetCursorPosition(2, 6);
                Console.Write("1:".PadRight(5) + $"Name: {SavedGames[0].Hero.Name}");
                Console.SetCursorPosition(2, 7);
                Console.Write($"Turn: {SavedGames[0].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[0].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 6);
                Console.Write("1:".PadRight(5) + "Empty");
            }

            if (SavedGames[1] is not null)
            {
                Console.SetCursorPosition(2, 9);
                Console.Write("2:".PadRight(5) + $"Name: {SavedGames[1].Hero.Name}");
                Console.SetCursorPosition(2, 10);
                Console.Write($"Turn: {SavedGames[1].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[1].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 9);
                Console.Write("2:".PadRight(5) + "Empty");
            }

            if (SavedGames[2] is not null)
            {
                Console.SetCursorPosition(2, 12);
                Console.Write("3:".PadRight(5) + $"Name: {SavedGames[2].Hero.Name}");
                Console.SetCursorPosition(2, 13);
                Console.Write($"Turn: {SavedGames[2].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[2].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 12);
                Console.Write("3:".PadRight(5) + "Empty");
            }

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
                    Console.WriteLine("You have chosen to animate dice throws.");
                    Console.WriteLine("Please maximise your console window now. Press any key when ready.");
                    Console.ReadKey();
                    Console.Clear();
                }

                Console.Clear();
                SelectedGame.CurrentLevel.DrawLevel();
                SelectedGame.RunGameLoop();
            }

            Console.Clear();
        }
    }
    public int SelectSaveFileForSaving()
    {
        Console.Clear();

        WriteMenuBorders();

        ConsoleKeyInfo input = new();

        Console.SetCursorPosition(4, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Select save file: 1, 2 or 3. ");
        Console.SetCursorPosition(4, 3);
        Console.Write("Return to game - Backspace");
        Console.ForegroundColor = ConsoleColor.White;

        while (input.Key != ConsoleKey.D1 &&
            input.Key != ConsoleKey.D2 &&
            input.Key != ConsoleKey.D3 &&
            input.Key != ConsoleKey.Backspace)
        {
            if (SavedGames[0] is not null)
            {
                Console.SetCursorPosition(2, 6);
                Console.Write("1:".PadRight(5) + $"Name: {SavedGames[0].Hero.Name}");
                Console.SetCursorPosition(2, 7);
                Console.Write($"Turn: {SavedGames[0].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[0].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 6);
                Console.Write("1:".PadRight(5) + "Empty");
            }

            if (SavedGames[1] is not null)
            {
                Console.SetCursorPosition(2, 9);
                Console.Write("2:".PadRight(5) + $"Name: {SavedGames[1].Hero.Name}");
                Console.SetCursorPosition(2, 10);
                Console.Write($"Turn: {SavedGames[1].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[1].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 9);
                Console.Write("2:".PadRight(5) + "Empty");
            }

            if (SavedGames[2] is not null)
            {
                Console.SetCursorPosition(2, 12);
                Console.Write("3:".PadRight(5) + $"Name: {SavedGames[2].Hero.Name}");
                Console.SetCursorPosition(2, 13);
                Console.Write($"Turn: {SavedGames[2].Hero.Turn}".PadRight(18) + "|".PadRight(3) + $"HP: {SavedGames[2].Hero.HP}");
            }
            else
            {
                Console.SetCursorPosition(2, 12);
                Console.Write("3:".PadRight(5) + "Empty");
            }

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
                    Console.Clear();

                    WriteMenuBorders();

                    Console.SetCursorPosition(4, 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Overwrite save? YES/NO");

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

        Console.WriteLine("Enter hero name. Simply press enter for default name");
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
            for (int x = 0; x < 39; x++)
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
}