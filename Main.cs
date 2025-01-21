
using Dungeon_Crawler;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var saveFile = DataBaseHandler.LoadGamesFromDataBase();

var userInterface = new AppData(saveFile);

userInterface.RunMainMenu();
