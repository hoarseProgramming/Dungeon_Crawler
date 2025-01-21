namespace Dungeon_Crawler.GameMacro
{
    internal class Log
    {
        public List<LogMessage> LogMessages { get; set; } = [];

        public void AddLogMessage(LogMessage logMessage)
        {
            logMessage.LogNumber = LogMessages.Count + 1;

            LogMessages.Add(logMessage);
        }
    }
}
