using Dungeon_Crawler.GameMacro;
using MongoDB.Bson.Serialization.Attributes;

class Hero : Character
{
    public int Turn { get; set; }
    [BsonIgnore]
    public bool IsInMenu { get; set; } = false;
    public int VisionRange { get; set; }
    public void MakeTurn(LevelData currentLevel)
    {
        ConsoleKeyInfo input = new();

        while (
            input.Key != ConsoleKey.UpArrow &&
            input.Key != ConsoleKey.DownArrow &&
            input.Key != ConsoleKey.LeftArrow &&
            input.Key != ConsoleKey.RightArrow
            )
        {
            input = Console.ReadKey(true);

            if (input.Key == ConsoleKey.M)
            {
                Game.RunInGameMenu();
            }
        }

        Position potentialPosition = GetPotentialPosition(input.Key);

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

            //ClearAttackText();

            if (ShouldAnimateDiceThrows)
            {
                AttackDice.ClearDiceText();
            }
        }

        Turn++;
    }
    public Position GetPotentialPosition(ConsoleKey input)
    {
        //ConsoleKeyInfo input = Console.ReadKey(true);

        return input switch
        {
            ConsoleKey.UpArrow => Position.GetPositionOneStepIn(Direction.UP),
            ConsoleKey.DownArrow => Position.GetPositionOneStepIn(Direction.DOWN),
            ConsoleKey.LeftArrow => Position.GetPositionOneStepIn(Direction.LEFT),
            ConsoleKey.RightArrow => Position.GetPositionOneStepIn(Direction.RIGHT),
            _ => Position
        };
    }
    public Hero(Position position, bool shouldAnimateDiceThrows, string name, Game game)
    {
        Game = game;
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
        LogEvent += Game.levelElement_LogMessageSent;
    }
}
