//Dungeon Crawler.

LevelData levelOne = new LevelData();
levelOne.Load("Level1.txt");

foreach (var element in levelOne.Elements)
{
    element.Draw();
}



struct Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

abstract class LevelElement
{
    public Position Position { get; set; }
    public Char Sprite { get; set; }
    public ConsoleColor SpriteColor { get; set;  }
    public void Draw()
    {
        Console.ForegroundColor = SpriteColor;
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(Sprite);
    }
}
class Wall : LevelElement
{
    public Wall(Position position)
    {
        Sprite = '#';
        SpriteColor = ConsoleColor.White;
        Position = position;
    }
}

class Hero : LevelElement
{
    public string Name { get; set; }
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public void Move()
    {

    }
    public Hero(Position position, string name = "Hampus")
    {
        Sprite = '@';
        SpriteColor = ConsoleColor.Yellow;
        Position = position;
        Name = name;
        HP = 100;
    }
}

abstract class Enemy : LevelElement
{
    public string Name { get; set; }
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public abstract void Update();
    
}
class Rat : Enemy
{
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
    }
}
class Snake : Enemy
{
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
        HP = 10;
    }
}

class Dice
{

}

class LevelData
{
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }

    public void Load(string levelFileName)
    {
        string pathToLevel = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net8.0", $"\\Levels\\{levelFileName}");

        using (StreamReader reader = new StreamReader(pathToLevel))
        { 
            Position readerPosition = new Position(0, 0);
            while (!reader.EndOfStream)
            {
                char currentChar = (char)reader.Read();
                if (currentChar == '#')
                {
                    _elements.Add(new Wall(readerPosition));
                }
                else if (currentChar == 'r')
                {
                    _elements.Add(new Rat(readerPosition));
                }
                else if (currentChar == 's')
                {
                    _elements.Add(new Snake(readerPosition));
                }
                else if (currentChar == '\n')
                {
                    readerPosition.X = 0;
                    readerPosition.Y++;
                    continue;
                }
                readerPosition.X++;
            }
        }

    }

}