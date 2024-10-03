//Dungeon Crawler.
Console.CursorVisible = false;
LevelData levelOne = new LevelData();
levelOne.Load("Level1.txt");


//Game Loop
while (levelOne.Hero.IsAlive)
{
    levelOne.PrintStatusBar();
    levelOne.UpdateVision();
    levelOne.DrawLevel();
    levelOne.MoveCharacters();
    
}
