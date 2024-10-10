
class LevelData
{
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }
    private List<Character> _characters = new List<Character>();
    public List<Character> Characters 
    { 
        get 
        { 
            return _characters; 
        } 
        set 
        { 
            _characters = value; 
        } 
    }
    public Hero Hero { get; set; }
    public void Load(string levelFileName, List<object> settings)
    {
        LoadAllLevelElements(levelFileName, settings);
        LoadCharacters();
        UpdateVision();
        PrintStatusBar();
        DrawLevel();
    }
    public void LoadAllLevelElements(string levelFileName, List<Object> settings)
    {
        string pathToLevel = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net8.0", $"\\Levels\\{levelFileName}");

        using (StreamReader reader = new StreamReader(pathToLevel))
        {
            Position readerPosition = new Position(0, 3);
            while (!reader.EndOfStream)
            {
                char currentChar = (char)reader.Read();
                if (currentChar == '#')
                {
                    _elements.Add(new Wall(new Position(readerPosition.X, readerPosition.Y)));
                }
                else if (currentChar == '@')
                {
                    _elements.Add(new Hero(new Position(readerPosition.X, readerPosition.Y), (bool)settings[0], (string)settings[1]));
                    Hero = (Hero)Elements[Elements.Count - 1];
                }
                else if (currentChar == 'r')
                {
                    _elements.Add(new Rat(new Position(readerPosition.X, readerPosition.Y), (bool)settings[0]));
                }
                else if (currentChar == 's')
                {
                    _elements.Add(new Snake(new Position(readerPosition.X, readerPosition.Y), (bool)settings[0]));
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
        Console.WriteLine($"Name: {Hero.Name}  -  Health: {$"{heroHP}/100".PadLeft(7)}  -  Turn: {$"{Hero.Turn}".PadLeft(5)}");
    }
    public void DrawLevel()
    {
        PrintStatusBar();
        foreach (var element in Elements)
        {
            element.Draw();
        }
        if (!Hero.IsAlive)
        {            
            Console.ForegroundColor = ConsoleColor.Red;
            if (Hero.ShouldAnimateDiceThrows)
            {
                Hero.AttackDice.ClearDiceText();
            }
            Console.SetCursorPosition(0, 21);
            Console.WriteLine("Game Over! Press any key to enter main menu");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(true);
        }
    }
    public void UpdateVision()
    {
        foreach (var element in Elements)
        {
            element.UpdateIsInsideVisionRange(Hero);
        }
    }
    public void NewTurn()
    {
        Hero.MakeTurn(this);

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
}
