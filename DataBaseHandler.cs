using Dungeon_Crawler.GameMacro;
using MongoDB.Driver;

namespace Dungeon_Crawler
{
    internal static class DataBaseHandler
    {
        public static void SaveGameToDataBase(Game game)
        {
            var connectionString = "mongodb://localhost:27017/";

            var client = new MongoClient(connectionString);

            var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

            var filter = Builders<Game>.Filter.Eq("_id", game.Id);

            if (savedGamesCollection.Find(filter).FirstOrDefault() is not null)
            {
                var savedGameUpdate = Builders<Game>.Update
                    .Set("Hero", game.Hero)
                    .Set("CurrentLevel", game.CurrentLevel)
                    .Set("Levels", game.Levels)
                    .Set("Settings", game.Settings)
                    .Set("GameLog", game.GameLog);

                savedGamesCollection.UpdateOne(filter, savedGameUpdate);
            }
            else
            {
                savedGamesCollection.InsertOne(game);
            }

            client.Dispose();
        }
        public static Game[]? LoadGamesFromDataBase()
        {
            var connectionString = "mongodb://localhost:27017/";

            var client = new MongoClient(connectionString);

            var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

            var filter = Builders<Game>.Filter.Empty;

            var savedGames = savedGamesCollection.Find(filter).ToList();

            client.Dispose();

            var saveFile = new Game[3];

            if (savedGames.Count == 0)
            {
                return saveFile;
            }
            else
            {
                foreach (var game in savedGames)
                {
                    saveFile[game.Id - 1] = game;
                }
                return saveFile;
            }
        }

        internal static void DeleteGameFromDatabase(Game game)
        {
            var connectionString = "mongodb://localhost:27017/";

            var client = new MongoClient(connectionString);

            var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

            var filter = Builders<Game>.Filter.Eq("_id", game.Id);

            savedGamesCollection.DeleteOne(filter);
        }
    }
}
