
class Hero : Character
{
    public bool IsAlive { get; set; }
    public int Turn { get; set; }
    public int VisionRange { get; set; }
    public void Move(List<LevelElement> currentLevel)
    {
        Position potentialPosition = GetPotentialPosition();
        
        LevelElement elementCollidedWith = CheckCollision(currentLevel, potentialPosition);

        if (elementCollidedWith is Enemy enemy)
        {
            Console.SetCursorPosition(0, 1);
            Attack(enemy);
            enemy.Attack(this);

            if (enemy.HP <= 0)
            {
                currentLevel.Remove(enemy);
            }
            else if (HP <= 0)
            {
                IsAlive = false;
            }
        }
        else if (elementCollidedWith is Wall) {}
        else
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(' ');
            Position = potentialPosition;
        }

        Turn++;
    }
    public Position GetPotentialPosition()
    {
        ConsoleKeyInfo input = Console.ReadKey();

        if (input.Key == ConsoleKey.UpArrow)
        {
            return Position.GetPosition(Direction.UP);
        }
        else if (input.Key == ConsoleKey.DownArrow)
        {
            return Position.GetPosition(Direction.DOWN);
        }
        else if (input.Key == ConsoleKey.LeftArrow)
        {
            return Position.GetPosition(Direction.LEFT);
        }
        else if (input.Key == ConsoleKey.RightArrow)
        {
            return Position.GetPosition(Direction.RIGHT);
        }
        return Position;
    }

    public Hero(Position position, string name = "Hampus")
    {
        Sprite = '@';
        SpriteColor = ConsoleColor.Yellow;
        Position = position;
        Name = name;
        HP = 100;
        AttackDice = new Dice(2, 6, 2);
        DefenceDice = new Dice(2, 6, 0);
        IsAlive = true;
        Turn = 0;
        VisionRange = 5;
        
    }
}
