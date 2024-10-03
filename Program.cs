//Dungeon Crawler.
Console.CursorVisible = false;
LevelData levelOne = new LevelData();
levelOne.Load("Level1.txt");

//Game Loop
while (true)
{
    Console.SetCursorPosition(0, 0);
    Console.WriteLine($"HP: {levelOne.Hero.HP}");
    levelOne.UpdateVision();
    levelOne.DrawLevel();
    levelOne.MoveCharacters();
    
}
