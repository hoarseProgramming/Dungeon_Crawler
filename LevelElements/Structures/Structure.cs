using MongoDB.Bson.Serialization.Attributes;

namespace Dungeon_Crawler.LevelElements.Structures;
[BsonKnownTypes(typeof(Wall), typeof(Door))]

internal class Structure : LevelElement
{
    public override void UpdateIsInsideVisionRange(Hero hero)
    {
        double distanceBetweenPositions = Position.CalculateDistanceBetweenPositions(hero.Position, Position);
        if (distanceBetweenPositions <= hero.VisionRange)
        {
            IsInsideVisionRange = true;
        }
    }
}

