using Dungeon_Crawler.GameMacro;
using Dungeon_Crawler.LevelElements.Structures;
using MongoDB.Bson.Serialization.Attributes;

[BsonKnownTypes(typeof(Structure), typeof(Character))]
abstract class LevelElement
{
    [BsonIgnore]
    public Game Game { get; set; }
    protected bool _isInsideVisionRange = false;
    public bool IsInsideVisionRange
    {
        get { return _isInsideVisionRange; }
        set { _isInsideVisionRange = value; }
    }
    public Position Position { get; set; }
    public Char Sprite { get; set; }
    public ConsoleColor SpriteColor { get; set; }
    public event EventHandler<LogMessageSentEventArgs>? LogEvent;
    protected virtual void OnLogEvent(LogMessageSentEventArgs e) => LogEvent?.Invoke(this, e);
    public void Draw()
    {
        Console.SetCursorPosition(Position.X, Position.Y);

        if (IsInsideVisionRange)
        {
            if (this is Structure)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = SpriteColor;
                Console.Write(Sprite);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ForegroundColor = SpriteColor;
                Console.Write(Sprite);
            }
        }
        else
        {
            Console.Write(' ');
        }
    }
    public void RemoveFromPlayingField()
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');
    }
    public virtual void UpdateIsInsideVisionRange(Hero hero)
    {
        double distanceBetweenPositions = Position.CalculateDistanceBetweenPositions(hero.Position, Position);
        if (distanceBetweenPositions <= hero.VisionRange)
        {
            IsInsideVisionRange = true;
        }
        else
        {
            IsInsideVisionRange = false;
        }
    }
    public void SetGame(Game game)
    {
        this.Game = game;
    }
}
