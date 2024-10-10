
class Wall : LevelElement
{
    public Wall(Position position)
    {
        Sprite = '#';
        SpriteColor = ConsoleColor.White;
        Position = position;
    }
    public override void UpdateIsInsideVisionRange(Hero hero)
    {
        double distanceBetweenPositions = Position.CalculateDistanceBetweenPositions(hero.Position, Position);
        if (distanceBetweenPositions <= hero.VisionRange)
        {
            IsInsideVisionRange = true;
        }
    }
}
