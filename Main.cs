
using Dungeon_Crawler;
using Dungeon_Crawler.GameMacro;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var savedGames = DataBaseHandler.LoadGamesFromDataBase();

if (savedGames is null)
{
    savedGames = new List<Game>()
    {
        new Game()
    };

    savedGames[0].CreateNewGame(new Settings());
}


savedGames[0].DeMongoGame();

var userInterface = new AppData(savedGames);

userInterface.RunMainMenu();
