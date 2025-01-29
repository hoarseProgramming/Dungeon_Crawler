using Dungeon_Crawler.GameMacro;

namespace Dungeon_Crawler.LevelElements.Structures;

internal class Door : Structure
{
    bool IsOpen = false;
    bool IsLocked;
    public int LevelDirection { get; private set; }
    public Door(Position position, Game game, char sprite, bool isOpen = false, bool isLocked = false)
    {
        Position = position;
        Game = game;
        Sprite = sprite;
        SpriteColor = ConsoleColor.White;
        IsLocked = isLocked;

        if (sprite == '|')
        {
            LevelDirection = 1;
        }
        else
        {
            isOpen = true;
            LevelDirection = -1;
        }
    }
    public void OpenDoor()
    {
        IsOpen = true;
        Sprite = '_';
        Draw();
    }
}
