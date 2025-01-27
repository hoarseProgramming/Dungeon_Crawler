using Dungeon_Crawler.GameMacro;

namespace Dungeon_Crawler.LevelElements.Structures;

internal class Door : Structure
{
    bool IsOpen = false;
    bool IsLocked;
    public Door(Position position, Game game, char sprite, bool isLocked = false)
    {
        Position = position;
        Game = game;
        Sprite = sprite;
        SpriteColor = ConsoleColor.White;
        IsLocked = isLocked;

        if (sprite == '_') IsOpen = true;
    }
    public void OpenDoor()
    {
        IsOpen = true;
        Sprite = '_';
        Draw();
    }
}
