
using Dungeon_Crawler.GameMacro;

class Snake : Enemy
{
    public void MakeTurn(LevelData currentLevel)
    {
        double distanceFromHero = Position.CalculateDistanceBetweenPositions(Position, currentLevel.Hero.Position);
        bool heroIsInFleeingRange = CheckIfHeroIsInFleeingRange(distanceFromHero);

        if (heroIsInFleeingRange)
        {
            Position nextPosition = GetNextPosition(currentLevel, distanceFromHero);
            Move(nextPosition);
        }
        else if (distanceFromHero == 1)
        {
            if (ShouldAnimateDiceThrows)
            {
                EnterAnimatedCombatPhaseWith(currentLevel.Hero);
            }
            else
            {
                EnterCombatPhaseWith(currentLevel.Hero);
            }
        }

    }
    public bool CheckIfHeroIsInFleeingRange(double distanceFromHero)
    {
        if (distanceFromHero > 1 && distanceFromHero <= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Position GetNextPosition(LevelData currentLevel, double distanceFromHero)
    {
        Position bestPosition = CheckIfThereIsBetterPosition(currentLevel, distanceFromHero);

        return bestPosition;
    }
    public Position CheckIfThereIsBetterPosition(LevelData currentLevel, double distanceFromHero)
    {
        double largestDistanceFromHero = distanceFromHero;
        Position bestPosition = Position;

        for (int i = 0; i < 4; i++)
        {
            Direction direction = (Direction)i;

            Position potentialPosition = Position.GetPositionOneStepIn(direction);

            LevelElement elementCollidedWith = CheckCollision(currentLevel.Elements, potentialPosition);

            if (elementCollidedWith == null)
            {
                double potentialPositionDistanceFromHero = Position.CalculateDistanceBetweenPositions(potentialPosition, currentLevel.Hero.Position);

                bool potentialPositionIsFurtherFromHero = potentialPositionDistanceFromHero > largestDistanceFromHero;

                if (potentialPositionIsFurtherFromHero)
                {
                    largestDistanceFromHero = potentialPositionDistanceFromHero;
                    bestPosition = potentialPosition;
                }
            }
        }

        return bestPosition;
    }
    public Snake(Position position, bool shouldAnimateDiceThrows, Game game)
    {
        Sprite = 'S';
        SpriteColor = ConsoleColor.Green;
        Position = position;
        IsAlive = true;
        Name = "snake";
        HP = 10;
        AttackDice = new Dice(3, 4, 2);
        DefenceDice = new Dice(1, 8, 2);
        WasAttackedThisTurn = false;
        if (shouldAnimateDiceThrows)
        {
            ShouldAnimateDiceThrows = true;
        }
        Game = game;
        LogEvent += Game.levelElement_LogMessageSent;
    }
}
