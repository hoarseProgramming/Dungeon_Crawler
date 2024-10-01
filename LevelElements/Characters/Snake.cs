
class Snake : Enemy
{
    public void Move(List<LevelElement> currentLevel, Hero hero)
    {
        double distanceFromHero = Position.CalculateDistanceBetweenPositions(Position, hero.Position);

        if (distanceFromHero <= 2)
        {
            Position potentialPosition = GetPotentialPosition(currentLevel, hero, distanceFromHero);
            bool foundBetterPosition = !(potentialPosition.X == Position.X && potentialPosition.Y == Position.Y);
            if (foundBetterPosition)
            {
                Console.SetCursorPosition(Position.X, Position.Y);
                Console.Write(' ');
                Position = potentialPosition;
            }
        }

    }
    public Position GetPotentialPosition(List<LevelElement> currentLevel, Hero hero, double distanceFromHero)
    {
        Position potentialPosition;
        double potentialPositionDistanceFromHero;

        Position bestPosition = Position;
        double largestDistanceFromHero = distanceFromHero;

        for (int i = 0; i < 4; i++)
        {
            Direction direction = (Direction)i;

            if (direction == Direction.UP)
            {
                potentialPosition = Position.GetPosition(direction);
                potentialPositionDistanceFromHero = Position.CalculateDistanceBetweenPositions(potentialPosition, hero.Position);
                bool potentialPositionIsFurtherFromHero = potentialPositionDistanceFromHero > largestDistanceFromHero;

                if (CheckCollision(currentLevel, potentialPosition) == null && potentialPositionIsFurtherFromHero)
                {
                    largestDistanceFromHero = potentialPositionDistanceFromHero;
                    bestPosition = potentialPosition;
                }
            }
            else if (direction == Direction.DOWN)
            {
                potentialPosition = Position.GetPosition(direction);
                potentialPositionDistanceFromHero = Position.CalculateDistanceBetweenPositions(potentialPosition, hero.Position);
                bool potentialPositionIsFurtherFromHero = potentialPositionDistanceFromHero > distanceFromHero;

                if (CheckCollision(currentLevel, potentialPosition) == null && potentialPositionIsFurtherFromHero)
                {
                    largestDistanceFromHero = potentialPositionDistanceFromHero;
                    bestPosition = potentialPosition;
                }
            }
            else if (direction == Direction.LEFT)
            {
                potentialPosition = Position.GetPosition(direction);
                potentialPositionDistanceFromHero = Position.CalculateDistanceBetweenPositions(potentialPosition, hero.Position);
                bool potentialPositionIsFurtherFromHero = potentialPositionDistanceFromHero > distanceFromHero;

                if (CheckCollision(currentLevel, potentialPosition) == null && potentialPositionIsFurtherFromHero)
                {
                    largestDistanceFromHero = potentialPositionDistanceFromHero;
                    bestPosition = potentialPosition;
                }
            }
            else
            {
                potentialPosition = Position.GetPosition(direction);
                potentialPositionDistanceFromHero = Position.CalculateDistanceBetweenPositions(potentialPosition, hero.Position);
                bool potentialPositionIsFurtherFromHero = potentialPositionDistanceFromHero > distanceFromHero;

                if (CheckCollision(currentLevel, potentialPosition) == null && potentialPositionIsFurtherFromHero)
                {
                    largestDistanceFromHero = potentialPositionDistanceFromHero;
                    bestPosition = potentialPosition;
                }
            }

        }
        return bestPosition;


    }
    public override void Update()
    {
        throw new NotImplementedException();
    }
    public Snake(Position position)
    {
        Sprite = 'S';
        SpriteColor = ConsoleColor.Green;
        Position = position;
        Name = "snake";
        HP = 25;
        AttackDice = new Dice(3, 4, 2);
        DefenceDice = new Dice(1, 8, 5);
    }
}
