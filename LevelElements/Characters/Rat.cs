
class Rat : Enemy
{
    public void Move(List<LevelElement> currentLevel)
    {
        Position potentialPosition = GetPotentialPosition();

        LevelElement elementCollidedWith = CheckCollision(currentLevel, potentialPosition);

        if (elementCollidedWith is Hero)
        {
            Console.SetCursorPosition(0, 1);
            Attack((Character)elementCollidedWith);
            ((Character)elementCollidedWith).Attack(this);
        }
        else if (elementCollidedWith is Enemy || elementCollidedWith is Wall) { }
        else
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(' ');
            Position = potentialPosition;
        }
    }
    public Position GetPotentialPosition()
    {
        Random r = new Random();
        Direction direction = (Direction)r.Next(4);
        if (direction == Direction.UP)
        {
            return new Position(Position.X, Position.Y - 1);
        }
        else if (direction == Direction.DOWN)
        {
            return new Position(Position.X, Position.Y + 1);
        }
        else if (direction == Direction.LEFT)
        {
            return new Position(Position.X - 1, Position.Y);
        }
        else
        {
            return new Position(Position.X + 1, Position.Y);
        }
    }
    public override void Update()
    {
        throw new NotImplementedException();
    }
    public Rat(Position position)
    {
        Sprite = 'r';
        SpriteColor = ConsoleColor.Red;
        Position = position;
        Name = "rat";
        HP = 10;
        AttackDice = new Dice(1, 6, 3);
        DefenceDice = new Dice(1, 6, 1);
    }
}
