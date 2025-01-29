using Dungeon_Crawler.GameMacro;
using Dungeon_Crawler.LevelElements.Structures;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

class LevelData
{
    public LevelData(int levelNumber, Game game)
    {
        this.levelNumber = levelNumber;
        Game = game;
    }
    [BsonIgnore]
    public Game Game { get; set; }
    public int levelNumber { get; set; }
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements
    {
        get => _elements;
        set
        {
            _elements = value;
        }
    }
    [BsonIgnore]
    private List<Character> Characters { get; set; }
    public Hero Hero { get; set; }
    public Position EntryPoint { get; set; }
    public void Load(Settings settings, Hero hero)
    {
        LoadAllLevelElements(settings, hero);
        LoadCharacters();
    }
    public void LoadAllLevelElements(Settings settings, Hero hero)
    {
        string pathToLevel = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net8.0", $"\\Levels\\{levelNumber}.txt");

        using (StreamReader reader = new StreamReader(pathToLevel))
        {
            Position readerPosition = new Position(0, 3);
            while (!reader.EndOfStream)
            {
                char currentChar = (char)reader.Read();
                if (currentChar == '#')
                {
                    _elements.Add(new Wall(new Position(readerPosition.X, readerPosition.Y), Game));
                }
                else if (currentChar == '@')
                {
                    hero.Position = new Position(readerPosition.X, readerPosition.Y);
                    hero.ShouldAnimateDiceThrows = settings.ShouldAnimateDiceThrows;
                    hero.Name = settings.ChosenHeroName;
                    _elements.Add(hero);
                    Hero = hero;
                }
                else if (currentChar == '|' || currentChar == '_')
                {
                    _elements.Add(new Door(new Position(readerPosition.X, readerPosition.Y), Game, currentChar));
                }
                else if (currentChar == 'E')
                {
                    EntryPoint = new Position(readerPosition.X, readerPosition.Y);
                }
                else if (currentChar == 'r')
                {
                    _elements.Add(new Rat(new Position(readerPosition.X, readerPosition.Y), settings.ShouldAnimateDiceThrows, Game));
                }
                else if (currentChar == 's')
                {
                    _elements.Add(new Snake(new Position(readerPosition.X, readerPosition.Y), settings.ShouldAnimateDiceThrows, Game));
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

        if (Hero is null)
        {
            Hero = hero;
            _elements.Add(hero);
        }
    }
    public void LoadCharacters()
    {
        Characters = Elements
            .Where(element => element is Character)
            .ToList()
            .ConvertAll(new Converter<LevelElement, Character>(element => element as Character));
    }
    public void PrintStatusBar()
    {
        int heroHP = Hero.HP;
        if (Hero.HP <= 0)
        {
            heroHP = 0;
        }
        Console.SetCursorPosition(0, 0);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Name: {Hero.Name}  -  Health: {$"{heroHP}/100".PadLeft(7)}  -  Turn: {$"{Hero.Turn}".PadLeft(5)}    |   \"M\": Menu");
    }
    public void DrawLevel()
    {
        PrintStatusBar();
        foreach (var element in Elements)
        {
            element.Draw();
        }
    }

    public void EraseLevel()
    {
        foreach (var element in Elements)
        {
            element.RemoveFromPlayingField();
        }
    }
    public void UpdateVision()
    {
        foreach (var element in Elements)
        {
            element.UpdateIsInsideVisionRange(Hero);
        }
    }
    public async Task NewTurn()
    {
        await Hero.MakeTurn(this);
        if (Hero.HasExitedGame)
        {
            return;
        }
        UpdateVision();
        DrawLevel();

        foreach (var element in Elements)
        {
            if (Hero.IsAlive)
            {
                if (element is Enemy enemy)
                {
                    if (enemy.IsAlive)
                    {
                        (enemy as Rat)?.MakeTurn(this);
                        (enemy as Snake)?.MakeTurn(this);
                    }
                }
            }
        }
        UpdateVision();
        RemoveDeadCharacters();
    }
    public void RemoveDeadCharacters()
    {
        foreach (Character character in Characters)
        {
            if (!character.IsAlive)
            {
                if (character is Hero hero)
                {
                    hero.SpriteColor = ConsoleColor.Red;
                    hero.Draw();
                }
                else
                {
                    character.RemoveFromPlayingField();
                    Elements.Remove(character);
                }
            }
        }
    }
    internal void DeMongoLevel(Game game)
    {
        Game = game;

        foreach (var element in Elements)
        {
            element.SetGame(Game);
            element.LogEvent += Game.levelElement_LogMessageSent;
        }
        LoadCharacters();

        foreach (var character in Characters)
        {
            if (character is Hero)
            {
                Hero = character as Hero;
            }
        }
    }
}
