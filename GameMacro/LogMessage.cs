namespace Dungeon_Crawler.GameMacro;

enum MessageType
{
    Attack,
    Retaliation,
    Death,
    Movement,
    Save
}

internal class LogMessage
{
    public LogMessage(LevelElement sender, string message, MessageType messageType, int attackDamage = 0, bool isKillingBlow = false)
    {
        Sender = sender;
        Message = message;
        MessageType = messageType;
        AttackDamage = attackDamage;
        IsKillingBlow = isKillingBlow;
    }
    public MessageType MessageType { get; set; }
    public LevelElement Sender { get; set; }
    public string Message { get; set; }
    public int LogNumber { get; set; }
    public int AttackDamage { get; set; }
    public bool IsKillingBlow { get; set; }
}
