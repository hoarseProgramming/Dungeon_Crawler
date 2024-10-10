
class Hero : Character
{
    
    public int Turn { get; set; }
    public int VisionRange { get; set; }
    public void MakeTurn(LevelData currentLevel)
    {
        Position potentialPosition = GetPotentialPosition();
        
        LevelElement elementCollidedWith = CheckCollision(currentLevel.Elements, potentialPosition);

        if (elementCollidedWith is Enemy opponent)
        {
            if (ShouldAnimateDiceThrows)
            {
                EnterAnimatedCombatPhaseWith(opponent);
            }
            else
            {
                EnterCombatPhaseWith(opponent);
            }
        }
        else if (!(elementCollidedWith is Wall)) 
        {
            Move(potentialPosition);
            ClearAttackText();
            if (ShouldAnimateDiceThrows)
            {
                AttackDice.ClearDiceText();
            }       
        }
  
        Turn++;
    }
    public Position GetPotentialPosition()
    {
        ConsoleKeyInfo input = Console.ReadKey(true);

        return input.Key switch
        {
            ConsoleKey.UpArrow => Position.GetPositionOneStepIn(Direction.UP),
            ConsoleKey.DownArrow => Position.GetPositionOneStepIn(Direction.DOWN),
            ConsoleKey.LeftArrow => Position.GetPositionOneStepIn(Direction.LEFT),
            ConsoleKey.RightArrow => Position.GetPositionOneStepIn(Direction.RIGHT),
            _ => Position
        };
    }
    public Hero(Position position, bool shouldAnimateDiceThrows, string name)
    {
        Sprite = '@';
        SpriteColor = ConsoleColor.Yellow;
        Position = position;
        IsAlive = true;
        Name = name;
        HP = 40;
        AttackDice = new Dice(2, 6, 2);
        DefenceDice = new Dice(2, 6, 0);
        WasAttackedThisTurn = false;
        Turn = 0;
        VisionRange = 5;
        if (shouldAnimateDiceThrows)
        {
            ShouldAnimateDiceThrows = true;
        }
    }
}
