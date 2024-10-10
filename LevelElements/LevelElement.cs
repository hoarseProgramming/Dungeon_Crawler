
abstract class LevelElement
{
    protected bool _isInsideVisionRange = false;
    public bool IsInsideVisionRange
    {
        get { return _isInsideVisionRange; }
        set { _isInsideVisionRange = value; }     
    }
    public Position Position { get; set; }
    public Char Sprite { get; set; }
    public ConsoleColor SpriteColor { get; set;  }
    public void Draw()
    {
        Console.SetCursorPosition(Position.X, Position.Y);

        if (IsInsideVisionRange)
        {
            if (this is Wall)
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
}
