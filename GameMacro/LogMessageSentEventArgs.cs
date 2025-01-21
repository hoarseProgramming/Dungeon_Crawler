namespace Dungeon_Crawler.GameMacro
{
    internal class LogMessageSentEventArgs : EventArgs
    {
        public LogMessageSentEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
        public LogMessage LogMessage { get; set; }
    }
}
