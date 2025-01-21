using Dungeon_Crawler.GameMacro;

class Wall : LevelElement
{
    public Wall(Position position, Game game)
    {
        Sprite = '#';
        SpriteColor = ConsoleColor.White;
        Position = position;
        Game = game;
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
