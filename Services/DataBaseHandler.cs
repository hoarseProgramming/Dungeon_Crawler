using Dungeon_Crawler.GameMacro;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Dungeon_Crawler.Services;

internal static class DataBaseHandler
{
    public static async Task<bool> SaveGameToDataBase(Game game)
    {
        using var client = GetClient();

        var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

        var filter = Builders<Game>.Filter.Eq("_id", game.Id);

        try
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                if (await savedGamesCollection.Find(filter).FirstOrDefaultAsync(timeoutCancellationTokenSource.Token) is not null)
                {
                    var savedGameUpdate = Builders<Game>.Update
                        .Set("Hero", game.Hero)
                        .Set("CurrentLevel", game.CurrentLevel)
                        .Set("Levels", game.Levels)
                        .Set("Settings", game.Settings)
                        .Set("GameLog", game.GameLog);

                    await savedGamesCollection.UpdateOneAsync(filter, savedGameUpdate);
                }
                else
                {
                    await savedGamesCollection.InsertOneAsync(game);
                }

                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static async Task<Game[]> LoadGamesFromDataBase()
    {
        using var client = GetClient();

        var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

        var filter = Builders<Game>.Filter.Empty;

        var saveFile = new Game[3];

        try
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                var savedGames = await savedGamesCollection.Find(filter).ToListAsync(timeoutCancellationTokenSource.Token);

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
        }
        catch (Exception)
        {
            saveFile[0] = new Game()
            {
                Id = 4
            };
            return saveFile;
        }
    }

    private static MongoClient GetClient()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<LevelData>().Build();

        var connectionString = config["ConnectionString"];

        return new MongoClient(connectionString);
    }

    internal static async Task<bool> DeleteGameFromDatabase(Game game)
    {
        bool hasDeletedSuccessfully = false;

        using var client = GetClient();

        var savedGamesCollection = client.GetDatabase("HampusEiderströmSwahn").GetCollection<Game>("SavedGames");

        var filter = Builders<Game>.Filter.Eq("_id", game.Id);

        try
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                await savedGamesCollection.DeleteOneAsync(filter, timeoutCancellationTokenSource.Token);
                hasDeletedSuccessfully = true;
            }
        }
        catch (Exception)
        {
            hasDeletedSuccessfully = false;
        }

        return hasDeletedSuccessfully;
    }
}
