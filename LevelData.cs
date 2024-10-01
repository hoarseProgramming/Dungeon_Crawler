
class LevelData
{
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }
    public Hero Hero { get; set; }

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
                if (currentChar == '@')
                {
                    _elements.Add(new Hero(readerPosition));
                    Hero = (Hero)Elements[Elements.Count - 1];
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
    public void DrawLevel()
    {
        foreach (var element in Elements)
        {
            element.Draw(element.IsInsideVisionRange);
        }
    }
    public void UpdateVision()
    {
        foreach (var element in Elements)
        {
            element.UpdateIsInsideVisionRange(Hero);
        }
    }
    public void MoveCharacters()
    {
        Hero.Move(Elements);
        foreach (var element in Elements)
        {
            if(element is Enemy)
            {
                (element as Rat)?.Move(Elements);
                (element as Snake)?.Move(Elements, Hero);
            }
        }
    }
}