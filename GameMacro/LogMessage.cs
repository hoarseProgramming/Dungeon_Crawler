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
    public LogMessage(LevelElement sender, string message, MessageType messageType, int attackDamage = 0, bool isKillingBlow = false, Character enemy = null)
    {
        Sender = sender;
        Message = message;
        MessageType = messageType;
        AttackDamage = attackDamage;
        IsKillingBlow = isKillingBlow;
        Enemy = enemy;
    }
    public MessageType MessageType { get; set; }
    public LevelElement Sender { get; set; }
    public string Message { get; set; }
    public int LogNumber { get; set; }
    public int AttackDamage { get; set; }
    public bool IsKillingBlow { get; set; }
    public Character? Enemy { get; set; }
}
