
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
    public static void WriteMaximumTenInMenuLogMessages(List<LogMessage> logMessages, int currentLogIndex)
    {
        var logMessagesToPrint = logMessages.Where(l => l.LogNumber > currentLogIndex - 10 && l.LogNumber <= currentLogIndex);

        if (logMessagesToPrint is not null)
        {
            foreach (var logMessage in logMessagesToPrint)
            {
                SetTextColor(logMessage);
                Console.WriteLine($"{logMessage.LogNumber}: {logMessage.Message}");
            }
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

    public static void ClearInGameLogMessages()
    {
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(" ".PadLeft(Console.BufferWidth));
        Console.WriteLine(" ".PadLeft(Console.BufferWidth));
    }


}
