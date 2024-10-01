//Dungeon Crawler.


//Game Loop
abstract class Character : LevelElement
{
    public string Name { get; set; }
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    //public abstract void Move(List<LevelElement> currentLevel);
    //public abstract Position GetPotentialPosition();
    public LevelElement CheckCollision(List<LevelElement> currentLevel, Position potentialPosition)
    {
        foreach (LevelElement element in currentLevel)
        {
            if (element.Position.X == potentialPosition.X && element.Position.Y == potentialPosition.Y)
            {
                return element;         
            }
        }
        return null;
    }
}
