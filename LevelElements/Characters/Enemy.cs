using MongoDB.Bson.Serialization.Attributes;

[BsonKnownTypes(typeof(Rat), typeof(Snake))]

abstract class Enemy : Character
{

}
