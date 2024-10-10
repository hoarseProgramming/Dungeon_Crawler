
enum Direction { UP, DOWN, LEFT, RIGHT }
struct Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static double CalculateDistanceBetweenPositions(Position posOne, Position posTwo)
    {
        double xDifference = posOne.X - posTwo.X;
        double yDifference = posOne.Y - posTwo.Y;
        return Math.Sqrt(Math.Pow(xDifference, 2) + Math.Pow(yDifference, 2));
    }
    public Position GetPositionOneStepIn(Direction direction)
    {
        if (direction == Direction.UP)
        {
            Y--;
        }
        else if (direction == Direction.DOWN)
        {
            Y++;
        }
        else if (direction == Direction.LEFT)
        {
            X--;
        }
        else
        {
            X++;
        }
        return this;
    }

}
