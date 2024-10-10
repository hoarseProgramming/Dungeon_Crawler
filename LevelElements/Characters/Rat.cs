
class Rat : Enemy
{
    public void MakeTurn(LevelData currentLevel)
    {
        Position potentialPosition = GetPotentialPosition();

        LevelElement elementCollidedWith = CheckCollision(currentLevel.Elements, potentialPosition);

        if (elementCollidedWith is Hero opponent)
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
        else if (elementCollidedWith is Enemy || elementCollidedWith is Wall) { }
        else
        {
            Move(potentialPosition);
        }        
    }
    public Position GetPotentialPosition()
    {
        Random r = new Random();
        Direction direction = (Direction)r.Next(4);

        return direction switch
        {
            Direction.UP => Position.GetPositionOneStepIn(direction),
            Direction.DOWN => Position.GetPositionOneStepIn(direction),
            Direction.LEFT => Position.GetPositionOneStepIn(direction),
            Direction.RIGHT => Position.GetPositionOneStepIn(direction)
        };
    }
    public Rat(Position position, bool shouldAnimateDiceThrows)
    {
        Sprite = 'r';
        SpriteColor = ConsoleColor.Red;
        Position = position;
        IsAlive = true;
        Name = "rat";
        HP = 10;
        AttackDice = new Dice(1, 6, 3);
        DefenceDice = new Dice(1, 6, 1);
        WasAttackedThisTurn = false;
        if (shouldAnimateDiceThrows)
        {
            ShouldAnimateDiceThrows = true;
        }
    }
}
