//Dungeon Crawler.
Console.CursorVisible = false;
LevelData levelOne = new LevelData();
levelOne.Load("Level1.txt");

//Game Loop
while (true)
{
    levelOne.UpdateVision();
    levelOne.DrawLevel();
    //if not Wall?
    levelOne.MoveCharacters();
    //evelOne.Hero.Move(levelOne.Elements);  
}
