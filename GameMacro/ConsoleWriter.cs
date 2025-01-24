


namespace Dungeon_Crawler.GameMacro;

internal static class ConsoleWriter
{
    public static void WriteInGameLogMessages(LogMessage mostRecentLogMessage, LogMessage? lastLogMessage)
    {
        var messageType = mostRecentLogMessage.MessageType;

        if (lastLogMessage is null)
        {
            Console.SetCursorPosition(0, 1);

            Console.Write("".PadRight(Console.BufferWidth));

            SetTextColor(mostRecentLogMessage);

            Console.SetCursorPosition(0, 2);

            Console.Write(mostRecentLogMessage.Message.PadRight(Console.BufferWidth));
        }
        else
        {
            SetTextColor(lastLogMessage);

            Console.SetCursorPosition(0, 1);

            Console.Write(lastLogMessage.Message.PadRight(Console.BufferWidth));

            SetTextColor(mostRecentLogMessage);

            Console.SetCursorPosition(0, 2);

            Console.Write(mostRecentLogMessage.Message.PadRight(Console.BufferWidth));
        }
    }
    private static void WriteMenuLine()
    {
        for (int i = 0; i < 41; i++)
        {
            Console.Write('#');
        }
        Console.WriteLine();
    }
    public static void WriteMaximumTenInMenuLogMessages(List<LogMessage> logMessages, int currentLogIndex)
    {
        var logMessagesToPrint = logMessages.Where(l => l.LogNumber > currentLogIndex - 10 && l.LogNumber <= currentLogIndex).ToList();

        if (logMessagesToPrint.Count > 0)
        {
            WriteMenuLine();

            Console.WriteLine("#".PadRight(20) + "Log".PadRight(20) + "#");

            WriteMenuLine();

            foreach (var logMessage in logMessagesToPrint)
            {
                string message = logMessage.Message;

                if (logMessage.MessageType == MessageType.Attack || logMessage.MessageType == MessageType.Retaliation)
                {
                    string shorterMessage = string.Empty;

                    if (logMessage.Sender is Enemy enemy)
                    {
                        if (enemy is Rat)
                        {
                            shorterMessage = $"The rat attacked you for {logMessage.AttackDamage} dmg.";
                        }
                        else
                        {
                            shorterMessage = $"The snake attacked you for {logMessage.AttackDamage} dmg.";
                        }
                    }
                    else
                    {
                        shorterMessage = $"You attacked the {logMessage.Enemy.Name.ToLower()} for {logMessage.AttackDamage} dmg.";
                    }
                    message = shorterMessage;
                }
                Console.Write("# ");
                SetTextColor(logMessage);
                Console.Write($"{logMessage.LogNumber}: {message}".PadRight(38));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("#");
            }

            Console.ForegroundColor = ConsoleColor.White;
            WriteMenuLine();
            Console.WriteLine("# Press \"UP/DOWN\" to navigate log.".PadRight(40) + "#");
            Console.WriteLine("# Press \"Backspace\" to go back.".PadRight(40) + "#");

            for (int i = 0; i < 41; i++)
            {
                Console.Write('#');
            }
        }
        else
        {
            Console.WriteLine("No log");
            Console.WriteLine("Press \"Backspace\" to go back.");
        }
    }

    private static void SetTextColor(LogMessage logMessage)
    {
        if (logMessage.IsKillingBlow)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (logMessage.Sender is Hero)
        {
            if (logMessage.MessageType == MessageType.Save)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else
        {
            if (logMessage.MessageType == MessageType.Movement)
            {
                Console.ForegroundColor = logMessage.Sender.SpriteColor;
            }
            else
            {
                Console.ForegroundColor = GetColorFromDamage(logMessage.AttackDamage);
            }
        }
    }

    private static ConsoleColor GetColorFromDamage(int attackDamage) =>
        attackDamage switch
        {
            0 => ConsoleColor.Green,
            < 5 => ConsoleColor.DarkYellow,
            _ => ConsoleColor.DarkMagenta
        };

    private static void WriteMenuBorders()
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
                    if (x == 0 || x == 40)
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

    public static void ClearInGameLogMessages()
    {
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(" ".PadLeft(Console.BufferWidth));
        Console.WriteLine(" ".PadLeft(Console.BufferWidth));
    }

    internal static void WriteMainMenu(bool hasConnection)
    {
        WriteMenuBorders();

        Console.SetCursorPosition(13, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Hoarse Dungeon");

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(2, 6);
        Console.Write("Start new game".PadRight(19));
        Console.Write("|".PadRight(7) + "\"Enter\"");

        if (!hasConnection)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
        Console.SetCursorPosition(2, 7);
        Console.Write("Load Game".PadRight(19));
        Console.Write("|".PadRight(7) + "\"L\"");

        Console.ForegroundColor = ConsoleColor.White;

        Console.SetCursorPosition(2, 8);
        Console.Write("Settings".PadRight(19));
        Console.Write("|".PadRight(7) + "\"Space\"");

        if (hasConnection)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
        Console.SetCursorPosition(2, 9);
        Console.Write("Retry connection".PadRight(19));
        Console.Write("|".PadRight(7) + "\"R\"");

        Console.ForegroundColor = ConsoleColor.White;

        Console.SetCursorPosition(2, 10);
        Console.Write("Exit".PadRight(19));
        Console.Write("|".PadRight(7) + "\"ESC\"");

        Console.SetCursorPosition(0, 12);
        WriteMenuLine();

        Console.SetCursorPosition(2, 13);

        if (hasConnection)
        {
            Console.Write("Connected to database: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Yes");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.Write("Connected to database: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("No");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    internal static void WriteSaveFiles(Game[] savedGames)
    {
        Console.Clear();

        WriteMenuBorders();

        Console.SetCursorPosition(6, 1);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Select save file: 1, 2 or 3. ");
        Console.SetCursorPosition(8, 3);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("\"Backspace\" - Go back");

        Console.ForegroundColor = ConsoleColor.White;

        if (savedGames[0] is not null)
        {
            Console.SetCursorPosition(8, 5);
            Console.Write("1:".PadRight(5) + $"Name: {savedGames[0].Hero.Name}");
            Console.SetCursorPosition(8, 6);
            Console.WriteLine($"Turn: {savedGames[0].Hero.Turn}".PadRight(10) + "|".PadRight(3) + $"HP: {savedGames[0].Hero.HP}");
            WriteMenuLine();
            WriteMenuLine();
        }
        else
        {
            Console.SetCursorPosition(8, 5);
            Console.Write("1:".PadRight(5) + "Empty");
            Console.WriteLine();
            WriteMenuLine();
            WriteMenuLine();
        }

        if (savedGames[1] is not null)
        {
            Console.SetCursorPosition(8, 9);
            Console.Write("2:".PadRight(5) + $"Name: {savedGames[1].Hero.Name}");
            Console.SetCursorPosition(8, 10);
            Console.WriteLine($"Turn: {savedGames[1].Hero.Turn}".PadRight(10) + "|".PadRight(3) + $"HP: {savedGames[1].Hero.HP}");
            WriteMenuLine();
            WriteMenuLine();
        }
        else
        {
            Console.SetCursorPosition(8, 9);
            Console.WriteLine("2:".PadRight(5) + "Empty");
            Console.WriteLine();
            WriteMenuLine();
            WriteMenuLine();
        }

        if (savedGames[2] is not null)
        {
            Console.SetCursorPosition(8, 13);
            Console.Write("3:".PadRight(5) + $"Name: {savedGames[2].Hero.Name}");
            Console.SetCursorPosition(8, 14);
            Console.Write($"Turn: {savedGames[2].Hero.Turn}".PadRight(10) + "|".PadRight(3) + $"HP: {savedGames[2].Hero.HP}");
        }
        else
        {
            Console.SetCursorPosition(8, 13);
            Console.Write("3:".PadRight(5) + "Empty");
        }
    }

    internal static void WriteOverwriteQuery()
    {
        Console.Clear();

        WriteMenuBorders();

        Console.SetCursorPosition(4, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Overwrite save? YES/NO");
    }

    internal static void WriteLoadingStatus()
    {
        WriteMenuBorders();

        Console.SetCursorPosition(11, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Loading saved games");

        Console.SetCursorPosition(8, 6);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Establishing connection..");
    }

    internal static void WriteLoadFailMessage()
    {
        Console.SetCursorPosition(6, 6);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Couldn't connect to database!");

        Console.SetCursorPosition(6, 7);
        Console.Write("(Loading/saving disabled)");

        Console.SetCursorPosition(6, 9);
        Console.Write("Try reconnecting in menu.");

        Console.CursorVisible = false;
        Console.SetCursorPosition(6, 11);
        Console.Write("Press any key to continue");
    }
}
